using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows
{
	/// <summary>
	/// Interaction logic for PlanningWindow.xaml
	/// </summary>
	public partial class PlanningWindow : System.Windows.Window
	{
		#region Constructors
		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>PlanningWindow</c>.
		/// </summary>
		public PlanningWindow(MainWindow window, ExpanderControlType controlType,
			ComboBoxContent contentType, string teacherPesel, int? classId, int dayId, int hourId)
		{
			this.InitializeComponent();
			this.callingWindow = window;
			this.controlType = controlType;
			this.contentType = contentType;
			this.teacherPesel = teacherPesel;
			this.classId = classId;
			this.dayId = dayId;
			this.hourId = hourId;
		}
		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		#endregion

		#region Private methods

		#endregion

		#region Events
		private void planningWindow_Loaded(object sender, RoutedEventArgs e)
		{
			this.timetableDataSet = new TimetableDataSet();

			this.classesTableAdapter = new ClassesTableAdapter();
			this.classroomsTableAdapter = new ClassroomsTableAdapter();
			this.daysTableAdapter = new DaysTableAdapter();
			this.hoursTableAdapter = new HoursTableAdapter();
			this.lessonsTableAdapter = new LessonsTableAdapter();
			this.lessonsPlacesTableAdapter = new LessonsPlacesTableAdapter();
			this.studentsTableAdapter = new StudentsTableAdapter();
			this.subjectsTableAdapter = new SubjectsTableAdapter();
			this.teachersTableAdapter = new TeachersTableAdapter();

			this.classesTableAdapter.Fill(this.timetableDataSet.Classes);
			this.classroomsTableAdapter.Fill(this.timetableDataSet.Classrooms);
			this.daysTableAdapter.Fill(this.timetableDataSet.Days);
			this.hoursTableAdapter.Fill(this.timetableDataSet.Hours);
			this.lessonsTableAdapter.Fill(this.timetableDataSet.Lessons);
			this.lessonsPlacesTableAdapter.Fill(this.timetableDataSet.LessonsPlaces);
			this.studentsTableAdapter.Fill(this.timetableDataSet.Students);
			this.subjectsTableAdapter.Fill(this.timetableDataSet.Subjects);
			this.teachersTableAdapter.Fill(this.timetableDataSet.Teachers);

			this.dayRow = timetableDataSet.Days.FindById(this.dayId);
			this.hourRow = timetableDataSet.Hours.FindById(this.hourId);
			this.textBoxDayHour.Text = dayRow.Name + ", " + hourRow.Hour;

			var lessons = from l in this.timetableDataSet.Lessons
						  join s in this.timetableDataSet.Subjects on l.SubjectId equals s.Id into grp
						  join c in this.timetableDataSet.Classes on l.ClassId equals c.Id
						  orderby l.Id
						  from s in grp.DefaultIfEmpty()
						  select new PlannedLesson
						  {
							  Id = l.Id,
							  Teacher = l.TeacherPesel,
							  Subject = l.SubjectId,
							  SubjectName = s?.Name,
							  Class = l.ClassId,
							  ClassName = $"{c.Year} {c.CodeName}",
						  };

			switch (contentType)
			{
				case ComboBoxContent.Teachers:
					this.labelContentType.Text = "Teacher:";
					this.teacherRow = timetableDataSet.Teachers.FindByPesel(this.teacherPesel);
					this.textBoxContentType.Text = teacherRow.FirstName + " " + teacherRow.LastName;

					lessons = lessons.Where(l => l.Teacher == this.teacherPesel);
					this.comboBoxLessons.ItemsSource = lessons;
					this.comboBoxLessons.DisplayMemberPath = "SubjectClass";
					this.comboBoxLessons.SelectedValuePath = "Id";
					break;

				case ComboBoxContent.Classes:
					this.labelContentType.Text = "Class:";
					this.classRow = timetableDataSet.Classes.FindById(this.classId ?? -1);
					this.textBoxContentType.Text = classRow.Year + " " + classRow.CodeName;
					lessons = lessons.Where(l => l.Class == this.classId);

					this.comboBoxLessons.ItemsSource = lessons;
					this.comboBoxLessons.DisplayMemberPath = "SubjectName";
					this.comboBoxLessons.SelectedValuePath = "Id";
					break;
			}

			if (this.controlType == ExpanderControlType.Add)
			{
				currentLessonPlaceRow = timetableDataSet.LessonsPlaces.NewLessonsPlacesRow();
			}

			if (this.controlType == ExpanderControlType.Change)
			{
				currentLessonPlaceRow = timetableDataSet.LessonsPlaces
					.Where(lp => lp.DayId == dayId && lp.HourId == hourId)
					.FirstOrDefault(lp => lessons.Select(l => l.Id).Contains(lp.LessonId));

				if (currentLessonPlaceRow != null)
				{
					var currentLessonIndex = lessons.ToList().FindIndex(l => l.Id == currentLessonPlaceRow.LessonsRow.Id);
					this.comboBoxLessons.SelectedIndex = currentLessonIndex;
				}
			}
		}

		private void buttonOkMap_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Close();

		}

		private void buttonCancelMap_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Close();
		}
		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		private readonly MainWindow callingWindow;

		private readonly ExpanderControlType controlType;

		private TimetableDataSet timetableDataSet;
		private ClassesTableAdapter classesTableAdapter;
		private ClassroomsTableAdapter classroomsTableAdapter;
		private DaysTableAdapter daysTableAdapter;
		private HoursTableAdapter hoursTableAdapter;
		private LessonsTableAdapter lessonsTableAdapter;
		private LessonsPlacesTableAdapter lessonsPlacesTableAdapter;
		private StudentsTableAdapter studentsTableAdapter;
		private SubjectsTableAdapter subjectsTableAdapter;
		private TeachersTableAdapter teachersTableAdapter;

		private ComboBoxContent contentType;

		private string teacherPesel;

		private int? classId;

		private int
			dayId,
			hourId;

		private TimetableDataSet.LessonsRow currentLessonRow;
		private TimetableDataSet.LessonsPlacesRow currentLessonPlaceRow;
		private TimetableDataSet.TeachersRow teacherRow;
		private TimetableDataSet.ClassesRow classRow;
		private TimetableDataSet.DaysRow dayRow;
		private TimetableDataSet.HoursRow hourRow;

		private class PlannedLesson
		{
			public int Id { get; set; }
			public int Class { get; set; }
			public string ClassName { get; set; }
			public int Subject { get; set; }
			public string SubjectName { get; set; }
			public string SubjectClass => $"{SubjectName} - {ClassName}";
			public string Teacher { get; set; }
		}

		#endregion
	}
}
