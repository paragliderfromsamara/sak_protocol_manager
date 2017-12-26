using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SAKProtocolManager.DBEntities.TestResultEntities
{


    /// <summary>
    /// Класс описывает данные типы параметров "Rж", "dR", "Cр", "dCр", "Co", "Ea", "Rиз1", "Rиз2", "K1", "K2", "K3", "K9", "K10", "K11", "K12", "K2,K3", "K9-12"
    /// </summary>
    class PrimaryParametersTestResult : TestResult
    {

        public PrimaryParametersTestResult(MeasureParameterType pType)
        {
            this.ParameterType = pType;
            this.subElsNumber = GetValuesCount(ParameterType.Name, ParameterType.Structure.BendingTypeLeadsNumber);
            setDefaultParameters();
        }
        //
        public PrimaryParametersTestResult(MeasuredParameterData pData)
        {
            this.ParameterData = pData;
            this.ParameterType = pData.ParameterType;
            this.subElsNumber = GetValuesCount(ParameterType.Name, ParameterType.Structure.BendingTypeLeadsNumber);
            setDefaultParameters();
        }

        public PrimaryParametersTestResult(DataRow row, MeasureParameterType pType, int sub_els_number)
        {
            this.ParameterType = pType;
            this.subElsNumber = sub_els_number;
            fillParametersFromRow(row);
        }

        public PrimaryParametersTestResult(DataRow row, MeasuredParameterData pData, int sub_els_number)
        {
            this.ParameterData = pData;
            this.ParameterType = pData.ParameterType;
            this.subElsNumber = sub_els_number;
            fillParametersFromRow(row);
        }

        protected override void setDefaultParameters()
        {
            string[] jTables = getJoinedTables();
            string selQuery = String.Format("resultism.StruktElNum AS element_number," +
                                            "resultism.Resultat AS value_1{0}", jTables[0]);

            this.getAllQuery = String.Format("SELECT DISTINCT {0} FROM resultism {1} WHERE resultism.ParamInd = {4} AND resultism.IspInd = {2} AND resultism.StruktInd = {3} AND resultism.IsmerNum = 1", selQuery, jTables[1], this.ParameterType.Structure.Cable.Test.Id, this.ParameterType.Structure.Id, this.ParameterType.Id);
            colsList = new string[subElsNumber + 1];
            colsList[0] = "element_number";
            for (int i = 1; i <= subElsNumber; i++)
            {
                colsList[i] = String.Format("value_{0}", i);
            }
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.ElementNumber = ServiceFunctions.convertToInt16(row["element_number"]);
            RawValues = new decimal[subElsNumber];
            //Values = new decimal[subElsNumber];
            for (int i = 0; i < subElsNumber; i++)
            {
                decimal v = ServiceFunctions.convertToDecimal(row[String.Format("value_{0}", i + 1)]);
                //Values[i] = this.ParameterData.BringMeasuredValue(v);
                RawValues[i] = v;
            }
            //this.brLength = this.ParameterData.BringingLength.ToString();
        }


        private string[] getJoinedTables()
        {
            if (subElsNumber == 1) return new string[] { String.Empty, String.Empty };
            string[] val = new string[2];
            string join = String.Empty;
            string select = ", \n";
            for (int i = 2; i <= subElsNumber; i++)
            {
                join += String.Format(" LEFT JOIN ((SELECT resultism.Resultat, resultism.StruktElNum, resultism.StruktInd FROM resultism WHERE resultism.IspInd = {0} AND resultism.IsmerNum = {1} AND resultism.ParamInd = {2}) AS Res{1}) ON resultism.StruktInd = Res{1}.StruktInd AND resultism.StruktElNum = Res{1}.StruktElNum \n", ParameterType.Structure.Cable.Test.Id, i, this.ParameterType.Id);
                select += String.Format("Res{0}.Resultat AS value_{0}", i);
                if (i < subElsNumber) select += ", \n";
            }
            val[0] = select;
            val[1] = join;
            return val;
        }

        public new TestResult[] GetMeasuredResults()
        {
            List<TestResult> trs = new List<TestResult>(); 
            DataTable dt = GetAllFromDB();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PrimaryParametersTestResult ptr = new PrimaryParametersTestResult(dt.Rows[i], ParameterType, subElsNumber);
                    for(int j=0; j<ptr.RawValues.Length; j++)
                    {
                        TestResult tr = new TestResult();
                        /*
                         *Этот код нужен, если TestResult создаётся на ParameterData
                          tr.ParameterData = ptr.ParameterData;
                          tr.BringingValue = ptr.Values[j];
                          if (!tr.CheckIsItNorma()) ParameterData.NotNormalResults.Add(tr);
                        */
                        tr.ParameterType = ptr.ParameterType;
                        tr.RawValue = ptr.RawValues[j];
                        tr.ElementNumber = ptr.ElementNumber;
                        tr.SubElementNumber = j + 1;
                        
                        trs.Add(tr);
                    }
                }
                    
            }
            return trs.ToArray();
        }


        private int GetValuesCount(string tName, int elNumber)
        {
            switch(tName)
            {
                case "dR":
                case "Cр":
                case "Ea":
                    if (elNumber > 3) return 2;
                    else return 1;
                case "K1":
                case "K2":
                case "K3":
                case "K9":
                case "K10":
                case "K11":
                case "K12":
                case "K2,K3":
                case "K11,K12":
                    return 1;
                default:
                    return elNumber;
            }
        }
    }
}
