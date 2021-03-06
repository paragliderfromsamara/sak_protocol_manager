﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAKProtocolManager.DBEntities.TestResultEntities;

namespace SAKProtocolManager.DBEntities
{
    public class CableStructure : DBBase
    {

        public LeadMaterial LeadMaterial = null;
        public IsolationMaterial IsolationMaterial = null;
        public MeasureParameterType[] MeasuredParameters = new MeasureParameterType[] { };
        public MeasureParameterType[] MeasuredParameters_Full = new MeasureParameterType[] { };
       // public int[] AffectedElementNumbers = new int[] { };
        public Dictionary<int, ProzvonTestResult> AffectedElements = new Dictionary<int, ProzvonTestResult>();


        private string getByCableIdQuery;
        public Cable Cable = null;
        public string LeadMaterialId = String.Empty;
        public string IsolationMaterialId = String.Empty;
        public string BendimgTypeId = String.Empty;
        public int NumberInCable = 0;
        public int RealNumberInCable = 0;
        public decimal LeadDiameter = 0;
        public uint WaveResistance = 0;
        public uint Puchek = 0;
        public uint LeadLeadTestVoltage = 0;
        public uint LeadShieldTestVoltage = 0;
        public string BendingTypeName = String.Empty;
        public int BendingTypeLeadsNumber = 0;
        public string Name = String.Empty;
        public string dRBringingFormulaId = String.Empty;
        public string dRFormulaId = String.Empty;
        public string dRFormulaMeasure = String.Empty;
     

        public int CalculateAffectedElements()
        {
            int count = 0;
            List<int> elementIds = AffectedElements.Keys.ToList();
            foreach(MeasureParameterType t in MeasuredParameters)
            {
                foreach(MeasuredParameterData d in t.ParameterDataList)
                {
                    elementIds.AddRange(d.NotNormalResults.Keys);
                }
            }
            return elementIds.Distinct<int>().Count();
        }

        public int NormalElementsCount
        {
            get
            {
                return RealNumberInCable - CalculateAffectedElements();
            }
        }

        /// <summary>
        /// Возвращает название элемента структуры в родительском падеже
        /// </summary>
        /// <returns></returns>
        public string BendingTypeName_RodPadej_Multiple
        {
            get
            {
                switch (BendingTypeLeadsNumber)
                {
                    case 1:
                        return "жил";
                    case 2:
                        return "пар";
                    case 3:
                        return "троек";
                    case 4:
                        return "четвёрок";
                    default:
                        return "элементов";
                }
            }
        }
        public string BendingTypeName_Multiple
        {
            get
            {
                switch (BendingTypeLeadsNumber)
                {
                    case 1:
                        return "жилы";
                    case 2:
                        return "пары";
                    case 3:
                        return "тройки";
                    case 4:
                        return "четвёрки";
                    default:
                        return "элементы";
                }
            }
        }




        public CableStructure(Cable cable)
        {
            this.Cable = cable;
            setDefaultParameters();
        }



        public CableStructure(DataRow row, Cable cable)
        {
            this.Cable = cable;
            fillParametersFromRow(row);
            setDefaultParameters();
            FillAffectedElements();
            GetDependencies();
        }

        public ProzvonTestResult GetProzvonTestResult(int elNum)
        {
            if (AffectedElements.ContainsKey(elNum))
            {
                return AffectedElements[elNum];
            }
            else return null;
        }

