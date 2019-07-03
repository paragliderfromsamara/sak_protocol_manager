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
using System.Threading;
using Microsoft.Office.Core;

namespace SAKProtocolManager.MSWordProtocolBuilder
{
    public class MSWordProtocolBuilder
    {
        static CableTest CableTest;
        static MSWordProtocol wordProtocol;
        private const int MaxColsPerPage = 20;
        private static StatusPanel statusPanel;
        public static void BuildProtocolForTest(CableTest test, StatusPanel panel)
        {
            int tryingTime = 3;
            CableTest = test;
            statusPanel = panel;
            try
            {
                //statusForm.Show();
                statusPanel.Reset();
                statusPanel.SetBarRange(1, 100);
                statusPanel.SetBarPosition("Создание документа MS Word", "Инициализация", 5);
                wordProtocol = new MSWordProtocol();
                wordProtocol.Init();
                statusPanel.AddToBarPosition();
                statusPanel.AddToBarPosition("Добавление шапки документа", 5);
                wordProtocol.AddHeader(test);
                statusPanel.AddToBarPosition();

                foreach (CableStructure s in CableTest.TestedCable.Structures)
                {
                    statusPanel.AddToBarPosition($"Структура {s.Name}", "", 5);
                    PrintStructure(s);
                }
                statusPanel.AddToBarPosition("Добавление завершения документа", 5);
                wordProtocol.AddFooter(test);
                statusPanel.SetBarPosition("Расстановка таблиц", 99);
                Thread.Sleep(250);
                wordProtocol.Finalise();
                statusPanel.SetBarPosition("Расстановка таблиц", 100);
                //statusForm.Hide();

            }
            catch(System.Runtime.InteropServices.COMException ex)
            {
                if (tryingTime-- > 0) throw ex;
            }
        }

        public static void PrintStructure(CableStructure structure)
        {

            addPrimaryParametersTable(structure);
            addRizolByGroupTable(structure);
            add_al_Table(structure);
            add_AoAz_Table(structure, MeasureParameterType.Ao);
            add_AoAz_Table(structure, MeasureParameterType.Az);
            add_Statistic_Table(structure);
            add_VSVI_TestResult(structure);
            add_StructElements_Conclusion(structure);
        }

        /// <summary>
        /// Отрисовка вывода о годности элементов
        /// </summary>
        /// <param name="structure"></param>
        private static void add_StructElements_Conclusion(CableStructure structure)
        {
            List<OpenXML.Paragraph> paragraphs = new List<OpenXML.Paragraph>();
            paragraphs.Add(BuildParagraph(AddRun($"Номинальное количество {structure.BendingTypeName_RodPadej_Multiple}: {structure.RealNumberInCable}")));
            paragraphs.Add(BuildParagraph(AddRun($"Фактическое количество {structure.BendingTypeName_RodPadej_Multiple}: {structure.NumberInCable}")));
            paragraphs.Add(BuildParagraph(AddRun($"годных {structure.BendingTypeName_RodPadej_Multiple}: {structure.NormalElementsCount}")));
            OpenXML.Paragraph descriptionParagraph = BuildParagraph();
            descriptionParagraph.Append(AddRun("Значения измеренных параметров вышедшие за установленные нормы выделены", MSWordStringTypes.Typical, false, true));
            descriptionParagraph.Append(AddRun(" жирным ", MSWordStringTypes.Typical, true, true), AddRun($"шрифтом.", MSWordStringTypes.Typical, false, true));
            paragraphs.Add(descriptionParagraph);
            wordProtocol.AddElementsAsXML(paragraphs.ToArray(), 4 * 13, 400);
        }

        private static void add_VSVI_TestResult(CableStructure structure)
        {

            if (!structure.Cable.Test.HasVsvi) return;
            List<OpenXML.Paragraph> paragraphs = new List<OpenXML.Paragraph>();
            paragraphs.Add(BuildParagraph(AddRun("Испытательное напряжение (постоянный ток) в течение 1 мин, приложенное:")));
            if (structure.LeadLeadTestVoltage > 0) paragraphs.Add(BuildParagraph(  AddRun($"   -Между жилами                   {structure.LeadLeadTestVoltage} В")));
            if (structure.LeadShieldTestVoltage > 0) paragraphs.Add(BuildParagraph(AddRun($"   -Между жилами и экраном {structure.LeadShieldTestVoltage} В")));
            OpenXML.Paragraph vsviResultParagraph = BuildParagraph();
            if (structure.BrokenPairs.Length > 0)
            {
                if (structure.BrokenPairs.Length > 1)
                {
                    vsviResultParagraph.Append(AddRun($"Пробитые {structure.BendingTypeName_Multiple} №№: "));
                }
                else
                {
                    vsviResultParagraph.Append(AddRun($"Пробитая {structure.BendingTypeName} №"));
                }

                for (int i = 0; i < structure.BrokenPairs.Length; i++)
                {
                    if (i > 0) vsviResultParagraph.Append(AddRun(", ")); // s += ", ";
                    vsviResultParagraph.Append(AddRun(structure.BrokenPairs[i].ToString(), MSWordStringTypes.Typical, true));
                }
                vsviResultParagraph.Append(AddRun(";"));
            }else
            {
                vsviResultParagraph.Append(AddRun("Выдержал."));
            }
            paragraphs.Add(vsviResultParagraph);
            wordProtocol.AddElementsAsXML(paragraphs.ToArray(), paragraphs.Count*13, 400);

        }

        private static void add_Statistic_Table(CableStructure structure)
        {
            OpenXML.Table table = BuildTable();
            table.Append(BuildStatTable_HeaderRow());
            int rowsAmount = 2;
            foreach (MeasureParameterType t in structure.MeasuredParameters)
            {
                if (t.ParameterDataList.Length > 0)
                {
                    foreach(MeasuredParameterData pData in t.ParameterDataList)
                    {
                        if (pData.ParameterType.Id == MeasureParameterType.Risol3 || pData.ParameterType.Id == MeasureParameterType.Risol4) continue;
                        if (pData.TestResults.Length > 0)
                        {
                            OpenXML.TableRow row = BuildRow();
                            OpenXML.TableCell pNameCell = BuildCell(BuildParagraph(ParameterNameText(pData.ParameterType).ToArray()));
                            if (pData.ParameterType.IsFreqParameter)
                            {
                                if (pData.ParameterType.Id == MeasureParameterType.al)
                                {
                                    pNameCell.Append(BuildFreqParagraphs(pData));
                                }
                                else
                                {
                                    pNameCell.Append(BuildFreqsParagraph_AoAz(pData));
                                }
                            }
                            OpenXML.TableCell maxValCell = BuildCell(pData.MaxValue < decimal.MaxValue ? pData.MaxValue.ToString() : "");
                            OpenXML.TableCell minValCell = BuildCell(pData.MinValue > decimal.MinValue ? pData.MinValue.ToString() : "");
                            OpenXML.TableCell measureCell = BuildCell(pData.ResultMeasure());
                            OpenXML.TableCell normaPercentCell = BuildCell(pData.NormalPercent.ToString());
                            OpenXML.TableCell measuredPercentCell = BuildCell(BuildParagraph(AddRun(pData.MeasuredPercent.ToString(), MSWordStringTypes.Typical, pData.NormalPercent > pData.MeasuredPercent)));

                            row.Append(pNameCell, minValCell, maxValCell, measureCell, normaPercentCell, measuredPercentCell);
                            table.Append(row);
                            rowsAmount++;
                        }
                    }
                }
            }
            wordProtocol.AddTable(table, 19, rowsAmount+1);
        }

        private static OpenXML.TableRow[] BuildStatTable_HeaderRow()
        {
            OpenXML.TableRow row_1 = BuildRow();
            OpenXML.TableRow row_2 = BuildRow();

            OpenXML.TableCell cell_1_1 = BuildCell("Параметр");
            OpenXML.TableCell cell_1_2 = BuildCell();
            VerticalMergeCells(new OpenXML.TableCell[] { cell_1_1, cell_1_2 });
            row_1.Append(cell_1_1);
            row_2.Append(cell_1_2);

            OpenXML.TableCell cell_2_1_1 = BuildCell("Норма");
            OpenXML.TableCell cell_2_2_1 = BuildCell();
            OpenXML.TableCell cell_2_1_2 = BuildCell("min");
            OpenXML.TableCell cell_2_2_2 = BuildCell("max");
            HorizontalMergeCells(new OpenXML.TableCell[] { cell_2_1_1, cell_2_2_1 });
            row_1.Append(cell_2_1_1, cell_2_2_1);
            row_2.Append(cell_2_1_2, cell_2_2_2);

            OpenXML.TableCell cell_3_1 = BuildCell("Единица измерения");
            OpenXML.TableCell cell_3_2 = BuildCell();
            VerticalMergeCells(new OpenXML.TableCell[] { cell_3_1, cell_3_2 });
            row_1.Append(cell_3_1);
            row_2.Append(cell_3_2);

            OpenXML.TableCell cell_4_1 = BuildCell("Задано, %");
            OpenXML.TableCell cell_4_2 = BuildCell();
            VerticalMergeCells(new OpenXML.TableCell[] { cell_4_1, cell_4_2 });
            row_1.Append(cell_4_1);
            row_2.Append(cell_4_2);

            OpenXML.TableCell cell_5_1 = BuildCell("Измерено, %");
            OpenXML.TableCell cell_5_2 = BuildCell();
            VerticalMergeCells(new OpenXML.TableCell[] { cell_5_1, cell_5_2 });
            row_1.Append(cell_5_1);
            row_2.Append(cell_5_2);

            return new OpenXML.TableRow[] { row_1, row_2 };
        }

