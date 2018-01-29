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
						  join t in this.timetableDataSet.Teachers on l.TeacherPesel equals t.Pesel
						  orderby l.Id
						  from s in grp.DefaultIfEmpty()
						  select new PlannedLesson
						  {
							  Id = l.Id,
							  TeacherPesel = l.TeacherPesel,
							  TeacherName = t?.ToFriendlyString(false),
							  SubjectId = l.SubjectId,
							  SubjectName = s?.Name,
							  ClassId = l.ClassId,
							  ClassName = c?.ToFriendlyString(),
						  };

			var availableLessons = lessons;

			switch (contentType)
			{
				case ComboBoxContent.Teachers:
					this.labelContentType.Text = "Teacher:";
					this.teacherRow = timetableDataSet.Teachers.FindByPesel(this.teacherPesel);
					this.textBoxContentType.Text = $"{teacherRow.FirstName} {teacherRow.LastName}";
					availableLessons = lessons.Where(l => l.TeacherPesel == this.teacherPesel);
					break;

				case ComboBoxContent.Classes:
					this.labelContentType.Text = "Class:";
					this.classRow = timetableDataSet.Classes.FindById(this.classId ?? -1);
					this.textBoxContentType.Text = classRow.ToFriendlyString();
					availableLessons = lessons.Where(l => l.ClassId == this.classId);
					break;
			}

			if (this.controlType == ExpanderControlType.Add)
			{
				currentLessonPlaceRow = timetableDataSet.LessonsPlaces.NewLessonsPlacesRow();
			}
			else if (this.controlType == ExpanderControlType.Change)
			{
				currentLessonPlaceRow = timetableDataSet.LessonsPlaces
					.Where(lp => lp.DayId == dayId && lp.HourId == hourId)
					.FirstOrDefault(lp => availableLessons.Select(l => l.Id).Contains(lp.LessonId));
			}

			var unavailableLessonsPlaces = timetableDataSet.LessonsPlaces
				.Where(lp => lp.DayId == dayId && lp.HourId == hourId)
				.Where(lp => lp.LessonId != currentLessonPlaceRow.LessonsRow?.Id);

			switch (contentType)
			{
				case ComboBoxContent.Teachers:
					var unavailableClasses = unavailableLessonsPlaces.Select(lp => lp.LessonsRow.ClassId);

					availableLessons = availableLessons.Where(l => !unavailableClasses.Contains(l.ClassId))
						.OrderBy(l => l.SubjectName)
						.ThenBy(l => l.ClassName);

					this.comboBoxLessons.ItemsSource = availableLessons;
					this.comboBoxLessons.DisplayMemberPath = "SubjectClass";
					this.comboBoxLessons.SelectedValuePath = "Id";
					break;

				case ComboBoxContent.Classes:
					var unavailableTeachers = unavailableLessonsPlaces.Select(lp => lp.LessonsRow.TeacherPesel);

					availableLessons = availableLessons.Where(l => !unavailableTeachers.Contains(l.TeacherPesel))
						.OrderBy(l => l.SubjectName);

					this.comboBoxLessons.ItemsSource = availableLessons;
					this.comboBoxLessons.DisplayMemberPath = "SubjectTeacher";
					this.comboBoxLessons.SelectedValuePath = "Id";
					break;
			}

			var availableClassrooms = timetableDataSet.Classrooms
				.Where(cr => !unavailableLessonsPlaces.Select(lp => lp.ClassroomId).Contains(cr.Id));

			this.comboBoxClassrooms.ItemsSource = availableClassrooms;
			this.comboBoxClassrooms.DisplayMemberPath = "Name";
			this.comboBoxClassrooms.SelectedValuePath = "Id";

			if (this.controlType == ExpanderControlType.Change && currentLessonPlaceRow != null)
			{
				var currentLessonIndex = availableLessons.ToList().FindIndex(l => l.Id == currentLessonPlaceRow.LessonId);
				this.comboBoxLessons.SelectedIndex = currentLessonIndex;

				var currentClassroomIndex = availableClassrooms.ToList().FindIndex(cr => cr.Id == currentLessonPlaceRow.ClassroomId);
				this.comboBoxClassrooms.SelectedIndex = currentClassroomIndex;
			}
		}

		private void buttonOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (this.comboBoxLessons.SelectedValue == null
				|| this.comboBoxClassrooms.SelectedValue == null)
			{
				MessageBox.Show("All fields are required.", "Error");
				return;
			}

			if (this.comboBoxLessons.SelectedValue != null)
			{
				int lessonId;

				if (int.TryParse(this.comboBoxLessons.SelectedValue.ToString(), out lessonId))
				{
					currentLessonPlaceRow.LessonId = lessonId;
				}
			}

			if (this.comboBoxClassrooms.SelectedValue != null)
			{
				int classroomId;

				if (int.TryParse(this.comboBoxClassrooms.SelectedValue.ToString(), out classroomId))
				{
					currentLessonPlaceRow.ClassroomId = classroomId;
				}
			}

			if (this.controlType == ExpanderControlType.Add)
			{
				currentLessonPlaceRow.DayId = this.dayId;
				currentLessonPlaceRow.HourId = this.hourId;
				timetableDataSet.LessonsPlaces.Rows.Add(currentLessonPlaceRow);
			}

			lessonsPlacesTableAdapter.Update(timetableDataSet.LessonsPlaces);

			this.callingWindow.RefreshPlanning();
			this.Close();
		}

		private void buttonCancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Close();
		}
		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		private readonly MainWindow callingWindow;

		private readonly ExpanderControlType controlType;

		private readonly ComboBoxContent contentType;

		private readonly string teacherPesel;
		private readonly int? classId;
		private readonly int dayId;
		private readonly int hourId;

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

		private TimetableDataSet.LessonsRow currentLessonRow;
		private TimetableDataSet.LessonsPlacesRow currentLessonPlaceRow;
		private TimetableDataSet.TeachersRow teacherRow;
		private TimetableDataSet.ClassesRow classRow;
		private TimetableDataSet.DaysRow dayRow;
		private TimetableDataSet.HoursRow hourRow;

		private class PlannedLesson
		{
			public int Id { get; set; }
			public int ClassId { get; set; }
			public string ClassName { get; set; }
			public string TeacherPesel { get; set; }
			public string TeacherName { get; set; }
			public int SubjectId { get; set; }
			public string SubjectName { get; set; }
			public string SubjectClass => $"{SubjectName}\n-- {ClassName}";
			public string SubjectTeacher => $"{SubjectName}\n-- {TeacherName}";
		}

		#endregion
	}
}
