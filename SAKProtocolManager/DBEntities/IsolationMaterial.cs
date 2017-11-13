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


        public IsolationMaterial(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
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
    }
}