        private static void add_AoAz_Table(CableStructure structure, string type_id)
        {
            MeasureParameterType type = null;
            foreach(MeasureParameterType t in structure.MeasuredParameters)
            {
                if (t.Id == type_id)
                {
                    type = t;
                    break;
                }
            }
            if (type == null) return;
            statusPanel.AddToBarPosition($"Таблица {type.Name}", 0);
            foreach (MeasuredParameterData mpd in type.ParameterDataList)
            {
                if (mpd.TestResults.Length == 0) continue;
                List<AoAz_TableValues> resLists = SplitByGeneralTables_ForAoAz(mpd.TestResults);
                
                foreach (AoAz_TableValues rList in resLists)
                {
                    int pairsAmount = rList.ElementsCount * rList.endSubElNumber;
                    int tablesOnWide = 1, colsPerTable = pairsAmount;
                    //while (colsPerTable > MaxColsPerPage ) colsPerTable /= ++tablesOnWide;
                    //colsPerTable = rList.ElementsCount * rList.endSubElNumber * tablesOnWide;
                    
                    if (MaxColsPerPage < pairsAmount)
                    {
                        colsPerTable = MaxColsPerPage;
                        tablesOnWide = pairsAmount / colsPerTable;
                        if (pairsAmount % colsPerTable > 0) tablesOnWide++;
                    }
                    
                    Debug.WriteLine($"add_AoAz_Table: onWide = {tablesOnWide} colsPerTable = {colsPerTable}");
                    
                    for(int tableNum=0; tableNum < tablesOnWide; tableNum++)
                    {
                        int startGenEl = rList.startElNumber, endGenEl;
                        int startRecEl = rList.startElNumber+((colsPerTable)*tableNum);
                        int endRecEl = (startRecEl + colsPerTable-1 > rList.endElNumber) ? rList.endElNumber : startRecEl + colsPerTable-1;
                        int colsAmount = (endRecEl - startRecEl+1)*structure.BendingTypeLeadsNumber/2;
                        int rowsAll = rList.ElementsCount + 6;
                        Debug.WriteLine($"add_AoAz_Table: table = {tableNum} rowsAll = {rowsAll} colsAmount = {colsAmount}, startRec = {startRecEl}, endRec = {endRecEl}");
                        int[] tableRows = CalcMaxRowsCount_For_AoAz(colsAmount, rowsAll);
                        for(int rIdx = 0; rIdx < tableRows.Length; rIdx++)
                        {
                            int rowsAmount = tableRows[rIdx];
                            int curTableRowsAmount = rowsAmount;
                            endGenEl = (startGenEl + curTableRowsAmount - 1 > rList.endElNumber) ? rList.endElNumber : startGenEl + curTableRowsAmount - 1;
                            OpenXML.Table table = draw_AoAz_Table(rList, mpd, startGenEl, endGenEl, startRecEl, endRecEl);
                            if (tableNum == tablesOnWide-1 && rIdx == tableRows.Length-1)
                            {
                                OpenXML.TableRow rowMin = BuildRow();
                                OpenXML.TableRow rowMax = BuildRow();
                                OpenXML.TableRow rowAver = BuildRow();

                                OpenXML.TableCell[] minTitleCells = BuildCells(2, true);
                                OpenXML.TableCell[] minValCells = BuildCells(colsAmount, true);
                                OpenXML.TableCell[] maxTitleCells = BuildCells(2, true);
                                OpenXML.TableCell[] maxValCells = BuildCells(colsAmount, true);
                                OpenXML.TableCell[] averTitleCells = BuildCells(2, true);
                                OpenXML.TableCell[] averValCells = BuildCells(colsAmount, true);

                                FillCellText(minTitleCells[0], "min");
                                FillCellText(minValCells[0], ResultValueText(mpd.MinVal));

                                FillCellText(averTitleCells[0], "средн.");
                                FillCellText(averValCells[0], ResultValueText(mpd.AverageVal));

                                FillCellText(maxTitleCells[0], "max");
                                FillCellText(maxValCells[0], ResultValueText(mpd.MaxVal));

                                rowMin.Append(minTitleCells);
                                rowMin.Append(minValCells);
                                rowMax.Append(maxTitleCells);
                                rowMax.Append(maxValCells);
                                rowAver.Append(averTitleCells);
                                rowAver.Append(averValCells);

                                

                                curTableRowsAmount += 5;
                                table.Append(rowMin, rowAver, rowMax);

                            }
                            wordProtocol.AddTable(table, colsAmount, curTableRowsAmount+2);
                            statusPanel.AddToBarPosition();
                            startGenEl += rowsAmount;
                        }
                    }
                }
            }
        }

        private static OpenXML.Table draw_AoAz_Table(AoAz_TableValues resList, MeasuredParameterData mpd, int startGenEl, int endGenEl, int startRecEl, int endRecEl)
        {
            int curGenEl = 0;
            int rowsAmount = endGenEl - startGenEl+1;
            int colsAmount = endRecEl - startRecEl+1;
            rowsAmount += 5;
            OpenXML.Table table = BuildTable();
            OpenXML.TableRow[] headerRows = Build_AoAz_TableHeader(mpd, resList, startRecEl, endRecEl);
            table.Append(headerRows);
            for(int genEl = startGenEl; genEl<=endGenEl; genEl++)
            {
                OpenXML.TableRow[] elRows = new OpenXML.TableRow[resList.endSubElNumber-resList.startElNumber+1];
                for (int i = 0; i < elRows.Length; i++)
                {
                    OpenXML.TableCellProperties vertMerge = new OpenXML.TableCellProperties();
                    vertMerge.Append(new OpenXML.VerticalMerge()
                    {
                        Val = i == 0 ? OpenXML.MergedCellValues.Restart : OpenXML.MergedCellValues.Continue
                    });
                    elRows[i] = BuildRow();
                    OpenXML.TableCell cell_1_n = BuildCell();
                    cell_1_n.Append(vertMerge);
                    OpenXML.TableCell cell_2_n = BuildCell();
                    if (i == 0) FillCellText(cell_1_n, genEl.ToString());
                    if (elRows.Length > 1)
                    {
                        FillCellText(cell_2_n, $"{resList.startSubElNumber + 2*i}-{resList.startSubElNumber + 2*i+1}");// (resList.startSubElNumber+i).ToString());
                    }
                    else
                    {
                        HorizontalMergeCells(new OpenXML.TableCell[] { cell_1_n, cell_2_n });
                    }
                    elRows[i].Append(cell_1_n, cell_2_n);
                }
                //OpenXML.TableRow row1 = BuildRow();
                //OpenXML.TableRow row2 = BuildRow();
                //OpenXML.TableCell cell_1_1 = BuildCell(genEl.ToString());
                //OpenXML.TableCell cell_1_2 = BuildCell();
                for (int genSubEl = resList.startSubElNumber; genSubEl <= resList.endSubElNumber; genSubEl++)
                {
                    OpenXML.TableRow tabRow = elRows[genSubEl-1];
                    for (int recEl = startRecEl; recEl <= endRecEl; recEl++)
                    {
                        for(int recSubEl = resList.startSubElNumber; recSubEl <= resList.endSubElNumber; recSubEl++)
                        {
                            TestResult r = resList.GetResult(genEl, recEl, genSubEl, recSubEl);
                            OpenXML.TableCell resCell = BuildCell();
                            if (r != null)
                            {
                                resCell.GetFirstChild<OpenXML.Paragraph>().Append(ResultText(r));
                            }
                            else
                            {
                                if (genEl==recEl && genSubEl == recSubEl)
                                {
                                    FillCellByColor(resCell, "b2b2b2");
                                }
                            }
                            tabRow.Append(resCell);
                        }
                    }
                }
                table.Append(elRows);
            }
            return table;

        }

        private static List<AoAz_TableValues> SplitByGeneralTables_ForAoAz(TestResult[] testResults)
        {
            List<AoAz_TableValues> rslt = new List<AoAz_TableValues>();
            AoAz_TableValues tmpTableValues = new AoAz_TableValues();
            int maxElNumber = testResults[testResults.Length - 1].GeneratorElementNumber;
            foreach(TestResult r in testResults)
            {
                if (tmpTableValues.LastAdded != null)
                {
                    if (tmpTableValues.LastAdded.GeneratorElementNumber != r.GeneratorElementNumber && tmpTableValues.LastAdded.ElementNumber < maxElNumber - 1)
                    {
                        rslt.Add(tmpTableValues);
                        tmpTableValues = new AoAz_TableValues();
                    }
                }
                tmpTableValues.AddResult(r);
            }
            Debug.WriteLine($"SplitByGeneralTables_ForAoAz: {tmpTableValues.startElNumber} {tmpTableValues.endElNumber}");
            if (tmpTableValues.HasValues) rslt.Add(tmpTableValues);
            return rslt;
        }

