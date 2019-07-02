using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using SAKProtocolManager.DBEntities.TestResultEntities;
namespace SAKProtocolManager.DBEntities
{
    public class MeasureParameterType : DBBase
    {

        public string Name, Description, Measure;
        public static string[] PrimaryParametersList = new string[] {};
        internal TestResult[] TestResults = new TestResult[] { };
        internal MeasuredParameterData[] ParameterDataList = new MeasuredParameterData[] { };
        internal CableStructure Structure = null;
        private int curParameterDataIdx = -1;
        private string deviationMeasure = null;
        /// <summary>
        /// Флаг того, что это испытание проводилось и есть результаты
        /// </summary>
        internal bool IsTested = true; 

        public int OutOfNormaCount()
        {
            int counter = 0;
            if (ParameterDataList.Length > 0)
            {
                foreach (MeasuredParameterData pd in ParameterDataList) counter += pd.NotNormalResults.Length;
            }
            return counter;
        }

        public MeasureParameterType()
        {
            setDefaultParameters();
        }

        /// <summary>
        /// Чтобы получить типы параметров структуры, через функцию GetAll() необходимо её указать во входящем параметре
        /// </summary>
        /// <param name="structure"></param>
        public MeasureParameterType(CableStructure structure)
        {
            this.Structure = structure;
            setDefaultParameters();
        }

        public MeasureParameterType(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
        }

        public MeasureParameterType(DataRow row)
        {
            fillParametersFromRow(row);
        }

        public MeasureParameterType(DataRow row, CableStructure structure)
        {
            this.Structure = structure;
            fillParametersFromRow(row);
            setDefaultParameters();
            getStructureParameterData();
            if (ParameterDataList.Length>0)
            {
                GetTestResult();
                assaignTestResultsToParameterData();
            }
            //if (HasTest()) CheckIsItTested();
        }

        /*private void fillMeasuredParameterDataResult()
        {
            if(this.ParameterData.Length > 0 && this.TestResults.Length > 0)
            {
                for(int i=0; i< this.ParameterData.Length; i++)
                {
                    this.ParameterData[i].TestResults = TestResult.GetBigRound(i, this.TestResults);
                    this.ParameterData[i].CalculateMinMaxAverage();
                }
            }
        }*/

        public void SetParameterDataIdx(int idx)
        {
            if (idx >= ParameterDataList.Length || idx < 0) return;
            curParameterDataIdx = idx;
            assaignTestResultsToParameterData();
        }

        /// <summary>
        /// Обновляем измеренные параметры на MeasuredParameterData
        /// </summary>
        public void RefreshTestResultsOnParameterData(bool needToReloadResults)
        {
            if (needToReloadResults) GetTestResult();
            if (ParameterDataList.Length > 0 && this.TestResults.Length > 0) assaignTestResultsToParameterData();
        }


        private void assaignTestResultsToParameterData()
        {
            foreach(MeasuredParameterData pData in ParameterDataList)
            {
                List<TestResult> trListForPData = new List<TestResult>();
                List<TestResult> NotNormaTrListForPData = new List<TestResult>();
                List<decimal> vals = new List<decimal>();
                List<int> AffectedElements = new List<int>();
                repeat_assignment:
                foreach (TestResult tr in TestResults)
                {
                    TestResult cTr = tr.CloneIncludingParameterType();
                    if (cTr.SetParameterData(pData))
                    {
                        trListForPData.Add(cTr);
                        if (!cTr.Affected)
                        {
                            vals.Add(cTr.BringingValue);
                        }
                        if (cTr.DeviationPercent > 0) NotNormaTrListForPData.Add(cTr);
                    }
                }
                if (IsFreqParameter && trListForPData.Count == 0)
                {
                    if (CheckFailedFreqAssignment(pData))
                    {
                        GetTestResult();
                        goto repeat_assignment;
                    }
                }
                pData.TestResults = trListForPData.ToArray();
                pData.NotNormalResults = NotNormaTrListForPData.ToArray();
                if (pData.TestResults.Length > 0)
                {
                    pData.MeasuredPercent = Math.Round(100 * (((decimal)pData.TestResults.Length-(decimal)NotNormaTrListForPData.Count()) / (decimal)pData.TestResults.Length), 0);
                    if (vals.Count > 0)
                    {
                        pData.MaxVal = vals.Max();
                        pData.MinVal = vals.Min();
                        pData.AverageVal = Math.Round(vals.Sum() / (decimal)vals.Count(), 1);
                    }
                }
            }
        }

        /// <summary>
        /// Исправляет ошибку привязки к диапазону частоты
        /// </summary>
        /// <returns></returns>
        private bool CheckFailedFreqAssignment(MeasuredParameterData mpd)
        {
            foreach(TestResult tr in TestResults)
            {
                if (tr.FreqRange.MinFreq == mpd.MinFrequency && tr.freqRangeId != mpd.FrequencyRangeId)
                {
                    bool isOnUsing = false;
                    foreach(MeasuredParameterData pd in ParameterDataList)
                    {
                        if (isOnUsing = tr.freqRangeId == pd.FrequencyRangeId) break;
                    }
                    if (!isOnUsing)
                    {
                        TestResult.UpdateFreqRange(mpd, tr.freqRangeId);
                        return true;
                    }
                }
            }
            return false;
        }

