using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKProtocolManager.DBEntities
{
    public class Baraban : DBBase
    {
        public string Name = String.Empty;
        public string Number = String.Empty;
        public decimal Weight = 0;

        public Baraban(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
        }
        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["baraban_id"].ToString();
            this.Name = row["baraban_name"].ToString();
            this.Number = row["baraban_number"].ToString();
            this.Weight = ServiceFunctions.convertToDecimal(row["baraban_weight"]);
        }
        protected override void setDefaultParameters()
        {
            string selectQuery = "barabany.BarabanInd AS baraban_id, barabany.BarabanNum AS baraban_number, tipy_baraban.TipName AS baraban_name, tipy_baraban.Massa AS baraban_weight";
            this.getByIdQuery = String.Format("SELECT {0} FROM barabany LEFT JOIN tipy_baraban USING(TipInd) WHERE barabany.BarabanInd = {1}", selectQuery, this.Id);
            this.getAllQuery = String.Format("SELECT {0} FROM barabany LEFT JOIN tipy_baraban USING(TipInd)", selectQuery);
            this.colsList = new string[] 
            {
                "baraban_id",
                "baraban_number",
                "baraban_name",
                "baraban_weight"
            };
        }
    }
}
