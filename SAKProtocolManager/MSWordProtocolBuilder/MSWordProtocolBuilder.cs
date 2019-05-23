using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAKProtocolManager.DBEntities;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;
using System.Diagnostics;


namespace SAKProtocolManager.MSWordProtocolBuilder
{
    public class MSWordProtocolBuilder
    {
        static CableTest CableTest;
        static MSWordProtocol wordProtocol;
        public static void BuildProtocolForTest(CableTest test)
        {
            CableTest = test;
            wordProtocol = new MSWordProtocol();
            wordProtocol.Init();
            foreach (CableStructure s in CableTest.TestedCable.Structures)
            {
                PrintStructure(s);
            }
            wordProtocol.Finalise();
        }

        public static void PrintStructure(CableStructure structure)
        {
            addPrimaryParametersTable(structure);

        }

        private static void addPrimaryParametersTable(CableStructure structure)
        {
            int maxCols = 19;
            int colsCount = 1; //Первая колонка номер элемента
            List<MeasureParameterType> typesForTable = new List<MeasureParameterType>();

            for(int i = 0; i< structure.MeasuredParameters.Length; i++)
            {
                MeasureParameterType mpt = structure.MeasuredParameters[i];
                bool needToBuildTable = false;
                if (mpt.IsPrimaryParameter)
                {
                    int colsForParameter = ColumsCountForParameter(mpt, structure);
                    if ((colsCount + colsForParameter) > maxCols)
                    {
                        needToBuildTable = true;
                    } else
                    {
                        typesForTable.Add(mpt);
                        colsCount += ColumsCountForParameter(mpt, structure);
                    }
                }
                if (needToBuildTable || ((i+1) == structure.MeasuredParameters.Length && typesForTable.Count > 0))
                {
                    BuildPrimaryParametersTable(typesForTable.ToArray(), structure, colsCount);
                    typesForTable.Clear();
                    colsCount = 1;
                }
            }
        }

        private static void BuildPrimaryParametersTable(MeasureParameterType[] pTypes, CableStructure structure, int cols)
        {
            int curElementNumber = 1;
            int maxElementsPerTable = 50;
            do
            {
                int rows = 2;
                rows += (curElementNumber + maxElementsPerTable) > structure.RealNumberInCable ? structure.RealNumberInCable - curElementNumber : maxElementsPerTable;
                Word.Shape tableShape = wordProtocol.AddTable(cols, rows);
                tableShape.Width = cols * 30f;
                tableShape.Height = rows * 11.5f;
                tableShape.Line.Transparency = 1f;
                Word.Table table = tableShape.TextFrame.TextRange.Tables[1];
                BuildPrimaryParamsTableHeader(pTypes, structure, table);
                for(int i = 0; i< maxElementsPerTable; i++)
                {
                    table.Cell(i+3, 1).Range.Text = curElementNumber.ToString();
                    //curElementNumber++;
                    if (++curElementNumber > structure.RealNumberInCable) break;
                }
                //wordProtocol.ResizeShapeByTable(tableShape);
                // curElementNumber += maxElementsPerTable;

            } while (curElementNumber <= structure.RealNumberInCable);


        }

        /// <summary>
        /// Создаём шапку для таблицы первичных параметров
        /// </summary>
        /// <param name="pTypes"></param>
        /// <param name="structure"></param>
        /// <param name="table"></param>
        private static void BuildPrimaryParamsTableHeader(MeasureParameterType[] pTypes, CableStructure structure, Word.Table table)
        {
            int pNameColNumb = 1;
            int elColNumb = 1;
            table.Cell(1, 1).Merge(table.Cell(2, 1));
            table.Cell(1, 1).Range.Text = "№/№";
            elColNumb += 1;
            pNameColNumb += 1;
            for (int i = 0; i < pTypes.Length; i++)
            {
                MeasureParameterType mpt = pTypes[i];
                int colsForParameter = ColumsCountForParameter(mpt, structure);

                table.Cell(1, pNameColNumb).Range.Text = mpt.Name;
                if (colsForParameter > 1)
                {
                    table.Cell(1, pNameColNumb).Merge(table.Cell(1, pNameColNumb + colsForParameter - 1));
                    for (int x = 0; x < colsForParameter; x++)
                    {
                        table.Cell(2, elColNumb + x).Range.Text = (x + 1).ToString();
                    }
                }
                else
                {
                    table.Cell(1, pNameColNumb).Merge(table.Cell(2, elColNumb));
                }
                pNameColNumb += 1;
                elColNumb += colsForParameter;
            }
        }