        private void getStructureParameterData()
        {
            if (this.Structure == null) return;
            MeasuredParameterData mpd = new MeasuredParameterData(this);
            this.ParameterDataList = mpd.GetStructureMeasureParameters();

        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["measure_parameter_type_id"].ToString();
            this.Name = row["measure_parameter_type_name"].ToString();
            this.Measure = row["measure_parameter_type_measure"].ToString();
            this.Description = row["measure_parameter_type_description"].ToString();
        }

        protected override void setDefaultParameters()
        {
            string selectQuery = "ism_param.ParamInd AS measure_parameter_type_id, ism_param.ParamName AS measure_parameter_type_name, ism_param.Ed_izm AS measure_parameter_type_measure, ism_param.ParamOpis AS measure_parameter_type_description";
            this.getAllQuery = String.Format("SELECT {0} FROM ism_param ORDER BY ism_param.ParamInd", selectQuery);
            this.getByIdQuery = String.Format("SELECT {0} FROM ism_param WHERE ism_param.ParamInd = {1} ORDER BY ism_param.ParamInd LIMIT 1", selectQuery, Id);
            this.colsList = new string[] { "measure_parameter_type_id", "measure_parameter_type_name", "measure_parameter_type_measure", "measure_parameter_type_description" };
        }

        public MeasureParameterType[] GetAll()
        {
            MeasureParameterType[] els = new MeasureParameterType[] { };
            DataTable dt = GetAllFromDB();
            if (dt.Rows.Count > 0)
            {
                els = new MeasureParameterType[dt.Rows.Count];
                if (this.Structure == null) for (int i = 0; i < dt.Rows.Count; i++) els[i] = new MeasureParameterType(dt.Rows[i]);
                else for (int i = 0; i < dt.Rows.Count; i++) els[i] = new MeasureParameterType(dt.Rows[i], this.Structure);

            }
            return els;
        }

        /// <summary>
        /// Проверяет измерялся ли данный параметр или нет
        /// </summary>
        private void CheckIsItTested()
        {
            if(ParameterDataList.Length > 0)
            {
                foreach(MeasuredParameterData pd in ParameterDataList)
                {
                    this.IsTested = pd.TestResults.Length > 0;
                    if (this.IsTested) return;
                }
            }
            this.IsTested = false;
        }

        
        internal bool HasTest()
        {
            if (this.Structure == null) return false;
            if (this.Structure.Cable == null) return false;
            if (this.Structure.Cable.Test == null) return false;
            return true;
        }

        internal string NameWithMeasure()
        {
            if (String.IsNullOrWhiteSpace(this.Measure)) return this.Name;
            else return String.Format("{0}, {1}", this.Name, this.Measure);
        }

        /// <summary>
        /// Мера измерения отклонения от нормы %, дБ, с.
        /// </summary>
        /// <returns></returns>
        internal string DeviationMeasure()
        {
            if (this.deviationMeasure == null)
            {
                string val = String.Empty;
                switch (this.Id)
                {
                    case Ao:
                    case Az:
                    case al:
                        val = "дБ";
                        break;
                    case Risol2:
                    case Risol4:
                        val = "c.";
                        break;
                    default:
                        val = "%";
                        break;
                }
                this.deviationMeasure = val;
            } 
            return this.deviationMeasure;
        }

        internal void GetTestResult()
        {
            // return;
            if (!HasTest()) return;
            TestResult tResult = new TestResult(this);
            this.TestResults = tResult.GetMeasuredResults();
            this.IsTested = TestResults.Length > 0;
        }

        public bool IsFreqParameter
        {
            get
            {
                return (Id == Ao || Id == Az || Id == al);
            }
        }
            
        public decimal BringToLength(decimal value, decimal curLength, decimal brLength)
        {
            switch (this.Id)
            {
                case Rleads:
                case Cp:
                case Co:
                case Ea:
                case K1:
                case K2:
                case K3:
                case K9:
                case K10:
                case K11:
                case K12:
                case K23:
                case K9_12:
                    value *= brLength / curLength;
                    break;
                case Risol1:
                case Risol3:
                    value *= curLength / brLength;
                    break;
                case al:
                    value *= brLength / curLength;
                    break;
                case Ao:
                case Az:
                    value += 10 * (decimal)Math.Log10(((double)curLength / (double)brLength));
                    break;
            }
            return value;
        }

        public bool IsRizol
        {
            get
            {
                switch (Id)
                {
                    case Risol1:
                    case Risol2:
                    case Risol3:
                    case Risol4:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public bool Is_K_Parameter
        {
            get
            {
                switch(Id)
                {
                    case K1:
                    case K2:
                    case K3:
                    case K23:
                    case K9:
                    case K10:
                    case K11:
                    case K12:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public bool IsPrimaryParameter
        {
            get
            {
                return ((Id == Rleads) || (Id == dR) || Id == Risol1 || Id == Risol2 || (Id == Cp) || (Id == Co) || (Id == Ea) || Is_K_Parameter);
            }
        }
        public const string Calling = "1";
        public const string Rleads = "2";
        public const string dR = "3";
        public const string Risol1 = "4";
        public const string Risol2 = "5";
        public const string Risol3 = "6";
        public const string Risol4 = "7";
        public const string Cp = "8";
        public const string dCp = "9";
        public const string Co = "10";
        public const string Ea = "11";
        public const string K1 = "12";
        public const string K23 = "13";
        public const string K9_12 = "14";
        public const string al = "15";
        public const string Ao = "16";
        public const string Az = "17";
        public const string K2 = "18";
        public const string K3 = "19";
        public const string K9 = "20";
        public const string K10 = "21";
        public const string K11 = "22";
        public const string K12 = "23";
    }


}
