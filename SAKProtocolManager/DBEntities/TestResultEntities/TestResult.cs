using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKProtocolManager.DBEntities.TestResultEntities
{
    public class TestResult : DBBase
    {
        public static string[] PrimaryParametersList = new string[] { "Rж", "dR", "Cр", "dCр", "Co", "Ea", "Rиз1", "Rиз2", "K1", "K2", "K3", "K9", "K10", "K11", "K12", "K2,K3", "K9-12" };

        protected MeasureParameterType ParameterType = null;
        protected MeasuredParameterData ParameterData = null;
        protected int subElsNumber = 1;
        public decimal[] Values = new decimal[] { };
        public string[] Statuses= new string[] { };
        protected string testId = "0";
        public decimal RawValue = 0;
        public decimal BringingValue = 0;
        public int ElementNumber = 0;
        public int SubElementNumber = 1;
        public int GeneratorElementNumber = 0;
        public int GenetatorSubElementNumber = 0;

        public int StatusId = 0;

        public string Status = String.Empty;
        
        private string freqRangeId = "1";

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
            return String.Format("{0}", this.RawValue);
        }


        protected override void fillParametersFromRow(DataRow row)
        {

                this.ElementNumber = ServiceFunctions.convertToInt16(row["element_number"]);
                this.SubElementNumber = ServiceFunctions.convertToInt16(row["sub_element_number"]);
                this.GeneratorElementNumber = ServiceFunctions.convertToInt16(row["gen_element_number"]);
                this.GenetatorSubElementNumber = ServiceFunctions.convertToInt16(row["gen_sub_element_number"]);
                this.RawValue = ServiceFunctions.convertToDecimal(row["value"]);
                if (ParameterData != null) this.BringingValue = this.ParameterData.BringMeasuredValue(this.RawValue);
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

            if (this.ParameterType.Name != "Ao" && this.ParameterType.Name != "Az" && this.ParameterType.Name != "Rиз3" && this.ParameterType.Name != "Rиз4")
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
                    for (int i = 0; i < dt.Rows.Count; i++) trs[i] = new TestResult(dt.Rows[i], this.ParameterData);
                }
                return trs;
            }

        }

        internal static TestResult[] GetBigRound(int round, TestResult[] results)
        {
            TestResult[] trs = new TestResult[] { };
            int startIndex = -1;
            int endIndex = results.Length - 1;
            int prevElement = results.Length - 1;
            int prevMeasure = -1;
            int curRound = -1;
            for (int i = 0; i < results.Length; i++)
            {
                TestResult tr = results[i];
                if (tr.ElementNumber < prevElement || (tr.ElementNumber == prevElement && prevMeasure > tr.SubElementNumber))
                {
                    if (round != curRound)
                    {
                        curRound++;
                        if (round == curRound) startIndex = i;
                    }else
                    {
                        endIndex = i - 1;
                        break;
                    }
                }
                if (i == results.Length - 1) { endIndex = i; break;}
                prevElement = tr.ElementNumber;
                prevMeasure = tr.SubElementNumber;
            } 

            if (endIndex != -1 && startIndex != -1)
            {
                int length = endIndex - startIndex + 1;
                int j = 0;
                trs = new TestResult[length];
                for(int i=startIndex; i<=endIndex; i++)
                {
                    trs[j] = results[i];
                    j++;
                }
            } 
            return trs;
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

    }


}
