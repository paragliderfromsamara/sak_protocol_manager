using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKProtocolManager.DBEntities
{
    public class IsolationMaterial : DBBase
    {

        public string Name = String.Empty;
        private List<decimal> coeffs = new List<decimal>();
        private List<decimal> temperatures = new List<decimal>();


        public decimal GetCoefficient(decimal temperature)
        {
            int idx = temperatures.IndexOf(temperature);
            return (idx < 0) ? 1 : coeffs[idx];
        }

        public IsolationMaterial(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
            FillCoeffs();
        }

        public IsolationMaterial(DataRow row)
        {
            fillParametersFromRow(row);
            setDefaultParameters();
        }

        protected override void setDefaultParameters()
        {
            string selQuery = "materialy_izol.MaterInd AS isolation_material_id, materialy_izol.MaterName";
            this.getAllQuery = String.Format("SELECT {0} FROM materialy_izol", selQuery);
            this.getByIdQuery = String.Format("{0} WHERE materialy_izol.MaterInd = {1}", this.getAllQuery, this.Id);
            this.colsList = new string[]
            {
                "isolation_material_id",
                "isolation_material_name"
            };

        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["isolation_material_id"].ToString();
            this.Name = row["isolation_material_name"].ToString();
        }

        private void FillCoeffs()
        {
            string query = "SELECT TKC, Temperatur FROM tkc_izol WHERE MaterInd = {0}";
            query = String.Format(query, this.Id);
            DataTable dt = getFromDB(query);
            if (dt.Rows.Count > 0)
            {
                this.coeffs.Clear();
                this.temperatures.Clear();
                for(int i = 0; i<dt.Rows.Count; i++)
                {
                    this.coeffs.Add(ServiceFunctions.convertToDecimal(dt.Rows[i]["TKC"]));
                    this.temperatures.Add(ServiceFunctions.convertToDecimal(dt.Rows[i]["Temperatur"]));
                }
            }
        }
    }
}