        private static void addRizolByGroupTable(CableStructure structure)
        {
            MeasureParameterType type = null;
            MeasuredParameterData mpd = null;

            OpenXmlElement[] elementsToPage = new OpenXmlElement[2]; 
            int i = 0;
            foreach (MeasureParameterType mpt in structure.MeasuredParameters)
            {
                if (mpt.Id != MeasureParameterType.Risol3 && mpt.Id != MeasureParameterType.Risol4 || mpt.TestResults.Length == 0) continue;
                type = mpt;
                break;
            }
            if (type == null) return;
            statusPanel.AddToBarPosition($"Таблица {type.Name}", 0);
            mpd = type.ParameterDataList[0];
            Debug.WriteLine(mpd.ParameterType.Name);
            bool MoreThanOneGroups = mpd.TestResults[0].ElementNumber > 0;
            int colsAmount = 2;
            OpenXML.Table table = BuildTable();
            OpenXML.TableRow headerRow = BuildRow();
            OpenXML.TableCell cellGroupElNumber = BuildCell("№/№ Гр.");
            OpenXML.TableCell cellElNumber = BuildCell("№/№ Комб.");
            OpenXML.TableCell cellParameterName = BuildCell(ParameterNameText(mpd.ParameterType, mpd.ResultMeasure()).ToArray());//}, {mpd.ParameterType.ParameterDataList[0].ResultMeasure()}");



            if (MoreThanOneGroups)
            {
                colsAmount++;
                headerRow.AppendChild(cellGroupElNumber);
            }

            headerRow.AppendChild(cellElNumber);
            headerRow.AppendChild(cellParameterName);
            table.Append(headerRow);
            foreach (TestResult r in mpd.TestResults)
            {
                uint bottomDorderWidth = (uint)((i < mpd.TestResults.Length - 1) ? 0 : 2);
                OpenXML.TableCellBorders borderStyle = BuildBordersStyle(0, bottomDorderWidth);

                OpenXML.TableRow resRow = BuildRow();

                OpenXML.TableCell groupCell = BuildCell($"{r.ElementNumber}");
                SetCellBordersStyle(groupCell, BuildBordersStyle(0, bottomDorderWidth));

                OpenXML.TableCell numCell = BuildCell($"{r.SubElementNumber}");
                SetCellBordersStyle(numCell, BuildBordersStyle(0, bottomDorderWidth));

                OpenXML.TableCell resCell = BuildCell(BuildParagraph(ResultText(r)));
                SetCellBordersStyle(resCell, BuildBordersStyle(0, bottomDorderWidth));

                if (i++ % 2 == 1)
                {
                    FillCellByColor(groupCell, "ededed");
                    FillCellByColor(numCell, "ededed");
                    FillCellByColor(resCell, "ededed");
                }

                if (MoreThanOneGroups) resRow.Append(groupCell);
                resRow.Append(numCell);
                resRow.Append(resCell);
                table.Append(resRow);
            }
            OpenXML.Paragraph p = new OpenXML.Paragraph();
            List<OpenXML.Run> noteRun = new List<OpenXML.Run>();
            foreach (MeasureParameterType t in structure.MeasuredParameters_Full)
            {
                Debug.WriteLine($"Параметр {t.Id}");
                if (type.Id == MeasureParameterType.Risol4 && t.Id == MeasureParameterType.Risol3)
                {
                    noteRun = ParameterNameText(t);
                    noteRun.Add(AddRun($"норма: {t.ParameterDataList[0].MinValue} МОм"));
                    
                    //text = $"Rиз Норма: {}";
                }else if (type.Id == MeasureParameterType.Risol3 && t.Id == MeasureParameterType.Risol4)
                {
                    noteRun = ParameterNameText(t);
                    noteRun.Add(AddRun($"За время {t.ParameterDataList[0].MaxValue}"));
                }
            }
            //noteRun.Add(AddRun($"За время 100500 лет"));
            elementsToPage[0] = table;
            elementsToPage[1] = BuildParagraph(noteRun.ToArray());

           // wordProtocol.AddTable(table, colsAmount, mpd.ParameterType.TestResults.Length + 2);
            wordProtocol.AddElementsAsXML(elementsToPage, wordProtocol.CellHeight*(mpd.ParameterType.TestResults.Length + 4), wordProtocol.CellWidth * colsAmount);
            statusPanel.AddToBarPosition();
        }

        private static void add_al_Table(CableStructure structure)
        {
            MeasureParameterType alType = null;
            Dictionary<string, MeasuredParameterData> printedData = new Dictionary<string, MeasuredParameterData>();
            List<MeasuredParameterData> curTableData = new List<MeasuredParameterData>();
            int maxCols = MaxColsPerPage;
            int colsCount = 1; //Первая колонка номер элемента

            foreach (MeasureParameterType t in structure.MeasuredParameters)
            {
                if (t.Id == MeasureParameterType.al)
                {
                    alType = t;
                    foreach (MeasuredParameterData d in t.ParameterDataList)
                    {
                        if (!printedData.Keys.Contains(d.FrequencyRangeId)) printedData.Add(d.FrequencyRangeId, d);
                    }
                    break;
                }
            }
            if (alType == null || printedData.Count==0) return;
            statusPanel.AddToBarPosition($"Таблица {alType.Name}", 0);
            foreach (string key in printedData.Keys)
            {
                MeasuredParameterData mpd = printedData[key];
                repeat_adding:
                bool needToBuildTable = (colsCount + structure.BendingTypeLeadsNumber / 2) > maxCols;
                colsCount += structure.BendingTypeLeadsNumber / 2;
                if (!needToBuildTable)
                {
                    curTableData.Add(mpd);
                    colsCount += structure.BendingTypeLeadsNumber / 2;
                }
                if (needToBuildTable || key == printedData.Keys.Last())
                {
                    Build_al_Table(curTableData, colsCount, structure);
                    needToBuildTable = false;
                    curTableData.Clear();
                    colsCount = 1;
                    if (key != printedData.Keys.Last()) goto repeat_adding;
                }
            }
        }

