using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormaMeasure.DBControl.Tables
{
    [DBTable("dr_priv_formuls", "bd_isp", OldDBName = "bd_cable", OldTableName = "dr_priv_formuls")]
    public class dRBringingFormula : BaseEntity
    {
        public dRBringingFormula(DataRowBuilder builder) : base(builder)
        {
        }

        public static DBEntityTable get_all_as_table()
        {
            return get_all(typeof(dRBringingFormula));
        }

        #region Колонки таблицы 
        [DBColumn(FormulaId_ColumnName, ColumnDomain.UInt, Order = 10, OldDBColumnName = "DRPrivInd", Nullable = true, IsPrimaryKey = true)]
        public uint FormulaId
        {
            get
            {
                return tryParseUInt(FormulaId_ColumnName);
            }
            set
            {
                this[FormulaId_ColumnName] = value;
            }
        }


        [DBColumn(FormulaName_ColumnName, ColumnDomain.Tinytext, Order = 11, OldDBColumnName = "DRPrivName", Nullable = true)]
        public string FormulaName
        {
            get
            {
                return this[FormulaName_ColumnName].ToString();
            }
            set
            {
                this[FormulaName_ColumnName] = value;
            }
        }

        [DBColumn(FormulaDescription_ColumnName, ColumnDomain.Tinytext, Order = 12, OldDBColumnName = "DRPrivOpis", Nullable = true)]
        public string Formula
        {
            get
            {
                return this[FormulaDescription_ColumnName].ToString();
            }
            set
            {
                this[FormulaDescription_ColumnName] = value;
            }
        }

        public const string FormulaId_ColumnName = "DRPrivInd";
        public const string FormulaName_ColumnName = "DRPrivName";
        public const string FormulaDescription_ColumnName = "DRPrivOpis";

        #endregion
    }
}
