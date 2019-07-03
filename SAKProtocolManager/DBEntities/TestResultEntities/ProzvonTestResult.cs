using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SAKProtocolManager.DBEntities.TestResultEntities
{
    public class ProzvonTestResult : TestResult
    {
        private string structureId = String.Empty;
        public bool IsAffected = false;
        public ProzvonTestResult(string test_id, string  structure_id, int sub_elements_number)
        {
            this.testId = test_id;
            this.structureId = structure_id;
            this.subElsNumber = sub_elements_number;
            setDefaultParameters();
        }
        public ProzvonTestResult(DataRow row, int subNumber)
        {
            this.subElsNumber = subNumber;
            fillParametersFromRow(row);
        }

        public int ElementStatusId
        {
            get
            {
                decimal stsId = 0;
                foreach (decimal sId in Values)
                {
                    if (sId > stsId) stsId = sId;
                }
                return (int)stsId;
            }
        }

        public string ElementStatus
        {
            get
            {
                decimal stsId = ElementStatusId;
                if (stsId == 0) return "годна";
                else if (stsId == 1) return "обр.";
                else if (stsId == 2) return "зам.";
                else if (stsId == 3) return "проб.";
                else return "";
            }
        }

        protected override void setDefaultParameters()
        {
            string[] jTables = getJoinedTables();
            string selQuery = String.Format("resultism.StruktElNum AS element_number," +
                                               "status_gil.StatGilName AS status_1, " +
                                               "resultism.Resultat AS value_1{0}", jTables[0]);

            this.getAllQuery = String.Format("SELECT DISTINCT {0} FROM resultism LEFT JOIN status_gil ON resultism.Resultat = status_gil.StatGil {1} WHERE resultism.ParamInd = 1 AND resultism.IspInd = {2} AND resultism.StruktInd = {3} AND  resultism.IsmerNum = 1", selQuery, jTables[1], testId, structureId);
            colsList = new string[(subElsNumber*2) + 1];
            colsList[0] = "element_number";
            for(int i=1; i<=subElsNumber; i++)
            {
                int j = i * 2 - 2;
                colsList[j] = String.Format("value_{0}", i);
                colsList[j+1] = String.Format("status_{0}", i);
            }
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.ElementNumber = ServiceFunctions.convertToInt16(row["element_number"]);
            Statuses = new string[subElsNumber];
            Values = new decimal[subElsNumber];
            for(int i = 0; i<subElsNumber; i++)
            {
                Statuses[i] = row[String.Format("status_{0}", i + 1)].ToString();
                Values[i] = ServiceFunctions.convertToDecimal(row[String.Format("value_{0}", i + 1)].ToString());
                IsAffected |= Values[i] > 0;
            }
        }

        public new ProzvonTestResult[] GetMeasuredResults()
        {
            ProzvonTestResult[] trs = new ProzvonTestResult[] { };
            DataTable dt = GetAllFromDB();
            if (dt.Rows.Count > 0)
            {
                trs = new ProzvonTestResult[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++) trs[i] = new ProzvonTestResult(dt.Rows[i], subElsNumber);
            }
            return trs;
        }
        private string[] getJoinedTables()
        {
            if (subElsNumber == 1) return new string[] { String.Empty, String.Empty };
            string[] val = new string[2];
            string join = String.Empty;
            string select = ", \n";
            for(int i=2; i<=subElsNumber; i++)
            {
                join += String.Format(" LEFT JOIN ((SELECT resultism.Resultat, resultism.StruktElNum, resultism.StruktInd, status_gil.StatGilName AS StatGilName FROM resultism LEFT JOIN status_gil ON resultism.Resultat = status_gil.StatGil WHERE resultism.IspInd = {0} AND resultism.IsmerNum = {1} AND resultism.ParamInd = 1) AS Res{1}) ON resultism.StruktInd = Res{1}.StruktInd AND resultism.StruktElNum = Res{1}.StruktElNum \n", testId, i);
                select += String.Format("Res{0}.StatGilName AS status_{0}, Res{0}.Resultat AS value_{0}", i);
                if (i < subElsNumber) select += ", \n";
            }
            val[0] = select;
            val[1] = join;
            return val;
        }

    }
}