        private static void Build_al_Table(List<MeasuredParameterData> curTableData, int colsAmount, CableStructure structure)
        {
            int curElementNumber = 1;
            int[] tablesRowsCount = CalcMaxRowsCount(colsAmount, structure.RealNumberInCable + 3 + 2);

            for(int idx = 0; idx < tablesRowsCount.Length; idx++)
            {
                OpenXML.Table table = BuildTable();
                OpenXML.TableRow[] headerRows = Build_al_TableHeader_WithOpenXML(curTableData, structure);
                foreach (OpenXML.TableRow r in headerRows) table.Append(r);

                for (int i = 0; i < tablesRowsCount[idx]; i++)
                {
                    OpenXML.TableRow row = BuildRow();
                    if (curElementNumber <= structure.RealNumberInCable)
                    {
                        OpenXML.TableCell numbCell = BuildCell(curElementNumber.ToString());
                        if (i % 2 == 1) FillCellByColor(numbCell, "ededed");
                        OpenXML.TableCellBorders borderStyle = BuildBordersStyle(0, (uint)((i < tablesRowsCount[idx] - 1) ? 0 : 2));
                        SetCellBordersStyle(numbCell, borderStyle);
                        row.Append(numbCell); //Ячейка номера элемента
                        foreach (MeasuredParameterData mpd in curTableData)//(MeasureParameterType mpt in pTypes)
                        {
                            int elsColsPerParam = ColumsCountForParameter(mpd.ParameterType, structure);
                            TestResult[] results = mpd.TestResults;
                            int resIdx = (curElementNumber - 1) * elsColsPerParam;
                            for (int rIdx = resIdx; rIdx < resIdx + elsColsPerParam; rIdx++)
                            {
                                TestResult res = results[rIdx];
                                OpenXML.TableCellBorders resBordStyle = BuildBordersStyle(0, (uint)((i < tablesRowsCount[idx] - 1) ? 0 : 2));
                                OpenXML.TableCell resCell = BuildCell(BuildParagraph(ResultText(res)));
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
                        foreach (MeasuredParameterData mpd in curTableData)
                        {
                            int elsColsPerParam = ColumsCountForParameter(mpd.ParameterType, structure);
                            int resIdx = (curElementNumber - 1) * elsColsPerParam;
                            OpenXML.TableCell[] maxValCells = BuildCells(elsColsPerParam, elsColsPerParam > 1);
                            OpenXML.TableCell[] minValCells = BuildCells(elsColsPerParam, elsColsPerParam > 1);
                            OpenXML.TableCell[] averValCells = BuildCells(elsColsPerParam, elsColsPerParam > 1);

                            FillCellText(maxValCells[0], ResultValueText(mpd.MaxVal));
                            FillCellText(minValCells[0], ResultValueText(mpd.MinVal));
                            FillCellText(averValCells[0], ResultValueText(mpd.AverageVal));

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
                wordProtocol.AddTable(table, colsAmount, tablesRowsCount[idx]+2);
                statusPanel.AddToBarPosition();
            }
        }


        private static OpenXML.TableRow[] Build_al_TableHeader_WithOpenXML(List<MeasuredParameterData> curTableData, CableStructure structure)
        {

            OpenXML.TableRow row_1 = BuildRow();
            OpenXML.TableRow row_2 = BuildRow();

            OpenXML.TableCell cell_1_1 = BuildCell($"{ "№/№" } {BindingTypeText(structure.BendingTypeLeadsNumber)}");
            OpenXML.TableCell cell_1_2 = BuildCell();

            VerticalMergeCells(new OpenXML.TableCell[] { cell_1_1, cell_1_2 });

            row_1.Append(cell_1_1);
            row_2.Append(cell_1_2);

            for (int i = 0; i < curTableData.Count; i++)
            {
                MeasuredParameterData mpd = curTableData[i];
                OpenXML.Paragraph[] freqParagraphs = BuildFreqParagraphs(mpd);

                int colsForParameter = ColumsCountForParameter(mpd, structure);
                OpenXML.TableCell[] cellsFor_row1 = BuildCells(colsForParameter, true);
                OpenXML.TableCell[] cellsFor_row2 = BuildCells(colsForParameter);
                List<OpenXML.Run> parameterNameRun = ParameterNameText(mpd.ParameterType);
                parameterNameRun.Add(AddRun($",{mpd.ResultMeasure()}"));
                FillCellText(cellsFor_row1[0], parameterNameRun.ToArray());
                foreach (OpenXML.Paragraph p in freqParagraphs) cellsFor_row1[0].Append(p);

                for (int x = 0; x < colsForParameter; x++)
                {
                    if (colsForParameter > 1)
                    {
                        FillCellText(cellsFor_row2[x], $"Пара {x + 1}");// (x + 1).ToString());
                    }
                    else
                    {
                        VerticalMergeCells(new OpenXML.TableCell[] { cellsFor_row1[x], cellsFor_row2[x] });
                    }
                    row_1.Append(cellsFor_row1[x]);
                    row_2.Append(cellsFor_row2[x]);
                }
            }
            return new OpenXML.TableRow[] { row_1, row_2 };
        }

        private static OpenXML.Paragraph[] BuildFreqsParagraph_AoAz(MeasuredParameterData mpd)
        {
            OpenXML.Paragraph freqParagraph;
            freqParagraph = BuildParagraph(new OpenXML.Run[] { AddRun("f")});
            if (mpd.MaxFrequency > 0) freqParagraph.Append(AddRun("min", MSWordStringTypes.Subscript));
            freqParagraph.Append(AddRun($"={mpd.MinFrequency}кГц"));
            if (mpd.MaxFrequency > 0)
            {
                freqParagraph.Append(AddRun(", f"), AddRun("max", MSWordStringTypes.Subscript), AddRun($"={mpd.MaxFrequency}кГц"));
            }
            return new OpenXML.Paragraph[] { freqParagraph };
        }

        private static OpenXML.Paragraph[] BuildFreqParagraphs(MeasuredParameterData mpd)
        {
            OpenXML.Paragraph minFreqParagraph, maxFreqParagraph;
            minFreqParagraph = BuildParagraph(new OpenXML.Run[] { AddRun("f"), AddRun("1", MSWordStringTypes.Subscript), AddRun($"={mpd.MinFrequency}кГц")});
            if (mpd.MaxFrequency > 0)
            {
                maxFreqParagraph = BuildParagraph(new OpenXML.Run[] { AddRun("f"), AddRun("2", MSWordStringTypes.Subscript), AddRun($"={mpd.MaxFrequency}кГц") });
                return new OpenXML.Paragraph[] { minFreqParagraph, maxFreqParagraph };
            }else return new OpenXML.Paragraph[] { minFreqParagraph };


        }

        private static void addPrimaryParametersTable(CableStructure structure)
        {
            int maxCols = MaxColsPerPage;
            int colsCount = 1; //Первая колонка номер элемента
            bool progrBarIsInited = false;
            List<MeasureParameterType> typesForTable = new List<MeasureParameterType>();

            for (int i = 0; i < structure.MeasuredParameters.Length; i++)
            {
                MeasureParameterType mpt = structure.MeasuredParameters[i];
                bool needToBuildTable = false;
                if (mpt.IsPrimaryParameter)
                {
                    if (!progrBarIsInited)
                    {
                        statusPanel.SetBarPosition("Таблица перв. параметров", 10);
                        progrBarIsInited = true;
                    }
                    int colsForParameter = ColumsCountForParameter(mpt, structure);
                    if ((colsCount + colsForParameter) > maxCols)
                    {
                        needToBuildTable = true;
                    }
                    else
                    {
                        typesForTable.Add(mpt);
                        colsCount += ColumsCountForParameter(mpt, structure);
                    }
                }
                if (needToBuildTable || ((i + 1) == structure.MeasuredParameters.Length && typesForTable.Count > 0))
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
            int[] tablesRowsCount = CalcMaxRowsCount(colsAmount, structure.RealNumberInCable + 3+2);
            Debug.WriteLine($"{structure.RealNumberInCable}");

            for (int idx = 0; idx < tablesRowsCount.Length; idx++)
            {

                int rows = 2 + tablesRowsCount[idx];
                OpenXML.Table table = BuildTable();
                OpenXML.TableRow[] headerRows = BuildPrimaryParamsTableHeader_WithOpenXML(pTypes, structure);
                List<OpenXmlElement> elementsToPage = new List<OpenXmlElement>();
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
                                OpenXML.TableCell resCell = BuildCell(BuildParagraph(ResultText(res)));
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
                elementsToPage.Add(table);
                if (idx == tablesRowsCount.Length -1)
                {
                    foreach (MeasureParameterType t in pTypes)
                    {
                        if (t.Id == MeasureParameterType.Risol2)
                        {
                            decimal norma = 600;
                            string measure = "МОм/км";
                            foreach (MeasureParameterType r in structure.MeasuredParameters_Full)
                            {
                                if (r.Id == MeasureParameterType.Risol1)
                                {
                                    norma = r.ParameterDataList[0].MinValue;
                                    measure = r.ParameterDataList[0].ResultMeasure();
                                }
                            }

                            List<OpenXML.Run> txt = new List<DocumentFormat.OpenXml.Wordprocessing.Run>();
                            txt.Add(AddRun("*"));
                            foreach(OpenXML.Run r in ParameterNameText(t)) txt.Add(r);

                            
                            txt.Add(AddRun($" - время достижения сопротивления изоляции свыше {norma} {measure}."));
                            elementsToPage.Add(BuildParagraph(txt.ToArray()));
                            //wordProtocol.AddParagraph($"* Tиз—время достижения сопротивления изоляции свыше {norma} {measure}.", 18f);
                        }
                    }

                }
                wordProtocol.AddElementsAsXML(elementsToPage.ToArray(), wordProtocol.CellHeight * (tablesRowsCount[idx]+3), wordProtocol.CellWidth*colsAmount);
                statusPanel.AddToBarPosition();

                //  wordProtocol.AddParagraph("Каждый охотник желает знать где сидит фазан", 18f);
            }


        }
        private static OpenXML.TableRow[] Build_AoAz_TableHeader(MeasuredParameterData data, AoAz_TableValues vals, int startRec, int endRec)
        {
            int leadsNumber = data.ParameterType.Structure.BendingTypeLeadsNumber;
            int elsCount = endRec-startRec+1;
            List<OpenXML.TableRow> RowsForHeader = new List<OpenXML.TableRow>();

            OpenXML.Paragraph[] freqParagraphs = BuildFreqsParagraph_AoAz(data);
            OpenXML.TableRow row_1 = BuildRow();
            OpenXML.TableRow row_2 = BuildRow();
            OpenXML.TableRow row_3 = BuildRow();
            OpenXML.TableCell cell_1_1 = BuildCell($"{ "№/№" } {BindingTypeText(leadsNumber)}");
            OpenXML.TableCell cell_1_2 = BuildCell();
            OpenXML.TableCell cell_1_3 = BuildCell();
            OpenXML.TableCell cell_2_1 = BuildCell();
            OpenXML.TableCell cell_2_2 = BuildCell();
            OpenXML.TableCell cell_2_3 = BuildCell();

            OpenXML.TableCell[] dataCells_Row1 = BuildCells(elsCount * leadsNumber / 2, true);
            OpenXML.TableCell[] dataCells_Row2 = BuildCells(elsCount * leadsNumber / 2);
            OpenXML.TableCell[] dataCells_Row3 = BuildCells(elsCount * leadsNumber / 2);
            HorizontalMergeCells(new OpenXML.TableCell[] { cell_1_1, cell_2_1 });
            HorizontalMergeCells(new OpenXML.TableCell[] { cell_1_2, cell_2_2 });
            VerticalMergeCells(new OpenXML.TableCell[] { cell_1_1, cell_1_2 });

            OpenXML.Run[] pNameRun = ParameterNameText(data.ParameterType, data.ResultMeasure()).ToArray();
            //OpenXML.Paragraph pNameParagraph = BuildParagraph(pNameRun);
            FillCellText(dataCells_Row1[0], pNameRun);// dataCells_Row1[0].Append(pNameParagraph);
            dataCells_Row1[0].Append(freqParagraphs);
            for (int elNum = startRec, i=0; elNum<=endRec; elNum++, i++)
            {
                if (leadsNumber>2)
                {
                    HorizontalMergeCells(new OpenXML.TableCell[] { dataCells_Row2[i * 2], dataCells_Row2[i * 2+1] });
                    FillCellText(dataCells_Row2[i * 2], elNum.ToString());
                    for(int subElNum = 1; subElNum<= leadsNumber/2; subElNum++)
                    {
                        Debug.WriteLine($"subElNum = {subElNum}; i = {i} ");
                        int j = Math.Abs(1-subElNum);
                        FillCellText(dataCells_Row3[i * 2 + subElNum - 1], $"{j*2+1}-{j * 2+2}");//(subElNum.ToString());
                    }
                }
                else
                {
                    VerticalMergeCells(new OpenXML.TableCell[] { dataCells_Row2[i], dataCells_Row3[i] });
                    FillCellText(dataCells_Row2[i], elNum.ToString()); 
                }
            }
            if (leadsNumber > 2)
            {
                SetCellBordersStyle(cell_1_2, BuildBordersStyle(2,0,2,2));
                SetCellBordersStyle(cell_1_3, BuildBordersStyle(0, 2, 2, 2));
                FillCellText(cell_2_3, "пара");
            }
            else
            {
                HorizontalMergeCells(new OpenXML.TableCell[] { cell_1_3, cell_2_3});
                VerticalMergeCells(new OpenXML.TableCell[] { cell_1_1, cell_1_2, cell_1_3 });
            }


            row_1.Append(cell_1_1, cell_2_1);
            row_2.Append(cell_1_2, cell_2_2);
            row_3.Append(cell_1_3, cell_2_3);

            row_1.Append(dataCells_Row1);
            row_2.Append(dataCells_Row2);
            row_3.Append(dataCells_Row3);

            RowsForHeader.Add(row_1);
            RowsForHeader.Add(row_2);
            if (leadsNumber > 2) RowsForHeader.Add(row_3);
            return RowsForHeader.ToArray();
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
                List<OpenXML.Run> parameterNameRun;
                if (pTypes[i].Id != MeasureParameterType.Risol2)
                {
                    parameterNameRun = ParameterNameText(mpt, mpt.ParameterDataList[0].ResultMeasure());
                }else
                {
                    parameterNameRun = ParameterNameText(mpt);
                    parameterNameRun.Add(AddRun("*", MSWordStringTypes.Superscript));
                    parameterNameRun.Add(AddRun($",{mpt.ParameterDataList[0].ResultMeasure()}"));
                }
                FillCellText(cellsFor_row1[0], parameterNameRun.ToArray());
               
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

        private static OpenXML.Paragraph BuildParagraph(OpenXML.Run run)
        {
            return BuildParagraph(new OpenXML.Run[] { run });
        }

        private static OpenXML.Paragraph BuildParagraph(OpenXML.Run[] textRuns)
        {
            OpenXML.Paragraph p = BuildParagraph();
            p.Append(textRuns);
            return p;
        }

        private static OpenXML.Paragraph BuildParagraph()
        {
            OpenXML.Paragraph p = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
            OpenXML.ParagraphProperties props = new OpenXML.ParagraphProperties(
                 new OpenXML.Indentation() { End = "0" }
                );
            return p;
        }





        private static OpenXML.Run AddRun(string text, MSWordStringTypes strType = MSWordStringTypes.Typical, bool IsBold = false, bool IsItalic = false)
        {
            OpenXML.Run run = new OpenXML.Run();
            var props = new OpenXML.RunProperties(); 
            if (strType != MSWordStringTypes.Typical)
            {
                props.Append(new OpenXML.RunFonts() { Ascii = "Times New Roman" }, new OpenXML.FontSize() { Val = "18" });
            }else
            {
                if (IsBold) props.Append(new OpenXML.Bold());
                if (IsItalic) props.Append(new OpenXML.Italic());
            }
            switch (strType)
            {
                case MSWordStringTypes.Subscript:
                    props.Append(new OpenXML.VerticalTextAlignment() { Val = OpenXML.VerticalPositionValues.Subscript });
                    break;
                case MSWordStringTypes.Superscript:
                    props.Append(new OpenXML.VerticalTextAlignment() { Val = OpenXML.VerticalPositionValues.Superscript });
                    break;
            }
            run.Append(props);
            run.Append(new OpenXML.Text { Text = text, Space = SpaceProcessingModeValues.Preserve });// (text, SpaceProcessingModeValues=));
            return run;
        }

        
        private static OpenXML.TableCell BuildCell(OpenXML.Paragraph p)
        {
            OpenXML.TableCell cell = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
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
            cell.Append(p);
            return cell;
        }
        private static OpenXML.TableCell BuildCell(OpenXML.Run[] runList)
        {
            OpenXML.Paragraph cellParagraph = BuildParagraph();
            cellParagraph.Append(runList);
            return BuildCell(cellParagraph);
        }

        private static OpenXML.TableCell BuildCell(string content = "")
        {
            OpenXML.Run cellTextRun = AddRun(content);
            OpenXML.Paragraph cellParagraph = BuildParagraph();
            cellParagraph.Append(cellTextRun);
            return BuildCell(cellParagraph);
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

        private static void FillCellText(OpenXML.TableCell cell, OpenXML.Run[] textRun)
        {
            cell.RemoveAllChildren<OpenXML.Paragraph>();
            cell.Append(BuildParagraph());
            cell.GetFirstChild<OpenXML.Paragraph>().Append(textRun);
        }

        private static void FillCellText(OpenXML.TableCell cell, string content)
        {
            cell.RemoveAllChildren<OpenXML.Paragraph>();
            cell.Append(BuildParagraph());
            cell.GetFirstChild<OpenXML.Paragraph>().Append(AddRun(content));
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

        private static void HorizontalMergeCells(OpenXML.TableCell[] cells)
        {
            for(int i=0; i<cells.Length; i++)
            {
                OpenXML.TableCellProperties props = new OpenXML.TableCellProperties();
                props.Append(new OpenXML.HorizontalMerge()
                {
                    Val = i == 0 ? OpenXML.MergedCellValues.Restart : OpenXML.MergedCellValues.Continue
                });
                cells[i].AppendChild<OpenXML.TableCellProperties>(props);
            }
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

        private static int[] CalcMaxRowsCount_For_AoAz(int cols, int rows)
        {
            SubTable[] subTables = wordProtocol.EstimateTablePosition_For_AoAz(cols, rows);
            List<int> template = new List<int>();
            foreach (SubTable st in subTables) template.Add(st.RowsCount);
            return template.ToArray();
        }

        private static int[] CalcMaxRowsCount(int cols, int rows, int minRowsPerTable = 5)
        {
            SubTable[] subTables = wordProtocol.EstimateTablePosition(cols, rows, minRowsPerTable);
            List <int> template = new List<int>();
            foreach (SubTable st in subTables) template.Add(st.RowsCount);
            return template.ToArray();
        }

        private static OpenXML.Run ResultText(TestResult res)
        {
            if (res.Affected)
            {
                return AddRun(res.Status, MSWordStringTypes.Typical, true, true);
            }else
            {
                return AddRun(res.GetStringTableValue(), MSWordStringTypes.Typical, !res.CheckIsItNorma());
                //return ResultValueText(res.BringingValue);
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
            else if (els_amount == 3) return "тр.";
            else if (els_amount == 4) return "чет.";
            else return "пары";
        }

        private static List<OpenXML.Run> ParameterNameText(MeasureParameterType pType, string measure = null)
        {
            List<OpenXML.Run> runList = new List<OpenXML.Run>();
            switch(pType.Id)
            {
                case MeasureParameterType.K12:
                    runList.Add(AddRun("K"));
                    runList.Add(AddRun("12", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.K11:
                    runList.Add(AddRun("K"));
                    runList.Add(AddRun("11", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.K10:
                    runList.Add(AddRun("K"));
                    runList.Add(AddRun("10", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.K9:
                    runList.Add(AddRun("K"));
                    runList.Add(AddRun("9", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.K3:
                    runList.Add(AddRun("K"));
                    runList.Add(AddRun("3", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.K2:
                    runList.Add(AddRun("K"));
                    runList.Add(AddRun("2", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.K1:
                    runList.Add(AddRun("K"));
                    runList.Add(AddRun("1", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.Ea:
                    runList.Add(AddRun("E"));
                    runList.Add(AddRun("a", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.Cp:
                    runList.Add(AddRun("С"));
                    runList.Add(AddRun("р", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.Co:
                    runList.Add(AddRun("С"));
                    runList.Add(AddRun("0", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.Rleads:
                    runList.Add(AddRun("R"));
                    runList.Add(AddRun("ж", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.Ao:
                    runList.Add(AddRun("A"));
                    runList.Add(AddRun("0", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.Az:
                    runList.Add(AddRun("A"));
                    runList.Add(AddRun("з", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.al:
                    runList.Add(AddRun("α"));
                    runList.Add(AddRun("l", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.Risol1:
                case MeasureParameterType.Risol3:
                    runList.Add(AddRun("R"));
                    runList.Add(AddRun("из", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.Risol2:
                case MeasureParameterType.Risol4:
                    runList.Add(AddRun("T"));
                    runList.Add(AddRun("из", MSWordStringTypes.Subscript));
                    break;
                case MeasureParameterType.dR:
                    runList.Add(AddRun("ΔR"));
                    break;
                default:
                    runList.Add(AddRun(pType.Name));
                    break;
            }
            if (measure != null) runList.Add(AddRun($",{measure}"));
            return runList;

        }

        private static int ColumsCountForParameter(MeasuredParameterData d, CableStructure s)
        {
            return ColumsCountForParameter(d.ParameterType, s);
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
                case MeasureParameterType.al:
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
        public float PageWidth;
        public float PageHeight;
        public float CellWidth;
        public float CellHeight;
        private float[] PageLine;

        private List<ShapeCoord> ShapeCoordsList;

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

            PageWidth = WordDocument.PageSetup.PageWidth - WordDocument.PageSetup.LeftMargin;// - WordDocument.PageSetup.RightMargin;
            PageHeight = WordDocument.PageSetup.PageHeight - WordDocument.PageSetup.TopMargin;// - WordDocument.PageSetup.BottomMargin;

            CellHeight = FontSize * 1.3f + 0.3f;
            CellWidth = 24f;
            PageLine = new float[(int)PageWidth];
            ShapeCoordsList = new List<ShapeCoord>();

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

        public void AddParagraph(string text, float shape_height = 50f, float shape_width=0)
        {
            if (shape_width == 0) shape_width = PageWidth - MarginRight; 
            Word.Shape pShape = CreateShape(shape_height, shape_width);
        
            pShape.TextFrame.TextRange.Text = text;
            pShape.TextFrame.TextRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            pShape.TextFrame.MarginLeft = 0;
            pShape.TextFrame.MarginRight = 0;
            
            AddShapeToCoordsList(pShape);
        }

        public void AddElementsAsXML(OpenXmlElement[] elsToAdd, float shapeHeight, float shapeWidth )
        {
            int timesForTrying = 10;
            createTmp:
            DateTime time = DateTime.Now;
            repeat: 
            string filePath = AddTmpFile($"tmp-{time.Day}-{time.Month}-{time.Year}-{time.Hour}-{time.Minute}-{time.Second}-{time.Millisecond}");
            try
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
                {
                    doc.MainDocumentPart.Document.Body.RemoveAllChildren();
                    foreach (OpenXmlElement el in elsToAdd)
                    {
                        doc.MainDocumentPart.Document.Body.Append(el);
                    }
                }
            }
            catch(System.IO.IOException ex)
            {
                goto repeat;
            }

            //Thread.Sleep(1000);
            try
            {
                Word.Shape tableShape = CutCreatedTableFromTmpFile(filePath);

                tableShape.Width = shapeWidth;
                tableShape.Height = shapeHeight;
                tableShape.Line.Transparency = 1f;
                AddShapeToCoordsList(tableShape);
                DeleteTmpFile(filePath);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                DeleteTmpFile(filePath);
                Debug.WriteLine($"AddTable: timesForTrying = {timesForTrying}");
                if (timesForTrying-- > 0) goto createTmp;
                else throw ex;
            }
        }

        public void AddTable(OpenXML.Table table, int colsCount, int rowsCount)
        {
            int timesForTrying = 10;
            createTmp:
            DateTime time = DateTime.Now;
            string filePath = AddTmpFile($"tmp-{time.Day}-{time.Month}-{time.Year}-{time.Hour}-{time.Minute}-{time.Second}-{time.Millisecond}");
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                doc.MainDocumentPart.Document.Body.RemoveAllChildren();
                doc.MainDocumentPart.Document.Body.Append(table);
            }
            //Thread.Sleep(1000);
            try
            {
                Word.Shape tableShape = CutCreatedTableFromTmpFile(filePath);

                tableShape.Width = colsCount * CellWidth + 15f;
                tableShape.Height = rowsCount * CellHeight+15f;
                tableShape.Line.Transparency = 1f;
                AddShapeToCoordsList(tableShape);

                DeleteTmpFile(filePath);
            }
            catch(System.Runtime.InteropServices.COMException ex)
            {
                DeleteTmpFile(filePath);
                Debug.WriteLine($"AddTable: timesForTrying = {timesForTrying}");
                if (timesForTrying-- > 0) goto createTmp;
                else throw ex;
            }

        }



        public Word.Shape CutCreatedTableFromTmpFile(string file_path)
        {
            object file = file_path;//; @"C:\Users\KRA\Documents\Visual Studio 2015\Projects\SAKProtocolManager\SAKProtocolManager\bin\Debug\test.docx";
            Word.Document tmp = WordApp.Documents.Add(ref file, ref oMissing, ref oMissing, ref oMissing);
            tmp.Activate();


            tmp.Select();
            //tmp.Tables[1].Select();
            tmp.ActiveWindow.Selection.Copy();

            WordDocument.Activate();
            Word.Shape oShape = CreateShape();
            oShape.Select();
            WordDocument.ActiveWindow.Selection.Paste();
            tmp.Close(false);
            ///WordApp.Visible = true;
            if (oShape.TextFrame.TextRange.Tables.Count > 0)
            {
                for(int i=1; i<= oShape.TextFrame.TextRange.Tables.Count; i++)
                {
                    Word.Table table = oShape.TextFrame.TextRange.Tables[i];
                    table.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    table.Range.Paragraphs.LeftIndent = 0.0f;
                    table.Range.Paragraphs.SpaceAfter = 0f;
                }
            }
            SetDefaultShapeStyle(oShape);

            Clipboard.Clear();

            return oShape;



        }

        private void SetDefaultShapeStyle(Word.Shape oShape)
        {
            oShape.TextFrame.TextRange.Font.Size = FontSize;
            oShape.TextFrame.TextRange.Font.Name = FontName;
            oShape.TextFrame.TextRange.Font.Color = FontColor;
            oShape.TextFrame.MarginLeft = 1f;
            oShape.TextFrame.MarginRight = 1f;
            oShape.TextFrame.MarginTop = 1f;
            oShape.TextFrame.MarginBottom = 1f;
            oShape.TextFrame.TextRange.Paragraphs.LineUnitAfter = 0.07f;//FontSize * 1.3f;
            oShape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorTop;
            oShape.Fill.Transparency = 1f;
            oShape.Line.Visible = MsoTriState.msoFalse;
        }

        private Word.Shape CreateShape(float height = 50f, float width = 600f)
        {
            Word.Shape oShape = WordDocument.Shapes.AddShape(1, 50, 50, width, height, ref oMissing);
            SetDefaultShapeStyle(oShape);
            return oShape;
        }

        public SubTable[] EstimateTablePosition_For_AoAz(int colsCount, int rowsCount)
        {
            float tableWidth = colsCount * CellWidth; // ширина таблицы
            if (tableWidth > PageWidth) tableWidth = PageWidth; //если таблица выходит за границы её ширина равна ширине страницы
            int tablesOnPageRow = (int)(PageWidth / tableWidth); //количество помещающихся таблиц в одну строку
            int MaxRowsPerTable = 50;//65; //макс количество строк на одну таблицу
            //if (MinRowsPerTable > rowsCount) MinRowsPerTable = rowsCount;
            ShapeCoord lastCoord = LastShapeCoords == null ? new ShapeCoord() { x = 0, y = 0, width = 0, height = 0, page = 0 } : LastShapeCoords; //Координаты последней добавленной фигуры
            float[] pageLine = (float[])PageLine.Clone(); //создаём копию массива фигур для данной строки
            List<SubTable> subTables = new List<SubTable>(); 
            int curPage = lastCoord.page; //устанавливаем текущей странице страницу из координат последней фигуры в документе

            float xCoord = (int)lastCoord.x + (int)lastCoord.width; //устанавливаем текущую координату 
            if (xCoord + tableWidth > PageWidth) xCoord = 0f; //если фигура не умещается устанавливаем x = 0
            float line = GetLine(pageLine, (int)lastCoord.x, (int)tableWidth); // ищем строку для вставки
            if (line + (rowsCount/3) * CellHeight > PageHeight ) // высота 1/3 ячеек таблицы не помещается на текущей странице - переходим на новый лист
            {
                for (int ps = 0; ps < pageLine.Length; ps++) pageLine[ps] = 0f;
                xCoord = 0f;
                curPage++;
            }

            while (rowsCount > 0)
            { 
                line = GetLine(pageLine, (int)lastCoord.x, (int)tableWidth);
                int rowsOnCurrentPosition = (int)((PageHeight - line) / CellHeight); //Количество строк в данной строке документа
                int colsOnCurrentPosition = colsCount; //Количество столбцов в данной строке документа
                int tablesToAddCount = 0; //количестов таблиц для добавления в документ
                if (xCoord == 0)
                {
                    rowsOnCurrentPosition = (int)((PageHeight - line) / CellHeight);
                    tablesToAddCount = tablesOnPageRow;
                    if (rowsCount / tablesOnPageRow < rowsOnCurrentPosition)
                    {
                        if (rowsCount > 14)
                        {
                            rowsOnCurrentPosition = rowsCount / tablesOnPageRow;
                        }else
                        {
                            rowsOnCurrentPosition = rowsCount;
                        }
                    }
                }else
                {
                    tablesToAddCount = 1;
                }
                for (int tIdx = 0; tIdx < tablesToAddCount; tIdx++)
                {
                    int rowsToAddCount = rowsCount - rowsOnCurrentPosition < 0 ? rowsCount : rowsOnCurrentPosition;
                    if (tIdx == tablesToAddCount - 1 && (rowsCount - rowsToAddCount <= rowsOnCurrentPosition)) rowsToAddCount = rowsCount;

                    float tableHeight = rowsToAddCount * CellHeight;
                    ShapeCoord curTableCoord = GetNextShapeCoord(tableWidth, tableHeight, lastCoord, pageLine);
                    subTables.Add(new SubTable() { TableShapePlanedCoord = curTableCoord, ColumnsCount = colsOnCurrentPosition, RowsCount = rowsToAddCount });
                    rowsCount -= rowsToAddCount;
                    lastCoord = curTableCoord;
                }
            }
            return subTables.ToArray();
        }

        public SubTable[] EstimateTablePosition(int colsCount, int rowsCount, int MinRowsPerTable = 5)
        {
            float tableWidth = colsCount * CellWidth;
            if (tableWidth > PageWidth) tableWidth = PageWidth;
            int tablesOnPageRow = (int)(PageWidth / tableWidth);
            int MaxRowsPerTable = 65;
            if (MinRowsPerTable > rowsCount) MinRowsPerTable = rowsCount;
            ShapeCoord lastCoord = LastShapeCoords == null ? new ShapeCoord() { x = 0, y = 0, width = 0, height =0, page=0} : LastShapeCoords;
            float[] pageLine = (float[])PageLine.Clone();
            List<SubTable> subTables = new List<SubTable>();
            int curPage = lastCoord.page;

            float xCoord = (int)lastCoord.x + (int)lastCoord.width;
            if (xCoord + tableWidth > PageWidth || lastCoord.height < MinRowsPerTable*CellHeight) xCoord = 0f;
            float line = GetLine(pageLine, (int)lastCoord.x, (int)tableWidth);
            if (line + MinRowsPerTable*CellHeight > PageHeight)
            {
                for (int ps = 0; ps < pageLine.Length; ps++) pageLine[ps] = 0f;
                xCoord = 0f;
                curPage++;
            }
         
            while (rowsCount>0)
            {

                int rowsOnCurrentPosition; //Количество строк в данной строке документа
                int colsOnCurrentPosition; 
                int tablesToAddCount = 0;
                if (xCoord == 0)
                {
                    rowsOnCurrentPosition = (int)((PageHeight-line) / CellHeight);
                    if (rowsOnCurrentPosition > MaxRowsPerTable) rowsOnCurrentPosition = MaxRowsPerTable;
                    colsOnCurrentPosition = (int)((PageWidth) / CellWidth);
                    if (rowsOnCurrentPosition * tablesOnPageRow > rowsCount)
                    {
                        tablesToAddCount = tablesOnPageRow;
                        for(int tCnt = 1; tCnt <= tablesOnPageRow; tCnt++)
                        {
                            if (rowsCount / tCnt > MinRowsPerTable)
                            {
                                tablesToAddCount = tCnt;
                            }
                        }
                        rowsOnCurrentPosition = rowsCount / tablesToAddCount;
                    } else
                    {
                        tablesToAddCount = tablesOnPageRow;
                    }

                }else
                {
                    //Это выполняется только если таблица вставляется в строку с другим блоком и только для первой таблицы в коллекции
                    rowsOnCurrentPosition = (int)(lastCoord.height / CellHeight);
                    colsOnCurrentPosition = (int)((PageWidth - xCoord) / CellWidth);
                    if (rowsOnCurrentPosition >= rowsCount)
                    {
                        rowsOnCurrentPosition = rowsCount;
                        tablesToAddCount = 1;
                    }else
                    {
                        tablesToAddCount = colsOnCurrentPosition / colsCount;
                    }
                }
                for(int tIdx = 0; tIdx < tablesToAddCount; tIdx++)
                {
                    int rowsToAddCount = rowsCount - rowsOnCurrentPosition < 0 ? rowsCount : rowsOnCurrentPosition;
                    if (tIdx == tablesToAddCount-1 && (rowsCount - rowsToAddCount <= MinRowsPerTable)) rowsToAddCount = rowsCount;

                    float tableHeight = rowsToAddCount * CellHeight;
                    ShapeCoord curTableCoord = GetNextShapeCoord(tableWidth, tableHeight, lastCoord, pageLine);
                    subTables.Add(new SubTable() { TableShapePlanedCoord = curTableCoord, ColumnsCount = colsCount, RowsCount = rowsToAddCount });
                    rowsCount -= rowsToAddCount;
                    lastCoord = curTableCoord;
                }
            }
            return subTables.ToArray();
        }

        

        private ShapeCoord GetNextShapeCoord(float shapeWidth, float shapeHeight, ShapeCoord lastCoord, float[] LineArr=null)
        {
            float cx = lastCoord != null ? lastCoord.x + lastCoord.width : 0f;
            int cpage = lastCoord != null ? lastCoord.page : 0;
            if (LineArr == null) LineArr = PageLine;
            if (shapeWidth > PageWidth) shapeWidth = PageWidth;
            if ((cx + shapeWidth) > PageWidth) cx = 0f;
            if ((GetLine(LineArr, (int)cx, (int)shapeWidth) + shapeHeight) > PageHeight)
            {
                for (int ps = 0; ps < LineArr.Length; ps++) LineArr[ps] = 0f;
                cx = 0f;
                cpage++;
            }
            ShapeCoord newCoord = new ShapeCoord(cx, GetLine(LineArr, (int)cx, (int)shapeWidth), cpage) { width = shapeWidth, height = shapeHeight };
            SetLine(LineArr, (int)cx, (int)shapeWidth, (int)shapeHeight);
            return newCoord;
        }

        private void AddShapeToCoordsList(Word.Shape oShape)
        {
            //CutShape(oShape);
            ShapeCoord newShape = GetNextShapeCoord(oShape.Width, oShape.Height, LastShapeCoords);
            int lastShapePage = LastShapeCoords == null ? 0 : LastShapeCoords.page;
            object oEndOfDoc = "\\endofdoc";
            if (lastShapePage < newShape.page)
            {
                object ob = WdBreakType.wdPageBreak;
                WordDocument.Bookmarks.get_Item(ref oEndOfDoc).Range.InsertBreak(ref ob);
            }
            ShapeCoordsList.Add(newShape);
            Debug.WriteLine($"x={ShapeCoordsList.Last().x}; y={ShapeCoordsList.Last().y}; page={ShapeCoordsList.Last().page}; width={ShapeCoordsList.Last().width}; height={ShapeCoordsList.Last().height}");
        }

        public void PlaceShapes()
        {
            object pos = 1;
            for (int i = 0; i < WordDocument.Shapes.Count; i++)
            {
                if (ShapeCoordsList[i].page == 0)
                {
                    pos = i + 1;
                    WordDocument.Shapes.get_Item(ref pos).Left = ShapeCoordsList[i].x;
                    WordDocument.Shapes.get_Item(ref pos).Top = ShapeCoordsList[i].y;
                    pos = i + 2;
                }
                else
                {
                    Word.ShapeRange srng = WordDocument.Shapes.Range(ref pos);
                    object jdx = true;
                    srng.Select(ref jdx);
                    WordDocument.ActiveWindow.Selection.Cut();
                    int tm = ShapeCoordsList[i].page;
                    while (tm-- > 0) WordDocument.ActiveWindow.Selection.GoToNext(WdGoToItem.wdGoToPage);
                    WordDocument.ActiveWindow.Selection.Paste();
                    object tp = WordDocument.Shapes.Count;
                    WordDocument.Shapes.get_Item(ref tp).Left = ShapeCoordsList[i].x;
                    WordDocument.Shapes.get_Item(ref tp).Top = ShapeCoordsList[i].y;
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
            object oVisible = false;
            Word.Document doc = WordApp.Documents.Add(ref filePath, ref isTemplate, ref oMissing, ref oVisible);
            //doc..Add(ref oMissing);
            doc.SaveAs2(ref filePath);
            doc.Close();
            return string_path;
        }

        private void CreateWordFile(string file_path)
        {
            if (!File.Exists(file_path))
            {
                FileStream fs = File.Create(file_path);
                fs.Close();
                fs.Dispose();
            }
        }

        public string CreateTmpFile(string file_name)
        {
            string filePath = Path.Combine(TmpFilesFolderDir, file_name);
            if (File.Exists(filePath)) File.Delete(filePath);
            CreateWordFile(filePath);
            return filePath;
        }



        public void ResizeShapeByTable(Word.Shape oShape)
        {
            oShape.Line.Transparency = 1f;

            if (oShape.TextFrame.TextRange.Tables.Count > 0)
            {
                float width = 20f;
                float height = oShape.TextFrame.TextRange.Tables[1].Rows.Count * FontSize * 1.5f + 10f;
                for (int i = 1; ; i++)
                {
                    try
                    {
                        oShape.TextFrame.TextRange.Tables[1].Cell(1, i);
                        width += 20f;// oShape.TextFrame.TextRange.Tables[1].Cell(1, i).Width*0.8f;
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

        private float GetLine(float[] arr, int begin, int width)
        {
            float ret = 0f;
            int limit = (begin + width > PageWidth) ? (int)PageWidth : begin + width;
            for (int pos = begin; pos < limit; pos++) if (arr[pos] > ret) ret = arr[pos];
            return ret;
        }
        private void SetLine(float[] arr, int begin, int width, float height)
        {
            float val = GetLine(arr, begin, width) + height;
            for (int pos = begin; pos < (begin + width); pos++) arr[pos] = val;
        }

        private void replaceRegular(ref Word.Shape sh, CableTest test)
        {
            int quant = sh.TextFrame.TextRange.Paragraphs.Count;
            for (int i = 1; i <= quant; i++)
            {
                WdParagraphAlignment wpa = sh.TextFrame.TextRange.Paragraphs[i].Alignment;
                string text = sh.TextFrame.TextRange.Paragraphs[i].Range.Text;
                if (text.Length < 5) continue;
                text = text.Replace("#маркакабеля", test.TestedCable.Name);
                text = text.Replace("#температура", $"{test.Temperature} °С");
                text = text.Replace("#датаиспытания", test.TestDate.ToString("dd.MM.yyyy"));
                text = text.Replace("#номербарабана", test.Baraban.Number);
                text = text.Replace("#типбарабана", test.Baraban.Name);
                text = text.Replace("#брутто", $"{test.BruttoWeight} кг");
                text = text.Replace("#длинакабеля", $"{test.TestedLength} м");
                text = text.Replace("#номерзаказа", $"{test.OrderNumber}");
                //text = text.Replace("TestStatistic", tres.ValidElementsAmount.ToString());
                //string st = " пар ";
                //if (tres.Plan.Structure == 4) st = " четв. ";
                /*
                text = text.Replace("#колвоэлементов", st + tres.Plan.StructQuantity.ToString());
                if (text.Contains("TestStat"))
                {
                    text = text.Replace("TestStat", $"Фактическое число{st}- { tres.Plan.StructQuantity.ToString()}. Годных - {tres.ValidElementsAmount.ToString()}.");
                }
                if (tres.CableLen == 0) text = text.Replace("Lenght", "Без приведения к длине");
                else text = text.Replace("Length", tres.CableLen.ToString());
                text = text.Replace("Serial", tres.Serial);
                if (text.Contains("Goden"))
                {
                    bool goden = false;
                    for (int jdx = 0; jdx <= (int)MesParam.outside; jdx++) if (tres.CommonRes[jdx]) goden = true;
                    if (goden) text = text.Replace("Goden", "не");
                    else text = text.Replace("Goden", "");
                }
                text = text.Replace("GOST", tres.Plan.GostTU);
                */
                sh.TextFrame.TextRange.Paragraphs[i].Range.Text = text;
                sh.TextFrame.TextRange.Paragraphs[i].Alignment = wpa;
            }
        }

        internal void AddHeader(CableTest test)
        {
            object filePath = ProtocolHeaderFile;
            object oSave = false;
            Word.Document headerDoc = WordApp.Documents.Add(ref filePath, ref oMissing, ref oMissing, ref oMissing);
            if (headerDoc.Shapes.Count > 0)
            {
                object idx = 1;
                Word.Shape oShape = headerDoc.Shapes.get_Item(ref idx);
                oShape.Width = PageWidth - MarginRight;
                oShape.RelativeHorizontalPosition = WdRelativeHorizontalPosition.wdRelativeHorizontalPositionMargin;
                AddShapeToCoordsList(oShape);
                replaceRegular(ref oShape, test);
                //oShape.Width = PageWidth;
                Word.ShapeRange sr = headerDoc.Shapes.Range(ref idx);
                object rep = true;
                sr.Select(ref rep);
                headerDoc.ActiveWindow.Selection.Copy();
                WordDocument.Activate();
                WordDocument.ActiveWindow.Selection.Paste();
                WordDocument.ActiveWindow.Selection.Start = WordDocument.ActiveWindow.Selection.End;
            }
            headerDoc.Close(ref oSave, ref oMissing, ref oMissing);
        }

        internal void AddFooter(CableTest test)
        {
            object filePath = ProtocolFooterFile;
            object oSave = false;
            object oVisible = false;
            Word.Document footerDoc = WordApp.Documents.Add(ref filePath, ref oMissing, ref oMissing, ref oMissing);
            if (footerDoc.Shapes.Count > 0)
            {
                object idx = 1;
                Word.Shape oShape = footerDoc.Shapes.get_Item(ref idx);
                oShape.Width = PageWidth - MarginRight;
                oShape.RelativeHorizontalPosition = WdRelativeHorizontalPosition.wdRelativeHorizontalPositionMargin;
                AddShapeToCoordsList(oShape);
                replaceRegular(ref oShape, test);
                Word.ShapeRange sr = footerDoc.Shapes.Range(ref idx);
                object rep = true;
                sr.Select(ref rep);
                footerDoc.ActiveWindow.Selection.Copy();
                WordDocument.Activate();
                if (WordDocument.Paragraphs.Count > 0)
                {
                    object rpt = false;
                    WordDocument.Paragraphs[WordDocument.Paragraphs.Count].Range.Select();
                }
                WordDocument.ActiveWindow.Selection.Paste();
                WordDocument.ActiveWindow.Selection.Start = WordDocument.ActiveWindow.Selection.End;
                AddShapeToCoordsList(oShape);
            }
            footerDoc.Close(ref oSave, ref oMissing, ref oMissing);
        }

        private ShapeCoord LastShapeCoords => ShapeCoordsList.Count > 0 ? ShapeCoordsList.Last() : null;


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

        private string ProtocolTemplatesDir
        {
            get
            {
                string path = Path.Combine(RootProtocolsDir, "Шаблоны");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public string ProtocolFooterFile
        {
            get
            {
                string fileName = "Footer.docx";
                string filePath = Path.Combine(ProtocolTemplatesDir, fileName);
                if (!File.Exists(filePath))
                {
                    CreateWordFile(filePath);
                }
                return filePath;
            }
        }

        public string ProtocolHeaderFile
        {
            get
            {
                string fileName = "Header.docx";
                string filePath = Path.Combine(ProtocolTemplatesDir, fileName);
                if (!File.Exists(filePath))
                {
                    CreateWordFile(filePath);
                }
                return filePath;
            }
        }

        public string TmpFilesFolderDir
        {
            get
            {
                string path = path = Path.Combine(RootProtocolsDir, "tmp");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public string RootProtocolsDir
        {
            get
            {
                string path = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Протоколы MSWord");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
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

    public class SubTable
    {
        public int ColumnsCount;
        public int RowsCount;
        public ShapeCoord TableShapePlanedCoord;

        public SubTable()
        {

        }
    }

    public class ShapeCoord
    {
        public float x;
        public float y;
        public float height;
        public float width;
        public int page;

        public ShapeCoord()
        {

        }
        public ShapeCoord(float tx, float ty, int tpage)
        {
            x = tx;
            y = ty;
            page = tpage;
        }
    }


    internal enum MSWordStringTypes
    {
        Typical,
        Subscript,
        Superscript
    }


    internal class AoAz_TableValues
    {
        Dictionary<string, Dictionary<string, TestResult>> tableVals;
        public int startSubElNumber = 0;
        public int endSubElNumber = int.MinValue;
        public int endElNumber = int.MinValue;
        public int startElNumber = 0;
        public TestResult LastAdded = null;
        public bool HasValues = false;
        public int ElementsCount
        {
            get
            {
                return endElNumber - startElNumber + 1;
            }
        }
        public AoAz_TableValues()
        {
            tableVals = new Dictionary<string, Dictionary<string, TestResult>>();
        } 

        public bool IsValid(TestResult r)
        {
            if (LastAdded == null) return true;
            if (LastAdded.GeneratorElementNumber >= r.GeneratorElementNumber) return true;
            return r.ElementNumber < r.GeneratorElementNumber;
        }
        public void AddResult(TestResult r)
        {
            string genKey = $"{r.GeneratorElementNumber}-{r.GeneratorSubElementNumber}"; 
            string recKey = $"{r.ElementNumber}-{r.SubElementNumber}";

            //if (!IsValid(r)) return;
            if (startSubElNumber == 0) startSubElNumber = r.GeneratorSubElementNumber;
            if (endSubElNumber < r.SubElementNumber) endSubElNumber = r.SubElementNumber;
            if (startElNumber == 0) startElNumber = r.GeneratorElementNumber;
            if (endElNumber < r.ElementNumber) endElNumber = r.ElementNumber;
            if (!tableVals.ContainsKey(genKey)) tableVals.Add(genKey, new Dictionary<string, TestResult>());
            tableVals[genKey].Add(recKey, r);
            LastAdded = r;
            HasValues = true;
        }

        public TestResult GetResult(int genNumber, int recNumber, int subGenNumber = 1, int subRecNumber = 1 )
        {
            string genKey = $"{genNumber}-{subGenNumber}";
            string recKey = $"{recNumber}-{subRecNumber}";
            if (!tableVals.ContainsKey(genKey)) return null;
            if (!tableVals[genKey].ContainsKey(recKey)) return null;
            return tableVals[genKey][recKey];
        }

    }
}
