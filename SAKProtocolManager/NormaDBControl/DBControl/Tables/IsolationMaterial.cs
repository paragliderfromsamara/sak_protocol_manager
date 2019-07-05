using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormaMeasure.DBControl.Tables
{


    [DBTable("materialy_izol", "bd_isp", OldDBName = "bd_cable", OldTableName = "materialy_izol")]
    public class IsolationMaterial : BaseEntity
    {
        public IsolationMaterial(DataRowBuilder builder) : base(builder)
        {
        }

        public static DBEntityTable get_all_as_table()
        {
            return get_all(typeof(IsolationMaterial));
        }

        public static IsolationMaterial get_by_id(uint material_id)
        {
            DBEntityTable t = find_by_primary_key(material_id, typeof(IsolationMaterial));
            if (t.Rows.Count == 0) return null;
            else return (IsolationMaterial)t.Rows[0];
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

        public const string MaterialId_ColumnName = "MaterInd";
        public const string MaterialName_ColumnName = "MaterName";

        #endregion

        public float GetCoeffByTemperature(float temperature)
        {
            float coeff = 1;
            DataRow[] coeffRow = MaterialCoeffs.Select($"{IsolMaterialCoeffs.Temperature_ColumnName} = {temperature}");
            if (coeffRow.Length > 0) coeff = ((IsolMaterialCoeffs)coeffRow[0]).Temperature;
            return coeff;

        }
        private DBEntityTable materialCoeffs;
        public DBEntityTable MaterialCoeffs
        {
            get
            {
                if (materialCoeffs == null)
                {
                    materialCoeffs = IsolMaterialCoeffs.get_all_for_material(MaterialId);
                }
                return materialCoeffs;
            }
        }
    }
}
