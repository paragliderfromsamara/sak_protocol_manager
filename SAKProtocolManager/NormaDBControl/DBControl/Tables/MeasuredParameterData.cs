﻿using System;
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

        [DBColumn(FrequencyRange.FreqRangeId_ColumnName, ColumnDomain.UInt, Order = 13, OldDBColumnName = "FreqDiapInd", DefaultValue = 1, ReferenceTo = "freq_diap(FreqDiapInd)")]
        public uint FrequencyRangeId
        {
            get
            {
                return tryParseUInt(FrequencyRange.FreqRangeId_ColumnName);
            }
            set
            {
                this[FrequencyRange.FreqRangeId_ColumnName] = value;
            }
        }

        [DBColumn(LengthBringingType.BringingId_ColumnName, ColumnDomain.UInt, Order = 14, DefaultValue = 0, ReferenceTo = "lpriv_tip(LprivInd)")]
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

        [DBColumn(LengthBringing_ColumnName, ColumnDomain.Float, Order = 15, DefaultValue = LengthBringingDefault)]
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

        [DBColumn(MinValue_ColumnName, ColumnDomain.Float, Order = 16, DefaultValue = MinValueDefault)]
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

        [DBColumn(MaxValue_ColumnName, ColumnDomain.Float, Order = 17, DefaultValue = MaxValueDefault)]
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

        [DBColumn(Percent_ColumnName, ColumnDomain.Float, Order = 18, DefaultValue = PercentDefault)]
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


        public const string LengthBringing_ColumnName = "Lpriv";
        public const string MinValue_ColumnName = "Min";
        public const string MaxValue_ColumnName = "Max";
        public const string Percent_ColumnName = "Percent";
        #endregion

    }
}
