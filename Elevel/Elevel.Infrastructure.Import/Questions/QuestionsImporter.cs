using Elevel.Application.Interfaces;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Infrastructure.Import.Questions
{
    public class QuestionsImporter : IQuestionsImporter
    {
        public async Task GetDataAsync(Stream inputFileStream)
        {
            var dtTable = new DataTable();
            var rowList = new List<string>();
            ISheet sheet;
            inputFileStream.Position = 0;
            var workbook = WorkbookFactory.Create(inputFileStream);
            sheet = workbook.GetSheetAt(0);
            var headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (var j = 0; j < cellCount; j++)
            {
                var cell = headerRow.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                {
                    dtTable.Columns.Add(cell.ToString());
                }
            }

            for (var i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (!string.IsNullOrEmpty(row.GetCell(j).ToString()) &&
                            !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                        {
                            rowList.Add(row.GetCell(j).ToString());
                        }
                    }
                }

                //if (rowList.Count > 0)
                //    dtTable.Rows.Add(rowList.ToArray());
                //rowList.Clear();
            }
        }
    }
}