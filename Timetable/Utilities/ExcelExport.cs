using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.TimetableDataSetTableAdapters;
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

        public ExcelExport()
        {
            timetableDataSet = new TimetableDataSet();

            classesTableAdapter         = new ClassesTableAdapter();
            classroomsTableAdapter      = new ClassroomsTableAdapter();
            daysTableAdapter            = new DaysTableAdapter();
            hoursTableAdapter           = new HoursTableAdapter();
            lessonsTableAdapter         = new LessonsTableAdapter();
            lessonsPlacesTableAdapter   = new LessonsPlacesTableAdapter();
            studentsTableAdapter        = new StudentsTableAdapter();
            subjectsTableAdapter        = new SubjectsTableAdapter();
            teachersTableAdapter        = new TeachersTableAdapter();
            classesTableAdapter.Fill(timetableDataSet.Classes);
            classroomsTableAdapter.Fill(timetableDataSet.Classrooms);
            daysTableAdapter.Fill(timetableDataSet.Days);
            hoursTableAdapter        .Fill( timetableDataSet.Hours);
            lessonsTableAdapter      .Fill( timetableDataSet.Lessons);
            lessonsPlacesTableAdapter.Fill( timetableDataSet.LessonsPlaces);
            studentsTableAdapter     .Fill( timetableDataSet.Students);
            subjectsTableAdapter     .Fill( timetableDataSet.Subjects);
            teachersTableAdapter     .Fill( timetableDataSet.Teachers);

        }

        private void prepareExcel()
        {
            xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                return;
            }

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            prepareTable();
        }
        public void SaveTimeTableForClass(int classId)
        {
            prepareExcel();

            writeTimeTableForClass(classId);

            var applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            var date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var schoolClass = timetableDataSet.Classes.Where(c => c.Id == classId).First();
            String path = $"{applicationPath}Klasa {schoolClass.Year}{schoolClass.CodeName}-{date}.xls";

            save(path);

        }

        private void save(string path)
        {
            xlWorkSheet.Columns.AutoFit();
            double max = maxWidth();
            for (int i = 1; i < 7; i++)
            {
                xlWorkSheet.Columns[i].ColumnWidth = max;
            }

            xlWorkBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

        }
        private void prepareTable()
        {
            Excel.Range range;
            for (int i = 1; i <= 5; i++)
            {
                range = xlWorkSheet.Cells[3, 1 + i];
                xlWorkSheet.Cells[3, 1 + i] = timetableDataSet.Days.Where(d => d.Id == i).First().Name; //Database.GetDayById(i).Name;
                range.BorderAround(Excel.XlLineStyle.xlContinuous);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }
            for(int i = 1; i <= 8; i++)
            {
                range = xlWorkSheet.get_Range("A" + (1 + i * 3), "A" + (1 + i * 3 + 2));
                range.Merge();
                var beginHour = timetableDataSet.Hours.Where(h => h.Id == i).First().Hour;
                xlWorkSheet.Cells[1 + i * 3, 1] = beginHour.ToString(@"hh\:mm") + " - " + beginHour.Add(TimeSpan.FromMinutes(45)).ToString(@"hh\:mm");
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                range.BorderAround(Excel.XlLineStyle.xlContinuous);
            }
            range = xlWorkSheet.get_Range("B4", "F27");
            range.BorderAround(Excel.XlLineStyle.xlContinuous);
        }

        private void writeTimeTableForClass(int classId)
        {           
            for (int day = 0; day < 5; day++)
            {
                for(int hour = 0; hour < 8; hour++)
                {
                    var lessonPlace = timetableDataSet.LessonsPlaces.Where(lp =>
                    lp.DayId == day + 1 && lp.HourId == hour + 1 && lp.LessonsRow.ClassId == classId).FirstOrDefault();
                    if (lessonPlace == null) continue;

                    
                    var subject = timetableDataSet.Lessons.Where(l => l.Id == lessonPlace.LessonId).First();
                    var teacher = timetableDataSet.Teachers.Where(t => t.Pesel == subject.TeacherPesel).First();
                    xlWorkSheet.Cells[2 + day][4 + 3 * hour + 0] = timetableDataSet.Subjects.Where(s=>s.Id ==subject.Id).First().Name;
                    xlWorkSheet.Cells[2 + day][4 + 3 * hour + 1] = teacher.FirstName + " " + teacher.LastName;
                    xlWorkSheet.Cells[2 + day][4 + 3 * hour + 2] = "sala " + timetableDataSet.Classrooms.Where(c=>c.Id == lessonPlace.ClassroomId).First().Name;
                    Excel.Range range = xlWorkSheet.get_Range(""+ (char)('B'+day) + (4 + hour * 3), "" + (char)('B' + day) + (4 + hour * 3 + 2));
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    range.BorderAround(Excel.XlLineStyle.xlContinuous);
                }
            }
        }

        private double maxWidth()
        {
            double max = 0;
            for (int i = 1; i < 10; i++) {
                Excel.Range column = xlWorkSheet.Columns[i];
                Console.WriteLine("column width " + column.ColumnWidth);
                Console.WriteLine(" width " + column.Width);
                Console.WriteLine("max " + max);
                max = column.ColumnWidth > max ? column.ColumnWidth : max;
            }
            return max;
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

        #region Fields

        private TimetableDataSet timetableDataSet;

        //private static TimetableDataSet.ClassesDataTable ClassesTable;
        //private static TimetableDataSet.ClassroomsDataTable ClassroomsTable;
        //private static TimetableDataSet.DaysDataTable DaysTable;
        //private static TimetableDataSet.HoursDataTable HoursTable;
        //private static TimetableDataSet.LessonsDataTable LessonsTable;
        //private static TimetableDataSet.LessonsPlacesDataTable LessonsPlacesTable;
        //private static TimetableDataSet.StudentsDataTable StudentsTable;
        //private static TimetableDataSet.SubjectsDataTable SubjectsTable;
        //private static TimetableDataSet.TeachersDataTable TeachersTable;

        private static ClassesTableAdapter          classesTableAdapter;
        private static ClassroomsTableAdapter       classroomsTableAdapter;
        private static DaysTableAdapter             daysTableAdapter;
        private static HoursTableAdapter            hoursTableAdapter;
        private static LessonsTableAdapter          lessonsTableAdapter;
        private static LessonsPlacesTableAdapter    lessonsPlacesTableAdapter;
        private static StudentsTableAdapter         studentsTableAdapter;
        private static SubjectsTableAdapter         subjectsTableAdapter;
        private static TeachersTableAdapter         teachersTableAdapter;


        #endregion
    }
}
