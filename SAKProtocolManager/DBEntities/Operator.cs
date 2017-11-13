using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKProtocolManager.DBEntities
{
    public class Operator : DBBase
    {
        public string FirstName = String.Empty;
        public string LastName = String.Empty;
        public string ThirdName = String.Empty;
        public string RoleName = String.Empty;

        public Operator(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
        }

        protected override void setDefaultParameters()
        {
            string selectQuery = "familija_imja_ot.UserNum AS operator_id, familija_imja_ot.Imja AS first_name, familija_imja_ot.Familija AS last_name, familija_imja_ot.Otchestvo AS third_name, dolshnosti.Dolshnost AS role_name";
            this.getByIdQuery = String.Format("SELECT {0} FROM familija_imja_ot LEFT JOIN dolshnosti ON familija_imja_ot.dolshnost = dolshnosti.DolshNum WHERE familija_imja_ot.UserNum = {1} limit 1", selectQuery, this.Id);
            this.getAllQuery = String.Format("SELECT {0} FROM familija_imja_ot LEFT JOIN dolshnosti ON familija_imja_ot.dolshnost = dolshnosti.DolshNum ", selectQuery);
            this.colsList = new string[]
            {
                "operator_id",
                "first_name",
                "last_name",
                "third_name",
                "role_name"
            };
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["operator_id"].ToString();
            this.FirstName = row["first_name"].ToString();
            this.LastName = row["last_name"].ToString();
            this.ThirdName = row["third_name"].ToString();
            this.RoleName = row["role_name"].ToString();
        }
    }
}
