﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using SAKProtocolManager.DBEntities.TestResultEntities;
namespace SAKProtocolManager.DBEntities
{
    public class MeasureParameterType : DBBase
    {

        public string Name, Description, Measure;
        public static string[] PrimaryParametersList = new string[] {};
        internal TestResult[] TestResults = new TestResult[] { };
        internal MeasuredParameterData[] ParameterDataList = new MeasuredParameterData[] {};
        internal CableStructure Structure = null;
        private int curParameterDataIdx = -1;
        private string deviationMeasure = null;
        /// <summary>
        /// Флаг того, что это испытание проводилось и есть результаты
        /// </summary>
        internal bool IsTested = true; 

        public int OutOfNormaCount()
        {
            int counter = 0;
            if (ParameterDataList.Length > 0)
            {
                foreach (MeasuredParameterData pd in ParameterDataList) counter += pd.NotNormalResults.Length;
            }
            return counter;
        }

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
            if (ParameterDataList.Length>0)
            {
                GetTestResult();
                assaignTestResultsToParameterData();
            }
            //if (HasTest()) CheckIsItTested();
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

        public void SetParameterDataIdx(int idx)
        {
            if (idx >= ParameterDataList.Length || idx < 0) return;
            curParameterDataIdx = idx;
            assaignTestResultsToParameterData();
        }

        /// <summary>
        /// Обновляем измеренные параметры на MeasuredParameterData
        /// </summary>
        public void RefreshTestResultsOnParameterData(bool needToReloadResults)
        {
            if (needToReloadResults) GetTestResult();
            if (ParameterDataList.Length > 0 && this.TestResults.Length > 0) assaignTestResultsToParameterData();
        }


        private void assaignTestResultsToParameterData()
        {
         
            foreach(MeasuredParameterData pData in ParameterDataList)
            {
                List<TestResult> trListForPData = new List<TestResult>();
                List<TestResult> NotNormaTrListForPData = new List<TestResult>();
                List<decimal> vals = new List<decimal>();
                foreach (TestResult tr in TestResults)
                {
                    TestResult cTr = tr.CloneIncludingParameterType();
                    if (cTr.SetParameterData(pData))
                    {
                        trListForPData.Add(cTr);
                        if (!cTr.Affected) vals.Add(cTr.BringingValue);
                        if (cTr.DeviationPercent > 0) NotNormaTrListForPData.Add(cTr);
                    }
                }
                pData.TestResults = trListForPData.ToArray();
                pData.NotNormalResults = NotNormaTrListForPData.ToArray();
                if (pData.TestResults.Length > 0)
                {
                    pData.MeasuredPercent = Math.Round(100 * (((decimal)pData.TestResults.Length-(decimal)NotNormaTrListForPData.Count()) / (decimal)pData.TestResults.Length), 0);
                    if (vals.Count > 0)
                    {
                        pData.MaxVal = vals.Max();
                        pData.MinVal = vals.Min();
                        pData.AverageVal = Math.Round(vals.Sum() / (decimal)vals.Count(), 1);
                    }
                }




            }
        }

        private void getStructureParameterData()
        {
            if (this.Structure == null) return;
            MeasuredParameterData mpd = new MeasuredParameterData(this);
            this.ParameterDataList = mpd.GetStructureMeasureParameters();
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
            if(ParameterDataList.Length > 0)
            {
                foreach(MeasuredParameterData pd in ParameterDataList)
                {
                    this.IsTested = pd.TestResults.Length > 0;
                    if (this.IsTested) return;
                }
            }
            this.IsTested = false;
        }

        
        internal bool HasTest()
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

        /// <summary>
        /// Мера измерения отклонения от нормы %, дБ, с.
        /// </summary>
        /// <returns></returns>
        internal string DeviationMeasure()
        {
            if (this.deviationMeasure == null)
            {
                string val = String.Empty;
                switch (this.Name)
                {
                    case "Ao":
                    case "Az":
                    case "al":
                        val = "дБ";
                        break;
                    case "Rиз2":
                    case "Rиз4":
                        val = "c.";
                        break;
                    default:
                        val = "%";
                        break;
                }
                this.deviationMeasure = val;
            } 
            return this.deviationMeasure;
        }

        internal void GetTestResult()
        {
            // return;
            if (!HasTest()) return;
            TestResult tResult = new TestResult(this);
            this.TestResults = tResult.GetMeasuredResults();
            this.IsTested = TestResults.Length > 0;
        }

        public decimal BringToLength(decimal value, decimal curLength, decimal brLength)
        {
            int round = 2;
            switch (this.Name)
            {
                case "Rж":
                case "Cр":
                case "Co":
                case "Ea":
                case "K1":
                case "K2":
                case "K3":
                case "K9":
                case "K10":
                case "K11":
                case "K12":
                    value *= brLength / curLength;
                    round = value > 99 ? 1 : 2;
                    return Math.Round(value, round);
                case "Rиз1":
                case "Rиз3":
                    value *= curLength / brLength;
                    round = value > 99 ? 1 : 2;
                    return Math.Round(value, round);
                case "al":
                    value *= brLength / curLength;
                    return Math.Round(value, 1);
                case "Ao":
                case "Az":
                    value += 10 * (decimal)Math.Log10(((double)curLength / (double)brLength));
                    return Math.Round(value, 1);
                default:
                    return value;
            }
        }
    }


}
