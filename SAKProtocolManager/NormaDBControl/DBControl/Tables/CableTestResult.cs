using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormaMeasure.DBControl.Tables
{
    [DBTable("resultism", "bd_isp", OldDBName = "bd_isp", OldTableName = "resultism")]
    public class CableTestResult : BaseEntity
    {
        public CableTestResult(DataRowBuilder builder) : base(builder)
        {
        }

        public static DBEntityTable find_by_structure_id_and_parameter_type_id(uint structure_id, uint parameter_type_id)
        {
            DBEntityTable t = new DBEntityTable(typeof(CableTestResult));
            string q = t.SelectQuery;
            q = $"{q} WHERE {CableStructure.StructureId_ColumnName} = {structure_id} AND {MeasuredParameterType.ParameterTypeId_ColumnName} = {parameter_type_id}";
            t.FillByQuery(q);
            return t;
        }

        public static void delete_all_from_cable_test(uint cable_test_id)
        {
            delete_by_criteria($"{CableTest.CableTestId_ColumnName} = {cable_test_id}", typeof(CableTestResult));
        }


        #region Колонки таблицы
        [DBColumn(CableTest.CableTestId_ColumnName, ColumnDomain.UInt, Order = 10, OldDBColumnName = "IspInd", ReferenceTo = "ispytan(IspInd) ON DELETE CASCADE", Nullable = true)]
        public uint TestId
        {
            get
            {
                return tryParseUInt(CableTest.CableTestId_ColumnName);
            }
            set
            {
                this[CableTest.CableTestId_ColumnName] = value;
            }
        }

        [DBColumn(MeasuredParameterType.ParameterTypeId_ColumnName, ColumnDomain.UInt, Order = 11, OldDBColumnName = "ParamInd", Nullable = true)]
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

        [DBColumn(CableStructure.StructureId_ColumnName, ColumnDomain.UInt, Order = 12, OldDBColumnName = "StruktInd", Nullable = true)]
        public uint CableStructureId
        {
            get
            {
                return tryParseUInt(CableStructure.StructureId_ColumnName);
            }
            set
            {
                this[CableStructure.StructureId_ColumnName] = value;
            }
        }

        [DBColumn(StructElementNumber_ColumnName, ColumnDomain.UInt, Order = 13, OldDBColumnName = "StruktElNum", Nullable = true)]
        public uint ElementNumber
        {
            get
            {
                return tryParseUInt(StructElementNumber_ColumnName);
            }
            set
            {
                this[StructElementNumber_ColumnName] = value;
            }
        }

        [DBColumn(MeasureOnElementNumber_ColumnName, ColumnDomain.UInt, Order = 14, OldDBColumnName = "IsmerNum", Nullable = true)]
        public uint MeasureNumber
        {
            get
            {
                return tryParseUInt(MeasureOnElementNumber_ColumnName);
            }
            set
            {
                this[MeasureOnElementNumber_ColumnName] = value;
            }
        }

        [DBColumn(MeasureResult_ColumnName, ColumnDomain.Float, Order = 15, OldDBColumnName = "Resultat", Nullable = true)]
        public float Result
        {
            get
            {
                return tryParseFloat(MeasureResult_ColumnName);
            }
            set
            {
                this[MeasureResult_ColumnName] = value;
            }
        }

        [DBColumn(CableTest.Temperature_ColumnName, ColumnDomain.Float, Order = 16, OldDBColumnName = "Temperatur", Nullable = true)]
        public float Temperature
        {
            get
            {
                return tryParseFloat(CableTest.Temperature_ColumnName);
            }
            set
            {
                this[CableTest.Temperature_ColumnName] = value;
            }
        }

        [DBColumn("FreqDiap", ColumnDomain.UInt, Order = 17, OldDBColumnName = "FreqDiap", Nullable = true)]
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

        [DBColumn(ElementNumberOnGenerator_ColumnName, ColumnDomain.UInt, Order = 18, OldDBColumnName = "StruktElNum_gen", Nullable = true)]
        public uint GeneratorElementNumber
        {
            get
            {
                return tryParseUInt(ElementNumberOnGenerator_ColumnName);
            }
            set
            {
                this[ElementNumberOnGenerator_ColumnName] = value;
            }
        }

        [DBColumn(PairNumberOnGenerator_ColumnName, ColumnDomain.UInt, Order = 19, OldDBColumnName = "ParaNum_gen", Nullable = true)]
        public uint GeneratorPairNumber
        {
            get
            {
                return tryParseUInt(PairNumberOnGenerator_ColumnName);
            }
            set
            {
                this[PairNumberOnGenerator_ColumnName] = value;
            }
        }

        [DBColumn(BringingResult_ColumnName, ColumnDomain.UInt, Order = 20, IsVirtual = true, Nullable = true, DefaultValue = float.MinValue)]
        public float BringingResult
        {
            get
            {
                return tryParseFloat(BringingResult_ColumnName);
            }
            set
            {
                int round = 2;
                double brVal = (double)value;
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
                    case MeasuredParameterType.Risol1:
                    case MeasuredParameterType.Risol3:
                        round = value > 99 ? 1 : 2;
                        brVal = Math.Round(value, round);
                        break;
                    case MeasuredParameterType.al:
                    case MeasuredParameterType.Ao:
                    case MeasuredParameterType.Az:
                        brVal = Math.Round(value, 1);
                        break;
                    default:
                        brVal = value;
                        break;
                }
                this[BringingResult_ColumnName] = (float)brVal;
            }
        }

        [DBColumn(IsAffected_ColumnName, ColumnDomain.Boolean, Order = 21, IsVirtual = true, Nullable = true, DefaultValue = false)]
        public bool IsAffected
        {
            get
            {
                return tryParseBoolean(IsAffected_ColumnName, false);
            }
            set
            {
                this[IsAffected_ColumnName] = value;
            }
        }

        [DBColumn(IsOutOfNorma_ColumnName, ColumnDomain.Boolean, Order = 22, IsVirtual = true, Nullable = true, DefaultValue = false)]
        public bool IsOutOfNorma
        {
            get
            {
                return tryParseBoolean(IsOutOfNorma_ColumnName, false);
            }
            set
            {
                this[IsOutOfNorma_ColumnName] = value;
            }
        }

        [DBColumn(ResultForView_ColumnName, ColumnDomain.Tinytext, Order = 23, IsVirtual = true, Nullable = true, DefaultValue = "0")]
        public string ResultForView
        {
            get
            {
                return this[ResultForView_ColumnName].ToString();
            }
            set
            {
                this[ResultForView_ColumnName] = value;
            }
        }




        [DBColumn(MinAllowedValue_ColumnName, ColumnDomain.Float, Order = 24, IsVirtual = true, Nullable = true, DefaultValue = 0)]
        public float MinAllowed
        {
            get
            {
                return tryParseFloat(MinAllowedValue_ColumnName);
            }
            set
            {
                this[MinAllowedValue_ColumnName] = value;
            }
        }

        [DBColumn(MaxAllowedValue_ColumnName, ColumnDomain.Float, Order = 25, IsVirtual = true, Nullable = true, DefaultValue = 0)]
        public float MaxAllowed
        {
            get
            {
                return tryParseFloat(MaxAllowedValue_ColumnName);
            }
            set
            {
                this[MaxAllowedValue_ColumnName] = value;
            }
        }






        public uint ReceiverElementNumber
        {
            get
            {
                return ElementNumber;
            }
            set
            {
                ElementNumber = value;
            }
        }

        public uint SubElementNumber
        {
            get
            {
                return MeasureNumber;
            }
            set
            {
                MeasureNumber = value;
            }
        }




        public const string StructElementNumber_ColumnName = "StruktElNum";
        public const string MeasureOnElementNumber_ColumnName = "IsmerNum";
        public const string MeasureResult_ColumnName = "Resultat";
        public const string ElementNumberOnGenerator_ColumnName = "StruktElNum_gen";
        public const string PairNumberOnGenerator_ColumnName = "ParaNum_gen";

        public const string BringingResult_ColumnName = "bringing_result";
        public const string IsOutOfNorma_ColumnName = "is_out_of_norma";
        public const string IsAffected_ColumnName = "is_affected";
        public const string ResultForView_ColumnName = "result_for_view";
        public const string MaxAllowedValue_ColumnName = "max";
        public const string MinAllowedValue_ColumnName = "min";

        #endregion



        public string IniFileKey
        {
            get
            {
                string keyName = $"rslt_{this.ElementNumber}_{this.MeasureNumber}";
                if (MeasuredParameterType.IsItFreqParameter(ParameterTypeId)) keyName += $"_{GeneratorElementNumber}_{GeneratorPairNumber}";
                return keyName;
            }
        }

        public string IniFileSection
        {
            get
            {
                string sectName = $"results_{this.ParameterTypeId}_{this.CableStructureId}";
                if (MeasuredParameterType.IsItFreqParameter(ParameterTypeId)) sectName += $"_{FrequencyRangeId}";
                return sectName;
            }
        }


        public TestedCableStructure TestedCableStructure
        {
            get
            {
                if (cableStructure == null)
                {
                    cableStructure = TestedCableStructure.find_by_structure_id(CableStructureId);
                }
                return cableStructure;
            }
            set
            {
                cableStructure = value;
                CableStructureId = cableStructure.CableStructureId;
            }
        }

        public MeasuredParameterType ParameterType
        {
            get
            {
                if(parameterType== null)
                {
                    parameterType = MeasuredParameterType.find_by_id(ParameterTypeId);
                }
                return parameterType;
            }set
            {
                parameterType = value;
                ParameterTypeId = parameterType.ParameterTypeId;
            }
        }

        public FrequencyRange FrequencyRange
        {
            get
            {
                if (frequencyRange == null)
                {
                    frequencyRange = FrequencyRange.find_by_id(FrequencyRangeId);
                }
                return frequencyRange;
            }
            set
            {
                frequencyRange = value;
                FrequencyRangeId = frequencyRange.FrequencyRangeId;
            }
        }

        private MeasuredParameterData parameterData;
        public MeasuredParameterData ParameterData
        {
            set
            {
                parameterData = value;
                CheckIsAffected();
                float brResult = (float)parameterData.BringMeasuredValue((decimal)Result);
                BringingResult = brResult;
                ResultForView = MakeResultForView();
                CheckIsOutOfNorm();


            }
        }

        private void CheckIsOutOfNorm()
        {
            if (parameterData == null) return;
            IsOutOfNorma = false;
            float rslt = Tables.MeasuredParameterType.IsEKParameter(ParameterTypeId) ? Math.Abs(BringingResult) : BringingResult;
            if (parameterData.HasMaxLimit)
            {
                IsOutOfNorma |= parameterData.MaxValue < rslt;
                MaxAllowed = parameterData.MaxValue;
            }
            if (parameterData.HasMinLimit)
            {
                IsOutOfNorma |= parameterData.MinValue > rslt;
                MinAllowed = parameterData.MinValue;
            }
        }

        private LeadTestStatus leadTestStatus;
        public LeadTestStatus LeadTestStatus => leadTestStatus;
        private void CheckIsAffected()
        {
            if (parameterData == null) return;
            leadTestStatus = parameterData.TestedStructure.GetElementStatus(ElementNumber); 
            if (leadTestStatus.StatusId == LeadTestStatus.Ok && parameterData.IsFreqParameter) leadTestStatus = parameterData.TestedStructure.GetElementStatus(GeneratorElementNumber);
            IsAffected = leadTestStatus.StatusId != LeadTestStatus.Ok;
        }

        public string MakeResultForView()
        {
            if (IsAffected) return LeadTestStatus.StatusTitle;//Status;
            else
            {
                int pow = 0;
                float tmpVal = BringingResult;
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
                    return $"{BringingResult}";
                }
            }
        }

        private TestedCableStructure cableStructure;
        private MeasuredParameterType parameterType;
        private FrequencyRange frequencyRange;

    }
}
