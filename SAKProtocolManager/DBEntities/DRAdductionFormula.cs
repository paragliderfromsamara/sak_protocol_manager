using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKProtocolManager.DBEntities
{
    public class DRAdductionFormula : DBBase
    {
        public string Name = String.Empty; 
        public string Description = String.Empty;

        public DRAdductionFormula()
        {
            setDefaultParameters();
        }
        public DRAdductionFormula(string id)
        {
            this.Id = id;
            setDefaultParameters();
        }

        /// <summary>
        /// Формулы приведения оммической ассиметрии
        /// </summary>
        /// <param name="row"></param>
        public DRAdductionFormula(DataRow row)
        {
            fillParametersFromRow(row);
            setDefaultParameters();
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["dr_adduction_formula_id"].ToString();
            this.Name = row["dr_adduction_formula_name"].ToString();
            this.Description = row["dr_adduction_formula_description"].ToString();
        }

        protected override void setDefaultParameters()
        {
            string selectQuery = "dr_priv_formuls.DRPrivInd AS dr_adduction_formula_id, dr_priv_formuls.DRPrivName AS dr_adduction_formula_name, dr_priv_formuls.DRPrivOpis AS dr_adduction_formula_description";
            this.getAllQuery = String.Format("SELECT {0} FROM dr_priv_formuls", selectQuery);
            this.getByIdQuery = String.Format("SELECT {0} FROM dr_priv_formuls WHERE dr_priv_formuls.DRPrivInd = {1}  LIMIT 1", selectQuery, Id);
            colsList = new string[] { "dr_adduction_formula_id", "dr_adduction_formula_name", "dr_adduction_formula_desсription" };
        }

        /// <summary>
        ///  Формулы приведения оммической ассиметрии
        /// </summary>
        /// <returns></returns>
        public DRAdductionFormula[] GetAll()
        {
            DRAdductionFormula[] mTypes = new DRAdductionFormula[] { };
            DataTable dt = GetAllFromDB();
            if (dt.Rows.Count > 0)
            {
                mTypes = new DRAdductionFormula[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++) mTypes[i] = new DRAdductionFormula(dt.Rows[i]);
            }
            return mTypes;
        }

    }
}
