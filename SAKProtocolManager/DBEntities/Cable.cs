using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKProtocolManager.DBEntities
{
    public class Cable : DBBase
    {
        public CableStructure[] Structures = new CableStructure[] { };
        public CableTest Test = null;
        public string Name = String.Empty;
        public string CodeOKP = String.Empty;
        public string CodeKCH = String.Empty;
        public string DocumentName = String.Empty;
        public string DocumentNumber = String.Empty;
        public decimal LinearWeight = 0;
        public decimal BuildLength = 0;
        public uint UObol = 0;
        public uint PMin = 0;
        public uint PMax = 0;



        public Cable()
        {
            setDefaultParameters();
        }

        public Cable(CableTest test)
        {
            this.Test = test;
            this.Id = test.CableId;
            setDefaultParameters();
            GetById();
            loadDependencies();
        }

        public Cable(string id)
        {
            this.Id = id;
            setDefaultParameters();
            GetById();
            loadDependencies();
        }

        public Cable(DataRow row)
        {
            fillParametersFromRow(row);
            setDefaultParameters();
            loadDependencies();
        }

        protected override void fillParametersFromRow(DataRow row)
        {
            this.Id = row["cable_id"].ToString();
            this.Name = row["cable_name"].ToString();
            this.CodeOKP = row["cable_code_okp"].ToString();
            this.CodeKCH = row["cable_code_okp_kch"].ToString();
            this.BuildLength = ServiceFunctions.convertToDecimal(row["cable_build_length"]);
            this.LinearWeight = ServiceFunctions.convertToDecimal(row["cable_linear_weight"]);
            this.UObol = ServiceFunctions.convertToUInt(row["cable_u_obol"]);
            this.PMin = ServiceFunctions.convertToUInt(row["cable_p_min"]);
            this.PMax = ServiceFunctions.convertToUInt(row["cable_p_max"]);
            this.DocumentName = row["document_name"].ToString();
            this.DocumentNumber = row["document_number"].ToString();
            
        }
        protected override void setDefaultParameters()
        {
            string selectQuery = "cables.CabNum AS cable_id, CONCAT(cables.CabName,' ', cables.CabNameStruct) AS cable_name, cables.StrLengt AS cable_build_length, cables.DocInd AS document_id, cables.PogMass AS cable_linear_weight, cables.KodOKP AS cable_okp_code, cables.KodOKP_KCH AS cable_okp_kch_code, cables.U_Obol AS cable_u_obol, cables.P_min AS cable_p_min, cables.P_max AS cable_p_max, norm_docum.DocName AS document_name, norm_docum.DocNum AS document_number";
            this.getAllQuery = String.Format("SELECT {0} FROM cables LEFT JOIN norm_docum USING(DocInd)", selectQuery);
            this.getByIdQuery = String.Format("SELECT {0} FROM cables LEFT JOIN norm_docum USING(DocInd) WHERE cables.CabNum = {1}", selectQuery, this.Id);
            colsList = new string[] {
                                        "cable_id",
                                        "cable_name",
                                        "cable_document_id",
                                        "cable_code_okp",
                                        "cable_code_okp_kch",
                                        "cable_build_length",
                                        "cable_linear_weight",
                                        "cable_u_obol",
                                        "cable_p_min",
                                        "cable_p_max",
                                        "document_name",
                                        "document_number"
                                    };
        }

        public Cable[] GetAll()
        {
            Cable[] els = new Cable[] { };
            DataTable dt = GetAllFromDB();
            if (dt.Rows.Count > 0)
            {
                els = new Cable[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++) els[i] = new Cable(dt.Rows[i]);
            }
            return els;
        }

        private void loadDependencies()
        {
            CableStructure st= new CableStructure(this);
            this.Structures = st.GetCableStructures();
        }

        /// <summary>
        /// Выводим структуры у которых есть выход за норму
        /// </summary>
        /// <returns></returns>
        public CableStructure[] GetFailedStructures()
        {
            List<CableStructure> failedStructs = new List<CableStructure>();
            foreach (CableStructure cs in this.Structures)
            {
                if (cs.AffectedElements.Count() > 0)
                {
                    failedStructs.Add(cs);
                    break;
                }
                foreach (MeasureParameterType pt in cs.MeasuredParameters)
                {
                    if (pt.OutOfNormaCount() > 0)
                    {
                        failedStructs.Add(cs);
                        break;
                    }
                }
            }
            return failedStructs.ToArray();
        }
    }
}
