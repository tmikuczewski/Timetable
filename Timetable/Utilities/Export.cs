using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.TimetableDataSetTableAdapters;
using Excel = Microsoft.Office.Interop.Excel;

namespace Timetable.Utilities
{
    /// <summary>
    /// Typ pliku do jakiego eksportowany jest plan
    /// </summary>
    public enum ExportFileType
    {
        /// <summary>
        /// XLS (Excel)
        /// </summary>
        XLS,
        /// <summary>
        /// PDF
        /// </summary>
        PDF
    }

    //http://csharp.net-informations.com/excel/csharp-format-excel.htm
    class Export
    {
        private Excel.Application xlApp;
        private Excel.Workbook xlWorkBook;
        private Excel.Worksheet xlWorkSheet;
        private object misValue = System.Reflection.Missing.Value;

        /// <summary>
        /// Klasa eksportująca plan z bazy do Excelowego formatu XLS
        /// </summary>
        public Export()
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

        /// <summary>
        /// Przygotowanie nowego dokumentu
        /// </summary>
        private void prepareExcel()
        {
            xlApp = new Excel.Application();

            if (xlApp == null)
            {
                throw new ExcelApplicationException("Cannot initialize Microsoft.Office.Interop.Excel.Application");
            }

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            prepareTable();
        }

        /// <summary>
        /// Zapisuje
        /// </summary>
        /// <param name="classId"></param>
        public void SaveTimeTableForClass(int classId, ExportFileType fileType)
        {
            prepareExcel();
            writeTimeTableForClass(classId);
            var applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            var date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var schoolClass = timetableDataSet.Classes.Where(c => c.Id == classId).FirstOrDefault();
            if(schoolClass == null)
            {
                throw new EntityDoesNotExistException("Class with id=" + classId + " does not exists");
            }
            String path = $"{applicationPath}Klasa {schoolClass.Year}{schoolClass.CodeName}-{date}.xls";
            setHeader($"Klasa {schoolClass.Year}{schoolClass.CodeName}");
            save(path,fileType);

        }

        public void SaveTimeTableForTeacher(string pesel, ExportFileType fileType)
        {
            prepareExcel();

            writeTimeTableForTeacher(pesel);

            var applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            var date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var teacher = timetableDataSet.Teachers.Where(t => t.Pesel == pesel).FirstOrDefault();
            if(teacher == null)
            {
                throw new EntityDoesNotExistException("Teacher with PESEL=" + pesel + " does not exists");
            }
            String path = $"{applicationPath}{teacher.LastName} {teacher.FirstName}-{date}.xls";
            setHeader($"{teacher.LastName} {teacher.FirstName}");
            save(path, fileType);


        }

        public void SaveTimeTableForClassRoom(int classRoomId, ExportFileType fileType)
        {
            prepareExcel();

            writeTimeTableForClassRoom(classRoomId);

            var applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            var date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var classRoom = timetableDataSet.Classrooms.Where(c => c.Id == classRoomId).FirstOrDefault();
            if(classRoom == null)
            {
                throw new EntityDoesNotExistException("ClassRoom with id=" + classRoomId + " does not exists");
            }
            String path = $"{applicationPath}Sala {classRoom.Name}-{date}.xls";
            setHeader($"Sala {classRoom.Name}");
            save(path, fileType);

        }

        private void save(string path, ExportFileType fileType)
        {
            xlWorkSheet.Columns.AutoFit();
            double max = maxWidth();
            for (int i = 2; i < 7; i++)
            {
                xlWorkSheet.Columns[i].ColumnWidth = max;
            }

            if (fileType.Equals(ExportFileType.XLS))
            {
                xlWorkBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            }
            if (fileType.Equals(ExportFileType.PDF))
            {
                xlWorkSheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                xlWorkSheet.PageSetup.Zoom = false;
                xlWorkSheet.PageSetup.FitToPagesTall = 1;
                xlWorkSheet.PageSetup.FitToPagesWide = 1;
                xlWorkSheet.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA4;
                xlWorkBook.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, path);
            }