        private void FillAffectedElements()
        {
            ProzvonTestResult[] przvResults = GetProszvonResult();
            AffectedElements.Clear();
            if (przvResults.Length > 0)
            {
                foreach (ProzvonTestResult pr in przvResults)
                {
                    if (pr.IsAffected)
                    {
                        AffectedElements.Add(pr.ElementNumber, pr);
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает результат прозвонки
        /// </summary>
        /// <returns></returns>
        public ProzvonTestResult[] GetProszvonResult()
        {
            ProzvonTestResult tr = new ProzvonTestResult(this.Cable.Test.Id, this.Id, this.BendingTypeLeadsNumber);
            ProzvonTestResult[] trs = tr.GetMeasuredResults();
            return trs;
        }

        public bool HightVoltageWasTested
        {
            get
            {
                return LeadShieldTestVoltage > 499 || LeadLeadTestVoltage > 499;
            }
        }

        private int[] brokenPairs;
        public int[] BrokenPairs
        {
            get
            {
                if (brokenPairs == null)
                {
                    List<int> pairs = new List<int>();
                    if (AffectedElements.Count>0)
                    {
                        foreach(ProzvonTestResult r in AffectedElements.Values)
                        {
                            if (r.ElementStatusId == 3) pairs.Add(r.ElementNumber);
                        }
                    }
                    brokenPairs = pairs.ToArray();
                }
                return brokenPairs;
            }
        }

        protected override void setDefaultParameters()
        {
            string selectQuery =
                "struktury_cab.StruktInd AS cable_structure_id," +
                "struktury_cab.CabNum AS cable_id," +
                "struktury_cab.Kolvo AS cable_structure_real_number_in_cable," +
                "struktury_cab.Kolvo_ind AS cable_structure_number_in_cable," +
                "struktury_cab.DiamGil AS cable_structure_lead_diameter," +
                "struktury_cab.Zwave AS cable_structure_wave_resistance," +
                "struktury_cab.Puchek AS cable_structure_puchek," +
                "struktury_cab.U_gil_gil AS cable_structure_lead_lead_voltage," +
                "struktury_cab.U_gil_ekr AS cable_structure_lead_shield_voltage," +
                "struktury_cab.MaterGil AS lead_material_id," +
                "struktury_cab.MaterIsol AS isolation_material_id," +
                "tipy_poviv.StruktNum AS bending_type_id," +
                "tipy_poviv.StruktTip AS bending_type_name," +
                "tipy_poviv.ColvoGil AS bending_type_leads_number," +
                "struktury_cab.DRPrivInd AS dr_bringing_formula_id, " +
                "struktury_cab.Delta_R AS dr_formula_id, " + 
                "dr_formuls.ResEd AS dr_formula_measure";
            this.getAllQuery = String.Format("SELECT {0} FROM struktury_cab LEFT JOIN tipy_poviv ON struktury_cab.PovivTip = tipy_poviv.StruktNum LEFT JOIN dr_formuls ON struktury_cab.Delta_R = dr_formuls.DRInd", selectQuery);
            this.getByIdQuery = String.Format("{0} WHERE struktury_cab.StruktInd = {1}", this.getAllQuery, this.Id);
            this.getByCableIdQuery = String.Format("{0} WHERE struktury_cab.CabNum = {1}", this.getAllQuery, this.Cable.Id);
            this.colsList = new string[]
            {
               "cable_structure_id",
               "cable_id",
               "cable_structure_number_in_cable",
               "cable_structure_real_number_in_cable",
               "cable_structure_lead_diameter",
               "cable_structure_wave_resistance",
               "cable_structure_puchek",
               "cable_structure_lead_lead_voltage",
               "cable_structure_lead_shield_voltage",
               "lead_material_id",
               "isolation_material_id",
               "bending_type_id",
               "bending_type_name",
               "bending_type_leads_number",
               "dr_formula_id", 
               "dr_formula_measure",
               "dr_bringing_formula_id"
            };
        }

        private void GetDependencies()
        {
            MeasureParameterType mParameter = new MeasureParameterType(this);
            this.IsolationMaterial = new IsolationMaterial(this.IsolationMaterialId);
            this.LeadMaterial = new LeadMaterial(this.LeadMaterialId);
            this.MeasuredParameters = mParameter.GetAll();
            MeasuredParameters_Full = (MeasureParameterType[])this.MeasuredParameters.Clone();
            if (this.Cable.Test != null) filterTested(); // Если кабель вызван через испытание то фильтруем типы испытаний по наличию результата измерения
        }

        /// <summary>
        /// Оставляет в списке только те типы параметров, которые измерены
        /// </summary>
        private void filterTested()
        {
            List<MeasureParameterType> pTypes = new List<MeasureParameterType>();
            foreach (MeasureParameterType pt in this.MeasuredParameters) if (pt.TestResults.Length>0) pTypes.Add(pt);
            this.MeasuredParameters = pTypes.ToArray();
         }

     
        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["cable_structure_id"].ToString();
            this.NumberInCable = ServiceFunctions.convertToInt16(row["cable_structure_number_in_cable"]);
            this.RealNumberInCable = ServiceFunctions.convertToInt16(row["cable_structure_real_number_in_cable"]);
            this.LeadDiameter = ServiceFunctions.convertToDecimal(row["cable_structure_lead_diameter"]);
            this.WaveResistance = ServiceFunctions.convertToUInt(row["cable_structure_wave_resistance"]);
            this.Puchek = ServiceFunctions.convertToUInt(row["cable_structure_puchek"]);
            this.LeadLeadTestVoltage = ServiceFunctions.convertToUInt(row["cable_structure_lead_lead_voltage"]);
            this.LeadShieldTestVoltage = ServiceFunctions.convertToUInt(row["cable_structure_lead_shield_voltage"]);
            this.IsolationMaterialId = row["isolation_material_id"].ToString();
            this.LeadMaterialId = row["lead_material_id"].ToString();
            this.BendimgTypeId = row["bending_type_id"].ToString();
            this.BendingTypeName = row["bending_type_name"].ToString();
            this.BendingTypeLeadsNumber = ServiceFunctions.convertToInt16(row["bending_type_leads_number"]);
            this.Name = String.Format("{0}x{1}x{2}", this.NumberInCable, this.BendingTypeLeadsNumber, this.LeadDiameter);
            this.dRBringingFormulaId = row["dr_bringing_formula_id"].ToString();
            this.dRFormulaId = row["dr_formula_id"].ToString();
            this.dRFormulaMeasure = row["dr_formula_measure"].ToString();
        }

        public CableStructure[] GetCableStructures()
        {
            CableStructure[] ents = new CableStructure[] { };
            setDefaultParameters();
            DataTable dt = getFromDB(this.getByCableIdQuery);
            if (dt.Rows.Count > 0)
            {
                ents = new CableStructure[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++) ents[i] = new CableStructure(dt.Rows[i], this.Cable);
            }
            return ents;
        }


    }
}
