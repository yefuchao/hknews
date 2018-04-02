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
            ISheet sheet = workbook.CreateSheet();
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
    }
}
