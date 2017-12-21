using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SAKProtocolManager.DBEntities
{
    public abstract class DBBase
    {
        public string Id = "0";
        public string getAllQuery;
        protected string getByIdQuery;
        protected string tableName = "default_table_name";
        protected string[] colsList = new string[] { };

        protected abstract void fillParametersFromRow(DataRow row);

        protected abstract void setDefaultParameters();

        protected bool NeedLoadFromDB(DataRow row)
        {
            bool f = false;
            foreach(string colName in colsList)
            {
                f = row.IsNull(colName);
                if(f) break;
            }
            return f;
        }

        protected DataTable getFromDB(string query)
        {
            DataSet ds = makeDataSet();
            DBControl mySql = new DBControl(DBQueries.Default.DBName);
            mySql.MyConn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(query, mySql.MyConn);
            ds.Tables[tableName].Rows.Clear();
            da.Fill(ds.Tables[tableName]);
            mySql.MyConn.Close();
            return ds.Tables[tableName];
        }

        protected long UpdateField(string tableName, string updVals, string condition)
        {
            DBControl mySql = new DBControl(DBQueries.Default.DBName);
            string query = BuildUpdQuery(tableName, updVals, condition);
            long v;
            mySql.MyConn.Open();
            v = mySql.RunNoQuery(query);
            mySql.MyConn.Close();
            return v;
        }

        public static void SendQueriesList(string[] fields)
        {
            if (fields.Length == 0) return;
            DBControl mySql = new DBControl(DBQueries.Default.DBName);
            mySql.MyConn.Open();
            foreach(string f in fields) mySql.RunNoQuery(f);
            mySql.MyConn.Close();
        }

        protected static string BuildDestroyQueryWithCriteria(string tableName, string condition)
        {
            return String.Format("DELETE FROM {0} WHERE {1}", tableName, condition);
        }


        protected static string BuildUpdQuery(string tableName, string updVals, string condition)
        {
            return String.Format("UPDATE {0} SET {1} WHERE {2}", tableName, updVals, condition);
        }

        protected DataSet makeDataSet()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(tableName);
            foreach (string colName in colsList) ds.Tables[tableName].Columns.Add(colName); 
            return ds;
        }

        protected bool GetById()
        {
            DataTable tab = getFromDB(getByIdQuery);
            DataRow val = tab.Rows.Count > 0 ? tab.Rows[0] : null;
            if (val != null) fillParametersFromRow(val);
            return val != null;
        }

        protected DataTable GetAllFromDB()
        {
            return getFromDB(getAllQuery);
        }



    }
}
