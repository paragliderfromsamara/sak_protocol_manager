using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAKProtocolManager.DBEntities;
using SAKProtocolManager.DBEntities.TestResultEntities;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;
using System.Diagnostics;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using OpenXML =  DocumentFormat.OpenXml.Wordprocessing;
using System.Windows.Forms;


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
                    BuildPrimaryParametersTable_WithOpenXML(typesForTable.ToArray(), structure, colsCount);
                    //BuildPrimaryParametersTable(typesForTable.ToArray(), structure, colsCount);
                    typesForTable.Clear();
                    colsCount = 1;
                }
            }
        }


        private static void BuildPrimaryParametersTable_WithOpenXML(MeasureParameterType[] pTypes, CableStructure structure, int colsAmount)
        {
            int curElementNumber = 1;
            int[] tablesRowsCount = CalcMaxRowsCount(colsAmount, structure.RealNumberInCable + 3);
            Debug.WriteLine($"{structure.RealNumberInCable}");
            for (int idx = 0; idx < tablesRowsCount.Length; idx++)
            {
                int rows = 2 + tablesRowsCount[idx];
                OpenXML.Table table = BuildTable();
                OpenXML.TableRow[] headerRows = BuildPrimaryParamsTableHeader_WithOpenXML(pTypes, structure);
                foreach (OpenXML.TableRow r in headerRows) table.Append(r);
                for (int i = 0; i < tablesRowsCount[idx]; i++)
                {
                    OpenXML.TableRow row = BuildRow();
                    

                    if (curElementNumber <= structure.RealNumberInCable)
                    {
                        OpenXML.TableCell numbCell = BuildCell(curElementNumber.ToString());
                        if (i % 2 == 1) FillCellByColor(numbCell, "ededed");
                        OpenXML.TableCellBorders borderStyle = BuildBordersStyle(0, (uint)((i < tablesRowsCount[idx] - 1) ? 0 : 2) );
                        SetCellBordersStyle(numbCell, borderStyle);
                        row.Append(numbCell); //Ячейка номера элемента
                        for (int pIdx = 0; pIdx < pTypes.Length; pIdx++)//(MeasureParameterType mpt in pTypes)
                        {
                            int elsColsPerParam = ColumsCountForParameter(pTypes[pIdx], structure);
                            TestResult[] results = pTypes[pIdx].ParameterDataList[0].TestResults;
                            int resIdx = (curElementNumber - 1) * elsColsPerParam;
                            for (int rIdx = resIdx; rIdx < resIdx + elsColsPerParam; rIdx++)
                            {
                                TestResult res = results[rIdx];
                                OpenXML.TableCellBorders resBordStyle = BuildBordersStyle(0, (uint)((i < tablesRowsCount[idx] - 1) ? 0 : 2));
                                OpenXML.TableCell resCell = BuildCell(ResultText(res));
                                if (i % 2 == 1) FillCellByColor(resCell, "ededed");
                                SetCellBordersStyle(resCell, resBordStyle);
                                row.Append(resCell);
                            }
                        }
                        table.Append(row);
                        curElementNumber++;
                    }
                    else
                    {
                        OpenXML.TableRow maxValRow = BuildRow();
                        OpenXML.TableRow minValRow = BuildRow();
                        OpenXML.TableRow averValRow = BuildRow();

                        OpenXML.TableCellBorders maxCellTitleBordStyle = BuildBordersStyle(8);

                        maxValRow.Append(BuildCell("max"));
                        SetCellBordersStyle(maxValRow.GetFirstChild<OpenXML.TableCell>(), maxCellTitleBordStyle);
                        minValRow.Append(BuildCell("min"));
                        averValRow.Append(BuildCell("сред."));

                        for (int pIdx = 0; pIdx < pTypes.Length; pIdx++)
                        {
                            int elsColsPerParam = ColumsCountForParameter(pTypes[pIdx], structure);
                            int resIdx = (curElementNumber - 1) * elsColsPerParam;
                            OpenXML.TableCell[] maxValCells = BuildCells(elsColsPerParam, elsColsPerParam > 1);
                            OpenXML.TableCell[] minValCells = BuildCells(elsColsPerParam, elsColsPerParam > 1);
                            OpenXML.TableCell[] averValCells = BuildCells(elsColsPerParam, elsColsPerParam > 1);

                            FillCellText(maxValCells[0], ResultValueText(pTypes[pIdx].ParameterDataList[0].MaxVal));
                            FillCellText(minValCells[0], ResultValueText(pTypes[pIdx].ParameterDataList[0].MinVal));
                            FillCellText(averValCells[0], ResultValueText(pTypes[pIdx].ParameterDataList[0].AverageVal));

                            for (int cIdx = 0; cIdx < elsColsPerParam; cIdx++)
                            {
                                OpenXML.TableCellBorders maxCellBordStyle = BuildBordersStyle(8);
                                SetCellBordersStyle(maxValCells[cIdx], maxCellBordStyle);
                                maxValRow.Append(maxValCells[cIdx]);
                                minValRow.Append(minValCells[cIdx]);
                                averValRow.Append(averValCells[cIdx]);
                            }
                        }
                        table.Append(maxValRow);
                        table.Append(averValRow);
                        table.Append(minValRow);
                        break;
                    }
                }

                wordProtocol.AddTable(table);

            }

        }

        private static OpenXML.TableRow[] BuildPrimaryParamsTableHeader_WithOpenXML(MeasureParameterType[] pTypes, CableStructure structure)
        {
            OpenXML.TableRow row_1 = BuildRow();
            OpenXML.TableRow row_2 = BuildRow();

            OpenXML.TableCell cell_1_1 = BuildCell($"{ "№/№" } {BindingTypeText(structure.BendingTypeLeadsNumber)}");
            OpenXML.TableCell cell_1_2 = BuildCell();

            VerticalMergeCells(new OpenXML.TableCell[] { cell_1_1, cell_1_2 });

            row_1.Append(cell_1_1);
            row_2.Append(cell_1_2);

            for (int i = 0; i < pTypes.Length; i++)
            {
                MeasureParameterType mpt = pTypes[i];
                int colsForParameter = ColumsCountForParameter(mpt, structure);
                OpenXML.TableCell[] cellsFor_row1 = BuildCells(colsForParameter, true);
                OpenXML.TableCell[] cellsFor_row2 = BuildCells(colsForParameter);
                FillCellText(cellsFor_row1[0], $"{ParameterNameText(mpt)}, {mpt.ParameterDataList[0].ResultMeasure()}");
               
                for (int x = 0; x < colsForParameter; x++)
                {
                    if (colsForParameter>1)
                    {
                        FillCellText(cellsFor_row2[x], (x + 1).ToString());

                    }else
                    {
                        VerticalMergeCells(new OpenXML.TableCell[] { cellsFor_row1[x], cellsFor_row2[x] });
                    }
                    row_1.Append(cellsFor_row1[x]);
                    row_2.Append(cellsFor_row2[x]);
                }
            }
            return new OpenXML.TableRow[] { row_1, row_2 };
        }


        private static OpenXML.Paragraph BuildParagraph()
        {
            OpenXML.Paragraph p = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
            OpenXML.ParagraphProperties props = new OpenXML.ParagraphProperties(
                 new OpenXML.Indentation() { End = "0" }
                );
            return p;
        }



        private static OpenXML.Run GetDefaultTextRun(string content = null)
        {
            OpenXML.Run TextRun = new OpenXML.Run();
            TextRun.AppendChild<OpenXML.RunProperties>(new OpenXML.RunProperties(new OpenXML.RunFonts() { Ascii = "Times New Roman" }, new OpenXML.FontSize() { Val = "18" }));
            if (content != null) TextRun.Append(new OpenXML.Text(content));
            return TextRun;
        }

        private static OpenXML.TableCell BuildCell(string content = "")
        {
            OpenXML.TableCell cell = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
            OpenXML.Run cellTextRun = GetDefaultTextRun(content);
            OpenXML.Paragraph cellParagraph = BuildParagraph();
            cellParagraph.Append(cellTextRun);
            cell.Append(new OpenXML.TableCellProperties(
                                                        new OpenXML.TableCellWidth() { Type = OpenXML.TableWidthUnitValues.Dxa, Width = "750" },
                                                        new OpenXML.TableCellMargin(
                                                                                    new OpenXML.TableCellRightMargin() { Type = OpenXML.TableWidthValues.Dxa, Width = 0 },
                                                                                    new OpenXML.TableCellLeftMargin() { Type = OpenXML.TableWidthValues.Dxa, Width = 0 }
                                                                                     //new OpenXML.TableCellMargin() { Type = OpenXML.TableWidthValues.Dxa, Width = 0 }
                                                                                    ),
                                       
                                                        new OpenXML.TableCellVerticalAlignment() { Val = OpenXML.TableVerticalAlignmentValues.Center }
                                                        )
           
                        );
            cell.Append(cellParagraph);
            return cell;
        }

        private static void VerticalMergeCells(OpenXML.TableCell[] cells)
        {
            if (cells.Length>1)
            {
                for(int i=0; i<cells.Length; i++)
                {
                    OpenXML.TableCellProperties props = new OpenXML.TableCellProperties();
                    props.Append(new OpenXML.VerticalMerge()
                    {
                        Val = i == 0 ? OpenXML.MergedCellValues.Restart : OpenXML.MergedCellValues.Continue
                    });
                    cells[i].AppendChild<OpenXML.TableCellProperties>(props);
                }
            }
        }

        private static void FillCellText(OpenXML.TableCell cell, string content)
        {
            cell.RemoveAllChildren<OpenXML.Paragraph>();
            cell.Append(BuildParagraph());
            cell.GetFirstChild<OpenXML.Paragraph>().Append(GetDefaultTextRun(content));
        }

        private static void FillCellByColor(OpenXML.TableCell cell, string color)
        {
            OpenXML.TableCellProperties props = cell.GetFirstChild<OpenXML.TableCellProperties>();
            props.RemoveAllChildren<OpenXML.Shading>();
            props.Append(
                new OpenXML.Shading() { Val = OpenXML.ShadingPatternValues.Clear, Fill = color.ToUpper(), Color = "auto" }
                );
        }

        private static void SetCellBordersStyle(OpenXML.TableCell cell, OpenXML.TableCellBorders bordersStyle)
        {
            OpenXML.TableCellProperties props = cell.GetFirstChild<OpenXML.TableCellProperties>();
            props.RemoveAllChildren<OpenXML.TableCellBorders>();
            props.Append(bordersStyle);
        }

        private static OpenXML.TableCellBorders BuildBordersStyle(uint top = 2, uint bottom = 2, uint left = 2, uint right = 2)
        {
            OpenXML.TableCellBorders borderStyle = new DocumentFormat.OpenXml.Wordprocessing.TableCellBorders(
                new OpenXML.RightBorder() { Size = right < 2 ? 2 : right, Val = new EnumValue<OpenXML.BorderValues>(right != 0 ? OpenXML.BorderValues.Single : OpenXML.BorderValues.None) },
                new OpenXML.LeftBorder() { Size = left < 2 ? 2 : left, Val = new EnumValue<OpenXML.BorderValues>(left != 0 ? OpenXML.BorderValues.Single : OpenXML.BorderValues.None) },
                new OpenXML.TopBorder() { Size = top < 2 ? 2 : top, Val = new EnumValue<OpenXML.BorderValues>(top != 0 ? OpenXML.BorderValues.Single : OpenXML.BorderValues.None) },
                new OpenXML.BottomBorder() { Size = bottom < 2 ? 2 : bottom, Val = new EnumValue<OpenXML.BorderValues>(bottom != 0 ? OpenXML.BorderValues.Single : OpenXML.BorderValues.None) }
                );
            return borderStyle;
        }

        private static OpenXML.TableCell[] BuildCells(int count, bool isMerged = false)
        {
            List<OpenXML.TableCell> cells = new List<DocumentFormat.OpenXml.Wordprocessing.TableCell>();
            for (int i = 0; i < count; i++)
            {
                OpenXML.TableCell cell = BuildCell();
                if (isMerged)
                {
                    OpenXML.TableCellProperties props = new OpenXML.TableCellProperties();
                    props.Append(new OpenXML.HorizontalMerge()
                    {
                        Val = i == 0 ? OpenXML.MergedCellValues.Restart : OpenXML.MergedCellValues.Continue
                    });
                    cell.AppendChild<OpenXML.TableCellProperties>(props);
                }
                cells.Add(cell);
            }
            
            return cells.ToArray();
        }

        private static OpenXML.TableRow BuildRow()
        {
            OpenXML.TableRow row = new OpenXML.TableRow();
            OpenXML.TableRowProperties props = new OpenXML.TableRowProperties(
                new OpenXML.TableRowHeight() { HeightType = OpenXML.HeightRuleValues.AtLeast }
                );
            row.AppendChild<OpenXML.TableRowProperties>(props);
            return row;
        }



        private static OpenXML.Table BuildTable()
        {
            OpenXML.Table table = new OpenXML.Table();
            OpenXML.TableProperties tblProp = new OpenXML.TableProperties(
         new OpenXML.TableBorders(
        new OpenXML.TopBorder()
        {
            Val =
            new EnumValue<OpenXML.BorderValues>(OpenXML.BorderValues.Single),
            Size = 2
        },
        new OpenXML.BottomBorder()
        {
            Val =
            new EnumValue<OpenXML.BorderValues>(OpenXML.BorderValues.Single),
            Size = 2
        },
        new OpenXML.LeftBorder()
        {
            Val =
            new EnumValue<OpenXML.BorderValues>(OpenXML.BorderValues.Single),
            Size = 2
        },
        new OpenXML.RightBorder()
        {
            Val =
            new EnumValue<OpenXML.BorderValues>(OpenXML.BorderValues.Single),
            Size = 2
        },
        new OpenXML.InsideHorizontalBorder()
        {
            Val =
            new EnumValue<OpenXML.BorderValues>(OpenXML.BorderValues.Single),
            Size = 2
        },
        new OpenXML.InsideVerticalBorder()
        {
            Val =
            new EnumValue<OpenXML.BorderValues>(OpenXML.BorderValues.Single),
            Size = 2
        }
    )
);
            table.AppendChild<OpenXML.TableProperties>(tblProp);
            return table;
        }

        private static int[] CalcMaxRowsCount(int cols, int rows)
        {
            int tablesAmount = 16 / cols;
            int perTableRows = rows / tablesAmount;
            int lastTableRows;
            List<int> template = new List<int>();
            if (perTableRows > 50)
            {
                perTableRows = 50;
                tablesAmount = rows / perTableRows;
            }
            lastTableRows = (rows % perTableRows) + perTableRows;
            for(int i=0; i<tablesAmount; i++)
            {
                if (i==tablesAmount-1)
                {
                    template.Add(lastTableRows);
                }
                else
                {
                    template.Add(perTableRows);
                }
            }
            return template.ToArray();
        }

        private static string ResultText(TestResult res)
        {
            if (res.Affected)
            {
                return res.Status;
            }else
            {
                return ResultValueText(res.BringingValue);
            }
        }

        private static string ResultValueText(decimal value)
        {
            int pow = 0;
            decimal tmpVal = value;
            if (value > 9999)
            {
                while(tmpVal/10 > 1)
                {
                    pow++;
                    tmpVal /= 10;
                }
                return $"{Math.Round(tmpVal, 1)}∙e{pow}";
            }else
            {
                return $"{value}";
            }
        }


        private static string BindingTypeText(int els_amount)
        {
            if (els_amount == 1) return "жилы";
            else if (els_amount == 2) return "пары";
            else if (els_amount == 3) return "тройки";
            else if (els_amount == 4) return "четв.";
            else return "пары";
        }

        private static string ParameterNameText(MeasureParameterType pType)
        {
            switch(pType.Id)
            {
                case MeasureParameterType.Risol1:
                case MeasureParameterType.Risol3:
                    return "Rиз";
                case MeasureParameterType.Risol2:
                case MeasureParameterType.Risol4:
                    return "T*";
                case MeasureParameterType.dR:
                    return "ΔR";
                default:
                    return pType.Name;
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
            WordApp.Visible = false;
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

        private Table BuildTable(int cols, int rows, Word.Shape oShape)
        {
            object b1 = WdDefaultTableBehavior.wdWord9TableBehavior;
            object b2 = WdAutoFitBehavior.wdAutoFitContent;
            Table oTab = oShape.TextFrame.TextRange.Tables.Add(oShape.TextFrame.TextRange, rows, cols, ref b1, ref b2);
            oTab.AllowAutoFit = true;
            oTab.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            return oTab;
        }

        public void AddTable(OpenXML.Table table)
        {
            DateTime time = DateTime.Now;
            string filePath = AddTmpFile($"tmp-{time.Day}-{time.Month}-{time.Year}-{time.Hour}-{time.Minute}-{time.Second}-{time.Millisecond}");
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                doc.MainDocumentPart.Document.Body.Append(table);
            }
            CutCreatedTableFromTmpFile(filePath);
            DeleteTmpFile(filePath);
        }



        public void CutCreatedTableFromTmpFile(string file_path)
        {
            object file = file_path;//; @"C:\Users\KRA\Documents\Visual Studio 2015\Projects\SAKProtocolManager\SAKProtocolManager\bin\Debug\test.docx";
            Word.Document tmp = WordApp.Documents.Add(ref file, ref oMissing, ref oMissing, ref oMissing);
            tmp.Activate();

            tmp.Tables[1].Select();
            tmp.ActiveWindow.Selection.Copy();

            WordDocument.Activate();
            Word.Shape oShape = CreateShape();
            oShape.Select();
            WordDocument.ActiveWindow.Selection.Paste();
            tmp.Close(false);
            ///WordApp.Visible = true;
            Word.Table table = oShape.TextFrame.TextRange.Tables[1];
            table.Range.Font.Color = FontColor;
            table.Range.Font.Name = FontName;
            table.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            table.Range.Paragraphs.LeftIndent = 0.0f;
            table.Range.Paragraphs.SpaceAfter = 0f;
            //table.Range.Font.Size = FontSize;
            //table.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitContent);
            //table.Rows.Height = 10;
            //table.Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            //table.Range.Cells.AutoFit();
            //table.AllowAutoFit = true;
            ResizeShapeByTable(oShape);
            Clipboard.Clear();

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

        public void DeleteTmpFile(string file_path)
        {
            if (File.Exists(file_path))
            {
                File.Delete(file_path);
            }
        }

        public string AddTmpFile(string file_name)
        {
            object needSave = true;
            object isTemplate = false;
            object fileName = file_name;
            string string_path = CreateTmpFile($"{file_name}.docx");
            object filePath = string_path;
            Word.Document doc = WordApp.Documents.Add(ref filePath, ref isTemplate, ref oMissing, ref oMissing);
            doc.Content.Paragraphs.Add(ref oMissing);
            doc.SaveAs2(ref filePath);
            doc.Close();
            return string_path;
        }

        public string CreateTmpFile(string file_name)
        {
            string filePath = Path.Combine(GetTmpFileDir(), file_name);
            if (File.Exists(filePath)) File.Delete(filePath);
            FileStream fs = File.Create(filePath);
            fs.Close();
            fs.Dispose();
            return filePath;
        }

        public string GetTmpFileDir()
        {
            string path = Path.Combine(GetRootWordProtocolsDir(), "tmp");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
        public string GetRootWordProtocolsDir()
        {
            string path = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Протоколы MSWord");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
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
