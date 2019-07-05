using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormaMeasure.DBControl.Tables
{
    [DBTable("param_data", "bd_isp", OldDBName = "bd_isp", OldTableName = "param_data")]
    public class MeasuredParameterData : BaseEntity
    {
        public MeasuredParameterData(DataRowBuilder builder) : base(builder)
        {
        }

        /// <summary>
        /// Минимальное значение по умолчанию
        /// </summary>
        public const float MinValueDefault = -1000000000;
        /// <summary>
        /// Максимальное значение по умолчанию
        /// </summary>
        public const float MaxValueDefault = 1000000000;
        /// <summary>
        /// Длина приведения по умолчанию
        /// </summary>
        public const float LengthBringingDefault = 1000;
        /// <summary>
        /// Процент годности по умолчанию
        /// </summary>
        public const float PercentDefault = 100;

        /*
        public static MeasuredParameterData GetByParameters(CableStructureMeasuredParameterData cab_struct_data)
        {
            Random r = new Random();
            MeasuredParameterData mpd = build_with_data(cab_struct_data);
            //mpd.MeasureParameterDataId = (uint)r.Next();
            mpd.find_or_create();
            return mpd;
        }

        public static MeasuredParameterData build_with_data(CableStructureMeasuredParameterData cab_struct_data)
        {
            MeasuredParameterData data = build();
            data.ParameterTypeId = cab_struct_data.ParameterTypeId;

            data.MinValue =  MeasuredParameterType.IsHasMinLimit(data.ParameterTypeId) ? cab_struct_data.MinValue : MinValueDefault;
            data.MaxValue = MeasuredParameterType.IsHasMaxLimit(data.ParameterTypeId) ? cab_struct_data.MaxValue : MaxValueDefault;
            data.Percent = cab_struct_data.Percent;
            data.LngthBringingTypeId = cab_struct_data.LengthBringingTypeId;
            data.LengthBringing =  LengthBringingType.NoBringing == data.LngthBringingTypeId ? LengthBringingDefault :  cab_struct_data.LengthBringing;
            data.FrequencyRangeId = MeasuredParameterType.IsItFreqParameter(data.ParameterTypeId) ? FrequencyRange.get_by_frequencies(cab_struct_data.FrequencyMin, cab_struct_data.FrequencyStep, cab_struct_data.FrequencyMax).FrequencyRangeId : 1;
            data.find_or_create();
            return data;
        }

        protected void find_or_create()
        {
            DBEntityTable t = find_by_criteria(makeWhereQueryForAllColumns(), typeof(MeasuredParameterData));
            if (t.Rows.Count > 0)
            {
                this.MeasureParameterDataId = (t.Rows[0] as MeasuredParameterData).MeasureParameterDataId;
                //System.Windows.Forms.MessageBox.Show($"find_or_create() on measured_parameter_data {MeasureParameterDataId}");
            }
            else
            {
                this.Save();
            }
        }
        */

        public static DBEntityTable get_tested_structure_measured_parameters(uint structure_id)
        {
            DBEntityTable mdt = new DBEntityTable(typeof(MeasuredParameterData));
            DBEntityTable frt = new DBEntityTable(typeof(FrequencyRange));
            DBEntityTable pt = new DBEntityTable(typeof(MeasuredParameterType));
            DBEntityTable lbt = new DBEntityTable(typeof(LengthBringingType));
            DBEntityTable cs = new DBEntityTable(typeof(TestedCableStructure));
            string selectQuery = mdt.SelectQuery.Replace("*", $"*, {pt.TableName}.{MeasuredParameterType.ParameterMeasure_ColumnName} AS result_measure, {lbt.TableName}.Ed_izm AS length_bringing_measure");
            selectQuery = $"{selectQuery} LEFT OUTER JOIN {frt.TableName} ON param_data.FreqDiap = freq_diap.FreqDiapInd LEFT OUTER JOIN {pt.TableName} USING({pt.PrimaryKey[0].ColumnName}) LEFT OUTER JOIN {lbt.TableName} USING ({lbt.PrimaryKey[0].ColumnName}) WHERE {cs.PrimaryKey[0].ColumnName} = {structure_id}";
            return find_by_query(selectQuery, typeof(MeasuredParameterData));
        }

        public static MeasuredParameterData build()
        {
            DBEntityTable t = new DBEntityTable(typeof(MeasuredParameterData));
            MeasuredParameterData mpd = (MeasuredParameterData)t.NewRow();
            return mpd;
        }


        public static MeasuredParameterData find_by_id(uint id)
        {
            DBEntityTable t = find_by_primary_key(id,typeof(MeasuredParameterData));
            if (t.Rows.Count > 0) return (MeasuredParameterData)t.Rows[0];
            else
            {
                return null;
            }
        }



        #region Колонки таблицы
        [DBColumn(TestedCable.CableId_ColumnName, ColumnDomain.UInt, Order = 10, Nullable = true, DefaultValue = 0, ReferenceTo = "cables(" + CableTest.CableTestId_ColumnName + ") ON DELETE CASCADE")]
        public uint SourceCableId
        {
            get
            {
                return tryParseUInt(TestedCable.CableId_ColumnName);
            }
            set
            {
                this[TestedCable.CableId_ColumnName] = value;
            }
        }

        [DBColumn(TestedCableStructure.StructureId_ColumnName, ColumnDomain.UInt, Order = 11, Nullable = true, DefaultValue = 0)]
        public uint SourceStructureId
        {
            get
            {
                return tryParseUInt(TestedCableStructure.StructureId_ColumnName);
            }
            set
            {
                this[TestedCableStructure.StructureId_ColumnName] = value;
            }
        }


        [DBColumn(MeasuredParameterType.ParameterTypeId_ColumnName, ColumnDomain.UInt, Order = 12, ReferenceTo = "ism_param(ParamInd)")]
        public uint ParameterTypeId
        {
            get
            {
                return tryParseUInt(MeasuredParameterType.ParameterTypeId_ColumnName);
            }
            set
            {
                this[MeasuredParameterType.ParameterTypeId_ColumnName] = value;
            }
        }

        [DBColumn("FreqDiap", ColumnDomain.UInt, Order = 13, OldDBColumnName = "FreqDiapInd", DefaultValue = 1, Nullable = true, ReferenceTo = "freq_diap(FreqDiapInd)")]
        public uint FrequencyRangeId
        {
            get
            {
                return tryParseUInt("FreqDiap");
            }
            set
            {
                this["FreqDiap"] = value;
            }
        }

        [DBColumn(LengthBringingType.BringingId_ColumnName, ColumnDomain.UInt, Order = 14, Nullable = true, DefaultValue = 0, ReferenceTo = "lpriv_tip(LprivInd)")]
        public uint LngthBringingTypeId
        {
            get
            {
                return tryParseUInt(LengthBringingType.BringingId_ColumnName);
            }
            set
            {
                this[LengthBringingType.BringingId_ColumnName] = value;
            }
        }

        [DBColumn(LengthBringing_ColumnName, ColumnDomain.Float, Order = 15, Nullable = true, DefaultValue = LengthBringingDefault)]
        public float LengthBringing
        {
            get
            {
                return tryParseFloat(LengthBringing_ColumnName);
            }
            set
            {
                this[LengthBringing_ColumnName] = value;
            }
        }

        [DBColumn(MinValue_ColumnName, ColumnDomain.Float, Order = 16, Nullable = true, DefaultValue = MinValueDefault)]
        public float MinValue
        {
            get
            {
                return tryParseFloat(MinValue_ColumnName);
            }
            set
            {
                this[MinValue_ColumnName] = value;
            }
        }

        [DBColumn(MaxValue_ColumnName, ColumnDomain.Float, Order = 17, Nullable = true, DefaultValue = MaxValueDefault)]
        public float MaxValue
        {
            get
            {
                return tryParseFloat(MaxValue_ColumnName);
            }
            set
            {
                this[MaxValue_ColumnName] = value;
            }
        }

        [DBColumn(Percent_ColumnName, ColumnDomain.Float, Order = 18, Nullable = true, DefaultValue = PercentDefault)]
        public uint Percent
        {
            get
            {
                return tryParseUInt(Percent_ColumnName);
            }
            set
            {
                this[Percent_ColumnName] = value;
            }
        }

        #region Колонки типа измеряемого параметра (ParameterType)

        [DBColumn(MeasuredParameterType.ParameterName_ColumnName, ColumnDomain.Tinytext, Nullable = true, Order = 19, IsVirtual = true)]
        public string ParameterName
        {
            get
            {
                return this[MeasuredParameterType.ParameterName_ColumnName].ToString();
            }
            set
            {
                this[MeasuredParameterType.ParameterName_ColumnName] = value;
            }
        }

        [DBColumn(MeasuredParameterType.ParameterMeasure_ColumnName, ColumnDomain.Tinytext, Nullable = true, Order = 20, IsVirtual = true)]
        public string ParameterMeasure
        {
            get
            {
                return this[MeasuredParameterType.ParameterMeasure_ColumnName].ToString();
            }
            set
            {
                this[MeasuredParameterType.ParameterMeasure_ColumnName] = value;
            }
        }

        [DBColumn(MeasuredParameterType.ParameterDescription_ColumnName, ColumnDomain.Tinytext, Nullable = true, Order = 21, IsVirtual = true)]
        public string ParameterDescription
        {
            get
            {
                return this[MeasuredParameterType.ParameterDescription_ColumnName].ToString();
            }
            set
            {
                this[MeasuredParameterType.ParameterDescription_ColumnName] = value;
            }
        }

        #endregion

        #region Парметры частоты 

        [DBColumn(FrequencyRange.FreqMin_ColumnName, ColumnDomain.Float, Order = 22, Nullable = true, IsVirtual = true)]
        public float FrequencyMin
        {
            get
            {
                return tryParseFloat(FrequencyRange.FreqMin_ColumnName);
            }
            set
            {
                this[FrequencyRange.FreqMin_ColumnName] = value;
            }
        }

        [DBColumn(FrequencyRange.FreqMax_ColumnName, ColumnDomain.Float, Order = 23, Nullable = true, IsVirtual = true)]
        public float FrequencyMax
        {
            get
            {
                return tryParseFloat(FrequencyRange.FreqMax_ColumnName);
            }
            set
            {
                this[FrequencyRange.FreqMax_ColumnName] = value;
            }
        }

        [DBColumn(FrequencyRange.FreqStep_ColumnName, ColumnDomain.Float, Order = 24, Nullable = true, IsVirtual = true)]
        public float FrequencyStep
        {
            get
            {
                return tryParseFloat(FrequencyRange.FreqStep_ColumnName);
            }
            set
            {
                this[FrequencyRange.FreqStep_ColumnName] = value;
            }
        }


        #endregion

        #region Тип приведения результата 


        [DBColumn("length_bringing_measure", ColumnDomain.Tinytext, Order = 25, Nullable = true, IsVirtual = true)]
        public string MeasureLengthTitle
        {
            get
            {
                return this["length_bringing_measure"].ToString();
            }
            set
            {
                this["length_bringing_measure"] = value;
            }
        }

        [DBColumn(LengthBringingType.BringingName_ColumnName, ColumnDomain.Tinytext, Order = 26, Nullable = true, IsVirtual = true)]
        public string LengthBringingName
        {
            get
            {
                return this[LengthBringingType.BringingName_ColumnName].ToString();
            }
            set
            {
                this[LengthBringingType.BringingName_ColumnName] = value;
            }
        }

        #endregion

        [DBColumn("result_measure", ColumnDomain.Tinytext, Order = 27, Nullable = true, IsVirtual = true)]
        public string ResultMeasure
        {
            get
            {
                return this["result_measure"].ToString();
            }
            set
            {
                this["result_measure"] = value;
            }
        }




        public const string LengthBringing_ColumnName = "Lpriv";
        public const string MinValue_ColumnName = "Min";
        public const string MaxValue_ColumnName = "Max";
        public const string Percent_ColumnName = "Percent";
        #endregion


        public bool HasMaxLimit => MeasuredParameterType.IsHasMaxLimit(ParameterTypeId);
        public bool HasMinLimit => MeasuredParameterType.IsHasMinLimit(ParameterTypeId);

        public bool IsFreqParameter => MeasuredParameterType.IsItFreqParameter(ParameterTypeId);

        public TestedCableStructure TestedStructure;
        private DBEntityTable testResults;

        private void AssignResult(CableTestResult r)
        {
            r.ParameterData = this;
        }

        public DBEntityTable TestResults
        {
            get
            {
                if (testResults == null)
                {
                    DataRow[] rows = TestedStructure.GetTestResultsByParameterTypeId(ParameterTypeId).RowsAsArray();
                    TestResults = new DBEntityTable(typeof(CableTestResult), rows);
                }
                return testResults;
            }set
            {
                testResults = value;
                if (testResults.Rows.Count > 0)
                {
                    foreach (CableTestResult r in testResults.Rows) AssignResult(r);
                }
            }
        }
        public string GetFreqRangeTitle()
        {
            string r = String.Empty;
            if (this.FrequencyMin > 0) r = this.FrequencyMin.ToString();
            if (this.FrequencyMax > 0 && this.FrequencyMin > 0) r += "-";
            if (this.FrequencyMax > 0) r += this.FrequencyMax.ToString();
            if (!String.IsNullOrWhiteSpace(r)) r += "кГц";
            return r;
        }

        public string ResultMeasure_WithLength
        {
            get
            {
                string measure = String.Empty;
                string brMeasure = LngthBringingTypeId == LengthBringingType.ForAnotherLengthInMeters ? String.Format("/{0}м", this.LengthBringing) : this.MeasureLengthTitle;
                if (this.ParameterTypeId == MeasuredParameterType.dR)
                {
                    measure = (TestedStructure.DRBringingFormulaId > 2) ? TestedStructure.DRFormula.ResultMeasure : String.Format(" {0}{1}", TestedStructure.DRFormula.ResultMeasure, brMeasure);
                }
                else
                {
                    measure = String.Format(" {0}{1}", this.ResultMeasure, brMeasure);
                }
                return measure;
            }
        }

        public decimal BringMeasuredValue(decimal value)
        {
            if (this.ParameterTypeId == MeasuredParameterType.Risol2 || this.ParameterTypeId == MeasuredParameterType.Risol4) return value;
            decimal brLength = getBringingLength();
            decimal tstLength = (decimal)this.TestedStructure.TestedCable.Test.CableLength;
            value = bringToCoeffs(value);
            if (brLength == tstLength || tstLength == 0) return value;
            return BringToLength(value, tstLength, brLength);
        }

        public decimal BringToLength(decimal value, decimal curLength, decimal brLength)
        {
            switch (this.ParameterTypeId)
            {
                case MeasuredParameterType.Rleads:
                case MeasuredParameterType.Cp:
                case MeasuredParameterType.Co:
                case MeasuredParameterType.Ea:
                case MeasuredParameterType.K1:
                case MeasuredParameterType.K2:
                case MeasuredParameterType.K3:
                case MeasuredParameterType.K9:
                case MeasuredParameterType.K10:
                case MeasuredParameterType.K11:
                case MeasuredParameterType.K12:
                case MeasuredParameterType.K23:
                case MeasuredParameterType.K9_12:
                    value *= brLength / curLength;
                    break;
                case MeasuredParameterType.Risol1:
                case MeasuredParameterType.Risol3:
                    value *= curLength / brLength;
                    break;
                case MeasuredParameterType.al:
                    value *= brLength / curLength;
                    break;
                case MeasuredParameterType.Ao:
                case MeasuredParameterType.Az:
                    value += 10 * (decimal)Math.Log10(((double)curLength / (double)brLength));
                    break;
            }
            return value;
        }




        private decimal bringToCoeffs(decimal value)
        {
            decimal temperature = (decimal)this.TestedStructure.TestedCable.Test.Temperature;
            switch (this.ParameterTypeId)
            {
                case MeasuredParameterType.Rleads:
                    value *= (1 / (1 + (decimal)this.TestedStructure.LeadMaterial.MaterialTKC * (temperature - 20)));
                    return Math.Round(value, 1);
                case MeasuredParameterType.Risol1:
                case MeasuredParameterType.Risol3:
                    value *= (decimal)this.TestedStructure.IsolMaterial.GetCoeffByTemperature((float)temperature);
                    return Math.Round(value, 1);
                default:
                    return value;
            }

        }

        private decimal getBringingLength()
        {
            switch (LngthBringingTypeId)
            {
                case LengthBringingType.ForBuildLength:
                    return (decimal)this.TestedStructure.OwnCable.BuildLength;
                case LengthBringingType.ForOneKilometer:
                    return 1000;
                case LengthBringingType.ForAnotherLengthInMeters:
                    return (decimal)this.LengthBringing;
                default:
                    return (decimal)this.TestedStructure.TestedCable.Test.CableLength;
            }
        }


        public string GetNormaTitle()
        {
            string norma = String.Empty;
            string rMeasure = ResultMeasure_WithLength;
            if (HasMinLimit) norma += String.Format(" от {0}{1}", MinValue, rMeasure);
            if (HasMaxLimit) norma += String.Format(" до {0}{1}", MaxValue, rMeasure);
            norma += String.Format(" {0}%", Percent);
            return norma;
        }

        public string GetTitle()
        {
            string fRangeTitle, norma;
            fRangeTitle = GetFreqRangeTitle();
            norma = GetNormaTitle();
            return fRangeTitle + norma;
        }
    }
}
