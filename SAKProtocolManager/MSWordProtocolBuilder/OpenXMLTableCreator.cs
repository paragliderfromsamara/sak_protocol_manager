using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SAKProtocolManager.MSWordProtocolBuilder
{
    public class OpenXMLTableCreator
    {
        //Stream stream = new FileStream("test.docx", FileMode.CreateNew);
        private static object oMissing = System.Reflection.Missing.Value;

        public static void GetTableFromFile()
        {
            Word.Document WordDoc;
            Word.Document WordDocTmp;
            Word.Application wapp = new Word.Application();
            wapp.Visible = true;

        }

        public static void WDAddTable(string fileName, string[,] data)
        {

            using (WordprocessingDocument doc = WordprocessingDocument.Open(@"test.docx", true))
            {
                // Create an empty table.
                DocumentFormat.OpenXml.Wordprocessing.Table table = new DocumentFormat.OpenXml.Wordprocessing.Table();

                /*
                TableCellProperties cellOneProperties = new TableCellProperties();
                cellOneProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Restart
                });

                TableCellProperties cellTwoProperties = new TableCellProperties();
                cellTwoProperties.Append(new HorizontalMerge()
                {
                    Val = MergedCellValues.Continue
                });

                cell11.Append(new Paragraph(new Run(new Text("cell11"))));
                cell12.Append(new Paragraph(new Run(new Text("cell21"))));
                cell13.Append(new Paragraph(new Run(new Text("cell22"))));

                cell11.AppendChild<TableCellProperties>(cellOneProperties);
                cell12.AppendChild<TableCellProperties>(cellTwoProperties);

                row2.Append(cell11);
                row2.Append(cell12);
                row2.Append(cell13);

                */
                // Create a TableProperties object and specify its border information.
                TableProperties tblProp = new TableProperties(
                    new TableBorders(
                        new TopBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 2
                        },
                        new BottomBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 2
                        },
                        new LeftBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 2
                        },
                        new RightBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 2
                        },
                        new InsideHorizontalBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 2
                        },
                        new InsideVerticalBorder()
                        {
                            Val =
                            new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                            Size = 2
                        }
                    )
                );

                // Append the TableProperties object to the empty table.
                table.AppendChild<TableProperties>(tblProp);
                for(int i = 0; i<50; i++)
                {
                    TableRow row = new TableRow();
                    for(int j = 0; j<10; j++)
                    {
                        TableCell cell = new TableCell();
                        cell.Append(new TableCellProperties(
    new TableCellWidth() { Type = TableWidthUnitValues.Auto}));
                        cell.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text($"cell{i}{j}"))));
                        row.Append(cell);
                    }
                    table.Append(row);
                }
                // Create a row.
                //TableRow tr = new TableRow();


                // Create a cell.
                TableCell tc1 = new TableCell();
                

                // Specify the width property of the table cell.



                // Append the table to the document.
                doc.MainDocumentPart.Document.Body.Append(table);
            }
        }
    }
}
