using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SAKProtocolManager.DBEntities
{
    public class DRFormula : DBBase
    {
        public string Name, Description;

        public DRFormula()
        {
            setDefaultParameters();
        }
        public DRFormula(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
        }
        public DRFormula(DataRow row)
        {
            fillParametersFromRow(row);
            setDefaultParameters();
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["dr_formula_id"].ToString();
            this.Name = row["dr_formula_name"].ToString();
            this.Description = row["dr_formula_description"].ToString();
        }

        protected override void setDefaultParameters()
        {
            string selectQuery = "dr_formuls.DRInd AS dr_formula_id, dr_formuls.DRName AS dr_formula_name, dr_formuls.DROpis AS dr_formula_description";
            this.getAllQuery = String.Format("SELECT {0} FROM dr_formuls", selectQuery);
            this.getByIdQuery = String.Format("SELECT {0} FROM dr_formuls WHERE dr_formuls.DRInd = {1} LIMIT 1", selectQuery, Id);
            colsList = new string[] { "dr_formula_id", "dr_formula_name", "dr_formula_description" };
        }

        public DRFormula[] GetAll()
        {
            DRFormula[] mTypes = new DRFormula[] { };
            DataTable dt = GetAllFromDB();
            if (dt.Rows.Count > 0)
            {
                mTypes = new DRFormula[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++) mTypes[i] = new DRFormula(dt.Rows[i]);
            }
            return mTypes;
        }
    }
}
