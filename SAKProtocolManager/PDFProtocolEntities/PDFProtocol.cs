using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SAKProtocolManager.DBEntities;
using SAKProtocolManager.DBEntities.TestResultEntities;
using System.Windows.Forms;

namespace SAKProtocolManager.PDFProtocolEntities
{
   
    //примеры здесь https://gist.github.com/dedico/1567859
    public class PDFProtocol
    {
        string[] PrimaryParameterNames = new string[] { "Rж", "dR", "Cр", "dCр", "Co", "Rиз1", "Rиз2", "Ea", "K1", "K2", "K3", "K9", "K10", "K11", "K12", "K2,K3", "K9-12"};
        PdfWriter Writer;
        int MaxColsPerLine = 18;
        float defaultColWidth = 6;
        float workHeight, workWidth;

        BaseColor oddColor = new BaseColor(System.Drawing.Color.WhiteSmoke);

        private CableTest Test;
        Font DocFont;
        Font DocFontBold;
        Document ProtocolDoc;
        public PDFProtocol(CableTest test)
        {
            this.Test = test;
            SetFonts();
            FileStream file = File.Create("protocol.pdf");
            ProtocolDoc = new Document(PageSize.A4);
            Writer = PdfWriter.GetInstance(ProtocolDoc, file);
            workWidth = ProtocolDoc.PageSize.Width - ProtocolDoc.RightMargin - ProtocolDoc.LeftMargin;
            workHeight = ProtocolDoc.PageSize.Height - ProtocolDoc.TopMargin - ProtocolDoc.BottomMargin;
            ProtocolDoc.Open();
            BuildProtocol();
            ProtocolDoc.Close();
            Process.Start("protocol.pdf");
            //DemoProtocol();
        }

