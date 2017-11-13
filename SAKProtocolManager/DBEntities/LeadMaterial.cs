using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKProtocolManager.DBEntities
{
    public class LeadMaterial : DBBase
    {
        public string Name = String.Empty;

        public LeadMaterial()
        {
            setDefaultParameters();
        }

        public LeadMaterial(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
        }

        public LeadMaterial(DataRow row)
        {
            fillParametersFromRow(row);
            setDefaultParameters();
        }

        protected override void setDefaultParameters()
        {
            string selQuery = "materialy_gil.MaterInd AS lead_material_id, materialy_gil.MaterName AS lead_material_name";
            this.getAllQuery = String.Format("SELECT {0} FROM materialy_gil", selQuery);
            this.getByIdQuery = String.Format("{0} WHERE materialy_gil.MaterInd = {1}", this.getAllQuery, this.Id);
            this.colsList = new string[] 
            {
                "lead_material_id",
                "lead_material_name"
            };
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["lead_material_id"].ToString();
            this.Name = row["lead_material_name"].ToString();
        }



    }
}
