using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NormaMeasure.DBControl.Tables
{
    [DBTable("struktury_cab", "bd_isp", OldDBName = "bd_cable", OldTableName = "struktury_cab")]
    public class CableStructure : BaseEntity
    {
        public CableStructure(DataRowBuilder builder) : base(builder)
        {
        }

        public static CableStructure find_by_structure_id(uint structure_id)
        {
            DBEntityTable t = find_by_primary_key(structure_id, typeof(CableStructure));
            CableStructure structure = null;
            if (t.Rows.Count > 0) structure = (CableStructure)t.Rows[0];
            return structure;
        }

        /*
        public void CopyFromStructure(CableStructure structure)
        {
            this.FillColsFromEntity(structure);
            this.AddMeasuredParameterDataFromStructure(structure);
        }

        private void AddMeasuredParameterDataFromStructure(CableStructure structure)
        {
            this.MeasuredParameters.Clear();
            foreach(CableStructureMeasuredParameterData data in structure.MeasuredParameters.Rows)
            {
                CableStructureMeasuredParameterData dta = (CableStructureMeasuredParameterData)MeasuredParameters.NewRow();
                dta.FillColsFromEntity(data);
                dta.CableStructureId = this.CableStructureId;
                
                this.MeasuredParameters.Rows.Add(dta);
            }

        }
        */
        public override bool Destroy()
        {
            bool delFlag = true;
            if (!IsNewRecord())
            {
                delFlag = base.Destroy();
                if (delFlag) DeleteAllMeasuredParametersData(); //Удаляем неиспользуемые измеряемые параметры
                // System.Windows.Forms.MessageBox.Show(this.CableStructureId.ToString() + " ");

            }
            if (delFlag)
            {
                this.Delete();
            }
            return delFlag;
        }

        private void DeleteAllMeasuredParametersData()
        {
            this.MeasuredParameters.Rows.Clear();
            //CableStructureMeasuredParameterData.DeleteUnusedFromStructure(this);
        }

        public override bool Save()
        {
            try
            {
                if (base.Save())
                {
                    return SaveMeasuredParameters();
                }
                else return false;
            }catch(DBEntityException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Не удалось сохранить структуру", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

        }

        protected bool SaveMeasuredParameters()
        {
            /*
            bool flag = true;
            foreach(CableStructureMeasuredParameterData cdmpd in MeasuredParameters.Rows)
            {
                if (cdmpd.RowState == DataRowState.Deleted) continue;
                cdmpd.CableStructureId = this.CableStructureId;
                flag &= cdmpd.Save();
            }
            CableStructureMeasuredParameterData.DeleteUnusedFromStructure(this);
            return flag;
            */
            return true;
        }

        protected override void ValidateActions()
        {
            CheckLeadDiameter();
            CheckElementsNumber();
            CheckMeasuredParameters();
        }

        private void CheckMeasuredParameters()
        {
            /*
            foreach(CableStructureMeasuredParameterData mpd in MeasuredParameters.Rows)
            {
                if(!mpd.Validate())
                {
                    foreach(string e in mpd.ErrorsAsArray)
                    {
                        ErrorsList.Add($"{mpd.ParameterName}: {e};");
                    }
                    
                }
            }
            */
        }

        private void CheckElementsNumber()
        {
            if (RealAmount <= 0)
            {
                ErrorsList.Add("Реальное количество элементов структуры должно быть больше 0");
            }
            else if (RealAmount < DisplayedAmount) ErrorsList.Add("Реальное количество элементов структуры должно быть не меньше номинального");
        }

        private void CheckLeadDiameter()
        {
            if (LeadDiameter <= 0) ErrorsList.Add("Диаметр жилы должен быть больше 0");
        }

        public static CableStructure build_for_cable(uint cableId)
        {
            DBEntityTable t = new DBEntityTable(typeof(CableStructure));
            CableStructure s = (CableStructure)t.NewRow();
            s.CableId = cableId;
            s.LeadDiameter = 0.1f;
            return s;
        }

        public static DBEntityTable get_by_cable(Cable cable)
        {
            DBEntityTable cabStructures = find_by_criteria($"{Cable.CableId_ColumnName} = {cable.CableId}", typeof(CableStructure));
            foreach (CableStructure cs in cabStructures.Rows) cs.OwnCable = cable;
            return cabStructures;
        }

        public static uint get_last_structure_id()
        {
            CableStructure s = get_last_cable_structure();
            if (s == null) return 0;
            else return s.CableStructureId; 
        }

        public static CableStructure get_last_cable_structure()
        {
            DBEntityTable t = new DBEntityTable(typeof(CableStructure));
            string select_cmd = $"{t.SelectQuery} ORDER BY {StructureId_ColumnName} DESC LIMIT 1;";
            t.FillByQuery(select_cmd);
            if (t.Rows.Count > 0) return (CableStructure)t.Rows[0];
            else return null;
        }

        public MeasuredParameterData[] GetAll_MeasuredParameterData_By_ParameterTypeId(uint parameter_type_id)
        {
            DataRow[] rows = MeasuredParameters.Select($"{MeasuredParameterType.ParameterTypeId_ColumnName} = {parameter_type_id}");
            return (MeasuredParameterData[])rows;
        }


        #region Колонки таблицы
        [DBColumn(StructureId_ColumnName, ColumnDomain.UInt, Order = 10, OldDBColumnName = "StruktInd", IsPrimaryKey = true, Nullable = true, AutoIncrement = true)]
        public uint CableStructureId
        {
            get
            {
                return tryParseUInt(StructureId_ColumnName);
            }
            set
            {
                if (CableStructureId != value) structureType = null;
                this[StructureId_ColumnName] = value;

            }
        }


        [DBColumn(Cable.CableId_ColumnName, ColumnDomain.UInt, Order = 11, OldDBColumnName = "CabNum", Nullable = true, ReferenceTo = "cables(cable_id) ON DELETE CASCADE")]
        public uint CableId
        {
            get
            {
                return tryParseUInt(Cable.CableId_ColumnName);
            }
            set
            {
                this[Cable.CableId_ColumnName] = value;
            }
        }

        /// <summary>
        /// Реальное количество элементов структуры
        /// </summary>
        [DBColumn(RealAmount_ColumnName, ColumnDomain.UInt, Order = 12, OldDBColumnName = "Kolvo", Nullable = true, DefaultValue = 1)]
        public uint RealAmount
        {
            get
            {
                return tryParseUInt(RealAmount_ColumnName);
            }
            set
            {
                this[RealAmount_ColumnName] = value;
            }
        }

        /// <summary>
        /// Номинальное количество элементов структуры
        /// </summary>
        [DBColumn(ShownAmount_ColumnName, ColumnDomain.UInt, Order = 13, OldDBColumnName = "Kolvo_ind", Nullable = true, DefaultValue = 1)]
        public uint DisplayedAmount
        {
            get
            {
                return tryParseUInt(ShownAmount_ColumnName);
            }
            set
            {
                this[ShownAmount_ColumnName] = value;
            }
        }

        /// <summary>
        /// ID типа структуры
        /// </summary>
        [DBColumn("PovivTip", ColumnDomain.UInt, Order = 14, OldDBColumnName = "PovivTip", Nullable = true)]
        public uint StructureTypeId
        {
            get
            {
                return tryParseUInt("PovivTip");
            }
            set
            {
                this["PovivTip"] = value;
            }
        }

        /// <summary>
        /// ID типа материала токопроводящей жилы
        /// </summary>
        [DBColumn("MaterGil", ColumnDomain.UInt, Order = 15, OldDBColumnName = "MaterGil", Nullable = true)]
        public uint LeadMaterialTypeId
        {
            get
            {
                return tryParseUInt("MaterGil");
            }
            set
            {
                this["MaterGil"] = value;
            }
        }

        /// <summary>
        /// Диаметр жилы
        /// </summary>
        [DBColumn(LeadDiameter_ColumnName, ColumnDomain.Float, Order = 16, OldDBColumnName = "DiamGil", Nullable = true)]
        public float LeadDiameter
        {
            get
            {
                return tryParseFloat(LeadDiameter_ColumnName);
            }
            set
            {
                this[LeadDiameter_ColumnName] = value;
            }
        }

        /// <summary>
        /// ID типа материала изоляции жил структуры
        /// </summary>
        [DBColumn("MaterIsol", ColumnDomain.UInt, Order = 17, OldDBColumnName = "MaterIsol", Nullable = true)]
        public uint IsolationMaterialId
        {
            get
            {
                return tryParseUInt("MaterIsol");
            }
            set
            {
                this["MaterIsol"] = value;
            }
        }

        /// <summary>
        /// Волновое сопротивление кабеля
        /// </summary>
        [DBColumn(WaveResistance_ColumnName, ColumnDomain.Float, Order = 18, OldDBColumnName = "Zwave", Nullable = true)]
        public float WaveResistance
        {
            get
            {
                return tryParseFloat(WaveResistance_ColumnName);
            }
            set
            {
                this[WaveResistance_ColumnName] = value;
            }
        }

        /// <summary>
        /// Количество элементов в пучке
        /// </summary>
        [DBColumn(GroupedAmount_ColumnName, ColumnDomain.UInt, Order = 19, OldDBColumnName = "Puchek", Nullable = true, DefaultValue = 0)]
        public uint GroupedAmount
        {
            get
            {
                return tryParseUInt(GroupedAmount_ColumnName);
            }
            set
            {
                this[GroupedAmount_ColumnName] = value;
            }
        }

        /// <summary>
        /// Испытательное напряжение прочности оболочик жила-жила
        /// </summary>
        [DBColumn(LeadLeadVoltage_ColumnName, ColumnDomain.UInt, Order = 20, OldDBColumnName = "U_gil_gil", Nullable = true, DefaultValue = 0)]
        public uint LeadToLeadTestVoltage
        {
            get
            {
                return tryParseUInt(LeadLeadVoltage_ColumnName);
            }
            set
            {
                this[LeadLeadVoltage_ColumnName] = value;
            }
        }

        /// <summary>
        /// Испытательное напряжение прочности оболочик жила-экран
        /// </summary>
        [DBColumn(LeadShieldVoltage_ColumnName, ColumnDomain.UInt, Order = 21, OldDBColumnName = "U_gil_ekr", Nullable = true, DefaultValue = 0)]
        public uint LeadToShieldTestVoltage
        {
            get
            {
                return tryParseUInt(LeadShieldVoltage_ColumnName);
            }
            set
            {
                this[LeadShieldVoltage_ColumnName] = value;
            }
        }

        /// <summary>
        /// Рабочая ёмкость группы
        /// </summary>
        [DBColumn(WorkCapGroup_ColumnName, ColumnDomain.Boolean, Order = 22, OldDBColumnName = "Cr_grup", Nullable = true, DefaultValue = 0)]
        public bool WorkCapacityGroup
        {
            get
            {
                return tryParseBoolean(WorkCapGroup_ColumnName, false);
            }
            set
            {
                this[WorkCapGroup_ColumnName] = value;
            }
        }

        [DBColumn("Delta_R", ColumnDomain.UInt, Order = 23, OldDBColumnName = "Delta_R", Nullable = true, DefaultValue = 1)]
        public uint DRBringingFormulaId
        {
            get
            {
                return tryParseUInt("Delta_R");
            }
            set
            {
                this["Delta_R"] = value;
            }
        }

        [DBColumn("DRPrivInd", ColumnDomain.UInt, Order = 24, OldDBColumnName = "DRPrivInd", Nullable = true, DefaultValue = 1)]
        public uint DRFormulaId
        {
            get
            {
                return tryParseUInt("DRPrivInd");
            }
            set
            {
                this["DRPrivInd"] = value;
                loadDRFormula();
                refreshDRMeasureData();
            }
        }


        public const string StructureId_ColumnName = "StruktInd";
        public const string RealAmount_ColumnName = "Kolvo";
        public const string ShownAmount_ColumnName = "Kolvo_ind";
        public const string LeadDiameter_ColumnName = "DiamGil";
        public const string WaveResistance_ColumnName = "Zwave";
        public const string GroupedAmount_ColumnName = "Puchek";
        public const string LeadLeadVoltage_ColumnName = "U_gil_gil";
        public const string LeadShieldVoltage_ColumnName = "U_gil_ekr";
        public const string WorkCapGroup_ColumnName = "Cr_grup";

        #endregion 

        private LeadMaterial leadMaterial;
        public LeadMaterial LeadMaterial
        {
            get
            {
                if (leadMaterial == null)
                {
                    leadMaterial = LeadMaterial.get_by_id(LeadMaterialTypeId);
                }
                return leadMaterial;
            }
        }

        private IsolationMaterial isolMaterial;
        public IsolationMaterial IsolMaterial
        {
            get
            {
                if (isolMaterial == null)
                {
                    isolMaterial = IsolationMaterial.get_by_id(IsolationMaterialId);
                }
                return isolMaterial;
            }
        }

        private void refreshDRMeasureData()
        {
            /*
            foreach(CableStructureMeasuredParameterData mpd in MeasuredParameters.Rows)
            {
                if (mpd.ParameterTypeId == MeasuredParameterType.dR) mpd.ResultMeasure = drFormula.ResultMeasure;
            }
            */
        }

        public string StructureTitle
        {
            get
            {
                return $"{DisplayedAmount}x{StructureType.StructureLeadsAmount}x{LeadDiameter}";
            }
        }
        public CableStructureType StructureType
        {
            get
            {
                if (structureType == null)
                {
                    this.structureType = CableStructureType.get_by_id(this.StructureTypeId);
                }
                return this.structureType;
            }
            set
            {
                this.structureType = value;
                this.StructureTypeId = this.structureType.StructureTypeId;
            }
        }

        public DBEntityTable MeasuredParameters_was => measuredParameters_was;
        public virtual DBEntityTable MeasuredParameters
        {
            get
            {
                if (measuredParameters == null)
                {
                    if (IsNewRecord())
                    {
                        //measuredParameters = MeasuredParameterData.get_structure_measured_parameters(0);
                    }
                    else
                    {
                       // measuredParameters = MeasuredParameterData.get_structure_measured_parameters(this);
                    }
                    measuredParameters_was = measuredParameters.Clone() as DBEntityTable;
                }
                return measuredParameters;
            }
        }


        /// <summary>
        /// Достает MeasuredParameterDataId используемые текущей структурой
        /// </summary>
        public uint[] MeasuredParameters_ids
        {
            get
            {
               
                List<uint> idsList = new List<uint>();
                 /*
                foreach(CableStructureMeasuredParameterData csmpd in MeasuredParameters.Rows)
                {
                    idsList.Add(csmpd.MeasuredParameterDataId);
                }
                 */
                return idsList.ToArray();

            }
        }

        public bool HasFreqParameters
        {
            get
            {
                DBEntityTable t = new DBEntityTable(typeof(MeasuredParameterType));
                return StructureType.MeasuredParameterTypes.Select($"{t.PrimaryKey[0].ColumnName} IN ({MeasuredParameterType.al}, {MeasuredParameterType.Ao}, {MeasuredParameterType.Az})").Count() > 0;
            }
        }

        public Cable OwnCable
        {
            get
            {
                if (ownCable == null)
                {
                    Cable c = Cable.find_by_cable_id(CableId);
                    if (c != null) OwnCable = c;
                }
                return ownCable;
            }
            set
            {
                ownCable = value;
                CableId = ownCable.CableId;
            }
        }

        public dRFormula DRFormula
        {
            get
            {
                if (drFormula == null) loadDRFormula();
                return drFormula;
            }
            set
            {
                drFormula = value;
            }
        }

        private void loadDRFormula()
        {
            drFormula = dRFormula.find_drFormula_by_id(DRFormulaId);
        }

        /// <summary>
        /// Может ли тип параметра измеряться на данном типе структуры
        /// </summary>
        /// <param name="parameter_type_id"></param>
        /// <returns></returns>
        public bool IsAllowParameterType(uint parameter_type_id)
        {
            DBEntityTable t = new DBEntityTable(typeof(MeasuredParameterType));
            return StructureType.MeasuredParameterTypes.Select($"{t.PrimaryKey[0].ColumnName} = {parameter_type_id}").Length > 0 ;
        }


        protected CableStructureType structureType;
        protected DBEntityTable measuredParameters;
        protected DBEntityTable measuredParameters_was;
        protected Cable ownCable;
        protected dRFormula drFormula;
    }

    [DBTable("struktury_cab", "bd_isp", OldDBName = "bd_isp", OldTableName = "structury_cab")]
    public class TestedCableStructure : CableStructure
    {
        public TestedCableStructure(DataRowBuilder builder) : base(builder)
        {
        }

        public TestedCable testedCable = null;

        public TestedCable TestedCable
        {
            get
            {
                if (OwnCable != null && testedCable == null)
                {
                    testedCable = (TestedCable)OwnCable;
                }
                return testedCable;
            }
        } 


        public new static TestedCableStructure find_by_structure_id(uint structure_id)
        {
            DBEntityTable t = find_by_primary_key(structure_id, typeof(TestedCableStructure));
            TestedCableStructure structure = null;
            if (t.Rows.Count > 0) structure = (TestedCableStructure)t.Rows[0];
            return structure;
        }

        public static DBEntityTable get_by_cable(TestedCable cable)
        {
            DBEntityTable t = new DBEntityTable(typeof(TestedCable));
            DBEntityTable cabStructures = find_by_criteria($"{t.PrimaryKey[0].ColumnName} = {cable.CableId}", typeof(TestedCableStructure));
            foreach (TestedCableStructure cs in cabStructures.Rows) cs.OwnCable = cable;

            return cabStructures;
        }

        public new void CopyFromStructure(CableStructure structure)
        {
            this.FillColsFromEntity(structure);
            //this.AddMeasuredParameterDataFromStructure(structure);
        }

        /*
        private void AddMeasuredParameterDataFromStructure(CableStructure structure)
        {
            this.MeasuredParameters.Clear();
            foreach (CableStructureMeasuredParameterData data in structure.MeasuredParameters.Rows)
            {
                TestedStructureMeasuredParameterData dta = (TestedStructureMeasuredParameterData)MeasuredParameters.NewRow();
                dta.FillColsFromEntity(data);
                dta.CableStructureId = this.CableStructureId;

                this.MeasuredParameters.Rows.Add(dta);
            }

        }
        */

        [DBColumn(Cable.CableId_ColumnName, ColumnDomain.UInt, Order = 11, OldDBColumnName = "CabNum", Nullable = true, ReferenceTo = "cables("+ Cable.CableId_ColumnName +") ON DELETE CASCADE")]
        public new uint CableId
        {
            get
            {
                return tryParseUInt(Cable.CableId_ColumnName);
            }
            set
            {
                this[Cable.CableId_ColumnName] = value;
            }
        }

        public override DBEntityTable MeasuredParameters
        {
            get
            {

                if (measuredParameters == null)
                {
                    if (IsNewRecord())
                    {
                        measuredParameters = MeasuredParameterData.get_tested_structure_measured_parameters(0);
                    }
                    else
                    {
                        measuredParameters = MeasuredParameterData.get_tested_structure_measured_parameters(this.CableStructureId); 
                    }
                    if (measuredParameters.Rows.Count > 0)
                    {
                        MeasuredParameterData[] Kparams = (MeasuredParameterData[])measuredParameters.Select($"{MeasuredParameterType.ParameterTypeId_ColumnName} in ({MeasuredParameterType.K9_12}, {MeasuredParameterType.K23})");
                        foreach(MeasuredParameterData k_pd in Kparams)
                        {
                           DataRow[] kDatas = k_pd.MakeSplit_K_Parameters();
                           
                           foreach(MeasuredParameterData d in kDatas)
                            {
                                d.TestedStructure = this;
                                measuredParameters.ImportRow(d);
                            }
                           
                        }
                        foreach (MeasuredParameterData mpd in measuredParameters.Rows)
                        {
                            mpd.TestedStructure = this;
                        }
                    }
                    //measuredParameters = tmp;
                }
                return measuredParameters;
            }
        }

        public bool HasMeasuredParameters => MeasuredParameters.Rows.Count > 0;



        public bool HasResultsByParameter(uint parameter_type_id)
        {
            Debug.WriteLine($"HasResultsByParameter: ptype = {parameter_type_id}, Count = {GetTestResultsByParameterTypeId(parameter_type_id).Rows.Count}");
            return GetTestResultsByParameterTypeId(parameter_type_id).Rows.Count > 0;
        }

        public DBEntityTable GetAffectedResult()
        {
            DataRow[] rows = TestedCable.Test.TestResults.Select($"ParamInd = {MeasuredParameterType.Calling} AND {CableStructure.StructureId_ColumnName} = {CableStructureId} AND Resultat > {LeadTestStatus.Ok}");
            DBEntityTable t = new DBEntityTable(typeof(CableTestResult), rows);
            return t;
        }

        private DBEntityTable affectedElements;

        public DBEntityTable AffectedElements
        {
            get
            {
                if (affectedElements == null)
                {
                    affectedElements = GetAffectedResult();
                }
                return affectedElements;
            }
        }

        public LeadTestStatus GetElementStatus(uint element_number)
        {
            DataRow[] sts = AffectedElements.Select($"{CableTestResult.StructElementNumber_ColumnName} = {element_number}");
            float stsId = LeadTestStatus.Ok;
            if (sts.Length > 0)
            {
                foreach(DataRow r in sts)
                {
                    float curSts = ((CableTestResult)r).Result;
                    if (stsId < curSts) stsId = curSts; 
                }
            }
            return (LeadTestStatus)TestedCable.Test.LeadStatuses.Select($"{LeadTestStatus.StatusId_ColumnName} = {stsId}")[0];
        }


        public DBEntityTable GetTestResultsByParameterTypeId(uint parameter_type_id)
        {
            if (!TestResults_Dictionary.ContainsKey(parameter_type_id))
            {
                DataRow[] rows = TestedCable.Test.TestResults.Select($"ParamInd = {parameter_type_id} AND {CableStructure.StructureId_ColumnName} = {CableStructureId} ");
                DBEntityTable t = new DBEntityTable(typeof(CableTestResult), rows);

                //CableTestResult.find_by_structure_id_and_parameter_type_id(CableStructureId, parameter_type_id);
                TestResults_Dictionary.Add(parameter_type_id, t);
            }
            return TestResults_Dictionary[parameter_type_id];
        }

        internal DataRow[] GetLosted_TestResults_ByParameterTypeId_ForFreqParameters(MeasuredParameterData measuredParameterData)
        {
            DBEntityTable results = GetTestResultsByParameterTypeId(measuredParameterData.ParameterTypeId);
            DataRow[] returnedRows = new DataRow[] { };
            DBEntityTable sameFreqRanges = FrequencyRange.find_by_min_freq(measuredParameterData.FrequencyMin);
            uint failedRangeId = 0;
            foreach (FrequencyRange r in sameFreqRanges.Rows)
            {
                if (r.FrequencyRangeId != measuredParameterData.FrequencyRangeId)
                {
                    DataRow[] pDataUsingCurRange = MeasuredParameters.Select($"FreqDiap = {r.FrequencyRangeId}");
                    if (pDataUsingCurRange.Length == 0)
                    {
                        DataRow[] resWithThatRange = results.Select($"FreqDiap = {r.FrequencyRangeId}");
                        if (resWithThatRange.Length > 0)
                        {
                            failedRangeId = r.FrequencyRangeId;
                            results.WriteSingleQuery($"UPDATE {results.TableName} SET FreqDiap = {measuredParameterData.FrequencyRangeId} WHERE {CableTest.CableTestId_ColumnName} = {TestedCable.Test.TestId} AND {MeasuredParameterType.ParameterTypeId_ColumnName} = {measuredParameterData.ParameterTypeId} AND FreqDiap = {failedRangeId} AND {CableStructure.StructureId_ColumnName} = {CableStructureId}");
                            foreach (CableTestResult res in resWithThatRange) res.FrequencyRangeId = measuredParameterData.FrequencyRangeId;
                            returnedRows = resWithThatRange;
                            break;
                        }
                    }
                }
            }
            return returnedRows;
        }

        public MeasuredParameterType[] TestedParameterTypes
        {
            get
            {
                if (testedParameterTypes == null)
                {
                    List<MeasuredParameterType> types = new List<MeasuredParameterType>();
                    List<uint> ids = new List<uint>();
                    foreach (MeasuredParameterData pd in MeasuredParameters.Rows) ids.Add(pd.ParameterTypeId);
                    foreach(MeasuredParameterType type in MeasuredParameterType.get_all_by_ids_as_array(ids.Distinct().ToArray()))
                    {
                        if (HasResultsByParameter(type.ParameterTypeId)) types.Add(type);
                    }
                    testedParameterTypes = types.ToArray();
                }
                return testedParameterTypes;
            }
        }

        public int[] BrokenElements
        {
            get
            {
                if (brokenElements == null)
                {
                    List<int> brokenElementsList = new List<int>();
                    CableTestResult[] brokenElsRows = (CableTestResult[])(AffectedElements.Select($"{Tables.CableTestResult.MeasureResult_ColumnName} = {LeadTestStatus.Broken}"));
                    foreach(CableTestResult r in brokenElsRows)
                    {
                        brokenElementsList.Add((int)r.ElementNumber);
                    }
                    brokenElements = brokenElementsList.Distinct().ToArray();
                }
                return brokenElements;
            }
        }

        public uint[] BadElements
        {
            get
            {
                List<uint> badElsIds = new List<uint>();
                foreach (CableTestResult r in AffectedElements.Rows) badElsIds.Add(r.ElementNumber);
                foreach (MeasuredParameterType pType in TestedParameterTypes)
                {
                    MeasuredParameterData[] parameter_data_list = GetAll_MeasuredParameterData_By_ParameterTypeId(pType.ParameterTypeId);
                    foreach(MeasuredParameterData pd in parameter_data_list)
                    {
                        if (pd.TestResults.Rows.Count == 0) continue;
                        CableTestResult[] out_of_norma_results = (CableTestResult[])pd.TestResults.Select($"{CableTestResult.IsOutOfNorma_ColumnName} = {true}");
                        foreach(CableTestResult r in out_of_norma_results)
                        {
                            badElsIds.Add(r.ElementNumber);
                            if (r.GeneratorElementNumber > 0) badElsIds.Add(r.GeneratorElementNumber);
                        }
                    }
                }
                return badElsIds.Distinct().ToArray();
            }
        }

        public object NormalElementsAmount
        {
            get
            {
                return RealAmount - BadElements.Length;
            }
        }

        private int[] brokenElements;
        private MeasuredParameterType[] testedParameterTypes;
        protected Dictionary<uint, DBEntityTable> TestResults_Dictionary = new Dictionary<uint, DBEntityTable>();


    }




}
