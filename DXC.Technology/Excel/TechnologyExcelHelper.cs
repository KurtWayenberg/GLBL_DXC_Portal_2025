//using DXC.Technology.Excel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DXC.Technology.Excel
//{
//    public static class BestekExcelHelper
//    {

//        #region Support Methods

//        public static string GetStringFromCell(Row row, int index)
//        {
//            var returnvalue = "";
//            if (row.Cells != null && row.Cells.Length > index)
//            {
//                if (row.Cells[index] != null)
//                {
//                    returnvalue = DXC.Technology.Utilities.String.Left(row.Cells[index].Text, 50) ?? ""; //TODO: alter db for allowing bigger RU_Name
//                }
//            }
//            return returnvalue;
//        }

//        public static int GetIntFromCell(Row row, int index)
//        {
//            var returnvalue = GetStringFromCell(row, index);
//            if(int.TryParse(returnvalue, out int result))
//            {
//                return result;
//            }
//            else
//            {
//                throw new DXC.Technology.Exceptions.NamedExceptions.ActionFailedException()
//            }
//            return returnvalue;
//        }

//        public static void SetStringInCell(Row row, int index, string value)
//        {
//            if (row.Cells == null) return;

//            if (row.Cells.Length > index)
//            {
//                if (row.Cells[index] != null)
//                {
//                    row.Cells[index].Text = value;
//                }
//                else
//                {
//                    var cells = new Cell[row.Cells.Length];
//                    for (var j = 0; j < row.Cells.Length; j++)
//                    {
//                        if (row.Cells[j] == null) row.Cells[j] = new Cell();
//                    }
//                    row.Cells.CopyTo(cells, 0);
//                    cells[index] = new Cell();
//                    cells[index].Text = value;
//                    row.Cells = cells;
//                }
//            }
//            else
//            {
//                var cells = new Cell[index + 1];
//                for (var j = 0; j < row.Cells.Length; j++)
//                {
//                    if (row.Cells[j] == null) row.Cells[j] = new Cell();
//                }
//                row.Cells.CopyTo(cells, 0);
//                cells[index] = new Cell();
//                cells[index].Text = value;
//                row.Cells = cells;
//            }
//        }

//        public static string GetString255FromCell(Row row, int index)
//        {
//            var returnvalue = "";
//            if (row.Cells != null && row.Cells.Length > index)
//            {
//                if (row.Cells[index] != null)
//                {
//                    if (!string.IsNullOrEmpty(row.Cells[index].Text))
//                    {
//                        returnvalue = DXC.Technology.Utilities.String.Left(row.Cells[index].Text, 255).Replace("\"", "_QUOTE_");
//                    }
//                }
//            }

//            return returnvalue;
//        }

//        #endregion


//    }

//}
