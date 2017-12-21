using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAKProtocolManager.DBEntities.TestResultEntities;

namespace SAKProtocolManager.DBEntities
{
    public class MeasuredParameterData : DBBase
    {
        public List<TestResult> NotNormalResults = new List<TestResult>();
        /// <summary>
        /// Соответствует FreqDiapInd в таблице freq_diap
        /// </summary>
        public string FrequencyRangeId = String.Empty;
        /// <summary>
        /// Длина приведения
        /// </summary>
        public decimal BringingLength = 0;
        /// <summary>
        /// 
        /// </summary>
        public string BringingLengthTypeId = String.Empty;
        /// <summary>
        /// Мера измерения длины приведения
        /// </summary>
        public string BringingLengthMeasure = String.Empty;
        /// <summary>
        /// Название приведения  длине
        /// </summary>
        public string BringingLengthName = String.Empty;
        /// <summary>
        /// Максимально допустимое значение результата измерения 
        /// </summary>
        public decimal MinValue = Decimal.MinValue; 
        /// <summary>
        /// Минимальное допустимое значение результата измерения
        /// </summary>
        public decimal MaxValue = Decimal.MaxValue; 
        /// <summary>
        /// Минимальная частота
        /// </summary>
        public uint MinFrequency = 0;
        /// <summary>
        /// Максимальная частота
        /// </summary>
        public uint MaxFrequency = 0;
        /// <summary>
        /// Шаг частоты
        /// </summary>
        public uint FrequencuStep = 0;
        /// <summary>
        /// Допустимый процент брака
        /// </summary>
        public decimal Percent = 100;

        /// <summary>
        /// Минимальное измеренное значени
        /// </summary>
        public decimal MinVal = Decimal.MaxValue;
        /// <summary>
        /// Максимальное измеренное значение
        /// </summary>
        public decimal MaxVal = Decimal.MinValue;
        /// <summary>
        /// Среднее измеренное значение
        /// </summary>
        public decimal AverageVal = 0;
        /// <summary>
        /// Запрос для получения типов измеряемых параметров по id структуры
        /// </summary>
        private string getAllByStructIdQuery = String.Empty;
        private string StructureId = String.Empty;
        public TestResult[] TestResults = new TestResult[] { };
        internal MeasureParameterType ParameterType = null;


        public MeasuredParameterData(MeasureParameterType parameter_type)
        {
            this.ParameterType = parameter_type;
            setDefaultParameters();
        }

        public MeasuredParameterData(DataRow row, MeasureParameterType parameter_type)
        {
            fillParametersFromRow(row);
            this.ParameterType = parameter_type;
            setDefaultParameters();
            GetTestResult();
        }

        private void GetTestResult()
        {
           // return;
            if (!HasTest()) return;
            TestResult tResult = new TestResult(this);
            TestResults = tResult.GetMeasuredResults();
        } 

        public void CalculateMinMaxAverage()
        {
            decimal max, min, average;
            max = decimal.MinValue;
            min = decimal.MaxValue;
            average = 0;
            int counter = 0;
            if (this.TestResults.Length == 0) return;
            foreach(TestResult tr in this.TestResults)
            {
                if (tr.IsAffected()) continue;
                counter++;
                if (tr.RawValue > max) max = tr.RawValue;
                if (tr.RawValue < min) min = tr.RawValue;
                average += tr.RawValue;
            }
            if (counter > 0) average /= counter;
            this.MaxVal = max;
            this.MinVal = min;
            this.AverageVal = average;
        }

        public string GetFreqRange()
        {
            string r = String.Empty;
            if (this.MinFrequency > 0) r = this.MinFrequency.ToString();
            if (this.MinFrequency > 0 && this.MaxFrequency > 0) r += "-";
            if (this.MaxFrequency > 0) r += this.MaxFrequency.ToString();
            return r;
        }
        protected override void setDefaultParameters()
        {
            string selQuery = "param_data.Min AS measured_parameter_min_value," +
                              "param_data.Max AS measured_parameter_max_value," +
                              "param_data.Lpriv AS measured_parameter_bringing_length," +
                              "param_data.LprivInd AS bringing_length_type_id," + 
                              "param_data.Percent AS measured_parameter_percent," +
                              "param_data.FreqDiap AS frequency_range_id," +
                              "ism_param.ParamName AS measured_parameter_name," +
                              "ism_param.Ed_izm AS measured_parameter_measure," +
                              "ism_param.ParamOpis AS measured_parameter_description," +
                              "freq_diap.FreqMin AS measure_parameter_min_frequency, " +
                              "freq_diap.FreqMax AS measure_parameter_max_frequency," +
                              "freq_diap.FreqStep AS measure_parameter_frequency_step," +
                              "lpriv_tip.Ed_izm AS bringing_length_type_measure," +
                              "lpriv_tip.LprivName AS bringing_length_type_name";

            this.getAllQuery = String.Format("SELECT {0} FROM param_data LEFT JOIN ism_param USING(ParamInd) LEFT JOIN freq_diap ON param_data.FreqDiap = freq_diap.FreqDiapInd LEFT JOIN lpriv_tip USING(LprivInd)", selQuery);
            this.getAllByStructIdQuery = String.Format("{0} WHERE param_data.StruktInd = {1} AND param_data.ParamInd = {2}", this.getAllQuery, this.ParameterType.Structure.Id, this.ParameterType.Id);
            this.colsList = new string[]
            {
                "measured_parameter_min_value",
                "measured_parameter_max_value",
                "measured_parameter_bringing_length",
                "bringing_length_type_id",
                "measured_parameter_percent",
                "frequency_range_id",
                "measured_parameter_name",
                "measured_parameter_measure",
                "measured_parameter_description",
                "measure_parameter_min_frequency",
                "measure_parameter_max_frequency",
                "measure_parameter_frequency_step",
                "bringing_length_type_measure",
                "bringing_length_type_name"
            };
        }