        private static int ColumsCountForParameter(MeasureParameterType t, CableStructure s)
        {
            int LeadsNumber = s.BendingTypeLeadsNumber;
            switch (t.Id)
            {
                case MeasureParameterType.Rleads:
                case MeasureParameterType.Risol1:
                case MeasureParameterType.Risol2:
                case MeasureParameterType.Co:
                    return LeadsNumber;
                case MeasureParameterType.Cp:
                case MeasureParameterType.Ea:
                case MeasureParameterType.dR:
                    return (LeadsNumber / 2);
                default:
                    return 1;
            }
        }
        private static int CalcPrimaryParamsColumnsCount(CableStructure structure)
        {
            int count = 0;

            foreach (MeasureParameterType mpt in structure.MeasuredParameters)
            {
                if (mpt.IsPrimaryParameter)
                {
                    count += ColumsCountForParameter(mpt, structure);
                }
            }
            return count;
        }
    }

    public class MSWordProtocol
    {
        private Word.Application WordApp;
        private Word.Document WordDocument;
        private object oMissing = System.Reflection.Missing.Value;

        public MSWordProtocol()
        {
        }

        public void Init()
        {
            WordApp = new Word.Application();
            WordDocument = WordApp.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            WordDocument.PageSetup.LeftMargin = MarginLeft;
            WordDocument.PageSetup.RightMargin = MarginRight;
            WordDocument.PageSetup.TopMargin = MarginTop;
            WordDocument.PageSetup.BottomMargin = MarginBottom;
        }

        public void Finalise()
        {
            PlaceShapes();
            WordDocument.Saved = true;
            if (WordApp != null) WordApp.Visible = true;
        }

        public Word.Shape AddTable(int cols, int rows) //ProtocolTable table
        {
            Word.Shape oShape = CreateShape();
            Table oTab = BuildTable(cols, rows, oShape);
            return oShape;

        }

        public void AddTestTable()
        {
            Word.Shape oShape = CreateShape();
            Table oTab = BuildTable(10, 10, oShape);
            oTab.Cell(1, 1).Merge(oTab.Cell(2, 1));
            oTab.Cell(1, 2).Merge(oTab.Cell(1, 3));
            oTab.Cell(1, 3).Merge(oTab.Cell(2, 4));
            oTab.Cell(1, 4).Merge(oTab.Cell(1, 5));
            oTab.Cell(1, 5).Merge(oTab.Cell(2, 7));
            oTab.Cell(1, 6).Merge(oTab.Cell(1, 8));
            ResizeShapeByTable(oShape);
        }



        private Table BuildTable(int cols, int rows, Word.Shape oShape)
        {
            object b1 = WdDefaultTableBehavior.wdWord9TableBehavior;
            object b2 = WdAutoFitBehavior.wdAutoFitContent;
            Table oTab = oShape.TextFrame.TextRange.Tables.Add(oShape.TextFrame.TextRange, rows, cols, ref b1, ref b2);
            oTab.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            return oTab;
        }




        private Word.Shape CreateShape(float height = 50f, float width = 600f)
        {
            Word.Shape oShape = WordDocument.Shapes.AddShape(1, 50, 50, width, height, ref oMissing);
            oShape.TextFrame.TextRange.Font.Size = FontSize;
            oShape.TextFrame.TextRange.Font.Name = FontName;
            oShape.TextFrame.TextRange.Font.Color = FontColor;
            oShape.Fill.Transparency = 1f;
            return oShape;
        }

