using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;

namespace SAKProtocolManager.DBEntities
{
    public class MeasureParameterType : DBBase
    {

        public string Name, Description, Measure;
        public static string[] PrimaryParametersList = new string[] {};
        //internal TestResult[] TestResults = new TestResult[] { };
        internal MeasuredParameterData[] ParameterData = new MeasuredParameterData[] {};
        internal CableStructure Structure = null;
        /// <summary>
        /// Флаг того, что это испытание проводилось и есть результаты
        /// </summary>
        internal bool IsTested = true; 



        public MeasureParameterType()
        {
            setDefaultParameters();
        }

        /// <summary>
        /// Чтобы получить типы параметров структуры, через функцию GetAll() необходимо её указать во входящем параметре
        /// </summary>
        /// <param name="structure"></param>
        public MeasureParameterType(CableStructure structure)
        {
            this.Structure = structure;
            setDefaultParameters();
        }

        public MeasureParameterType(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
        }

        public MeasureParameterType(DataRow row)
        {
            fillParametersFromRow(row);
        }

        public MeasureParameterType(DataRow row, CableStructure structure)
        {
            this.Structure = structure;
            fillParametersFromRow(row);
            setDefaultParameters();
            getStructureParameterData();
            if (HasTest()) CheckIsItTested();
        }

        /*private void fillMeasuredParameterDataResult()
        {
            if(this.ParameterData.Length > 0 && this.TestResults.Length > 0)
            {
                for(int i=0; i< this.ParameterData.Length; i++)
                {
                    this.ParameterData[i].TestResults = TestResult.GetBigRound(i, this.TestResults);
                    this.ParameterData[i].CalculateMinMaxAverage();
                }
            }
        }*/

        private void getStructureParameterData()
        {
            if (this.Structure == null) return;
            MeasuredParameterData mpd = new MeasuredParameterData(this);
            this.ParameterData = mpd.GetStructureMeasureParameters();
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["measure_parameter_type_id"].ToString();
            this.Name = row["measure_parameter_type_name"].ToString();
            this.Measure = row["measure_parameter_type_measure"].ToString();
            this.Description = row["measure_parameter_type_description"].ToString();
        }

        protected override void setDefaultParameters()
        {
            string selectQuery = "ism_param.ParamInd AS measure_parameter_type_id, ism_param.ParamName AS measure_parameter_type_name, ism_param.Ed_izm AS measure_parameter_type_measure, ism_param.ParamOpis AS measure_parameter_type_description";
            this.getAllQuery = String.Format("SELECT {0} FROM ism_param ORDER BY ism_param.ParamInd", selectQuery);
            this.getByIdQuery = String.Format("SELECT {0} FROM ism_param WHERE ism_param.ParamInd = {1} ORDER BY ism_param.ParamInd LIMIT 1", selectQuery, Id);
            this.colsList = new string[] { "measure_parameter_type_id", "measure_parameter_type_name", "measure_parameter_type_measure", "measure_parameter_type_description" };
        }

        public MeasureParameterType[] GetAll()
        {
            MeasureParameterType[] els = new MeasureParameterType[] { };
            DataTable dt = GetAllFromDB();
            if (dt.Rows.Count > 0)
            {
                els = new MeasureParameterType[dt.Rows.Count];
                if (this.Structure == null) for (int i = 0; i < dt.Rows.Count; i++) els[i] = new MeasureParameterType(dt.Rows[i]);
                else for (int i = 0; i < dt.Rows.Count; i++) els[i] = new MeasureParameterType(dt.Rows[i], this.Structure);

            }
            return els;
        }

        /// <summary>
        /// Проверяет измерялся ли данный параметр или нет
        /// </summary>
        private void CheckIsItTested()
        {
            if(ParameterData.Length > 0)
            {
                foreach(MeasuredParameterData pd in ParameterData)
                {
                    this.IsTested = pd.TestResults.Length > 0;
                    if (this.IsTested) return;
                }
            }
            this.IsTested = false;
        }

        private bool HasTest()
        {
            if (this.Structure == null) return false;
            if (this.Structure.Cable == null) return false;
            if (this.Structure.Cable.Test == null) return false;
            return true;
        }

        internal string NameWithMeasure()
        {
            if (String.IsNullOrWhiteSpace(this.Measure)) return this.Name;
            else return String.Format("{0}, {1}", this.Name, this.Measure);
        }
    }


}
