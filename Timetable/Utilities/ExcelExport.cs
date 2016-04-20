using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

namespace Timetable.Utilities
{
    //http://csharp.net-informations.com/excel/csharp-format-excel.htm
    class ExcelExport
    {
        private Excel.Application xlApp;
        private Excel.Workbook xlWorkBook;
        private Excel.Worksheet xlWorkSheet;
        private object misValue = System.Reflection.Missing.Value;

        public void save()
        {
            xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                return;
            }



            

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Cells[1, 1] = "Sheet 1 content";
            prepareTable();

            //Excel.Range formatRange;
            //formatRange = xlWorkSheet.get_Range("b2", "e9");
            //formatRange.BorderAround(Excel.XlLineStyle.xlContinuous,
            //Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic,
            //Excel.XlColorIndex.xlColorIndexAutomatic);

            xlWorkBook.SaveAs("d:\\csharp-Excel.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

        }
        private void prepareTable()
        {
            for(int i = 1; i <= 5; i++)
            {
                xlWorkSheet.Cells[3, 1+i] = Database.GetDayById(i).Name;
            }
            for(int i = 1; i <= 8; i++)
            {
                var beginHour = Database.GetHourById(i).BeginHour;
                xlWorkSheet.Cells[3+i, 1] = beginHour.ToString(@"hh\:mm") + " - " + beginHour.Add(TimeSpan.FromMinutes(45)).ToString(@"hh\:mm");
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
