using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKProtocolManager.DBEntities
{
    public class CableTest : DBBase
    {
        public Cable TestedCable = null;
        public Operator Operator = null;
        public Baraban Baraban = null;



        internal string CableId = String.Empty;
        private string BarabanId = String.Empty;
        private string OperatorId = String.Empty;
        private string StatusId = String.Empty; 
        public uint TestedLength = 1000; //Длина испытываемого кабеля
        public decimal NettoWeight = 0;
        public decimal BruttoWeight = 0;
        public uint Temperature = 20;
        public bool HasVsvi = false; //Измеряется ли ВСВИ
        public uint PIzb = 0;
        public uint PObol = 0;
        public DateTime TestDate = DateTime.Now;
       

        public CableTest()
        {
            setDefaultParameters();
        }

        public CableTest(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
            LoadDependencies();
        }

        public CableTest(DataRow row)
        {
            fillParametersFromRow(row);
            setDefaultParameters();
            LoadDependencies();
        }


        protected override void setDefaultParameters()
        {

            string selectQuery = "ispytan.Operator AS cable_test_operator_id, ispytan.BarabanInd AS baraban_id,  ispytan.IspInd AS cable_test_id, ispytan.CabNum AS cable_id, ispytan.IspData AS cable_test_tested_at, ispytan.CabelLengt AS cable_test_cable_length, ispytan.Brutto AS cable_test_brutto, ispytan.Netto AS cable_test_netto, ispytan.Temperatur AS cable_test_temperature";
            this.getAllQuery = String.Format("SELECT {0} FROM ispytan", selectQuery);
            this.getByIdQuery = String.Format("SELECT {0} FROM ispytan WHERE ispytan.IspInd = {1}", selectQuery, this.Id);
            this.colsList = new string[] {
                                            "cable_test_id",
                                            "cable_id",
                                            "baraban_id",
                                            "status_id",
                                            "cable_test_tested_at",
                                            "cable_test_cable_length",
                                            "cable_test_vsvi",
                                            "cable_test_p_izb",
                                            "cable_test_p_obol",
                                            "cable_test_netto",
                                            "cable_test_brutto",
                                            "cable_test_temperature",
                                            "cable_test_operator_id"
                                        };
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["cable_test_id"].ToString();
            this.CableId = row["cable_id"].ToString();
            this.OperatorId = row["cable_test_operator_id"].ToString();
            this.BarabanId = row["baraban_id"].ToString();
            this.StatusId = row["status_id"].ToString();
            this.TestedLength = ServiceFunctions.convertToUInt(row["cable_test_cable_length"]);
            this.Temperature = ServiceFunctions.convertToUInt(row["cable_test_temperature"]);
            this.NettoWeight = ServiceFunctions.convertToDecimal(row["cable_test_netto"]);
            this.BruttoWeight = ServiceFunctions.convertToDecimal(row["cable_test_brutto"]);
            this.PIzb = ServiceFunctions.convertToUInt(row["cable_test_p_izb"].ToString());
            this.PObol = ServiceFunctions.convertToUInt(row["cable_test_p_obol"]);
            this.HasVsvi = row["cable_test_vsvi"].ToString() == "1" ? true : false;
            this.TestDate = DateTime.Parse(row["cable_test_tested_at"].ToString());
        }

        private void LoadDependencies()
        {
            if (!String.IsNullOrWhiteSpace(this.CableId)) this.TestedCable = new Cable(this);
            if (!String.IsNullOrWhiteSpace(this.OperatorId)) this.Operator = new Operator(this.OperatorId);
            if (!String.IsNullOrWhiteSpace(this.BarabanId)) this.Baraban = new Baraban(this.BarabanId);
           
        }
    }
}
