using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using NormaMeasure.Utils;
using System.IO;

namespace NormaMeasure.DBControl.Tables
{

    [DBTable("ispytan", "bd_isp", OldDBName = "bd_isp", OldTableName = "ispytan")]
    public class CableTest : BaseEntity
    {
        public CableTest(DataRowBuilder builder) : base(builder)
        {
        }

        private static string selectQueryForTestList
        {
            get
            {
                DBEntityTable ct = new DBEntityTable(typeof(CableTest));
                DBEntityTable tc = new DBEntityTable(typeof(TestedCable));
                DBEntityTable b = new DBEntityTable(typeof(ReleasedBaraban));
                string selectQuery = ct.SelectQuery.Replace("*", $"*, CONCAT({tc.TableName}.{TestedCable.CableName_ColumnName}, ' ', {tc.TableName}.{TestedCable.StructName_ColumnName}) AS cable_name, {b.TableName}.{ReleasedBaraban.BarabanSerialNumber_ColumnName} AS baraban_name");

                selectQuery = $"{selectQuery} LEFT OUTER JOIN {tc.TableName} USING({tc.PrimaryKey[0].ColumnName}) LEFT OUTER JOIN {b.TableName} USING({b.PrimaryKey[0].ColumnName})";
                return selectQuery;
            }
        }

        public static DBEntityTable find_by_id_for_test_list(uint test_id)
        {
            string q = $"{selectQueryForTestList} WHERE {CableTest.CableTestId_ColumnName} = {test_id} limit 1";
            return find_by_query(q, typeof(CableTest));
        }

        public static DBEntityTable find_all_for_test_list()
        {
            return find_by_query(selectQueryForTestList, typeof(CableTest));
        }

        public static DBEntityTable find_by_date_for_test_list(string date_from, string date_to)
        {
            string q = $"{selectQueryForTestList} WHERE {CableTest.TestDate_ColumnName} >= '{date_from}' AND {CableTest.TestDate_ColumnName} <= '{date_to}'";
            return find_by_query(q, typeof(CableTest));
        }

        public static CableTest GetLastOrCreateNew()
        {
            DBEntityTable t = find_stoped_tests();
            CableTest test;
            if (t.Rows.Count > 0)
            {
                test= (CableTest)t.Rows[0];
            }else
            {
                test = get_new_test();
            }
            return test;

        }

        public static CableTest get_new_test()
        {
            CableTest test = find_not_started_test();
            return test;
        }

        public static CableTest find_not_started_test()
        {
            string select_cmd = $"{CableTestStatus.StatusId_ColumnName} = {CableTestStatus.NotStarted}";
            DBEntityTable t = find_by_criteria(select_cmd, typeof(CableTest));
            if (t.Rows.Count > 0) return (CableTest)t.Rows[0];
            else return create_not_started_test();
        }

        private static CableTest create_not_started_test()
        {
            DBEntityTable t = new DBEntityTable(typeof(CableTest));
            CableTest test = (CableTest)t.NewRow();
            test.StatusId = CableTestStatus.NotStarted;
            test.TestId = 0;
            test.CableLength = 1000;
            test.Temperature = 20;
            test.Save();
            t.Rows.Add(test);
            return test;
        }


        public void SetNotStarted()
        {
            SetStatus(CableTestStatus.NotStarted);
            if (HasTestedCable)
            {
                TestedCable.Destroy();
                testedCable = null;
            }
        }

        public void SetStarted()
        {
            GetTestedCable();
            SetStatus(CableTestStatus.Started);
        }


        public void SetStoppedByOperator()
        {
            SetStatus(CableTestStatus.StopedByOperator);
        }

        private void SetStatus(uint status_id)
        {
            this.StatusId = status_id;
            this.Save();
        }

        private void GetTestedCable()
        {
            if (TestedCable == null)
            {
                TestedCable = TestedCable.create_for_test(this);
            }
        }

        private static DBEntityTable find_stoped_tests()
        {
            string select_cmd = $"{CableTestStatus.StatusId_ColumnName} IN ({CableTestStatus.StopedByOperator}, {CableTestStatus.StopedOutOfNorma}, {CableTestStatus.Started})";
            return find_by_criteria(select_cmd, typeof(CableTest));
        }

        public bool IsFinished => StatusId == CableTestStatus.Finished;
        public bool IsNotStarted => StatusId == CableTestStatus.NotStarted;
        public bool IsInterrupted => StatusId == CableTestStatus.StopedByOperator || StatusId == CableTestStatus.StopedOutOfNorma;




        /// <summary>
        /// Все доступные измеряемые параметры для данного кабеля
        /// </summary>
        public MeasuredParameterType[] AllowedMeasuredParameterTypes
        {
            get
            {
                if (measuredParameterTypes == null)
                {
                    measuredParameterTypes = getFromTestedCable();
                }
                return measuredParameterTypes;
            }
            set
            {
                measuredParameterTypes = value;
            }
        }

