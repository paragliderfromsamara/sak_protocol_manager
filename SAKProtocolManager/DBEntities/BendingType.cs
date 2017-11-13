using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKProtocolManager.DBEntities
{
    /// <summary>
    /// Тип повива
    /// </summary>
    public class BendingType : DBBase
    {
        public string Name;
        public int NumberOfLeads;

        public BendingType()
        {
            setDefaultParameters();
        }
        public BendingType(string id)
        {
            this.Id = id;
            setDefaultParameters();
        }

        public BendingType(DataRow dataRow)
        {
            setDefaultParameters();
            fillParametersFromRow(dataRow);
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["bending_type_id"].ToString();
            this.Name = row["bending_type_name"].ToString();
            this.NumberOfLeads = Convert.ToInt32(row["bending_type_leads_number"].ToString());
        }

        protected override void setDefaultParameters()
        {
            string selectQuery = "tipy_poviv.StruktNum AS bending_type_id, tipy_poviv.StruktTip AS bending_type_name, tipy_poviv.ColvoGil AS bending_type_leads_number";
            this.getAllQuery = String.Format("SELECT {0} FROM tipy_poviv", selectQuery);
            this.getByIdQuery = String.Format("SELECT {0} FROM tipy_poviv WHERE tipy_poviv.StruktNum = {1} LIMIT 1", selectQuery, Id);
            this.colsList = new string[] { "bending_type_id", "bending_type_name", "bending_type_leads_number" };
        }

        /// <summary>
        /// Вытаскиваем все типы повива
        /// </summary>
        /// <returns></returns>
        /// 

        public BendingType[] GetAll()
        {
            BendingType[] mTypes = new BendingType[] { };
            DataTable dt = GetAllFromDB();
            if (dt.Rows.Count > 0)
            {
                mTypes = new BendingType[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++) mTypes[i] = new BendingType(dt.Rows[i]);
            }
            return mTypes;
        }

    }

}