        public void PlaceShapes()
        {
            object oEndOfDoc = "\\endofdoc";
            float cx = 0f;
            float wd = WordDocument.PageSetup.PageWidth - WordDocument.PageSetup.LeftMargin;// -WordDoc.PageSetup.RightMargin;
            float ht = WordDocument.PageSetup.PageHeight - WordDocument.PageSetup.TopMargin;// -WordDoc.PageSetup.BottomMargin;
            int cpage = 0;
            List<ShapeCoord> lst = new List<ShapeCoord>();
            float[] line = new float[(int)wd];
            foreach (Word.Shape oShape in WordDocument.Shapes)
            {
                //CutShape(oShape);
                float w = oShape.Width;
                float h = oShape.Height;
                if (w > wd) w = wd;
                if ((cx + w) > wd) cx = 0f;
                if ((GetLine(line, (int)cx, (int)w) + h) > ht)
                {
                    object ob = WdBreakType.wdPageBreak;
                    WordDocument.Bookmarks.get_Item(ref oEndOfDoc).Range.InsertBreak(ref ob);
                    for (int ps = 0; ps < line.Length; ps++) line[ps] = 0f;
                    cx = 0f;
                    cpage++;
                }
                lst.Add(new ShapeCoord(cx, GetLine(line, (int)cx, (int)w), cpage));
                SetLine(line, (int)cx, (int)w, (int)h);
                cx += w;
            }
            object pos = 1;
            for (int i = 0; i < WordDocument.Shapes.Count; i++)
            {
                if (lst[i].page == 0)
                {
                    pos = i + 1;
                    WordDocument.Shapes.get_Item(ref pos).Left = lst[i].x;
                    WordDocument.Shapes.get_Item(ref pos).Top = lst[i].y;
                    pos = i + 2;
                }
                else
                {
                    Word.ShapeRange srng = WordDocument.Shapes.Range(ref pos);
                    object jdx = true;
                    srng.Select(ref jdx);
                    WordDocument.ActiveWindow.Selection.Cut();
                    int tm = lst[i].page;
                    while (tm-- > 0) WordDocument.ActiveWindow.Selection.GoToNext(WdGoToItem.wdGoToPage);
                    WordDocument.ActiveWindow.Selection.Paste();
                    object tp = WordDocument.Shapes.Count;
                    WordDocument.Shapes.get_Item(ref tp).Left = lst[i].x;
                    WordDocument.Shapes.get_Item(ref tp).Top = lst[i].y;
                }
            }
        }

       

        public void ResizeShapeByTable(Word.Shape oShape)
        {
            oShape.Line.Transparency = 1f;

            if (oShape.TextFrame.TextRange.Tables.Count > 0)
            {
                float width = 20f;
                float height = oShape.TextFrame.TextRange.Tables[1].Rows.Count * FontSize * 1.44f;
                for (int i = 1; ; i++)
                {
                    try
                    {
                        width += oShape.TextFrame.TextRange.Tables[1].Cell(1, i).Width;
                    }
                    catch (System.Runtime.InteropServices.COMException ex)
                    {
                        break;
                    }
                }
                //foreach(Cell cell in oShape.TextFrame.TextRange.Tables[1].Rows[1].Cells) width += cell.Width;
                oShape.Width = width;
                oShape.Height = height;
                Debug.WriteLine($"MSWordProtocol.CutShape(): RowsHeight = {oShape.TextFrame.TextRange.Tables[1].Rows.Height}");
                Debug.WriteLine($"MSWordProtocol.CutShape(): CalculatedWidth = {width}; CalculatedHeight = {height}");
            }
        }

        private float GetLine(float[] mass, int begin, int width)
        {
            float ret = 0f;
            for (int pos = begin; pos < (begin + width); pos++) if (mass[pos] > ret) ret = mass[pos];
            return ret;
        }
        private void SetLine(float[] mass, int begin, int width, float height)
        {
            float val = GetLine(mass, begin, width) + height;
            for (int pos = begin; pos < (begin + width); pos++) mass[pos] = val;
        }


        private Word.WdColor FontColor
        {
            get
            {
                return WordProtocolSettings.Default.FontColor;
            }
            set
            {
                WordProtocolSettings.Default.FontColor = value;
                WordProtocolSettings.Default.Save();
            }
        }

        private float FontSize
        {
            get
            {
                return WordProtocolSettings.Default.FontSize;
            }
            set
            {
                WordProtocolSettings.Default.FontSize = value;
                WordProtocolSettings.Default.Save();
            }
        }

        private string FontName
        {
            get
            {
                return WordProtocolSettings.Default.FontName;
            }
            set
            {
                WordProtocolSettings.Default.FontName = value;
                WordProtocolSettings.Default.Save();
            }
        }


        private float MarginRight
        {
            get
            {
                return WordProtocolSettings.Default.Page_MarginRight;

            }
        }

        private float MarginTop
        {
            get
            {
                return WordProtocolSettings.Default.Page_MarginTop;

            }
        }
        private float MarginLeft
        {
            get
            {
                return WordProtocolSettings.Default.Page_MarginLeft;

            }
        }
        private float MarginBottom
        {
            get
            {
                return WordProtocolSettings.Default.Page_MarginBottom;

            }
        }
    }

    internal class ShapeCoord
    {
        public float x;
        public float y;
        public int page;

        public ShapeCoord(float tx, float ty, int tpage)
        {
            x = tx;
            y = ty;
            page = tpage;
        }
    }
}