        public uint[] MeasuredParameterTypes_IDs
        {
            get
            {
                if(measuredParameterTypes_IDs == null)
                {
                    List<uint> ids = new List<uint>();
                    foreach (MeasuredParameterType type in AllowedMeasuredParameterTypes) ids.Add(type.ParameterTypeId);
                    if (ids.Count > 0) measuredParameterTypes_IDs = ids.ToArray();
                }
                return measuredParameterTypes_IDs;
            }
        }

        public TestedCable TestedCable
        {
            get
            {
                if (testedCable == null)
                {
                    testedCable = TestedCable.find_by_cable_id(SourceCableId);
                    if (testedCable != null) testedCable.Test = this;
                }
                return testedCable;
            }
            set
            {
                testedCable = value;
            }
        }


        public Cable SourceCable
        {
            get
            {
                if (sourceCable == null)
                {
                    sourceCable = Cable.find_by_cable_id(SourceCableId);
                    
                }
                return sourceCable;
            }
            set
            {
                sourceCable = value;
                SourceCableId = sourceCable.CableId;
                CleanCableVariables();
            }
        }

        public ReleasedBaraban ReleasedBaraban
        {
            get
            {
                if(releasedBaraban == null)
                {
                    releasedBaraban = ReleasedBaraban.find_by_id(BarabanId);
                }
                return releasedBaraban;
            }
        }

        /// <summary>
        /// Очищаем переменные связанные с исходным кабелем
        /// </summary>
        private void CleanCableVariables()
        {
            measuredParameterTypes = null;
            measuredParameterTypes_IDs = null;
            if (TestedCable != null)
            {
                TestedCable.Destroy();
                TestedCable = null;
            }
        }

        private MeasuredParameterType[] getFromTestedCable()
        {   
            List<uint> ids = new List<uint>();
            if (SourceCable == null && TestedCable == null) return null;
            ids.Add(MeasuredParameterType.Calling);
            uint[] idsFromCable = TestedCable == null ? SourceCable.MeasuredParameterTypes_IDs : TestedCable.MeasuredParameterTypes_IDs;
            foreach (uint id in idsFromCable) ids.Add(id);
            return MeasuredParameterType.get_all_by_ids_as_array(ids.ToArray());
        }


        #region Колонки таблицы
        [DBColumn(CableTestId_ColumnName, ColumnDomain.UInt, Order = 10, OldDBColumnName = "IspInd", Nullable = true, DefaultValue =0, IsPrimaryKey = true, AutoIncrement = true)]
        public uint TestId
        {
            get
            {
                return tryParseUInt(CableTestId_ColumnName);
            }
            set
            {
                this[CableTestId_ColumnName] = value;
            }
        }

        [DBColumn(TestedCable.CableId_ColumnName, ColumnDomain.UInt, Order = 11, Nullable =true, DefaultValue = 0)]
        public uint SourceCableId
        {
            get
            {
                return tryParseUInt(TestedCable.CableId_ColumnName);
            }
            set
            {
                this[TestedCable.CableId_ColumnName] = value;
            }
        }

        [DBColumn(ReleasedBaraban.BarabanId_ColumnName, ColumnDomain.UInt, Order = 12, DefaultValue = 0, OldDBColumnName = "BarabanInd", Nullable = true)]
        public uint BarabanId
        {
            get
            {
                return tryParseUInt(ReleasedBaraban.BarabanId_ColumnName);
            }
            set
            {
                this[ReleasedBaraban.BarabanId_ColumnName] = value;
            }
        }

        [DBColumn(OperatorId_ColumnName, ColumnDomain.UInt, Order = 13, OldDBColumnName = "Operator", DefaultValue = 0, Nullable = true)]
        public uint OperatorId
        {
            get
            {
                return tryParseUInt(OperatorId_ColumnName);
            }
            set
            {
                this[OperatorId_ColumnName] = value;
            }
        }

        [DBColumn(CableTestStatus.StatusId_ColumnName, ColumnDomain.UInt, Order = 14, OldDBColumnName = "Status", DefaultValue = CableTestStatus.NotStarted, Nullable = false)]
        public uint StatusId
        {
            get
            {
                return tryParseUInt(CableTestStatus.StatusId_ColumnName);
            }
            set
            {
                this[CableTestStatus.StatusId_ColumnName] = value;
            }
        }

        [DBColumn(Temperature_ColumnName, ColumnDomain.Float, Order = 15, OldDBColumnName = "Temperetur", DefaultValue = 20, Nullable = true)]
        public float Temperature
        {
            get
            {
                return tryParseFloat(Temperature_ColumnName);
            }
            set
            {
                this[Temperature_ColumnName] = value;
            }
        }

