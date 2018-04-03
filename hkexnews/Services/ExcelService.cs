using Infrastructure;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace hkexnews.Services
{
    public static class ExcelService
    {
        private static HSSFWorkbook workbook;

        /// <summary>
        /// 初始化
        /// </summary>
        private static void InitializeWorkbook()
        {
            if (workbook == null)
            {
                workbook = new HSSFWorkbook();
            }

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Alex";
            workbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Alex";
            si.Title = "Alex";
            si.Author = "Alex";
            si.Comments = "yefuchao@live.com";
            workbook.SummaryInformation = si;
        }

        public static Stream GenerateDayExcel(DataTable SourceTable)
        {
            workbook = new HSSFWorkbook();
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet(SourceTable.TableName);
            var headerRow = sheet.CreateRow(0);

            foreach (DataColumn column in SourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }

            int rowIndex = 1;

            foreach (DataRow row in SourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;

            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        public static Stream RenderDataSetToExcel(DataSet DbSet)
        {
            workbook = new HSSFWorkbook();
            InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            foreach (DataTable Table in DbSet.Tables)
            {
                ISheet sheet = workbook.CreateSheet(Table.TableName);
                var headerRow = sheet.CreateRow(0);

                foreach (DataColumn column in Table.Columns)
                {
                    var cell = headerRow.CreateCell(column.Ordinal); //.SetCellValue(column.ColumnName);
                    cell.SetCellValue(column.ColumnName);
                }

                int rowIndex = 1;

                foreach (DataRow row in Table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in Table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            workbook = null;
            return ms;
        }

        public static byte[] StreamToByteArray(Stream s)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                s.CopyTo(ms);

                return ms.ToArray();
            }
        }

        public static void WriteStreamToFile(byte[] data, string FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            data = null;
            fs = null;
        }
    }
}
