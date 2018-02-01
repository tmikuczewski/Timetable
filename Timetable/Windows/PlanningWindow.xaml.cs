using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows
{
	/// <summary>
	///     Interaction logic for PlanningWindow.xaml
	/// </summary>
	public partial class PlanningWindow : Window
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private TimetableDataSet timetableDataSet;
		private ClassesTableAdapter classesTableAdapter;
		private ClassroomsTableAdapter classroomsTableAdapter;
		private DaysTableAdapter daysTableAdapter;
		private HoursTableAdapter hoursTableAdapter;
		private LessonsTableAdapter lessonsTableAdapter;
		private LessonsPlacesTableAdapter lessonsPlacesTableAdapter;
		private SubjectsTableAdapter subjectsTableAdapter;
		private TeachersTableAdapter teachersTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly ExpanderControlType _controlType;
		private readonly ComboBoxContentType _contentType;

		private readonly int? _classId;
		private readonly string _teacherPesel;
		private readonly int _dayId;
		private readonly int _hourId;
		private int _currentLessonId;
		private int _currentClassroomId;
		private TimetableDataSet.LessonsPlacesRow _currentLessonPlaceRow;
		private TimetableDataSet.TeachersRow _teacherRow;
		private TimetableDataSet.ClassesRow _classRow;
		private TimetableDataSet.DaysRow _dayRow;
		private TimetableDataSet.HoursRow _hourRow;

		private IEnumerable<PlannedLesson> _plannedLessons;
		private IEnumerable<PlannedLesson> _availableLessons;
		private IEnumerable<TimetableDataSet.LessonsPlacesRow> _unavailableLessonsPlaces;
		private IEnumerable<TimetableDataSet.ClassroomsRow> _availableClassrooms;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>PlanningWindow</c>.
		/// </summary>
		public PlanningWindow(MainWindow mainWindow, ExpanderControlType controlType, ComboBoxContentType contentType,
			int? classId, string teacherPesel, int dayId, int hourId)
		{
			InitializeComponent();

			_callingWindow = mainWindow;
			_controlType = controlType;
			_contentType = contentType;
			_classId = classId;
			_teacherPesel = teacherPesel;
			_dayId = dayId;
			_hourId = hourId;
		}

		#endregion

		#region Events

		private async void planningWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					InitDatabaseObjects();

					PreparePlannedLessons();
					PrepareAvailableLessons();
					PrepareEntity();
					FillControls();

					PrepareUnavailableLessonsPlaces();
					UpdateAvailableLessons();
					FillLessonComboBox();

					PrepareAvailableClassrooms();
					FillClassroomComboBox();
					SetComboBoxes();
				});
			});
		}

		private async void buttonOk_Click(object sender, RoutedEventArgs e)
		{
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(SaveEntity);
			});
		}

		private async void buttonCancel_Click(object sender, RoutedEventArgs e)
		{
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(Close);
			});
		}

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		#endregion


		#region Private methods

		private void InitDatabaseObjects()
		{
			timetableDataSet = new TimetableDataSet();
			classesTableAdapter = new ClassesTableAdapter();
			classroomsTableAdapter = new ClassroomsTableAdapter();
			daysTableAdapter = new DaysTableAdapter();
			hoursTableAdapter = new HoursTableAdapter();
			lessonsTableAdapter = new LessonsTableAdapter();
			lessonsPlacesTableAdapter = new LessonsPlacesTableAdapter();
			subjectsTableAdapter = new SubjectsTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();

			classesTableAdapter.Fill(timetableDataSet.Classes);
			classroomsTableAdapter.Fill(timetableDataSet.Classrooms);
			daysTableAdapter.Fill(timetableDataSet.Days);
			hoursTableAdapter.Fill(timetableDataSet.Hours);
			lessonsTableAdapter.Fill(timetableDataSet.Lessons);
			lessonsPlacesTableAdapter.Fill(timetableDataSet.LessonsPlaces);
			subjectsTableAdapter.Fill(timetableDataSet.Subjects);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);
		}

		private void PreparePlannedLessons()
		{
			_plannedLessons = from l in timetableDataSet.Lessons
							  join s in timetableDataSet.Subjects on l.SubjectId equals s.Id into grp
							  join c in timetableDataSet.Classes on l.ClassId equals c.Id
							  join t in timetableDataSet.Teachers on l.TeacherPesel equals t.Pesel
							  orderby l.Id
							  from s in grp.DefaultIfEmpty()
							  select new PlannedLesson
							  {
								  Id = l.Id,
								  TeacherPesel = l.TeacherPesel,
								  TeacherName = t?.ToFriendlyString(),
								  SubjectId = l.SubjectId,
								  SubjectName = s?.Name,
								  ClassId = l.ClassId,
								  ClassName = c?.ToFriendlyString()
							  };
		}

		private void PrepareAvailableLessons()
		{
			_availableLessons = _plannedLessons;

			switch (_contentType)
			{
				case ComboBoxContentType.Classes:
					_classRow = timetableDataSet.Classes.FindById(_classId ?? -1);
					_availableLessons = _plannedLessons.Where(l => l.ClassId == _classId);
					break;
				case ComboBoxContentType.Teachers:
					_teacherRow = timetableDataSet.Teachers.FindByPesel(_teacherPesel);
					_availableLessons = _plannedLessons.Where(l => l.TeacherPesel == _teacherPesel);
					break;
			}
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_controlType)
				{
					case ExpanderControlType.Add:
						_currentLessonPlaceRow = timetableDataSet.LessonsPlaces.NewLessonsPlacesRow();
						break;
					case ExpanderControlType.Change:
						_currentLessonPlaceRow = PrepareLessonPlace();
						break;
				}
			}
			catch (EntityDoesNotExistException)
			{
				ShowErrorMessageBox("Lesson with given date, time and other data does not exist.");
				Close();
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
				Close();
			}
		}

		private TimetableDataSet.LessonsPlacesRow PrepareLessonPlace()
		{
			var lessonsPlaceRow = timetableDataSet.LessonsPlaces
				.Where(lp => lp.DayId == _dayId && lp.HourId == _hourId)
				.FirstOrDefault(lp => _availableLessons.Select(l => l.Id).Contains(lp.LessonId));

			if (lessonsPlaceRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return lessonsPlaceRow;
		}

		private void FillControls()
		{
			_dayRow = timetableDataSet.Days.FindById(_dayId);
			_hourRow = timetableDataSet.Hours.FindById(_hourId);
			textBoxDayHour.Text = _dayRow.Name + ", " + _hourRow.Hour;

			switch (_contentType)
			{
				case ComboBoxContentType.Classes:
					if (_classRow == null)
						return;

					labelContentType.Text = "Class:";
					textBoxContentType.Text = _classRow.ToFriendlyString();
					break;
				case ComboBoxContentType.Teachers:
					if (_teacherRow == null)
						return;

					labelContentType.Text = "Teacher:";
					textBoxContentType.Text = $"{_teacherRow.FirstName} {_teacherRow.LastName}";
					break;
			}
		}

		private void PrepareUnavailableLessonsPlaces()
		{
			_unavailableLessonsPlaces = timetableDataSet.LessonsPlaces
				.Where(lp => lp.DayId == _dayId && lp.HourId == _hourId)
				.Where(lp => lp.LessonId != _currentLessonPlaceRow?.LessonsRow?.Id);
		}

		private void UpdateAvailableLessons()
		{
			switch (_contentType)
			{
				case ComboBoxContentType.Classes:
					var unavailableTeachers = _unavailableLessonsPlaces.Select(lp => lp.LessonsRow.TeacherPesel);

					_availableLessons = _availableLessons.Where(l => !unavailableTeachers.Contains(l.TeacherPesel))
						.OrderBy(l => l.SubjectName);

					break;
				case ComboBoxContentType.Teachers:
					var unavailableClasses = _unavailableLessonsPlaces.Select(lp => lp.LessonsRow.ClassId);

					_availableLessons = _availableLessons.Where(l => !unavailableClasses.Contains(l.ClassId))
						.OrderBy(l => l.SubjectName)
						.ThenBy(l => l.ClassName);

					break;
			}
		}

		private void FillLessonComboBox()
		{
			switch (_contentType)
			{
				case ComboBoxContentType.Classes:
					comboBoxLessons.ItemsSource = _availableLessons;
					comboBoxLessons.DisplayMemberPath = "SubjectTeacher";
					comboBoxLessons.SelectedValuePath = "Id";
					break;
				case ComboBoxContentType.Teachers:
					comboBoxLessons.ItemsSource = _availableLessons;
					comboBoxLessons.DisplayMemberPath = "SubjectClass";
					comboBoxLessons.SelectedValuePath = "Id";
					break;
			}
		}

		private void PrepareAvailableClassrooms()
		{
			_availableClassrooms = timetableDataSet.Classrooms
				.Where(cr => !_unavailableLessonsPlaces.Select(lp => lp.ClassroomId).Contains(cr.Id));
		}

		private void FillClassroomComboBox()
		{
			comboBoxClassrooms.ItemsSource = _availableClassrooms;
			comboBoxClassrooms.DisplayMemberPath = "Name";
			comboBoxClassrooms.SelectedValuePath = "Id";
		}

		private void SetComboBoxes()
		{
			switch (_controlType)
			{
				case ExpanderControlType.Change:
					if (_currentLessonPlaceRow == null)
						return;

					var currentLessonIndex = _availableLessons.ToList().FindIndex(l => l.Id == _currentLessonPlaceRow.LessonId);
					comboBoxLessons.SelectedIndex = currentLessonIndex;

					comboBoxClassrooms.SelectedValue = _currentLessonPlaceRow.ClassroomId;
					break;
			}

			buttonOk.IsEnabled = true;
			buttonCancel.IsEnabled = true;
		}

		private void SaveEntity()
		{
			try
			{
				SaveLessonPlace();
			}
			catch (FieldsNotFilledException)
			{
				ShowWarningMessageBox("All fields are required.");
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void SaveLessonPlace()
		{
			if (comboBoxLessons.SelectedValue == null
				|| comboBoxClassrooms.SelectedValue == null)
			{
				throw new FieldsNotFilledException();
			}

			if (int.TryParse(comboBoxLessons.SelectedValue.ToString(), out _currentLessonId)
				&& int.TryParse(comboBoxClassrooms.SelectedValue.ToString(), out _currentClassroomId))
			{
				_currentLessonPlaceRow.LessonId = _currentLessonId;
				_currentLessonPlaceRow.ClassroomId = _currentClassroomId;
			}
			else
			{
				throw new FieldsNotFilledException();
			}


			if (_controlType == ExpanderControlType.Add)
			{
				_currentLessonPlaceRow.DayId = _dayId;
				_currentLessonPlaceRow.HourId = _hourId;
				timetableDataSet.LessonsPlaces.Rows.Add(_currentLessonPlaceRow);
			}

			lessonsPlacesTableAdapter.Update(timetableDataSet.LessonsPlaces);

			_callingWindow.RefreshViews(ComboBoxContentType.LessonsPlaces);

			Close();
		}

		private MessageBoxResult ShowErrorMessageBox(string message)
		{
			return MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private MessageBoxResult ShowWarningMessageBox(string message)
		{
			return MessageBox.Show(this, message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		#endregion


		#region Claases

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
