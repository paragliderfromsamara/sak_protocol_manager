using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormaMeasure.DBControl.Tables
{
    [DBTable("materialy_gil", "bd_isp", OldDBName = "bd_cable", OldTableName = "materialy_gil")]
    public class LeadMaterial : BaseEntity
    {
        public LeadMaterial(DataRowBuilder builder) : base(builder)
        {
        }

        public static DBEntityTable get_all_as_table()
        {
            return get_all(typeof(LeadMaterial));
        }

        public static LeadMaterial get_by_id(uint material_id)
        {
            DBEntityTable t = find_by_primary_key(material_id, typeof(LeadMaterial));
            if (t.Rows.Count == 0) return null;
            else return (LeadMaterial)t.Rows[0];
        }

        #region Колонки таблицы 
        [DBColumn(MaterialId_ColumnName, ColumnDomain.UInt, Order = 11, OldDBColumnName = "MaterInd", Nullable = false, IsPrimaryKey = true, AutoIncrement = true)]
        public uint MaterialId
        {
            get
            {
                return tryParseUInt(MaterialId_ColumnName);
            }
            set
            {
                this[MaterialId_ColumnName] = value;
            }
        }

        [DBColumn(MaterialName_ColumnName, ColumnDomain.Tinytext, Order = 12, OldDBColumnName = "MaterName", Nullable = true)]
        public string MaterialName
        {
            get
            {
                return this[MaterialName_ColumnName].ToString();
            }
            set
            {
                this[MaterialName_ColumnName] = value;
            }
        }

        [DBColumn(MaterialTKC_ColumnName, ColumnDomain.Float, Order = 13, OldDBColumnName = "TKC_1", Nullable = true)]
        public float MaterialTKC
        {
            get
            {
                return tryParseFloat(MaterialTKC_ColumnName);
            }
            set
            {
                this[MaterialTKC_ColumnName] = value;
            }
        }

        public const string MaterialId_ColumnName = "MaterInd";
        public const string MaterialName_ColumnName = "MaterName";
        public const string MaterialTKC_ColumnName = "TKC_1";

        #endregion
    }
}