        internal MeasuredParameterData[] GetStructureMeasureParameters()
        {
            MeasuredParameterData[] ents = new MeasuredParameterData[] { };
            DataTable dt = getFromDB(this.getAllByStructIdQuery);
            if (dt.Rows.Count > 0)
            {
                ents = new MeasuredParameterData[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++) ents[i] = new MeasuredParameterData(dt.Rows[i], this.ParameterType);
            }
            return ents;
        }



        public string ResultMeasure()
        {
            
            string measure = String.Empty;
            string brMeasure = BringingLengthTypeId == "3" ? String.Format("/{0}м", this.BringingLength) : BringingLengthMeasure;
            if (this.ParameterType.Name == "dR")
            {
                int drId = ServiceFunctions.convertToInt16(this.ParameterType.Structure.dRFormulaId);
                measure = (drId > 2) ? this.ParameterType.Structure.dRFormulaMeasure : String.Format(" {0}{1}", this.ParameterType.Structure.dRFormulaMeasure, brMeasure);
            }
            else
            {
                measure = String.Format(" {0}{1}", this.ParameterType.Measure, brMeasure);
            }
            return measure;
        }

        /// <summary>
        /// Подсчитывает сколько параметров одного типа измеряется на данном типе кабеля
        /// </summary>
        /// <param name="paramsList">Список измеряемых параметров</param>
        /// <param name="paramId">id измеряемого параметра</param>
        /// <returns></returns>
        internal static int NumberOfParameterTypeOnCable(MeasuredParameterData[] paramsList, string paramId)
        {
            int count = 0;
            foreach(MeasuredParameterData pd in paramsList) if (pd.Id == paramId) count++;
            return count;
        }
        protected override void fillParametersFromRow(DataRow row)
        {
            string minVal = row["measured_parameter_min_value"].ToString();
            string maxVal = row["measured_parameter_max_value"].ToString();
            if (!String.IsNullOrEmpty(minVal)) this.MinValue = ServiceFunctions.convertToDecimal(minVal);
            if (!String.IsNullOrEmpty(maxVal)) this.MaxValue = ServiceFunctions.convertToDecimal(maxVal);
            this.Percent = ServiceFunctions.convertToDecimal(row["measured_parameter_percent"]);

            this.MinFrequency = ServiceFunctions.convertToUInt(row["measure_parameter_min_frequency"]);
            this.MaxFrequency = ServiceFunctions.convertToUInt(row["measure_parameter_max_frequency"]);
            this.FrequencuStep = ServiceFunctions.convertToUInt(row["measure_parameter_frequency_step"]);

            this.BringingLength = ServiceFunctions.convertToDecimal(row["measured_parameter_bringing_length"]);
            this.BringingLengthName = row["bringing_length_type_name"].ToString();
            this.BringingLengthMeasure = row["bringing_length_type_measure"].ToString();
            this.BringingLengthTypeId = row["bringing_length_type_id"].ToString();
            this.FrequencyRangeId = row["frequency_range_id"].ToString();
        }

        internal bool HasTest()
        {
            if (this.ParameterType == null) return false;
            if (this.ParameterType.Structure == null) return false;
            if (this.ParameterType.Structure.Cable == null) return false;
            if (this.ParameterType.Structure.Cable.Test == null) return false;
            return true;
        }

        public decimal BringMeasuredValue(decimal value)
        { 
            decimal brLength = getBringingLength();
            decimal tstLength = this.ParameterType.Structure.Cable.Test.TestedLength;
            if (brLength == tstLength || tstLength == 0) return value;
            return BringToLength(value, tstLength, brLength);
        }


        public decimal BringToLength(decimal value, decimal curLength, decimal brLength)
        {
            int round = 2;
            switch (this.ParameterType.Name)
            {
                case "Rж":
                case "Cр":
                case "Co":
                case "Rиз1":
                case "Rиз2":
                    value *= brLength / curLength;
                    round = value > 99 ? 1 : 2;
                    return Math.Round(value, round);
                case "al":
                    value *= brLength / curLength;
                    return Math.Round(value, 1);
                case "Ao":
                case "Az":
                    value += 10 * (decimal)Math.Log10(((double)curLength / (double)brLength));
                    return Math.Round(value, 1);
                default:
                    return value;
            }
        }
        private decimal getBringingLength()
        {
            int brLengthId = ServiceFunctions.convertToInt16(this.BringingLengthTypeId);
            switch (brLengthId)
            {
                case 1:
                    return this.ParameterType.Structure.Cable.BuildLength;
                case 2:
                    return 1000;
                case 3:
                    return this.BringingLength;
                default:
                    return this.ParameterType.Structure.Cable.Test.TestedLength;
            }
        }

        public void RefreshNotNormaResultsList()
        {
            this.NotNormalResults.Clear();
            foreach(TestResult tr in TestResults)
            {
                if (tr.DeviationPercent != 0 && !tr.IsAffected()) this.NotNormalResults.Add(tr);
            }
        }
    }
}