            //xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

        }
        /// <summary>
        /// 
        /// </summary>
        private void prepareTable()
        {
            Excel.Range range;
            int dayID = 1; ;
            try {
                for (dayID = 1; dayID <= 5; dayID++)
                {
                    range = xlWorkSheet.Cells[3, 1 + dayID];
                    xlWorkSheet.Cells[3, 1 + dayID] = timetableDataSet.Days.Where(d => d.Id == dayID).First().Name;
                    range.BorderAround(Excel.XlLineStyle.xlContinuous);
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }
            } catch (Exception e1)
            {
                throw new EntityDoesNotExistException("Day with id=" + dayID + " does not exist");
            }
            int hourID = 1;
            try {
                for (hourID = 1; hourID <= 8; hourID++)
                {
                    range = xlWorkSheet.get_Range("A" + (1 + hourID * 3), "A" + (1 + hourID * 3 + 2));
                    range.Merge();
                    var beginHour = timetableDataSet.Hours.Where(h => h.Id == hourID).First().Hour;
                    xlWorkSheet.Cells[1 + hourID * 3, 1] = beginHour.ToString(@"hh\:mm") + " - " + beginHour.Add(TimeSpan.FromMinutes(45)).ToString(@"hh\:mm");
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    range.BorderAround(Excel.XlLineStyle.xlContinuous);
                }
            } catch (Exception e2)
            {
                throw new EntityDoesNotExistException("Hour with id=" + hourID + " does not exists");
            }
            range = xlWorkSheet.get_Range("B4", "F27");
            range.BorderAround(Excel.XlLineStyle.xlContinuous);
        }

        private void setHeader(string header)
        {
            Excel.Range range;
            xlWorkSheet.Cells[1, 1] = header;
            range = xlWorkSheet.get_Range("A1", "F2");
            range.Merge();
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            range.Font.Size = range.Font.Size + 5;
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

        private void writeTimeTableForTeacher(string pesel)
        {
            for (int day = 0; day < 5; day++)
            {
                for (int hour = 0; hour < 8; hour++)
                {
                    var lessonPlace = timetableDataSet.LessonsPlaces.Where(lp =>
                    lp.DayId == day + 1 && lp.HourId == hour + 1 && lp.LessonsRow.TeacherPesel == pesel).FirstOrDefault();
                    if (lessonPlace == null) continue;


                    var subject = timetableDataSet.Lessons.Where(l => l.Id == lessonPlace.LessonId).First();
                    var schoolClass = timetableDataSet.Classes.Where(sc => sc.Id == subject.ClassId).First();
                    //var teacher = timetableDataSet.Teachers.Where(t => t.Pesel == subject.TeacherPesel).First();
                    xlWorkSheet.Cells[2 + day][4 + 3 * hour + 0] = timetableDataSet.Subjects.Where(s => s.Id == subject.Id).First().Name;
                    xlWorkSheet.Cells[2 + day][4 + 3 * hour + 1] = schoolClass.Year + schoolClass.CodeName;
                    xlWorkSheet.Cells[2 + day][4 + 3 * hour + 2] = "sala " + timetableDataSet.Classrooms.Where(c => c.Id == lessonPlace.ClassroomId).First().Name;
                    Excel.Range range = xlWorkSheet.get_Range("" + (char)('B' + day) + (4 + hour * 3), "" + (char)('B' + day) + (4 + hour * 3 + 2));
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    range.BorderAround(Excel.XlLineStyle.xlContinuous);
                }
            }
        }


        private void writeTimeTableForClassRoom(int classRoomId)
        {
            for (int day = 0; day < 5; day++)
            {
                for (int hour = 0; hour < 8; hour++)
                {
                    var lessonPlace = timetableDataSet.LessonsPlaces.Where(lp =>
                    lp.DayId == day + 1 && lp.HourId == hour + 1 && lp.ClassroomId == classRoomId).FirstOrDefault();
                    if (lessonPlace == null) continue;


                    var subject = timetableDataSet.Lessons.Where(l => l.Id == lessonPlace.LessonId).First();
                    var schoolClass = timetableDataSet.Classes.Where(sc => sc.Id == subject.ClassId).First();
                    var teacher = timetableDataSet.Teachers.Where(t => t.Pesel == subject.TeacherPesel).First();
                    xlWorkSheet.Cells[2 + day][4 + 3 * hour + 0] = timetableDataSet.Subjects.Where(s => s.Id == subject.Id).First().Name;
                    xlWorkSheet.Cells[2 + day][4 + 3 * hour + 1] = teacher.FirstName + " " + teacher.LastName;
                    xlWorkSheet.Cells[2 + day][4 + 3 * hour + 2] = schoolClass.Year + schoolClass.CodeName;
                    //xlWorkSheet.Cells[2 + day][4 + 3 * hour + 2] = "sala " + timetableDataSet.Classrooms.Where(c => c.Id == lessonPlace.ClassroomId).First().Name;
                    Excel.Range range = xlWorkSheet.get_Range("" + (char)('B' + day) + (4 + hour * 3), "" + (char)('B' + day) + (4 + hour * 3 + 2));
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
