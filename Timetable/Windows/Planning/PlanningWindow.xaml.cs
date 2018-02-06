using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows.Planning
{
	/// <summary>
	///     Interaction logic for PlanningWindow.xaml
	/// </summary>
	public partial class PlanningWindow : Window
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private TimetableDataSet _timetableDataSet;
		private ClassesTableAdapter _classesTableAdapter;
		private ClassroomsTableAdapter _classroomsTableAdapter;
		private DaysTableAdapter _daysTableAdapter;
		private HoursTableAdapter _hoursTableAdapter;
		private LessonsTableAdapter _lessonsTableAdapter;
		private LessonsPlacesTableAdapter _lessonsPlacesTableAdapter;
		private SubjectsTableAdapter _subjectsTableAdapter;
		private TeachersTableAdapter _teachersTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly ActionType _actionType;
		private readonly EntityType _entityType;

		private readonly int? _classId;
		private readonly string _teacherPesel;
		private readonly int _dayId;
		private readonly int _hourId;
		private int _currentLessonId;
		private int _currentClassroomId;
		private TimetableDataSet.LessonsPlacesRow _currentLessonsPlaceRow;
		private TimetableDataSet.TeachersRow _teacherRow;
		private TimetableDataSet.ClassesRow _classRow;
		private TimetableDataSet.DaysRow _dayRow;
		private TimetableDataSet.HoursRow _hourRow;

		private IEnumerable<CellViewModel> _plannedLessons;
		private IEnumerable<CellViewModel> _availableLessons;
		private IEnumerable<TimetableDataSet.LessonsPlacesRow> _unavailableLessonsPlaces;
		private IEnumerable<TimetableDataSet.ClassroomsRow> _availableClassrooms;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>PlanningWindow</c>.
		/// </summary>
		public PlanningWindow(MainWindow mainWindow, ActionType actionType, EntityType entityType,
			int? classId, string teacherPesel, int dayId, int hourId)
		{
			InitializeComponent();

			_callingWindow = mainWindow;
			_actionType = actionType;
			_entityType = entityType;
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
			_timetableDataSet = new TimetableDataSet();
			_classesTableAdapter = new ClassesTableAdapter();
			_classroomsTableAdapter = new ClassroomsTableAdapter();
			_daysTableAdapter = new DaysTableAdapter();
			_hoursTableAdapter = new HoursTableAdapter();
			_lessonsTableAdapter = new LessonsTableAdapter();
			_lessonsPlacesTableAdapter = new LessonsPlacesTableAdapter();
			_subjectsTableAdapter = new SubjectsTableAdapter();
			_teachersTableAdapter = new TeachersTableAdapter();

			_classesTableAdapter.Fill(_timetableDataSet.Classes);
			_classroomsTableAdapter.Fill(_timetableDataSet.Classrooms);
			_daysTableAdapter.Fill(_timetableDataSet.Days);
			_hoursTableAdapter.Fill(_timetableDataSet.Hours);
			_lessonsTableAdapter.Fill(_timetableDataSet.Lessons);
			_lessonsPlacesTableAdapter.Fill(_timetableDataSet.LessonsPlaces);
			_subjectsTableAdapter.Fill(_timetableDataSet.Subjects);
			_teachersTableAdapter.Fill(_timetableDataSet.Teachers);
		}

		private void PreparePlannedLessons()
		{
			_plannedLessons = from l in _timetableDataSet.Lessons
							  join s in _timetableDataSet.Subjects on l.SubjectId equals s.Id into grp
							  join c in _timetableDataSet.Classes on l.ClassId equals c.Id
							  join t in _timetableDataSet.Teachers on l.TeacherPesel equals t.Pesel
							  orderby l.Id
							  from s in grp.DefaultIfEmpty()
							  select new CellViewModel
							  {
								  Id = l.Id,
								  TeacherPesel = l.TeacherPesel,
								  TeacherFirstName = t?.FirstName,
								  TeacherLastName = t?.LastName,
								  TeacherFriendlyName = t?.ToFriendlyString(),
								  SubjectId = l.SubjectId,
								  SubjectName = s?.Name,
								  ClassId = l.ClassId,
								  ClassYear = c?.Year,
								  ClassCodeName = c?.CodeName,
								  ClassFriendlyName = c?.ToFriendlyString()
							  };
		}

		private void PrepareAvailableLessons()
		{
			_availableLessons = _plannedLessons;

			switch (_entityType)
			{
				case EntityType.Class:
					_classRow = _timetableDataSet.Classes.FindById(_classId ?? -1);
					_availableLessons = _plannedLessons.Where(l => l.ClassId == _classId);
					break;
				case EntityType.Teacher:
					_teacherRow = _timetableDataSet.Teachers.FindByPesel(_teacherPesel);
					_availableLessons = _plannedLessons.Where(l => l.TeacherPesel == _teacherPesel);
					break;
			}
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_actionType)
				{
					case ActionType.Add:
						_currentLessonsPlaceRow = _timetableDataSet.LessonsPlaces.NewLessonsPlacesRow();
						break;
					case ActionType.Change:
						_currentLessonsPlaceRow = PrepareLessonsPlace();
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

		private TimetableDataSet.LessonsPlacesRow PrepareLessonsPlace()
		{
			var lessonsPlaceRow = _timetableDataSet.LessonsPlaces
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
			_dayRow = _timetableDataSet.Days.FindById(_dayId);
			_hourRow = _timetableDataSet.Hours.FindById(_hourId);
			textBoxDetails.Text = $"{_dayRow.Name}, {_hourRow.Begin} - {_hourRow.End}\n";

			switch (_entityType)
			{
				case EntityType.Class:
					if (_classRow == null)
						return;

					labelDetails.Text += $"Class:";
					textBoxDetails.Text += $"{_classRow.ToFriendlyString()}";
					break;
				case EntityType.Teacher:
					if (_teacherRow == null)
						return;

					labelDetails.Text += $"Teacher:";
					textBoxDetails.Text += $"{_teacherRow.FirstName} {_teacherRow.LastName}";
					break;
			}
		}

		private void PrepareUnavailableLessonsPlaces()
		{
			_unavailableLessonsPlaces = _timetableDataSet.LessonsPlaces
				.Where(lp => lp.DayId == _dayId && lp.HourId == _hourId)
				.Where(lp => lp.LessonId != _currentLessonsPlaceRow?.LessonsRow?.Id);
		}

		private void UpdateAvailableLessons()
		{
			switch (_entityType)
			{
				case EntityType.Class:
					var unavailableTeachers = _unavailableLessonsPlaces.Select(lp => lp.LessonsRow.TeacherPesel);

					_availableLessons = _availableLessons.Where(l => !unavailableTeachers.Contains(l.TeacherPesel))
						.OrderBy(l => l.SubjectName);

					break;
				case EntityType.Teacher:
					var unavailableClasses = _unavailableLessonsPlaces.Select(lp => lp.LessonsRow.ClassId);

					_availableLessons = _availableLessons.Where(l => (l.ClassId != null) && !unavailableClasses.Contains(l.ClassId ?? -1))
						.OrderBy(l => l.SubjectName)
						.ThenBy(l => l.ClassFriendlyName);

					break;
			}
		}

		private void FillLessonComboBox()
		{
			switch (_entityType)
			{
				case EntityType.Class:
					comboBoxLessons.ItemsSource = _availableLessons;
					comboBoxLessons.DisplayMemberPath = "SubjectTeacher";
					comboBoxLessons.SelectedValuePath = "Id";
					break;
				case EntityType.Teacher:
					comboBoxLessons.ItemsSource = _availableLessons;
					comboBoxLessons.DisplayMemberPath = "SubjectClass";
					comboBoxLessons.SelectedValuePath = "Id";
					break;
			}
		}

		private void PrepareAvailableClassrooms()
		{
			_availableClassrooms = _timetableDataSet.Classrooms
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
			switch (_actionType)
			{
				case ActionType.Change:
					if (_currentLessonsPlaceRow == null)
						return;

					var currentLessonIndex = _availableLessons.ToList().FindIndex(l => l.Id == _currentLessonsPlaceRow.LessonId);
					comboBoxLessons.SelectedIndex = currentLessonIndex;

					comboBoxClassrooms.SelectedValue = _currentLessonsPlaceRow.ClassroomId;
					break;
			}

			buttonOk.IsEnabled = true;
			buttonCancel.IsEnabled = true;
		}

		private void SaveEntity()
		{
			try
			{
				SaveLessonsPlace();
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

		private void SaveLessonsPlace()
		{
			if (comboBoxLessons.SelectedValue == null
				|| comboBoxClassrooms.SelectedValue == null)
			{
				throw new FieldsNotFilledException();
			}

			if (int.TryParse(comboBoxLessons.SelectedValue.ToString(), out _currentLessonId)
				&& int.TryParse(comboBoxClassrooms.SelectedValue.ToString(), out _currentClassroomId))
			{
				_currentLessonsPlaceRow.LessonId = _currentLessonId;
				_currentLessonsPlaceRow.ClassroomId = _currentClassroomId;
			}
			else
			{
				throw new FieldsNotFilledException();
			}


			if (_actionType == ActionType.Add)
			{
				_currentLessonsPlaceRow.DayId = _dayId;
				_currentLessonsPlaceRow.HourId = _hourId;
				_timetableDataSet.LessonsPlaces.Rows.Add(_currentLessonsPlaceRow);
			}

			_lessonsPlacesTableAdapter.Update(_timetableDataSet.LessonsPlaces);

			_callingWindow.RefreshViews(EntityType.LessonsPlace);

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
	}
}