        private void SetFonts()
        {
            //string sylfaenpath = "Fonts/Ubuntu-R.ttf";//Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\tahoma.ttf";
            this.DocFont = FontFactory.GetFont("Fonts/Ubuntu-R.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            this.DocFont.SetStyle(Font.NORMAL);
            this.DocFont.Size = 10;
            this.DocFontBold = FontFactory.GetFont("Fonts/Ubuntu-B.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            this.DocFontBold.SetStyle(Font.NORMAL);
            this.DocFontBold.Size = 10;
        }
        private void BuildProtocol()
        {
            PrintProtocolHeader();
            PrintCableTestInformation();
            //DemoProtocol();
            foreach (CableStructure str in Test.TestedCable.Structures) PrintStructureTestResult(str);
        }

        private void PrintProtocolHeader()
        {
            Font f = new Font(DocFontBold);
            f.Size = 14;
            Chunk header= new Chunk("Паспорт качества. \n \n", f);
            Paragraph p = new Paragraph(header);
            p.Alignment = Element.ALIGN_CENTER;
            ProtocolDoc.Add(p);
        }
        private void PrintCableTestInformation()
        {
            float[] leftLeftCol = { ProtocolDoc.LeftMargin }; 
            Paragraph left = new Paragraph();
            Paragraph right = new Paragraph();
            Chunk newLine = new Chunk("\n");
            Chunk cableNumber = new Chunk(String.Format("Марка кабеля: {0}", Test.TestedCable.Name), DocFont);
            Chunk barabanNumber = new Chunk(String.Format("№ Барабана: {0}", Test.Baraban.Number), DocFont);
            Chunk cableLength = new Chunk(String.Format("Длина кабеля: {0}м", Test.TestedLength), DocFont);
            Chunk cableBrutto = new Chunk(String.Format("БРУТТО: {0}кг", Test.BruttoWeight), DocFont);
            Chunk barabanType = new Chunk(String.Format("Тип барабана {0}", Test.Baraban.Name), DocFont);
            Chunk temperature = new Chunk(String.Format("Температура {0}°С", Test.Temperature), DocFont);
            Chunk testDate = new Chunk(String.Format("Дата испытаний {0}", ServiceFunctions.MyDate(Test.TestDate)), DocFont);

            

            left.Add(cableNumber);
            left.Add(newLine);
            left.Add(barabanNumber);
            left.Add(newLine);
            left.Add(cableLength);
            left.Add(newLine);
            left.Add(cableBrutto);

            right.Add(barabanType);
            right.Add(newLine);
            right.Add(temperature);
            right.Add(newLine);
            right.Add(testDate);


            PdfContentByte cb = Writer.DirectContent;
            ColumnText ct = new ColumnText(cb);

            float columnWidth = workWidth/2;
            float top = ProtocolDoc.Top - 100f;
            float bottom = top - 200f;
            float[] left1 = { ProtocolDoc.Left, top, ProtocolDoc.Left, bottom};
            float[] right1 = { ProtocolDoc.Left + columnWidth, top, ProtocolDoc.Left + columnWidth, bottom };
            float[] left2 = { ProtocolDoc.Right - columnWidth, top, ProtocolDoc.Right - columnWidth, bottom };
            float[] right2 = { ProtocolDoc.Right, top, ProtocolDoc.Right, bottom };

            ct.SetColumns(left1, right1);
            ct.AddText(left);
            ct.Go();

            ct.SetColumns(left2, right2);
            ct.AddText(right);
            ct.Go();
           // ProtocolDoc.Add(ct);
        }

        
        private void PrintStructureTestResult(CableStructure structure)
        {
            if(Test.TestedCable.Structures.Length > 1)
            {
                Font f = DocFontBold;
                f.Size = 12;
                Chunk structHeader = new Chunk(String.Format("Параметры по структуре {0}", structure.Name), f);
                Paragraph strHeader = new Paragraph(structHeader);
                ProtocolDoc.Add(strHeader);
            }
            int colsCount = GetPrimaryParametersCount(structure) + 2;
            MeasuredParameterData[] firstTableData = GetTableData(1, structure);
            PdfPTable Table1 = PrimeParamsTable(firstTableData);
            //ProtocolDoc.Add(Table1);
            //if (colsCount > MaxColsPerLine)
            //{
            //    MeasuredParameterData[] firstTableData = GetTableData(1, structure);
            //    PdfPTable Table1 = DrawPrimaryParametersTable(firstTableData);
            //     ProtocolDoc.Add(Table1);
            //}else
            //{
            //    MeasuredParameterData[] firstTableData = GetTableData(1, structure);
            //    PdfPTable Table1 = DrawPrimaryParametersTable(firstTableData);
            //     ProtocolDoc.Add(Table1);
            // }

        }

        private PdfPTable PrimeParamsTable(MeasuredParameterData[] pData)
        {
            List<PdfPTable> tables = new List<PdfPTable>();
            Font thFont = new Font(DocFontBold);
            Font trFont = new Font(DocFont);
            trFont.Size = thFont.Size = 8;
            List<float> colsWidth = new List<float>();
            colsWidth.Add(defaultColWidth * 2 / 3);
            
            foreach (MeasuredParameterData pd in pData)
            {
                TestResult tr = pd.TestResults[0];
                for (int i = 0; i < tr.Values.Length; i++) colsWidth.Add(defaultColWidth);
            }
            int tablesCount = GetDivider(colsWidth.Count, pData[0].TestResults.Length);
            int maxRows = pData[0].TestResults.Length / tablesCount;
            for(int i = 0; i < tablesCount; i++)
            {
                bool isLastTable = i == (tablesCount-1);
                int startRow = i*maxRows;
                int endRow = isLastTable ? pData[0].TestResults.Length : i*maxRows + maxRows;
                PdfPTable table = new PdfPTable(colsWidth.ToArray());
                Phrase elNumber = new Phrase(String.Format("{0} №", pData[0].ParameterType.Structure.BendingTypeName), thFont);
                PdfPCell c = new PdfPCell(elNumber);
                c.Rotation = 90;
                table.AddCell(c);
                foreach (MeasuredParameterData pd in pData)
                {
                    Phrase pName = new Phrase(String.Format("{0}, {1}", pd.ParameterType.Name, pd.ResultMeasure()), thFont);
                    c = new PdfPCell(pName);
                    c.VerticalAlignment = Element.ALIGN_MIDDLE;
                    c.HorizontalAlignment = Element.ALIGN_CENTER;
                    c.Colspan = pd.TestResults[0].Values.Length;
                    c.MinimumHeight = 30;
                    table.AddCell(c);
                }
                table.HeaderRows = 1;
                int j = 0;
                for (int idx = startRow; idx < endRow; idx++)
                {
                    bool isOdd = j % 2 == 0;
                    int el = pData[0].TestResults[idx].ElementNumber;
                    elNumber = new Phrase(el.ToString(), trFont);
                    PdfPCell numCell = new PdfPCell(elNumber);
                    if (isOdd) numCell.BackgroundColor = oddColor;
                    table.AddCell(numCell);
                    for (int pdIdx = 0; pdIdx < pData.Length; pdIdx++)
                    {
                        TestResult tr = pData[pdIdx].TestResults[idx];
                        for (int vIdx = 0; vIdx < tr.Values.Length; vIdx++)
                        {
                            Phrase val = new Phrase(tr.Values[vIdx].ToString(), trFont);
                            PdfPCell valCell = new PdfPCell(val);
                            valCell.VerticalAlignment = Element.ALIGN_CENTER;
                            valCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            if (isOdd) valCell.BackgroundColor = oddColor;
                            table.AddCell(valCell);
                            
                        }
                    }
                    j++;
                }
                tables.Add(table);
            }
            float top = ProtocolDoc.Top - 20f;
            float bottom = ProtocolDoc.Bottom;
            colsWidth = new List<float>();
            int cols = (int)tables.Count;
            float colWidth = workWidth / tables.Count;
            List<float[]> coords = new List<float[]>();
            for (int i = 0; i < cols; i++)
            {
                float left = ProtocolDoc.Left + (i*colWidth);
                coords.Add(new float[] { left, top, left, bottom });
                coords.Add(new float[] { left + colWidth, top, left+colWidth, bottom });
            }
            PdfContentByte cb = Writer.DirectContent;
            ColumnText ct = new ColumnText(cb);
            /*
            for(int i = 0; i<tables.Count; i++)
            {
                colsWidth.Add(1);
            }
            PdfPTable rTable = new PdfPTable(colsWidth.ToArray());
            */
            for(int i = 0; i<tables.Count; i++)
            {
                PdfPTable t = tables[i];
                ct.SetColumns(coords[2*i], coords[2*i+1]);
                ct.AddElement(t);
            }
            ct.Go();
           
            return tables[0];
        }

        private PdfPTable DrawPrimaryParametersTable(MeasuredParameterData[] pData)
        {
            Font thFont = new Font(DocFontBold);
            Font trFont = new Font(DocFont);
            trFont.Size = thFont.Size = 8;
            List<float> colsWidth = new List<float>();
            colsWidth.Add(defaultColWidth*2/3);
            foreach (MeasuredParameterData pd in pData)
            {
                TestResult tr = pd.TestResults[0];
                for (int i = 0; i < tr.Values.Length; i++) colsWidth.Add(defaultColWidth);
            }
            int baseCount = colsWidth.Count;
            int divider = GetDivider(baseCount, pData[0].TestResults.Length);
            if (divider > 1)
            {
                for (int j = 1; j < divider; j++)
                {
                    for(int i=0; i< baseCount; i++)
                    {
                        colsWidth.Add(colsWidth[i]);
                    }
                }
            }
            PdfPTable table = new PdfPTable(colsWidth.ToArray());
            table.DefaultCell.UseAscender = true;
            table.DefaultCell.UseDescender = true;
            table.WidthPercentage = (100 / MaxColsPerLine) * colsWidth.Count;
            Phrase elNumber = new Phrase(String.Format("{0} №", pData[0].ParameterType.Structure.BendingTypeName), thFont);
            for(int j = 0; j<divider; j++)
            {
                PdfPCell c = new PdfPCell(elNumber);
                c.Rotation = 90;
                table.AddCell(c);
                foreach (MeasuredParameterData pd in pData)
                {
                    Phrase pName = new Phrase(String.Format("{0}{1}", pd.ParameterType.Name, pd.ResultMeasure()), thFont);
                    c = new PdfPCell(pName);
                    c.VerticalAlignment = Element.ALIGN_MIDDLE;
                    c.HorizontalAlignment = Element.ALIGN_CENTER;
                    c.Colspan = pd.TestResults[0].Values.Length;
                    c.MinimumHeight = 30;
                    table.AddCell(c);
                }
            }
            
            table.HeaderRows = 1;
            int length = pData[0].TestResults.Length;
            int maxRows = length / divider;
            for (int i = 0; i<maxRows; i++)
            {
                bool isOdd = i % 2 == 0;
                for(int j=0; j<divider; j++)
                {
                    int idx = j * maxRows+i;
                    if (j*i < length)
                    {

                        int el = pData[0].TestResults[idx].ElementNumber;
                        elNumber = new Phrase(el.ToString(), trFont);
                        PdfPCell numCell = new PdfPCell(elNumber);
                        if (isOdd) numCell.BackgroundColor = oddColor;
                        table.AddCell(numCell);
                        for(int pdIdx = 0; pdIdx<pData.Length; pdIdx++ )
                        {
                            TestResult tr = pData[pdIdx].TestResults[idx];
                            for(int vIdx = 0; vIdx < tr.Values.Length; vIdx++)
                            {
                                Phrase val = new Phrase(tr.Values[vIdx].ToString(), trFont);
                                PdfPCell c = new PdfPCell(val);
                                c.VerticalAlignment = Element.ALIGN_CENTER;
                                c.HorizontalAlignment = Element.ALIGN_CENTER;
                                
                                if(isOdd) c.BackgroundColor = oddColor;
                                table.AddCell(c);
                            }
                        }
                    }
                   
                }
                
            }
            return table;
        }
        private void DemoProtocol()
        {
            Font thFont = new Font(DocFontBold);
            Font trFont = new Font(DocFont);
            trFont.Size = thFont.Size = 7;
            int cols = 5;
            int rows = 600;
            int divider = GetDivider(cols, rows);
            float[] columnWidths = new float[cols*divider];
            for (int i = 0; i < columnWidths.Length; i++) columnWidths[i] = 1;
            
            PdfPTable table = new PdfPTable(columnWidths);
            table.WidthPercentage = (100*cols*divider)/MaxColsPerLine;
            //table.DefaultCell.UseAscender = true;
            //table.DefaultCell.UseDescender = true;

            //table.HeaderRows = 1;
            for (int j = 0; j < divider; j++)
            {
                for (int i = 0; i < cols; i++)
                 {

                    Phrase p = new Phrase("Столбец_" + (i + 1).ToString(), thFont);
                    table.AddCell(p);
                 }
            }
            /*
            Random r = new Random();
            for (int counter = 0; counter < rows; counter++)
            {
                for(int i = 0; i < cols; i++)
                {
                    int v = r.Next(100,300);
                    Phrase p = new Phrase(v.ToString(), trFont);
                    table.AddCell(p);
                }

            }
            PdfPRow r = new PdfPRow()
            table.Rows.Add;
            Phrase pr = new Phrase(table.CalculateHeights().ToString(), trFont);
            ProtocolDoc.Add(pr);
            */
            ProtocolDoc.Add(table);
        }

        private MeasuredParameterData[] GetTableData(int table_number, CableStructure structure)
        {
            List<MeasuredParameterData> pData = new List<MeasuredParameterData>();
            int count = 1;
            int tNumber = 1;
            foreach (MeasureParameterType pt in structure.MeasuredParameters)
            {
                foreach (MeasuredParameterData pd in pt.ParameterData)
                {
                    if (pd.TestResults[0].GetType().Name == "PrimaryParametersTestResult")
                    {
                        int l = pd.TestResults[0].Values.Length;
                        if (tNumber != table_number)
                        {
                            if (count + l > MaxColsPerLine)
                            {
                                count = 1 + l;
                                tNumber++;
                            }
                            else count += l;
                        }
                        if (tNumber == table_number)
                        {
                            pData.Add(pd);
                            count += l;
                        }
                    }
                }
            }
            return pData.ToArray();
        }
        private int GetPrimaryParametersCount(CableStructure structure)
        {
            int count = 0;
            List<MeasuredParameterData> table = new List<MeasuredParameterData>();
            foreach(MeasureParameterType pt in structure.MeasuredParameters)
            {
                foreach(MeasuredParameterData pd in pt.ParameterData)
                {
                    if (pd.TestResults[0].GetType().Name == "PrimaryParametersTestResult") count += pd.TestResults[0].Values.Length;
                }
            }
            return count;
        }

        private int GetDivider(int cCount, int tResultLength)
        {
            int max_divider = (tResultLength/10 != 0) ? tResultLength / 10 : 1;
            int divider = MaxColsPerLine / cCount;
            if (divider > max_divider) return max_divider;
            else return divider;
        }

        public static void MakeOldStylePDFProtocol(string test_id)
        {
            try
            {
                checkPath:
                string filePath = Properties.Settings.Default.PathToClient3;
                if (!File.Exists(filePath) || filePath.IndexOf("Client3.exe") < 0)
                {
                    DialogResult dr = MessageBox.Show(String.Format("Приложение Client3.exe, необходимое для формирования протокола не найдено по адресу \n{0}\n\nВыбрать новое место расположения?", filePath), "Client3.exe не найден", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                    if (dr == DialogResult.Yes)
                    {
                        OpenFileDialog dlg = new OpenFileDialog();
                        dlg.FileName = "Client3.exe";
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            Properties.Settings.Default.PathToClient3 = dlg.FileName;
                        }
                        goto checkPath;
                    }
                    else
                    {
                        MessageBox.Show("Протокол не был экспортирован в PDF", "", MessageBoxButtons.OK);
                        return;
                    }
                }
                string workPath = filePath.Replace("Client3.exe", "");
                ProcessStartInfo startInfo = new ProcessStartInfo(String.Format(@"{0}", filePath));
                startInfo.WorkingDirectory = String.Format(@"{0}", workPath);
                startInfo.Arguments = String.Format("{0} {1}", test_id, 1);
                Process pr = Process.Start(startInfo);
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Не найдено приложение Client3.exe", e.Message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

        }

    }
}