        [DBColumn(CableLength_ColumnName, ColumnDomain.Float, Order = 16, OldDBColumnName = "CabelLengt", DefaultValue = 1000, Nullable = true)]
        public float CableLength
        {
            get
            {
                return tryParseFloat(CableLength_ColumnName);
            }
            set
            {
                this[CableLength_ColumnName] = value;
            }
        }

        [DBColumn(VSVI_ColumnName, ColumnDomain.Boolean, Order = 17, OldDBColumnName = "Vsvi", DefaultValue = 0, Nullable = true)]
        public bool VSVILeadLeadResult
        {
            get
            {
                return tryParseBoolean(VSVI_ColumnName, false);
            }
            set
            {
                this[VSVI_ColumnName] = value;
            }
        }

        [DBColumn(VSVISkin_ColumnName, ColumnDomain.Boolean, Order = 18, OldDBColumnName = "Vsvi_Obol", DefaultValue = 0, Nullable = true)]
        public bool VSVILeadShieldResult
        {
            get
            {
                return tryParseBoolean(VSVISkin_ColumnName, false);
            }
            set
            {
                this[VSVISkin_ColumnName] = value;
            }
        }

        [DBColumn(NettoWeight_ColumnName, ColumnDomain.Float, Order = 19, OldDBColumnName = "Netto", DefaultValue = 0, Nullable = true)]
        public float NettoWeight
        {
            get
            {
                return tryParseFloat(NettoWeight_ColumnName);
            }
            set
            {
                this[NettoWeight_ColumnName] = value;
            }
        }

        [DBColumn(BruttoWeight_ColumnName, ColumnDomain.Float, Order = 20, OldDBColumnName = "Brutto", DefaultValue = 0, Nullable = true)]
        public float BruttoWeight
        {
            get
            {
                return tryParseFloat(BruttoWeight_ColumnName);
            }
            set
            {
                this[BruttoWeight_ColumnName] = value;
            }
        }

        [DBColumn(TestDate_ColumnName, ColumnDomain.DateTime, Order = 21, OldDBColumnName = "IspData", Nullable = true)]
        public DateTime TestDate
        {
            get
            {
                return tryParceDateTime(TestDate_ColumnName);
            }
            set
            {
                this[TestDate_ColumnName] = value;
            }
        }

        [DBColumn("cable_name", ColumnDomain.Tinytext, Order = 22, Nullable = true, IsVirtual = true)]
        public string CableName
        {
            get
            {
                return this["cable_name"].ToString();
            }
            set
            {
                this["cable_name"] = value;
            }
        }

        [DBColumn("baraban_name", ColumnDomain.Tinytext, Order = 23, Nullable = true, IsVirtual = true)]
        public string BarabanName
        {
            get
            {
                return this["baraban_name"].ToString();
            }
            set
            {
                this["baraban_name"] = value;
            }
        }

        public uint TestedCableId
        {
            get
            {
                return HasTestedCable ? TestedCable.CableId : 0;
            }
        }









        public const string CableTestId_ColumnName = "IspInd";
        public const string TestedCableId_ColumnName = "CabNum";
       // public const string SourceCableId_ColumnName = "source_cable_id";
        public const string OperatorId_ColumnName = "operator";
        public const string Temperature_ColumnName = "Temperatur";
        public const string CableLength_ColumnName = "CabelLengt";
        public const string VSVI_ColumnName = "vsvi";
        public const string VSVISkin_ColumnName = "vsvi_obol";
        public const string NettoWeight_ColumnName = "netto";
        public const string BruttoWeight_ColumnName = "brutto";
        public const string TestDate_ColumnName = "IspData";


        #endregion

        public bool HasTestedCable => TestedCableId > 0;
        public bool HasSourceCable => SourceCableId > 0;
        public bool HasOperator => OperatorId > 0;
        public bool HasBaraban => ReleasedBaraban != null;

        public string TestDateString => TestDate.ToString("dd.MM.yyyy в hh:mm");

        public MeasuredParameterType[] measuredParameterTypes;
        public uint[] measuredParameterTypes_IDs;

        private Cable sourceCable;
        private TestedCable testedCable;
        private ReleasedBaraban releasedBaraban;

        public DBEntityTable TestResults
        {
            get
            {
                if(testResults == null)
                {
                    testResults = CableTestResult.find_by_criteria($"IspInd = {TestId}", typeof(CableTestResult));
                }
                return testResults;
            }

        }
        private DBEntityTable testResults; 

        public User Operator
        {
            get
            {
                if (_operator == null && HasOperator)
                {
                  
                    _operator = User.get_by_id(OperatorId);
                }
                return _operator;
            }
        }

        private User _operator;


        private DBEntityTable leadStatuses;
        public DBEntityTable LeadStatuses
        {
            get
            {
                if (leadStatuses == null) leadStatuses = LeadTestStatus.get_all();
                return leadStatuses;
            }
        }



    }



}
