using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKProtocolManager.DBEntities
{
    public class FrequencyRange : DBBase
    {

        public string MaxFreq, MinFreq;

        public FrequencyRange()
        {
            setDefaultParameters();
        }

        public FrequencyRange(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
        }
        public FrequencyRange(DataRow row)
        {
            setDefaultParameters();
            fillParametersFromRow(row);
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["frequency_range_id"].ToString();
            this.MaxFreq = row["frequency_range_max_freq"].ToString();
            this.MinFreq = row["frequency_range_min_freq"].ToString();
        }
        protected override void setDefaultParameters()
        {
            string selectQuery = "freq_diap.FreqDiapInd AS frequency_range_id, freq_diap.FreqMin AS frequency_range_min_freq, freq_diap.FreqMax AS frequency_range_max_freq";
            this.getAllQuery = String.Format("SELECT {0} FROM freq_diap", selectQuery);
            this.getByIdQuery = String.Format("SELECT {0} WHERE freq_diap.FreqDiapInd = {1}  LIMIT 1", selectQuery, Id);
            this.colsList = new string[] { "frequency_range_id", "frequency_range_min_freq", "frequency_range_max_freq" };
        }
        
       /// <summary>
       /// Возвращает список диапазонов частот
       /// </summary>
       /// <returns></returns>
       public FrequencyRange[] GetAll()
       {
           FrequencyRange[] mTypes = new FrequencyRange[] { };
           DataTable dt = GetAllFromDB();
           if (dt.Rows.Count > 0)
           {
               mTypes = new FrequencyRange[dt.Rows.Count];
               for (int i = 0; i < dt.Rows.Count; i++) mTypes[i] = new FrequencyRange(dt.Rows[i]);
           }
           return mTypes;
       }
    

    }
}
