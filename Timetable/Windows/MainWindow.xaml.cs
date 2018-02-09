using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Timetable.Controls;
using Timetable.DAL.DataSets.MySql;
using Timetable.DAL.DataSets.MySql.TimetableDataSetTableAdapters;
using Timetable.DAL.Utilities;
using Timetable.DAL.ViewModels;
using Timetable.Utilities;

namespace Timetable.Windows
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
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
		private StudentsTableAdapter _studentsTableAdapter;
		private SubjectsTableAdapter _subjectsTableAdapter;
		private TeachersTableAdapter _teachersTableAdapter;

		private MainWindowTabType _mainWindowTabType;

		private EntityType
			_currentEntityType,
			_currentPlanningEntityType,
			_currentSummaryEntityType;

		private bool _windowLoaded;

		#endregion


		#region Properties

		private IOrderedEnumerable<TimetableDataSet.ClassesRow> ClassesEnumerable => _timetableDataSet.Classes
			.OrderBy(c => c.ToFriendlyString());

		private IEnumerable<string> ClassessFriendlyNamesEnumerable => ClassesEnumerable.Select(c => c.ToFriendlyString());

		private IOrderedEnumerable<TimetableDataSet.ClassroomsRow> ClassroomsEnumerable => _timetableDataSet.Classrooms
			.OrderBy(cr => cr.Name);

		private IEnumerable<string> ClassroomsFriendlyNamesEnumerable => ClassroomsEnumerable.Select(cr => cr.Name);

		private IOrderedEnumerable<TimetableDataSet.DaysRow> DaysEnumerable => _timetableDataSet.Days
			.OrderBy(d => d.Number);

		private IOrderedEnumerable<TimetableDataSet.HoursRow> HoursEnumerable => _timetableDataSet.Hours
			.OrderBy(d => d.Number);

		private IEnumerable<int> LessonsDistinctIdsEnumerable => _timetableDataSet.LessonsPlaces
			.Select(lp => lp.LessonId).Distinct();

		private IOrderedEnumerable<TimetableDataSet.LessonsRow> LessonsEnumerable => _timetableDataSet.Lessons
			.OrderBy(l => l.SubjectsRow.Name)
			.ThenBy(l => l.ClassesRow.ToFriendlyString());

		private IOrderedEnumerable<TimetableDataSet.LessonsPlacesRow> LessonsPlacesEnumerable => _timetableDataSet.LessonsPlaces
			.OrderBy(lp => lp.DaysRow.Number)
			.ThenBy(lp => lp.HoursRow.Number);

		private IOrderedEnumerable<TimetableDataSet.StudentsRow> StudentsEnumerable => _timetableDataSet.Students
			.OrderBy(s => s.LastName)
			.ThenBy(s => s.FirstName);

		private IEnumerable<string> StudentsFriendlyNamesEnumerable => StudentsEnumerable.Select(c => c.ToFriendlyString(true));

		private IOrderedEnumerable<TimetableDataSet.SubjectsRow> SubjectsEnumerable => _timetableDataSet.Subjects
			.OrderBy(s => s.Name);

		private IOrderedEnumerable<TimetableDataSet.TeachersRow> TeachersEnumerable => _timetableDataSet.Teachers
			.OrderBy(s => s.LastName)
			.ThenBy(s => s.FirstName);

		private IEnumerable<string> TeachersFriendlyNamesEnumerable => TeachersEnumerable.Select(c => c.ToFriendlyString(true));

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>MainWindow</c>.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
		}

		#endregion


		#region Events

		private async void mainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					InitDatabaseObjects();

					FillComboBoxes();
					FillExpanders();

					tabControl.SelectedIndex = (int) MainWindowTabType.Management;
					_windowLoaded = true;

					RefreshCurrentTabView(EntityType.Class);
					RefreshMappingTabView(EntityType.Class);
					RefreshPlanningTabView(EntityType.Class);
					RefreshSummaryTabView(EntityType.Class);
				});
			});
		}

		private async void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			switch (tabControl.SelectedIndex)
			{
				case 0:
					_mainWindowTabType = MainWindowTabType.Management;
					_currentEntityType = GetCurrentManagementFilterEntityType();
					break;
				case 1:
					_mainWindowTabType = MainWindowTabType.Mapping;
					_currentEntityType = EntityType.Lesson;
					break;
				case 2:
					_mainWindowTabType = MainWindowTabType.Planning;
					_currentEntityType = EntityType.LessonsPlace;
					break;
				case 3:
					_mainWindowTabType = MainWindowTabType.Summary;
					_currentEntityType = EntityType.LessonsPlace;
					break;
			}

			if (!_windowLoaded || sender == null)
				return;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					RefreshCurrentTabView();
				});
			});
		}

		/// <summary>
		///     Metoda obsługująca zdarzenie z kontrolki <c>OperationsStackPanel</c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void comboBoxManagementFilterEntityType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			_currentEntityType = GetCurrentManagementFilterEntityType();

			if (!_windowLoaded || sender == null)
				return;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					RefreshManagementTabView(_currentEntityType);
				});
			});
		}

		/// <summary>
		///     Metoda obsługująca zdarzenie z kontrolki <c>OperationsStackPanel</c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void comboBoxPlanningFilterEntityType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			_currentPlanningEntityType = GetCurrentPlanningFilterEntityType();

			if (!_windowLoaded || sender == null)
				return;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					RefreshPlanningTabView(_currentPlanningEntityType);
				});
			});
		}

		/// <summary>
		///     Metoda obsługująca zdarzenie z kontrolki <c>OperationsStackPanel</c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void comboBoxPlanningFilterEntity_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			if (!_windowLoaded || sender == null)
				return;

			if (stackPanelOperations.comboBoxPlanningFilterEntity.SelectedIndex != -1)
			{
				await Task.Factory.StartNew(() =>
				{
					Dispatcher.Invoke(() =>
					{
						RefreshPlanningTabView(EntityType.LessonsPlace);
					});
				});
			}
		}

		/// <summary>
		///     Metoda obsługująca zdarzenie z kontrolki <c>OperationsStackPanel</c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void comboBoxSummaryFilterEntityType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			_currentSummaryEntityType = GetCurrentSummaryFilterEntityType();

			if (!_windowLoaded || sender == null)
				return;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					RefreshSummaryTabView(_currentSummaryEntityType);
				});
			});
		}

		/// <summary>
		///     Metoda obsługująca zdarzenie z kontrolki <c>OperationsStackPanel</c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void comboBoxSummaryFilterEntity_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			if (!_windowLoaded || sender == null)
				return;

			if (stackPanelOperations.comboBoxSummaryFilterEntity.SelectedIndex != -1)
			{
				await Task.Factory.StartNew(() =>
				{
					Dispatcher.Invoke(() =>
					{
						RefreshSummaryTabView(EntityType.LessonsPlace);
					});
				});
			}
		}

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods


		/// <summary>
		///     Metoda odświeżająca aktualnie wyświetlany widok.
		/// </summary>
		/// <param name="changedDataEntityType">Informacja o rodzaju zmienionych danych.</param>
		public async void RefreshViews(EntityType changedDataEntityType = EntityType.None)
		{
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					RefreshDatabaseObjects(changedDataEntityType);

					RefreshCurrentTabView(changedDataEntityType);

					RefreshOtherTabViews(changedDataEntityType);
				});
			});
		}

		/// <summary>
		///     Metoda zwracająca informację o aktualnie wyświetlanym widoku.
		/// </summary>
		/// <returns></returns>
		public MainWindowTabType GetCurrentTabType()
		{
			return _mainWindowTabType;
		}

		/// <summary>
		///     Metoda zwracająca informację o aktualnie wyświetlanej grupie encji w widoku zarządzania.
		/// </summary>
		/// <returns></returns>
		public EntityType GetCurrentEntityType()
		{
			return _currentEntityType;
		}

		/// <summary>
		///     Metoda zwracająca informację o aktualnie wyświetlanej grupie encji w widoku planowania.
		/// </summary>
		/// <returns></returns>
		public EntityType GetPlanningEntityType()
		{
			return _currentPlanningEntityType;
		}

		/// <summary>
		///     Metoda zwracająca informację o aktualnie wyświetlanej grupie encji w widoku pogdlądu.
		/// </summary>
		/// <returns></returns>
		public EntityType GetSummaryEntityType()
		{
			return _currentSummaryEntityType;
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych klas.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedClasses()
		{
			if (_currentEntityType != EntityType.Class)
				yield break;

			foreach (ClassControl classControl in gridTabManagement.scrollViewerGrid.Children)
				if (classControl.IsChecked())
					yield return classControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca listę numerów PESEL zaznaczonych osób.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetPeselsOfMarkedPeople()
		{
			if (_currentEntityType != EntityType.Student && _currentEntityType != EntityType.Teacher)
				yield break;

			foreach (PersonControl personControl in gridTabManagement.scrollViewerGrid.Children)
				if (personControl.IsChecked())
					yield return personControl.Pesel;
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych sal.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedClassrooms()
		{
			if (_currentEntityType != EntityType.Classroom)
				yield break;

			foreach (ClassroomControl classroomControl in gridTabManagement.scrollViewerGrid.Children)
				if (classroomControl.IsChecked())
					yield return classroomControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych przedmiotów.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedSubjects()
		{
			if (_currentEntityType != EntityType.Subject)
				yield break;

			foreach (SubjectControl subjectControl in gridTabManagement.scrollViewerGrid.Children)
				if (subjectControl.IsChecked())
					yield return subjectControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych dni.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedDays()
		{
			if (_currentEntityType != EntityType.Day)
				yield break;

			foreach (DayControl dayControl in gridTabManagement.scrollViewerGrid.Children)
				if (dayControl.IsChecked())
					yield return dayControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych godzin.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedHours()
		{
			if (_currentEntityType != EntityType.Hour)
				yield break;

			foreach (HourControl hourControl in gridTabManagement.scrollViewerGrid.Children)
				if (hourControl.IsChecked())
					yield return hourControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych lekcji.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedLessons()
		{
			if (_currentEntityType != EntityType.Lesson)
				yield break;

			foreach (LessonControl lessonControl in gridTabMapping.scrollViewerGrid.Children)
				if (lessonControl.IsChecked())
					yield return lessonControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca listę obiektów typu <c>LessonsPlaceViewModel</c> zaznaczonych lekcji.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<LessonsPlaceViewModel> GetCollectionOfMarkedLessonsPlaces()
		{
			if (_currentEntityType != EntityType.LessonsPlace)
				yield break;

			foreach (var cellControl in gridTabPlanning.gridPlanning.Children)
			{
				if (cellControl is CellControl)
				{
					if (((CellControl) cellControl).IsChecked())
						yield return ((CellControl) cellControl).LessonsPlaceViewModel;
				}
			}
		}

		/// <summary>
		///     Metoda zwracająca numer ID klasy wyświetlanej w widoku pogdlądu.
		/// </summary>
		/// <returns></returns>
		public int? GetSummaryClassId()
		{
			int? currentSummaryClassId = null;

			if (_currentSummaryEntityType == EntityType.Class
				&& stackPanelOperations.comboBoxSummaryFilterEntity.SelectedIndex != -1)
			{
				currentSummaryClassId = ClassesEnumerable
					.ElementAt(stackPanelOperations.comboBoxSummaryFilterEntity.SelectedIndex).Id;
			}

			return currentSummaryClassId;
		}

		/// <summary>
		///     Metoda zwracająca numer PESEL nauczyciela wyświetlanego w widoku pogdlądu.
		/// </summary>
		/// <returns></returns>
		public string GetSummaryTeacherPesel()
		{
			string currentSummaryTeacherPesel = null;

			if (_currentSummaryEntityType == EntityType.Teacher
				&& stackPanelOperations.comboBoxSummaryFilterEntity.SelectedIndex != -1)
			{
				currentSummaryTeacherPesel = TeachersEnumerable
					.ElementAt(stackPanelOperations.comboBoxSummaryFilterEntity.SelectedIndex).Pesel;
			}

			return currentSummaryTeacherPesel;
		}

		/// <summary>
		///     Metoda zwracająca numer ID sali wyświetlanej w widoku pogdlądu.
		/// </summary>
		/// <returns></returns>
		public int? GetSummaryClassroomId()
		{
			int? currentSummaryClassroomId = null;

			if (_currentSummaryEntityType == EntityType.Classroom
				&& stackPanelOperations.comboBoxSummaryFilterEntity.SelectedIndex != -1)
			{
				currentSummaryClassroomId = ClassroomsEnumerable
					.ElementAt(stackPanelOperations.comboBoxSummaryFilterEntity.SelectedIndex).Id;
			}

			return currentSummaryClassroomId;
		}

		#endregion


		#region Private methods

		#region Entities operations

		private IEnumerable<TimetableDataSet.LessonsPlacesRow> GetLessonsPlaces(object objectId, EntityType entityType)
		{
			IEnumerable<TimetableDataSet.LessonsPlacesRow> lessonsPlacesRows = null;

			switch (entityType)
			{
				case EntityType.Class:
					lessonsPlacesRows = LessonsPlacesEnumerable
						.Where(lp => lp.LessonsRow.ClassId == (int) objectId);
					break;
				case EntityType.Teacher:
					lessonsPlacesRows = LessonsPlacesEnumerable
						.Where(lp => lp.LessonsRow.TeacherPesel == (string) objectId);
					break;
				case EntityType.Classroom:
					lessonsPlacesRows = LessonsPlacesEnumerable
						.Where(lp => lp.ClassroomId == (int) objectId);
					break;
			}

			return lessonsPlacesRows;
		}

		private IEnumerable<TimetableDataSet.LessonsRow> GetRemainingLessons(object objectId, EntityType entityType)
		{
			IEnumerable<TimetableDataSet.LessonsRow> lessonsRows = null;

			switch (entityType)
			{
				case EntityType.Class:
					lessonsRows = LessonsEnumerable
						.Where(l => l.ClassId == (int) objectId)
						.Where(l => !LessonsDistinctIdsEnumerable.Contains(l.Id));
					break;
				case EntityType.Teacher:
					lessonsRows = LessonsEnumerable
						.Where(l => l.TeacherPesel == (string) objectId)
						.Where(l => !LessonsDistinctIdsEnumerable.Contains(l.Id));
					break;
			}

			return lessonsRows;
		}

		private void InitDatabaseObjects()
		{
			_timetableDataSet = new TimetableDataSet();
			_classesTableAdapter = new ClassesTableAdapter();
			_classroomsTableAdapter = new ClassroomsTableAdapter();
			_daysTableAdapter = new DaysTableAdapter();
			_hoursTableAdapter = new HoursTableAdapter();
			_lessonsTableAdapter = new LessonsTableAdapter();
			_lessonsPlacesTableAdapter = new LessonsPlacesTableAdapter();
			_studentsTableAdapter = new StudentsTableAdapter();
			_subjectsTableAdapter = new SubjectsTableAdapter();
			_teachersTableAdapter = new TeachersTableAdapter();

			_classesTableAdapter.Fill(_timetableDataSet.Classes);
			_classroomsTableAdapter.Fill(_timetableDataSet.Classrooms);
			_daysTableAdapter.Fill(_timetableDataSet.Days);
			_hoursTableAdapter.Fill(_timetableDataSet.Hours);
			_lessonsTableAdapter.Fill(_timetableDataSet.Lessons);
			_lessonsPlacesTableAdapter.Fill(_timetableDataSet.LessonsPlaces);
			_studentsTableAdapter.Fill(_timetableDataSet.Students);
			_subjectsTableAdapter.Fill(_timetableDataSet.Subjects);
			_teachersTableAdapter.Fill(_timetableDataSet.Teachers);
		}

		private void RefreshDatabaseObjects(EntityType changedDataEntityType)
		{
			switch (changedDataEntityType)
			{
				case EntityType.Class:
					_classesTableAdapter.Fill(_timetableDataSet.Classes);
					break;
				case EntityType.Classroom:
					_classroomsTableAdapter.Fill(_timetableDataSet.Classrooms);
					break;
				case EntityType.Day:
					_daysTableAdapter.Fill(_timetableDataSet.Days);
					break;
				case EntityType.Hour:
					_hoursTableAdapter.Fill(_timetableDataSet.Hours);
					break;
				case EntityType.Lesson:
					_lessonsTableAdapter.Fill(_timetableDataSet.Lessons);
					break;
				case EntityType.LessonsPlace:
					_lessonsPlacesTableAdapter.Fill(_timetableDataSet.LessonsPlaces);
					break;
				case EntityType.Student:
					_studentsTableAdapter.Fill(_timetableDataSet.Students);
					break;
				case EntityType.Subject:
					_subjectsTableAdapter.Fill(_timetableDataSet.Subjects);
					break;
				case EntityType.Teacher:
					_teachersTableAdapter.Fill(_timetableDataSet.Teachers);
					break;
			}
		}

		#endregion

		#region Main view controls

		private EntityType GetCurrentManagementFilterEntityType()
		{
			EntityType entityType = EntityType.None;

			switch (stackPanelOperations.comboBoxManagementFilterEntityType.SelectedIndex)
			{
				case 0:
					entityType = EntityType.Class;
					break;
				case 1:
					entityType = EntityType.Student;
					break;
				case 2:
					entityType = EntityType.Teacher;
					break;
				case 3:
					entityType = EntityType.Classroom;
					break;
				case 4:
					entityType = EntityType.Subject;
					break;
				case 5:
					entityType = EntityType.Day;
					break;
				case 6:
					entityType = EntityType.Hour;
					break;
			}

			return entityType;
		}

		private EntityType GetCurrentPlanningFilterEntityType()
		{
			EntityType entityType = EntityType.None;

			switch (stackPanelOperations.comboBoxPlanningFilterEntityType.SelectedIndex)
			{
				case 0:
					entityType = EntityType.Class;
					break;
				case 1:
					entityType = EntityType.Teacher;
					break;
			}

			return entityType;
		}

		private EntityType GetCurrentSummaryFilterEntityType()
		{
			EntityType entityType = EntityType.None;

			switch (stackPanelOperations.comboBoxSummaryFilterEntityType.SelectedIndex)
			{
				case 0:
					entityType = EntityType.Class;
					break;
				case 1:
					entityType = EntityType.Teacher;
					break;
				case 2:
					entityType = EntityType.Classroom;
					break;
			}

			return entityType;
		}

		private void FillComboBoxes()
		{
			stackPanelOperations.CallingWindow = this;

			stackPanelOperations.comboBoxManagementFilterEntityType.Items.Add(EntityType.Class.ToString());
			stackPanelOperations.comboBoxManagementFilterEntityType.Items.Add(EntityType.Student.ToString());
			stackPanelOperations.comboBoxManagementFilterEntityType.Items.Add(EntityType.Teacher.ToString());
			stackPanelOperations.comboBoxManagementFilterEntityType.Items.Add(EntityType.Classroom.ToString());
			stackPanelOperations.comboBoxManagementFilterEntityType.Items.Add(EntityType.Subject.ToString());
			stackPanelOperations.comboBoxManagementFilterEntityType.Items.Add(EntityType.Day.ToString());
			stackPanelOperations.comboBoxManagementFilterEntityType.Items.Add(EntityType.Hour.ToString());
			stackPanelOperations.comboBoxManagementFilterEntityType.SelectedIndex = 0;

			stackPanelOperations.comboBoxPlanningFilterEntityType.Items.Add(EntityType.Class.ToString());
			stackPanelOperations.comboBoxPlanningFilterEntityType.Items.Add(EntityType.Teacher.ToString());
			stackPanelOperations.comboBoxPlanningFilterEntityType.SelectedIndex = 0;

			stackPanelOperations.comboBoxSummaryFilterEntityType.Items.Add(EntityType.Class.ToString());
			stackPanelOperations.comboBoxSummaryFilterEntityType.Items.Add(EntityType.Teacher.ToString());
			stackPanelOperations.comboBoxSummaryFilterEntityType.Items.Add(EntityType.Classroom.ToString());
			stackPanelOperations.comboBoxSummaryFilterEntityType.SelectedIndex = 0;
		}

		private void FillExpanders()
		{
			var stackPanel = new StackPanel();
			stackPanel.Children.Add(new ExpanderControl(this, ActionType.Add, ActionType.Add.ToString()));
			stackPanel.Children.Add(new ExpanderControl(this, ActionType.Change, ActionType.Change.ToString()));
			stackPanel.Children.Add(new ExpanderControl(this, ActionType.Remove, ActionType.Remove.ToString()));
			stackPanelOperations.expanderOperation.Content = stackPanel;

			var stackPanelPlanning = new StackPanel();
			stackPanelPlanning.Children.Add(new ExpanderControl(this, ActionType.RemoveLessonsPlace, ActionType.Remove.ToString()));
			stackPanelOperations.expanderPlanning.Content = stackPanelPlanning;

			var stackPanelSummary = new StackPanel();
			stackPanelSummary.Children.Add(new ExpanderControl(this, ActionType.XLS, "Excel"));
			stackPanelSummary.Children.Add(new ExpanderControl(this, ActionType.PDF, "PDF"));
			stackPanelOperations.expanderSummary.Content = stackPanelSummary;
		}

		private void RefreshCurrentTabView(EntityType changedDataEntityType = EntityType.None)
		{
			stackPanelOperations.expanderOperation.IsExpanded = false;
			stackPanelOperations.expanderPlanning.IsExpanded = false;
			stackPanelOperations.expanderSummary.IsExpanded = false;

			switch (_mainWindowTabType)
			{
				case MainWindowTabType.Management:
					stackPanelOperations.gridManagementFilter.Visibility = Visibility.Visible;
					stackPanelOperations.gridPlanningFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridSummaryFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderOperation.Visibility = Visibility.Visible;
					stackPanelOperations.expanderPlanning.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderSummary.Visibility = Visibility.Collapsed;
					RefreshManagementTabView(changedDataEntityType);
					break;
				case MainWindowTabType.Mapping:
					stackPanelOperations.gridManagementFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridPlanningFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridSummaryFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderOperation.Visibility = Visibility.Visible;
					stackPanelOperations.expanderPlanning.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderSummary.Visibility = Visibility.Collapsed;
					RefreshMappingTabView(changedDataEntityType);
					break;
				case MainWindowTabType.Planning:
					stackPanelOperations.gridManagementFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridPlanningFilter.Visibility = Visibility.Visible;
					stackPanelOperations.gridSummaryFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderOperation.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderPlanning.Visibility = Visibility.Visible;
					stackPanelOperations.expanderSummary.Visibility = Visibility.Collapsed;
					RefreshPlanningTabView(changedDataEntityType);
					break;
				case MainWindowTabType.Summary:
					stackPanelOperations.gridManagementFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridPlanningFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridSummaryFilter.Visibility = Visibility.Visible;
					stackPanelOperations.expanderOperation.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderPlanning.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderSummary.Visibility = Visibility.Visible;
					RefreshSummaryTabView(changedDataEntityType);
					break;
			}
		}

		private void RefreshOtherTabViews(EntityType changedDataEntityType)
		{
			switch (_mainWindowTabType)
			{
				case MainWindowTabType.Management:
				case MainWindowTabType.Mapping:
					RefreshPlanningTabView(changedDataEntityType);
					RefreshSummaryTabView(changedDataEntityType);
					break;
				case MainWindowTabType.Planning:
					RefreshSummaryTabView(changedDataEntityType);
					break;
				case MainWindowTabType.Summary:
					RefreshPlanningTabView(changedDataEntityType);
					break;
			}
		}

		private void RefreshManagementTabView(EntityType changedDataEntityType = EntityType.None)
		{
			if (changedDataEntityType == EntityType.None)
				return;

			FillScrollViewerGridForView(MainWindowTabType.Management, _currentEntityType);
		}

		private void RefreshMappingTabView(EntityType changedDataEntityType = EntityType.None)
		{
			if (changedDataEntityType == EntityType.None)
				return;

			FillScrollViewerGridForView(MainWindowTabType.Mapping, EntityType.Lesson);
		}

		private void RefreshPlanningTabView(EntityType changedDataEntityType = EntityType.None)
		{
			if (changedDataEntityType == EntityType.None)
				return;

			if (_currentPlanningEntityType == changedDataEntityType)
				FillFilterComboBoxesForView(MainWindowTabType.Planning);

			RefreshPlanningEntityView();
		}

		private void RefreshPlanningEntityView()
		{
			FillTimetableGridForView(MainWindowTabType.Planning);
		}

		private void RefreshSummaryTabView(EntityType changedDataEntityType = EntityType.None)
		{
			if (changedDataEntityType == EntityType.None)
				return;

			if (_currentSummaryEntityType == changedDataEntityType)
				FillFilterComboBoxesForView(MainWindowTabType.Summary);

			RefreshSummaryEntityView();
		}

		private void RefreshSummaryEntityView()
		{
			FillTimetableGridForView(MainWindowTabType.Summary);
		}

		private void ClearGrid(Grid grid)
		{
			if (grid.Children.Count > 0)
			{
				grid.Children.Clear();
				grid.ColumnDefinitions.Clear();
				grid.RowDefinitions.Clear();
			}
		}

		#endregion

		#region Management & Mapping

		private Grid GetScrollViewerGridForView(MainWindowTabType tabType)
		{
			Grid grid = null;

			switch (tabType)
			{
				case MainWindowTabType.Management:
					grid = gridTabManagement.scrollViewerGrid;
					break;
				case MainWindowTabType.Mapping:
					grid = gridTabMapping.scrollViewerGrid;
					break;
			}

			return grid;
		}

		private void FillScrollViewerGridForView(MainWindowTabType tabType, EntityType entityType = EntityType.None)
		{
			Grid grid = GetScrollViewerGridForView(tabType);

			ClearGrid(grid);

			switch (entityType)
			{
				case EntityType.Class:
					AddAllClassesToGrid(grid);
					break;
				case EntityType.Classroom:
					AddAllClassroomsToGrid(grid);
					break;
				case EntityType.Day:
					AddAllDaysToGrid(grid);
					break;
				case EntityType.Hour:
					AddAllHoursToGrid(grid);
					break;
				case EntityType.Lesson:
					AddAllLessonsToGrid(grid);
					break;
				case EntityType.Student:
					AddAllStudentsToGrid(grid);
					break;
				case EntityType.Subject:
					AddAllSubjectsToGrid(grid);
					break;
				case EntityType.Teacher:
					AddAllTeachersToGrid(grid);
					break;
			}
		}

		#endregion

		#region Planning & Summary

		private EntityType GetFilterEntityTypeForView(MainWindowTabType tabType)
		{
			EntityType entityType = EntityType.None;

			switch (tabType)
			{
				case MainWindowTabType.Planning:
					entityType = _currentPlanningEntityType;
					break;
				case MainWindowTabType.Summary:
					entityType = _currentSummaryEntityType;
					break;
			}

			return entityType;
		}

		private ComboBox GetFilterComboBoxForView(MainWindowTabType tabType)
		{
			ComboBox entityComboBox = null;

			switch (tabType)
			{
				case MainWindowTabType.Planning:
					entityComboBox = stackPanelOperations.comboBoxPlanningFilterEntity;
					break;
				case MainWindowTabType.Summary:
					entityComboBox = stackPanelOperations.comboBoxSummaryFilterEntity;
					break;
			}

			return entityComboBox;
		}

		private void FillFilterComboBoxesForView(MainWindowTabType tabType)
		{
			EntityType entityType = GetFilterEntityTypeForView(tabType);
			ComboBox entityComboBox = GetFilterComboBoxForView(tabType);

			switch (entityType)
			{
				case EntityType.Class:
					entityComboBox.ItemsSource = ClassessFriendlyNamesEnumerable;
					entityComboBox.SelectedIndex = (entityComboBox.Items.Count > 0) ? 0 : -1;
					break;
				case EntityType.Teacher:
					entityComboBox.ItemsSource = TeachersFriendlyNamesEnumerable;
					entityComboBox.SelectedIndex = (entityComboBox.Items.Count > 0) ? 0 : -1;
					break;
				case EntityType.Classroom:
					entityComboBox.ItemsSource = ClassroomsFriendlyNamesEnumerable;
					entityComboBox.SelectedIndex = (entityComboBox.Items.Count > 0) ? 0 : -1;
					break;
			}
		}

		private Grid GetLessonsPlacesGridForView(MainWindowTabType tabType)
		{
			Grid grid = null;

			switch (tabType)
			{
				case MainWindowTabType.Planning:
					grid = gridTabPlanning.gridPlanning;
					break;
				case MainWindowTabType.Summary:
					grid = gridTabSummary.gridSummary;
					break;
			}

			return grid;
		}

		private Grid GetRemainingLessonsGridForView(MainWindowTabType tabType)
		{
			Grid grid = null;

			switch (tabType)
			{
				case MainWindowTabType.Planning:
					grid = gridTabPlanning.gridPlanningRemainingLessons;
					break;
			}

			return grid;
		}

		private void FillTimetableGridForView(MainWindowTabType tabType)
		{
			EntityType entityType = GetFilterEntityTypeForView(tabType);
			ComboBox entityComboBox = GetFilterComboBoxForView(tabType);

			int selectedIndex = entityComboBox.SelectedIndex;

			if (selectedIndex < 0)
				return;

			object currentId = null;

			switch (entityType)
			{
				case EntityType.Class:
					currentId = ClassesEnumerable.ElementAt(selectedIndex).Id;
					break;
				case EntityType.Teacher:
					currentId = TeachersEnumerable.ElementAt(selectedIndex).Pesel;
					break;
				case EntityType.Classroom:
					currentId = ClassroomsEnumerable.ElementAt(selectedIndex).Id;
					break;
			}

			Grid lessonsPlacesGrid = GetLessonsPlacesGridForView(tabType);
			Grid remainingLessonsGrid = GetRemainingLessonsGridForView(tabType);

			ClearGrid(lessonsPlacesGrid);
			AddDaysToGrid(lessonsPlacesGrid);
			AddHoursToGrid(lessonsPlacesGrid);
			AddAllEmptyLessonsPlacesToGrid(lessonsPlacesGrid, currentId, tabType, entityType);
			AddAllLessonsPlacesToGrid(lessonsPlacesGrid, currentId, tabType, entityType);

			if (remainingLessonsGrid == null)
				return;

			ClearGrid(remainingLessonsGrid);
			AddAllRemainingLessonsToGrid(remainingLessonsGrid, currentId, tabType, entityType);
		}

		#endregion

		#region Management & Mapping - subcontrols

		private void AddAllClassesToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<ClassControl>(grid);

			foreach (TimetableDataSet.ClassesRow classRow in ClassesEnumerable)
				AddEntityControlToGrid<ClassControl, TimetableDataSet.ClassesRow>(grid, classRow);
		}

		private void AddAllClassroomsToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<ClassroomControl>(grid);

			foreach (TimetableDataSet.ClassroomsRow classroomRow in ClassroomsEnumerable)
				AddEntityControlToGrid<ClassroomControl, TimetableDataSet.ClassroomsRow>(grid, classroomRow);
		}

		private void AddAllDaysToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<DayControl>(grid);

			foreach (TimetableDataSet.DaysRow dayRow in DaysEnumerable)
				AddEntityControlToGrid<DayControl, TimetableDataSet.DaysRow>(grid, dayRow);
		}

		private void AddAllHoursToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<HourControl>(grid);

			foreach (TimetableDataSet.HoursRow hourRow in HoursEnumerable)
				AddEntityControlToGrid<HourControl, TimetableDataSet.HoursRow>(grid, hourRow);
		}

		private void AddAllLessonsToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<LessonControl>(grid);

			foreach (TimetableDataSet.LessonsRow lessonRow in LessonsEnumerable)
				AddEntityControlToGrid<LessonControl, TimetableDataSet.LessonsRow>(grid, lessonRow);
		}

		private void AddAllStudentsToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<PersonControl>(grid);

			foreach (TimetableDataSet.StudentsRow studentRow in StudentsEnumerable)
				AddEntityControlToGrid<PersonControl, TimetableDataSet.StudentsRow>(grid, studentRow);
		}

		private void AddAllSubjectsToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<SubjectControl>(grid);

			foreach (TimetableDataSet.SubjectsRow subjectRow in SubjectsEnumerable)
				AddEntityControlToGrid<SubjectControl, TimetableDataSet.SubjectsRow>(grid, subjectRow);
		}

		private void AddAllTeachersToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<PersonControl>(grid);

			foreach (TimetableDataSet.TeachersRow teacherRow in TeachersEnumerable)
				AddEntityControlToGrid<PersonControl, TimetableDataSet.TeachersRow>(grid, teacherRow);
		}

		private void AddDescriptionControlToGrid<TControl>(Grid grid)
		{
			Control control = (Control) Activator.CreateInstance(typeof(TControl));
			int height = (int) typeof(TControl).GetField("HEIGHT").GetValue(null);

			AppendControlToGrid(grid, control, height);
		}

		private void AddEntityControlToGrid<TControl, TRow>(Grid grid, TRow dataRow)
		{
			Control control = (Control) Activator.CreateInstance(typeof(TControl), dataRow);
			int height = (int) typeof(TControl).GetField("HEIGHT").GetValue(null);

			AppendControlToGrid(grid, control, height);
		}

		private void AppendControlToGrid(Grid grid, Control control, int height)
		{
			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(height) });
			Grid.SetRow(control, grid.RowDefinitions.Count - 1);
			grid.Children.Add(control);
		}

		#endregion

		#region Planning & Summary - subcontrols

		private void AddDaysToGrid(Grid grid)
		{
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(TermCellControl.WIDTH) });

			var daysList = DaysEnumerable.ToList();

			for (var i = 1; i <= daysList.Count; i++)
			{
				var dayRow = daysList.ElementAt(i - 1);
				string dayName = dayRow?.Name ?? $"{i}";

				var cellControl = new TermCellControl(dayName);
				Grid.SetColumn(cellControl, i);
				Grid.SetRow(cellControl, 0);

				grid.ColumnDefinitions.Add(new ColumnDefinition());
				grid.Children.Add(cellControl);
			}
		}

		private void AddHoursToGrid(Grid grid)
		{
			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(TermCellControl.WIDTH) });

			var hoursList = HoursEnumerable.ToList();

			for (var j = 1; j <= hoursList.Count; j++)
			{
				var hourRow = hoursList.ElementAt(j - 1);
				string hourName = hourRow?.Number.ToString() ?? $"{j}";

				var cellControl = new TermCellControl(hourName);
				Grid.SetColumn(cellControl, 0);
				Grid.SetRow(cellControl, j);

				grid.RowDefinitions.Add(new RowDefinition());
				grid.Children.Add(cellControl);
			}
		}

		private void AddAllEmptyLessonsPlacesToGrid(Grid grid, object objectId, MainWindowTabType tabType, EntityType entityType)
		{
			var actionType = (tabType == MainWindowTabType.Planning) ? ActionType.Add : ActionType.None;

			foreach (var day in DaysEnumerable)
			{
				foreach (var hour in HoursEnumerable)
				{
					var cell = new LessonsPlaceViewModel
					{
						DayId = day.Id,
						HourId = hour.Id
					};

					switch (entityType)
					{
						case EntityType.Class:
							cell.ClassId = (int) objectId;
							break;
						case EntityType.Teacher:
							cell.TeacherPesel = (string) objectId;
							break;
						case EntityType.Classroom:
							cell.ClassroomId = (int) objectId;
							break;
					}

					AddLessonsPlaceToGrid(grid, cell, actionType, entityType);
				}
			}
		}

		private void AddAllLessonsPlacesToGrid(Grid grid, object objectId, MainWindowTabType tabType, EntityType entityType)
		{
			IEnumerable<TimetableDataSet.LessonsPlacesRow> allLessonsPlaces = GetLessonsPlaces(objectId, entityType);

			if (allLessonsPlaces == null)
				return;

			var actionType = (tabType == MainWindowTabType.Planning) ? ActionType.Change : ActionType.None;

			foreach (var lessonsPlace in allLessonsPlaces)
			{
				var cell = new LessonsPlaceViewModel(lessonsPlace);

				AddLessonsPlaceToGrid(grid, cell, actionType, entityType);
			}
		}

		private void AddAllRemainingLessonsToGrid(Grid grid, object objectId, MainWindowTabType tabType, EntityType entityType)
		{
			IEnumerable<TimetableDataSet.LessonsRow> remainingLessons = GetRemainingLessons(objectId, entityType);

			if (remainingLessons == null)
				return;

			foreach (var lesson in remainingLessons)
			{
				var cell = new LessonsPlaceViewModel
				{
					SubjectName = lesson.SubjectsRow.Name,
					ClassFriendlyName = lesson.ClassesRow.ToFriendlyString(),
					TeacherFriendlyName = lesson.TeachersRow.ToFriendlyString()
				};

				AppendRemainingLessonToGrid(grid, cell);
			}
		}

		private void AddLessonsPlaceToGrid(Grid grid, LessonsPlaceViewModel lessonsPlaceViewModel, ActionType actionType, EntityType entityType)
		{
			CellControl cellControl = null;

			var dayIndex = DaysEnumerable.ToList().IndexOf(DaysEnumerable.FirstOrDefault(d => d.Id == lessonsPlaceViewModel.DayId));
			var hourIndex = HoursEnumerable.ToList().IndexOf(HoursEnumerable.FirstOrDefault(h => h.Id == lessonsPlaceViewModel.HourId));

			switch (entityType)
			{
				case EntityType.None:
					cellControl = new CellControl(this, hourIndex % 2 == 0);
					break;
				case EntityType.Class:
					cellControl = new CellControl(lessonsPlaceViewModel, actionType, entityType, TimetableType.Class,
						this, (hourIndex % 2 == 0));
					break;
				case EntityType.Teacher:
					cellControl = new CellControl(lessonsPlaceViewModel, actionType, entityType, TimetableType.Teacher,
						this, (hourIndex % 2 == 0));
					break;
				case EntityType.Classroom:
					cellControl = new CellControl(lessonsPlaceViewModel, actionType, entityType, TimetableType.Classroom,
						this, (hourIndex % 2 == 0));
					break;
			}

			if (cellControl == null)
				return;

			cellControl.BorderThickness = CalculateCellControlBorderThickness(grid, dayIndex, hourIndex);

			Grid.SetColumn(cellControl, dayIndex + 1);
			Grid.SetRow(cellControl, hourIndex + 1);

			grid.Children.Add(cellControl);
		}

		private void AppendRemainingLessonToGrid(Grid grid, LessonsPlaceViewModel lessonsPlaceViewModel)
		{
			var cellControl = new CellControl(lessonsPlaceViewModel, ActionType.None, EntityType.Lesson, TimetableType.Lesson,
				this, (grid.RowDefinitions.Count % 2 == 0));
			cellControl.Margin = CellControl.SEPARATOR_MARGIN;

			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(CellControl.HEIGHT) });
			Grid.SetRow(cellControl, grid.RowDefinitions.Count - 1);
			grid.Children.Add(cellControl);
		}

		private static Thickness CalculateCellControlBorderThickness(Grid grid, int dayIndex, int hourIndex)
		{
			if ((dayIndex + 2) == grid.ColumnDefinitions.Count
				&& (hourIndex + 2) == grid.RowDefinitions.Count)
			{
				return new Thickness(1, 1, 1, 1);
			}

			if ((dayIndex + 2) == grid.ColumnDefinitions.Count)
			{
				return new Thickness(1, 1, 1, 0);
			}

			if ((hourIndex + 2) == grid.RowDefinitions.Count)
			{
				return new Thickness(1, 1, 0, 1);
			}

			return new Thickness(1, 1, 0, 0);
		}

		#endregion

		#endregion
	}
}
