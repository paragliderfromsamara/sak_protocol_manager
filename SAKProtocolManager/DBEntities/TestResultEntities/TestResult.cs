using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SAKProtocolManager.DBEntities.TestResultEntities
{
    public class TestResult : DBBase
    {
        public static string[] PrimaryParametersList = new string[] { "Rж", "dR", "Cр", "dCр", "Co", "Ea", "Rиз1", "Rиз2", "K1", "K2", "K3", "K9", "K10", "K11", "K12", "K2,K3", "K9-12" };
        public string brLength = "none";
        public MeasureParameterType ParameterType = null;
        public MeasuredParameterData ParameterData = null;
        protected int subElsNumber = 1;
        public decimal[] Values = new decimal[] { };
        public decimal[] RawValues = new decimal[] {};
        public string[] Statuses= new string[] { };
        protected string testId = "0";
        public decimal RawValue = 0;
        public decimal BringingValue = 0;
        public int ElementNumber = 0;
        public int SubElementNumber = 1;
        public int GeneratorElementNumber = 0;
        public int GenetatorSubElementNumber = 0;
        public decimal DeviationPercent = 0;
        public decimal NormaValue = 0;


        public int StatusId = 0;

        public string Status = String.Empty;
        
        protected string freqRangeId = "1";

        public TestResult()
        {

        }

        public TestResult(DataRow row, MeasureParameterType mp)
        {
            ParameterType = mp;
            fillParametersFromRow(row);
        }

        public TestResult(DataRow row, MeasuredParameterData pData)
        {
            ParameterType = pData.ParameterType;
            ParameterData = pData;
            fillParametersFromRow(row);
        }

        public bool IsAffected()
        {
            int[] affEls = this.ParameterType == null ? new int[] { } : this.ParameterType.Structure.AffectedElementNumbers;
            if (affEls.Length == 0) return false;
            foreach (int i in affEls) if (i == this.ElementNumber) return true;
            return false;
        }


        public TestResult(MeasuredParameterData measuredParameter)
        {
            this.ParameterData = measuredParameter;
            this.ParameterType = measuredParameter.ParameterType;
            this.freqRangeId = measuredParameter.FrequencyRangeId;
            this.testId = measuredParameter.HasTest() ? measuredParameter.ParameterType.Structure.Cable.Test.Id : "0";
            setDefaultParameters();
        }
        
        public string GetStringTableValue()
        {
            if (IsAffected()) return "Брак";
            return String.Format("{0}", this.BringingValue);
        }


        protected override void fillParametersFromRow(DataRow row)
        {
                float val = ServiceFunctions.convertToFloat(row["value"]);
                this.ElementNumber = ServiceFunctions.convertToInt16(row["element_number"]);
                this.SubElementNumber = ServiceFunctions.convertToInt16(row["sub_element_number"]);
                this.GeneratorElementNumber = ServiceFunctions.convertToInt16(row["gen_element_number"]);
                this.GenetatorSubElementNumber = ServiceFunctions.convertToInt16(row["gen_sub_element_number"]);
                this.RawValue = (decimal)val;
                if (ParameterData != null)
                {
                    this.BringingValue = this.ParameterData.BringMeasuredValue(this.RawValue);
                    if (!IsAffected()) CheckIsItNorma();
                }
                    
        }

        public bool CheckIsItNorma()
        {
            if (ParameterData == null) return true;
            if (IsAffected()) return true;
            decimal prc = 0;
            if (this.BringingValue < ParameterData.MinValue)
            {
                this.NormaValue = ParameterData.MinValue;
                
                if (this.ParameterType.Name == "Ao" || this.ParameterType.Name == "Az" || this.ParameterType.Name == "al" || this.ParameterType.Name == "Rиз2" || this.ParameterType.Name == "Rиз4" || this.ParameterType.Name == "dR")
                {
                    prc = ParameterData.MinValue - this.BringingValue;
                }
                else prc = (decimal)100 * (ParameterData.MinValue - this.BringingValue) / ParameterData.MinValue;
            }
            else if (this.BringingValue > ParameterData.MaxValue)
            {
                this.NormaValue = ParameterData.MaxValue;
                if (this.ParameterType.Name == "Ao" || this.ParameterType.Name == "Az" || this.ParameterType.Name == "al" || this.ParameterType.Name == "Rиз2" || this.ParameterType.Name == "Rиз4" || this.ParameterType.Name == "dR")
                {
                    prc = this.BringingValue - ParameterData.MaxValue;
                }
                else
                {
                    prc = (decimal) 100 * (this.BringingValue - ParameterData.MaxValue) / ParameterData.MaxValue;
                }
            }
            this.DeviationPercent = Math.Round(prc, 1);
            return this.DeviationPercent == 0;
        }

        protected override void setDefaultParameters()
        {

                string freqQuery = freqRangeId == "1" ? "" : String.Format(" AND resultism.FreqDiap = {0}", freqRangeId);
                string selQuery = "resultism.StruktElNum AS element_number," +
                              "resultism.IsmerNum AS sub_element_number," +
                              "resultism.StruktElNum_gen AS gen_element_number," +
                              "resultism.ParaNum_gen AS gen_sub_element_number," + 
                              "resultism.Resultat AS value";
                this.getAllQuery = String.Format("SELECT {0} FROM resultism WHERE resultism.IspInd = {1} AND resultism.ParamInd = {2} AND resultism.StruktInd = {3} {4}", selQuery, testId, ParameterType.Id, ParameterType.Structure.Id, freqQuery);
            this.colsList = new string[]
            {
                "element_number",
                "sub_element_number",
                "gen_element_number",
                "gen_sub_element_number",
                "value"
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public TestResult[] GetMeasuredResults()
        {
            if (this.ParameterType.Name != "al" && this.ParameterType.Name != "Ao" && this.ParameterType.Name != "Az" && this.ParameterType.Name != "Rиз3" && this.ParameterType.Name != "Rиз4")
            {
                PrimaryParametersTestResult pptr = new PrimaryParametersTestResult(ParameterData);
                TestResult[] trs = pptr.GetMeasuredResults();
                return trs;
            }
            else
            {
                TestResult[] trs = new TestResult[] { };
                DataTable dt = GetAllFromDB();
                if (dt.Rows.Count > 0)
                {
                    trs = new TestResult[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TestResult tr = new TestResult(dt.Rows[i], this.ParameterData);
                        if (!tr.CheckIsItNorma()) tr.ParameterData.NotNormalResults.Add(tr);
                        trs[i] = tr;
                    }
                        
                }
                return trs;
            }

        }

        public string SubElementTitle()
        {
            return SubTitle(this.SubElementNumber);
        }

        public string GeneratorSubElementTitle()
        {
            return SubTitle(this.GenetatorSubElementNumber);
        }

        private string SubTitle(int SubNumber)
        {
            string title = SubNumber.ToString();
            switch (this.ParameterType.Name)
            {
                case "dR":
                case "Cр":
                case "Ea":
                case "al":
                case "Ao":
                case "Az":
                    if (this.ParameterType.Structure.BendingTypeLeadsNumber > 3)
                    {
                        return SubNumber == 1 ? "1-2" : "3-4";
                    }
                    else return title;
                default:
                    return title;
            }
        }


        public string BuildUpdLengthQuery(decimal curLength, decimal newLength)
        {
            this.RawValue = ParameterData.BringToLength(this.RawValue, curLength, newLength);
            return UpdRawValueQuery();
        }

        public void CorrectResult()
        {
            if (this.DeviationPercent == 0) return;
            Random r = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Thread.Sleep(1);
            decimal corrCoeff = (decimal)r.Next(100, 999)/1000;//this.DeviationPercent;// * (decimal)Math.Sqrt(2);
            if (this.NormaValue < this.BringingValue) corrCoeff *= -1;
            switch (this.ParameterType.Name)
            {
                case "Ao":
                case "Az":
                    this.RawValue = this.ParameterData.BringToLength(this.NormaValue + corrCoeff, this.ParameterData.BringingLength, this.ParameterType.Structure.Cable.Test.TestedLength);
                    break;
                case "dR":
                case "Rиз2":
                case "Rиз4":
                    this.RawValue = this.NormaValue + corrCoeff;
                    break;
                default:
                    this.RawValue = ((this.NormaValue + corrCoeff) * this.RawValue / this.BringingValue);//(this.NormaValue * corrCoeff) / 100;
                    break;
            }
            this.BringingValue = this.ParameterData.BringMeasuredValue(this.RawValue);
            this.CheckIsItNorma();
        }

        public string UpdRawValueQuery()
        {
            string q, w, tName, elNumber;
            q = String.Format("Resultat = {0}", this.RawValue);
            tName = "resultism";
            elNumber = (this.ElementNumber == 0) ? "StruktElNum IS NULL" : "StruktElNum = " + this.ElementNumber.ToString();
            w = String.Format("IspInd = {0} AND ParamInd = {1} AND StruktInd = {2} AND {3} AND IsmerNum = {4}", this.ParameterType.Structure.Cable.Test.Id, this.ParameterType.Id, this.ParameterType.Structure.Id, elNumber, this.SubElementNumber);
            if (this.ParameterType.Name == "Ao" || this.ParameterType.Name == "Az") w += String.Format(" AND StruktElNum_gen = {0} AND ParaNum_gen = {1} AND FreqDiap = {2}", this.GeneratorElementNumber, this.GenetatorSubElementNumber, this.ParameterData.FrequencyRangeId);
            return BuildUpdQuery(tName, q, w);
        }

    }


}
