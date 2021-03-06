﻿using System;
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
        public MeasureParameterType ParameterType = null;
        public MeasuredParameterData ParameterData = null;
        public FrequencyRange FreqRange = null;
        public int subElsNumber = 1;
        public decimal[] Values = new decimal[] { };
        public decimal[] RawValues = new decimal[] {};
        public string[] Statuses= new string[] { };
        protected string testId = "0";
        public decimal RawValue = 0;
        private decimal bringingValue = 0;

        public decimal BringingValue
        {
            get
            {
                return bringingValue;
            }set
            {
                int round = 2;
                switch (this.ParameterType.Id)
                {
                    case MeasureParameterType.Rleads:
                    case MeasureParameterType.Cp:
                    case MeasureParameterType.Co:
                    case MeasureParameterType.Ea:
                    case MeasureParameterType.K1:
                    case MeasureParameterType.K2:
                    case MeasureParameterType.K3:
                    case MeasureParameterType.K9:
                    case MeasureParameterType.K10:
                    case MeasureParameterType.K11:
                    case MeasureParameterType.K12:
                    case MeasureParameterType.K23:
                    case MeasureParameterType.K9_12:
                    case MeasureParameterType.Risol1:
                    case MeasureParameterType.Risol3:
                        round = value > 99 ? 1 : 2;
                        bringingValue = Math.Round(value, round);
                        break;
                    case MeasureParameterType.al:
                    case MeasureParameterType.Ao:
                    case MeasureParameterType.Az:
                        bringingValue = Math.Round(value, 1);
                        break;
                    default:
                        bringingValue = value;
                        break;
                }
            }
        }
        public int ElementNumber = 0;
        public int SubElementNumber = 1;
        public int GeneratorElementNumber = 0;
        public int GeneratorSubElementNumber = 0;
        public decimal DeviationPercent = 0;
        public decimal NormaValue = 0;
        public bool Affected = false;

        public int StatusId = 0;

        public string Status = String.Empty;
        public string freqRangeId = "1";

        public TestResult CloneIncludingParameterType()
        {
            TestResult newTr = new TestResult(this.ParameterType);
            newTr.Affected = this.Affected;
            newTr.RawValue = this.RawValue;
            newTr.BringingValue = this.BringingValue;
            newTr.ElementNumber = this.ElementNumber;
            newTr.SubElementNumber = this.SubElementNumber;
            newTr.GeneratorElementNumber = this.GeneratorElementNumber;
            newTr.GeneratorSubElementNumber = this.GeneratorSubElementNumber;
            newTr.freqRangeId = this.freqRangeId;
            return newTr;
        }
        public TestResult()
        {

        }


        public TestResult(DataRow row, MeasureParameterType mp)
        {
            ParameterType = mp;
            fillParametersFromRow(row);
        }

        public TestResult(MeasureParameterType pType)
        {
            this.ParameterType = pType;
            this.testId = ParameterType.HasTest() ? this.ParameterType.Structure.Cable.Test.Id : "0";
            setDefaultParameters();
        }

        /// <summary>
        /// Устанавливает ParameterData результата если он соответствует ему.
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public bool SetParameterData(MeasuredParameterData pData)
        {
            bool criteria = pData.FrequencyRangeId == this.freqRangeId;
            if (criteria)
            {
                this.ParameterData = pData;
                this.BringingValue = this.ParameterData.BringMeasuredValue(this.RawValue);
                if (!IsAffected()) CheckIsItNorma();
            }
            return criteria;
        }

        //public TestResult(DataRow row, MeasuredParameterData pData)
        //{
        //    ParameterType = pData.ParameterType;
        //    ParameterData = pData;
        //    fillParametersFromRow(row);
        //}

        private bool IsAffected()
        {
            if (this.ParameterType == null) return false;
            if (ParameterType.Name == "Rиз3" || ParameterType.Name == "Rиз4" || this.ParameterType.Structure.AffectedElements.Count == 0) return false;
            ProzvonTestResult recElProzvonTestResult = ParameterType.Structure.GetProzvonTestResult(ElementNumber);
            ProzvonTestResult genElProzvonTestResult = null;
            if (ParameterType.Name == "Ao" || ParameterType.Name == "Az") genElProzvonTestResult = ParameterType.Structure.GetProzvonTestResult(GeneratorElementNumber);
            if (recElProzvonTestResult != null)
            {
                this.Status = recElProzvonTestResult.ElementStatus;
            }
            if (genElProzvonTestResult != null)
            {
                this.Status = genElProzvonTestResult.ElementStatus;
            }

           // ParameterType.ib
          //  flag |= this.ParameterType.Structure.AffectedElements.Keys.Contains(this.ElementNumber);
            return this.Affected = genElProzvonTestResult != null || recElProzvonTestResult != null;
        }


        /// <summary>
        /// устарело 25-12-2017
        /// </summary>
        /// <param name="measuredParameter"></param>
        public TestResult(MeasuredParameterData measuredParameter)
        {
            this.ParameterData = measuredParameter;
            this.ParameterType = measuredParameter.ParameterType; 
            this.freqRangeId = measuredParameter.FrequencyRangeId; //можно удалить
            this.testId = measuredParameter.HasTest() ? measuredParameter.ParameterType.Structure.Cable.Test.Id : "0"; 
            setDefaultParameters();
        }
        

        public string GetStringTableValue()
        {
            if (IsAffected()) return Status;
            else
            {
                int pow = 0;
                decimal tmpVal = BringingValue;
                if (tmpVal > 9999)
                {
                    while (tmpVal / 10 > 1)
                    {
                        pow++;
                        tmpVal /= 10;
                    }
                    return $"{Math.Round(tmpVal, 1)}∙e{pow}";
                }
                else
                {
                    return $"{BringingValue}";
                }
            }
        }




        protected override void fillParametersFromRow(DataRow row)
        {
            float val = ServiceFunctions.convertToFloat(row["value"]);
            string fRangeId = row["frequency_range_id"].ToString();
            this.ElementNumber = ServiceFunctions.convertToInt16(row["element_number"]);
            this.SubElementNumber = ServiceFunctions.convertToInt16(row["sub_element_number"]);
            this.GeneratorElementNumber = ServiceFunctions.convertToInt16(row["gen_element_number"]);
            this.GeneratorSubElementNumber = ServiceFunctions.convertToInt16(row["gen_sub_element_number"]);
            this.RawValue = (decimal)val;
            this.freqRangeId = String.IsNullOrWhiteSpace(fRangeId) || fRangeId == "NULL" ? "1" : fRangeId;
            if (ParameterData != null)
            {
                this.BringingValue = this.ParameterData.BringMeasuredValue(this.RawValue);
                if (!IsAffected()) CheckIsItNorma();
            }
            if (freqRangeId != "1")
            {
                FreqRange = new FrequencyRange(row);
            }
                    
        }

        public bool CheckIsItNorma()
        {
            if (ParameterData == null) return true;
            if (IsAffected()) return true;
            decimal prc = 0;
            decimal val = Math.Abs(this.BringingValue);
            if (val < ParameterData.MinValue)
            {
                this.NormaValue = ParameterData.MinValue;
                
                if (this.ParameterType.Name == "Ao" || this.ParameterType.Name == "Az" || this.ParameterType.Name == "al" || this.ParameterType.Name == "Rиз2" || this.ParameterType.Name == "Rиз4" || this.ParameterType.Name == "dR")
                {
                    prc = ParameterData.MinValue - val;
                }
                else prc = (decimal)100 * (ParameterData.MinValue - val) / ParameterData.MinValue;
            }
            else if (val > ParameterData.MaxValue)
            {
                this.NormaValue = ParameterData.MaxValue;
                if (this.ParameterType.Name == "Ao" || this.ParameterType.Name == "Az" || this.ParameterType.Name == "al" || this.ParameterType.Name == "Rиз2" || this.ParameterType.Name == "Rиз4" || this.ParameterType.Name == "dR")
                {
                    prc = val - ParameterData.MaxValue;
                }
                else
                {
                    prc = (decimal) 100 * (val - ParameterData.MaxValue) / ParameterData.MaxValue;
                }
            }
            this.DeviationPercent = Math.Round(prc, 3);
            return this.DeviationPercent == 0;
        }

        protected override void setDefaultParameters()
        {
            //string freqQuery = freqRangeId == "1" ? "" : String.Format(" AND resultism.FreqDiap = {0}", freqRangeId);
            string selQuery = "resultism.StruktElNum AS element_number," +
                              "resultism.IsmerNum AS sub_element_number," +
                              "resultism.StruktElNum_gen AS gen_element_number," +
                              "resultism.ParaNum_gen AS gen_sub_element_number," + 
                              "resultism.Resultat AS value, " +
                              "resultism.FreqDiap AS frequency_range_id, " +
                              "freq_diap.FreqMin AS frequency_range_min_freq, " +
                              "freq_diap.FreqMax AS frequency_range_max_freq, " +
                              "freq_diap.FreqStep AS frequency_range_freq_step ";
                this.getAllQuery = String.Format("SELECT {0} FROM resultism LEFT JOIN freq_diap ON freq_diap.FreqDiapInd = resultism.freqdiap WHERE resultism.IspInd = {1} AND resultism.ParamInd = {2} AND resultism.StruktInd = {3}", selQuery, testId, ParameterType.Id, ParameterType.Structure.Id);
            this.colsList = new string[]
            {
                "element_number",
                "sub_element_number",
                "gen_element_number",
                "gen_sub_element_number",
                "value",
                "frequency_range_id",
                "frequency_range_min_freq",
                "frequency_range_max_freq",
                "frequency_range_freq_step"
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public TestResult[] GetMeasuredResults()
        {
            if (PrimaryParametersList.Contains(this.ParameterType.Name))//(this.ParameterType.Name != "al" && this.ParameterType.Name != "Ao" && this.ParameterType.Name != "Az" && this.ParameterType.Name != "Rиз3" && this.ParameterType.Name != "Rиз4")
            {
                PrimaryParametersTestResult pptr = new PrimaryParametersTestResult(ParameterType);
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
                        TestResult tr = new TestResult(dt.Rows[i], this.ParameterType);
                        //if (!tr.CheckIsItNorma()) tr.ParameterData.NotNormalResults.Add(tr);
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
            return SubTitle(this.GeneratorSubElementNumber);
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


        public static void UpdateFreqRange(MeasuredParameterData mpd, string changedFreqRangeId)
        {
            string setString = $"resultism.FreqDiap = {mpd.FrequencyRangeId}";// $"UPDATE resultism SET resultism.FreqDiap = {mpd.FrequencyRangeId} WHERE IspInd = {mpd.ParameterType.Structure.Cable.Test.Id} AND ParamId = {mpd.ParameterType.Id} AND StruktInd = {mpd.ParameterType.Structure.Id} AND resultism.FreqDiap = {changedFreqRangeId}";
            string whereString = $"resultism.IspInd = {mpd.ParameterType.Structure.Cable.Test.Id} AND resultism.ParamInd = {mpd.ParameterType.Id} AND resultism.StruktInd = {mpd.ParameterType.Structure.Id} AND resultism.FreqDiap = {changedFreqRangeId}";
            string updQuery = BuildUpdQuery("resultism", setString, whereString);
            SendQuery(updQuery);
        }

        public string BuildUpdLengthQuery(decimal curLength, decimal newLength)
        {
            this.RawValue = ParameterType.BringToLength(this.RawValue, curLength, newLength);
            return UpdRawValueQuery();
        }

        public void UpdateResult(decimal newValue)
        {
            this.RawValue = this.ParameterType.BringToLength(newValue, this.ParameterData.BringingLength, this.ParameterType.Structure.Cable.Test.TestedLength);
            this.BringingValue = this.ParameterData.BringMeasuredValue(this.RawValue);
            SendQuery(UpdRawValueQuery());
            this.CheckIsItNorma();
        }

        public string UpdRawValueQuery()
        {
            string q, w, tName, elNumber;
            q = String.Format("Resultat = {0}", this.RawValue);
            tName = "resultism";
            elNumber = (this.ElementNumber == 0) ? "StruktElNum IS NULL" : "StruktElNum = " + this.ElementNumber.ToString();
            w = String.Format("IspInd = {0} AND ParamInd = {1} AND StruktInd = {2} AND {3} AND IsmerNum = {4}", this.ParameterType.Structure.Cable.Test.Id, this.ParameterType.Id, this.ParameterType.Structure.Id, elNumber, this.SubElementNumber);
            if (this.ParameterType.Name == "Ao" || this.ParameterType.Name == "Az") w += String.Format(" AND StruktElNum_gen = {0} AND ParaNum_gen = {1} AND FreqDiap = {2}", this.GeneratorElementNumber, this.GeneratorSubElementNumber, this.freqRangeId);
            return BuildUpdQuery(tName, q, w);
        }

    }


}
