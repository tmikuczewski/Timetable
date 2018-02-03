using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Timetable.Controls;
using Timetable.TimetableDataSetTableAdapters;
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

		private MainWindowTabType _mainWindowTabType;

		private EntityType
			_currentEntityType,
			_currentPlanningEntityType,
			_currentSummaryEntityType;

		private bool _windowLoaded;

		#endregion


		#region Properties

		private IOrderedEnumerable<TimetableDataSet.DaysRow> DaysEnumerable => timetableDataSet.Days
			.OrderBy(d => d.Id);

		private IOrderedEnumerable<TimetableDataSet.HoursRow> HoursEnumerable => timetableDataSet.Hours
			.OrderBy(d => d.Id);

		private IOrderedEnumerable<TimetableDataSet.StudentsRow> StudentsEnumerable => timetableDataSet.Students
			.OrderBy(s => s.LastName)
			.ThenBy(s => s.FirstName);

		private IEnumerable<string> StudentsFriendlyNamesEnumerable => StudentsEnumerable.Select(c => c.ToFriendlyString(true));

		private IOrderedEnumerable<TimetableDataSet.TeachersRow> TeachersEnumerable => timetableDataSet.Teachers
			.OrderBy(s => s.LastName)
			.ThenBy(s => s.FirstName);

		private IEnumerable<string> TeachersFriendlyNamesEnumerable => TeachersEnumerable.Select(c => c.ToFriendlyString(true));

		private IOrderedEnumerable<TimetableDataSet.ClassesRow> ClassesEnumerable => timetableDataSet.Classes
			.OrderBy(c => c.ToFriendlyString());

		private IEnumerable<string> ClassessFriendlyNamesEnumerable => ClassesEnumerable.Select(c => c.ToFriendlyString());

		private IOrderedEnumerable<TimetableDataSet.ClassroomsRow> ClassroomsEnumerable => timetableDataSet.Classrooms
			.OrderBy(cr => cr.Name);

		private IEnumerable<string> ClassroomsFriendlyNamesEnumerable => ClassroomsEnumerable.Select(cr => cr.Name);


		private IOrderedEnumerable<TimetableDataSet.SubjectsRow> SubjectsEnumerable => timetableDataSet.Subjects
			.OrderBy(s => s.Name);

		private IOrderedEnumerable<TimetableDataSet.LessonsRow> LessonsEnumerable => timetableDataSet.Lessons
			.OrderBy(l => l.SubjectsRow.Name)
			.ThenBy(l => l.ClassesRow.ToFriendlyString());

		private IOrderedEnumerable<TimetableDataSet.LessonsPlacesRow> LessonsPlacesEnumerable => timetableDataSet.LessonsPlaces
			.OrderBy(lp => lp.DayId)
			.ThenBy(lp => lp.HourId);

		private IEnumerable<int> LessonsDistinctIdsEnumerable => timetableDataSet.LessonsPlaces
			.Select(lp => lp.LessonId).Distinct();

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

					RefreshCurrentTabView();
					FillFilterComboBoxesForView(MainWindowTabType.Planning);
					FillFilterComboBoxesForView(MainWindowTabType.Summary);
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
					_currentEntityType = EntityType.Lessons;
					break;
				case 2:
					_mainWindowTabType = MainWindowTabType.Planning;
					_currentEntityType = EntityType.LessonsPlaces;
					break;
				case 3:
					_mainWindowTabType = MainWindowTabType.Summary;
					_currentEntityType = EntityType.LessonsPlaces;
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
					RefreshManagementTabView();
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
					RefreshPlanningTabView(true);
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
					Dispatcher.Invoke(RefreshPlanningEntityView);
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
					RefreshSummaryTabView(true);
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
					Dispatcher.Invoke(RefreshSummaryEntityView);
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

					RefreshCurrentTabView(true);

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
		///     Metoda zwracająca informację o aktualnie wyświetlanej grupie encji w widoku podsumowania.
		/// </summary>
		/// <returns></returns>
		public EntityType GetSummaryEntityType()
		{
			return _currentSummaryEntityType;
		}

		/// <summary>
		///     Metoda zwracająca listę numerów PESEL zaznaczonych osób.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetPeselsOfMarkedPeople()
		{
			foreach (PersonControl personControl in gridTabManagement.scrollViewerGrid.Children)
				if (personControl.IsChecked())
					yield return personControl.Pesel;
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych klas.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedClasses()
		{
			foreach (ClassControl classControl in gridTabManagement.scrollViewerGrid.Children)
				if (classControl.IsChecked())
					yield return classControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych przedmiotów.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedSubjects()
		{
			foreach (SubjectControl subjectControl in gridTabManagement.scrollViewerGrid.Children)
				if (subjectControl.IsChecked())
					yield return subjectControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych lekcji.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedLessons()
		{
			foreach (LessonControl lessonControl in gridTabMapping.scrollViewerGrid.Children)
				if (lessonControl.IsChecked())
					yield return lessonControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca listę obiektów typu <c>CellViewModel</c> zaznaczonych lekcji.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<CellViewModel> GetCollectionOfMarkedLessonsPlaces()
		{
			foreach (var cellControl in gridTabPlanning.gridPlanning.Children)
			{
				if (cellControl is CellControl)
				{
					if (((CellControl) cellControl).IsChecked())
						yield return ((CellControl) cellControl).CellViewModel;
				}
			}
		}

		/// <summary>
		///     Metoda zwracająca numer ID klasy wyświetlanej w widoku podsumowania.
		/// </summary>
		/// <returns></returns>
		public int? GetSummaryClassId()
		{
			int? currentSummaryClassId = null;

			if (_currentSummaryEntityType == EntityType.Classes
				&& stackPanelOperations.comboBoxSummaryFilterEntity.SelectedIndex != -1)
			{
				currentSummaryClassId = ClassesEnumerable
					.ElementAt(stackPanelOperations.comboBoxSummaryFilterEntity.SelectedIndex).Id;
			}

			return currentSummaryClassId;
		}

		/// <summary>
		///     Metoda zwracająca numer PESEL nauczyciela wyświetlanego w widoku podsumowania.
		/// </summary>
		/// <returns></returns>
		public string GetSummaryTeacherPesel()
		{
			string currentSummaryTeacherPesel = null;

			if (_currentSummaryEntityType == EntityType.Teachers
				&& stackPanelOperations.comboBoxSummaryFilterEntity.SelectedIndex != -1)
			{
				currentSummaryTeacherPesel = TeachersEnumerable
					.ElementAt(stackPanelOperations.comboBoxSummaryFilterEntity.SelectedIndex).Pesel;
			}

			return currentSummaryTeacherPesel;
		}

		/// <summary>
		///     Metoda zwracająca numer ID sali wyświetlanej w widoku podsumowania.
		/// </summary>
		/// <returns></returns>
		public int? GetSummaryClassroomId()
		{
			int? currentSummaryClassroomId = null;

			if (_currentSummaryEntityType == EntityType.Classrooms
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
				case EntityType.Classes:
					lessonsPlacesRows = LessonsPlacesEnumerable
						.Where(lp => lp.LessonsRow.ClassId == (int) objectId);
					break;
				case EntityType.Teachers:
					lessonsPlacesRows = LessonsPlacesEnumerable
						.Where(lp => lp.LessonsRow.TeacherPesel == (string) objectId);
					break;
				case EntityType.Classrooms:
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
				case EntityType.Classes:
					lessonsRows = LessonsEnumerable
						.Where(l => l.ClassId == (int) objectId)
						.Where(l => !LessonsDistinctIdsEnumerable.Contains(l.Id));
					break;
				case EntityType.Teachers:
					lessonsRows = LessonsEnumerable
						.Where(l => l.TeacherPesel == (string) objectId)
						.Where(l => !LessonsDistinctIdsEnumerable.Contains(l.Id));
					break;
			}

			return lessonsRows;
		}

		private void InitDatabaseObjects()
		{
			timetableDataSet = new TimetableDataSet();
			classesTableAdapter = new ClassesTableAdapter();
			classroomsTableAdapter = new ClassroomsTableAdapter();
			daysTableAdapter = new DaysTableAdapter();
			hoursTableAdapter = new HoursTableAdapter();
			lessonsTableAdapter = new LessonsTableAdapter();
			lessonsPlacesTableAdapter = new LessonsPlacesTableAdapter();
			studentsTableAdapter = new StudentsTableAdapter();
			subjectsTableAdapter = new SubjectsTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();

			classesTableAdapter.Fill(timetableDataSet.Classes);
			classroomsTableAdapter.Fill(timetableDataSet.Classrooms);
			daysTableAdapter.Fill(timetableDataSet.Days);
			hoursTableAdapter.Fill(timetableDataSet.Hours);
			lessonsTableAdapter.Fill(timetableDataSet.Lessons);
			lessonsPlacesTableAdapter.Fill(timetableDataSet.LessonsPlaces);
			studentsTableAdapter.Fill(timetableDataSet.Students);
			subjectsTableAdapter.Fill(timetableDataSet.Subjects);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);
		}

		private void RefreshDatabaseObjects(EntityType changedDataEntityType)
		{
			switch (changedDataEntityType)
			{
				case EntityType.Students:
					studentsTableAdapter.Fill(timetableDataSet.Students);
					break;
				case EntityType.Teachers:
					teachersTableAdapter.Fill(timetableDataSet.Teachers);
					break;
				case EntityType.Classes:
					classesTableAdapter.Fill(timetableDataSet.Classes);
					break;
				case EntityType.Classrooms:
					classroomsTableAdapter.Fill(timetableDataSet.Classrooms);
					break;
				case EntityType.Subjects:
					subjectsTableAdapter.Fill(timetableDataSet.Subjects);
					break;
				case EntityType.Lessons:
					lessonsTableAdapter.Fill(timetableDataSet.Lessons);
					break;
				case EntityType.LessonsPlaces:
					lessonsPlacesTableAdapter.Fill(timetableDataSet.LessonsPlaces);
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
					entityType = EntityType.Students;
					break;
				case 1:
					entityType = EntityType.Teachers;
					break;
				case 2:
					entityType = EntityType.Classes;
					break;
				case 3:
					entityType = EntityType.Subjects;
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
					entityType = EntityType.Classes;
					break;
				case 1:
					entityType = EntityType.Teachers;
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
					entityType = EntityType.Classes;
					break;
				case 1:
					entityType = EntityType.Teachers;
					break;
				case 2:
					entityType = EntityType.Classrooms;
					break;
			}

			return entityType;
		}

		private void FillComboBoxes()
		{
			stackPanelOperations.CallingWindow = this;

			stackPanelOperations.comboBoxManagementFilterEntityType.Items.Add(EntityType.Students.ToString());
			stackPanelOperations.comboBoxManagementFilterEntityType.Items.Add(EntityType.Teachers.ToString());
			stackPanelOperations.comboBoxManagementFilterEntityType.Items.Add(EntityType.Classes.ToString());
			stackPanelOperations.comboBoxManagementFilterEntityType.Items.Add(EntityType.Subjects.ToString());
			stackPanelOperations.comboBoxManagementFilterEntityType.SelectedIndex = 0;

			stackPanelOperations.comboBoxPlanningFilterEntityType.Items.Add(EntityType.Classes.ToString());
			stackPanelOperations.comboBoxPlanningFilterEntityType.Items.Add(EntityType.Teachers.ToString());
			stackPanelOperations.comboBoxPlanningFilterEntityType.SelectedIndex = 0;

			stackPanelOperations.comboBoxSummaryFilterEntityType.Items.Add(EntityType.Classes.ToString());
			stackPanelOperations.comboBoxSummaryFilterEntityType.Items.Add(EntityType.Teachers.ToString());
			stackPanelOperations.comboBoxSummaryFilterEntityType.Items.Add(EntityType.Classrooms.ToString());
			stackPanelOperations.comboBoxSummaryFilterEntityType.SelectedIndex = 0;

			_currentEntityType = EntityType.Students;
			_currentPlanningEntityType = EntityType.Classes;
			_currentSummaryEntityType = EntityType.Classes;
		}

		private void FillExpanders()
		{
			var stackPanel = new StackPanel();
			stackPanel.Children.Add(new ExpanderControl(this, ActionType.Add, ActionType.Add.ToString()));
			stackPanel.Children.Add(new ExpanderControl(this, ActionType.Change, ActionType.Change.ToString()));
			stackPanel.Children.Add(new ExpanderControl(this, ActionType.Remove, ActionType.Remove.ToString()));
			stackPanelOperations.expanderOperation.Content = stackPanel;

			var stackPanelPlanning = new StackPanel();
			stackPanelPlanning.Children.Add(new ExpanderControl(this, ActionType.RemoveLessonPlace, ActionType.Remove.ToString()));
			stackPanelOperations.expanderPlanning.Content = stackPanelPlanning;

			var stackPanelExport = new StackPanel();
			stackPanelExport.Children.Add(new ExpanderControl(this, ActionType.XLS, "Excel"));
			stackPanelExport.Children.Add(new ExpanderControl(this, ActionType.PDF, "PDF"));
			stackPanelOperations.expanderExport.Content = stackPanelExport;
		}

		private void RefreshCurrentTabView(bool forceRefresh = false)
		{
			stackPanelOperations.expanderOperation.IsExpanded = false;
			stackPanelOperations.expanderPlanning.IsExpanded = false;
			stackPanelOperations.expanderExport.IsExpanded = false;

			switch (_mainWindowTabType)
			{
				case MainWindowTabType.Management:
					stackPanelOperations.gridManagementFilter.Visibility = Visibility.Visible;
					stackPanelOperations.gridPlanningFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridSummaryFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderOperation.Visibility = Visibility.Visible;
					stackPanelOperations.expanderPlanning.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderExport.Visibility = Visibility.Collapsed;
					RefreshManagementTabView();
					break;
				case MainWindowTabType.Mapping:
					stackPanelOperations.gridManagementFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridPlanningFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridSummaryFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderOperation.Visibility = Visibility.Visible;
					stackPanelOperations.expanderPlanning.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderExport.Visibility = Visibility.Collapsed;
					RefreshMappingTabView();
					break;
				case MainWindowTabType.Planning:
					stackPanelOperations.gridManagementFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridPlanningFilter.Visibility = Visibility.Visible;
					stackPanelOperations.gridSummaryFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderOperation.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderPlanning.Visibility = Visibility.Visible;
					stackPanelOperations.expanderExport.Visibility = Visibility.Collapsed;
					RefreshPlanningTabView(forceRefresh);
					break;
				case MainWindowTabType.Summary:
					stackPanelOperations.gridManagementFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridPlanningFilter.Visibility = Visibility.Collapsed;
					stackPanelOperations.gridSummaryFilter.Visibility = Visibility.Visible;
					stackPanelOperations.expanderOperation.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderPlanning.Visibility = Visibility.Collapsed;
					stackPanelOperations.expanderExport.Visibility = Visibility.Visible;
					RefreshSummaryTabView(forceRefresh);
					break;
			}
		}

		private void RefreshOtherTabViews(EntityType changedDataEntityType)
		{
			switch (changedDataEntityType)
			{
				case EntityType.Teachers:
				case EntityType.Classes:
				case EntityType.Classrooms:
				case EntityType.Subjects:
				case EntityType.Lessons:
				case EntityType.LessonsPlaces:
					if (_mainWindowTabType != MainWindowTabType.Planning)
					{
						RefreshPlanningTabView(true);

					}
					if (_mainWindowTabType != MainWindowTabType.Summary)
					{
						RefreshSummaryTabView(true);

					}
					break;
			}
		}

		private void RefreshManagementTabView(bool forceRefresh = false)
		{
			FillScrollViewerGridForView(MainWindowTabType.Management, _currentEntityType);
		}

		private void RefreshMappingTabView(bool forceRefresh = false)
		{
			FillScrollViewerGridForView(MainWindowTabType.Mapping, EntityType.Lessons);
		}

		private void RefreshPlanningTabView(bool forceRefresh = false)
		{
			if (forceRefresh)
			{
				FillFilterComboBoxesForView(MainWindowTabType.Planning);
				RefreshPlanningEntityView();
			}
		}

		private void RefreshPlanningEntityView()
		{
			FillTimetableGridForView(MainWindowTabType.Planning);
		}

		private void RefreshSummaryTabView(bool forceRefresh = false)
		{
			if (forceRefresh)
			{
				FillFilterComboBoxesForView(MainWindowTabType.Summary);
				RefreshSummaryEntityView();
			}
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
				case EntityType.Students:
					AddAllStudentsToGrid(grid);
					break;
				case EntityType.Teachers:
					AddAllTeachersToGrid(grid);
					break;
				case EntityType.Classes:
					AddAllClassesToGrid(grid);
					break;
				case EntityType.Subjects:
					AddAllSubjectsToGrid(grid);
					break;
				case EntityType.Lessons:
					AddAllLessonsToGrid(grid);
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
				case EntityType.Classes:
					entityComboBox.ItemsSource = ClassessFriendlyNamesEnumerable;
					entityComboBox.SelectedIndex = (entityComboBox.Items.Count > 0) ? 0 : -1;
					break;
				case EntityType.Teachers:
					entityComboBox.ItemsSource = TeachersFriendlyNamesEnumerable;
					entityComboBox.SelectedIndex = (entityComboBox.Items.Count > 0) ? 0 : -1;
					break;
				case EntityType.Classrooms:
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
				case EntityType.Classes:
					currentId = ClassesEnumerable.ElementAt(selectedIndex).Id;
					break;
				case EntityType.Teachers:
					currentId = TeachersEnumerable.ElementAt(selectedIndex).Pesel;
					break;
				case EntityType.Classrooms:
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

		private void AddAllStudentsToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<PersonControl>(grid);

			foreach (TimetableDataSet.StudentsRow studentRow in StudentsEnumerable)
				AddEntityControlToGrid<PersonControl, TimetableDataSet.StudentsRow>(grid, studentRow);
		}

		private void AddAllTeachersToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<PersonControl>(grid);

			foreach (TimetableDataSet.TeachersRow teacherRow in TeachersEnumerable)
				AddEntityControlToGrid<PersonControl, TimetableDataSet.TeachersRow>(grid, teacherRow);
		}

		private void AddAllClassesToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<ClassControl>(grid);

			foreach (TimetableDataSet.ClassesRow classRow in ClassesEnumerable)
				AddEntityControlToGrid<ClassControl, TimetableDataSet.ClassesRow>(grid, classRow);
		}

		private void AddAllSubjectsToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<SubjectControl>(grid);

			foreach (TimetableDataSet.SubjectsRow subjectRow in SubjectsEnumerable)
				AddEntityControlToGrid<SubjectControl, TimetableDataSet.SubjectsRow>(grid, subjectRow);
		}

		private void AddAllLessonsToGrid(Grid grid)
		{
			AddDescriptionControlToGrid<LessonControl>(grid);

			foreach (TimetableDataSet.LessonsRow lessonRow in LessonsEnumerable)
				AddEntityControlToGrid<LessonControl, TimetableDataSet.LessonsRow>(grid, lessonRow);
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
				string dayName = (dayRow != null) ? dayRow.Name : $"{i}";

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
				string hourName = (hourRow != null) ? $"{j}" : $"{j}";

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

			for (var col = 1; col <= grid.ColumnDefinitions.Count - 1; col++)
			{
				for (var row = 1; row <= grid.RowDefinitions.Count - 1; row++)
				{
					var cell = new CellViewModel
					{
						DayId = col,
						HourId = row
					};

					switch (entityType)
					{
						case EntityType.Classes:
							cell.ClassId = (int) objectId;
							break;
						case EntityType.Teachers:
							cell.TeacherPesel = (string) objectId;
							break;
						case EntityType.Classrooms:
							cell.ClassroomId = (int) objectId;
							break;
					}

					AddLessonPlaceToGrid(grid, cell, actionType, entityType);
				}
			}
		}

		private void AddAllLessonsPlacesToGrid(Grid grid, object objectId, MainWindowTabType tabType, EntityType entityType)
		{
			IEnumerable<TimetableDataSet.LessonsPlacesRow> allLessonsPlaces = GetLessonsPlaces(objectId, entityType);

			if (allLessonsPlaces == null)
				return;

			var actionType = (tabType == MainWindowTabType.Planning) ? ActionType.Change : ActionType.None;

			foreach (var lessonPlace in allLessonsPlaces)
			{
				var cell = new CellViewModel(lessonPlace);

				AddLessonPlaceToGrid(grid, cell, actionType, entityType);
			}
		}

		private void AddAllRemainingLessonsToGrid(Grid grid, object objectId, MainWindowTabType tabType, EntityType entityType)
		{
			IEnumerable<TimetableDataSet.LessonsRow> remainingLessons = GetRemainingLessons(objectId, entityType);

			if (remainingLessons == null)
				return;

			foreach (var lesson in remainingLessons)
			{
				var cell = new CellViewModel
				{
					SubjectName = lesson.SubjectsRow.Name,
					ClassFriendlyName = lesson.ClassesRow.ToFriendlyString(),
					TeacherFriendlyName = lesson.TeachersRow.ToFriendlyString()
				};

				AppendRemainingLessonToGrid(grid, cell);
			}
		}

		private void AddLessonPlaceToGrid(Grid grid, CellViewModel cell, ActionType actionType, EntityType entityType)
		{
			CellControl cellControl = null;

			switch (entityType)
			{
				case EntityType.None:
					cellControl = new CellControl(this, cell.HourId % 2 != 0);
					break;
				case EntityType.Classes:
					cellControl = new CellControl(cell, actionType, entityType, TimetableType.Class,
						this, (cell.HourId % 2 != 0));
					break;
				case EntityType.Teachers:
					cellControl = new CellControl(cell, actionType, entityType, TimetableType.Teacher,
						this, (cell.HourId % 2 != 0));
					break;
				case EntityType.Classrooms:
					cellControl = new CellControl(cell, actionType, entityType, TimetableType.Classroom,
						this, (cell.HourId % 2 != 0));
					break;
			}

			if (cellControl == null)
				return;

			cellControl.BorderThickness = CalculateCellControlBorderThickness(grid, cell.DayId, cell.HourId);

			Grid.SetColumn(cellControl, cell.DayId);
			Grid.SetRow(cellControl, cell.HourId);

			grid.Children.Add(cellControl);
		}

		private void AppendRemainingLessonToGrid(Grid grid, CellViewModel cell)
		{
			var cellControl = new CellControl(cell, ActionType.None, EntityType.Lessons, TimetableType.Lesson,
				this, (grid.RowDefinitions.Count % 2 != 0));
			cellControl.Margin = CellControl.SEPARATOR_MARGIN;

			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(CellControl.HEIGHT) });
			Grid.SetRow(cellControl, grid.RowDefinitions.Count - 1);
			grid.Children.Add(cellControl);
		}

		private static Thickness CalculateCellControlBorderThickness(Grid grid, int day, int hour)
		{
			if (day == grid.ColumnDefinitions.Count - 1
				&& hour == grid.RowDefinitions.Count - 1)
			{
				return new Thickness(1, 1, 1, 1);
			}
			else if (day == grid.ColumnDefinitions.Count - 1)
			{
				return new Thickness(1, 1, 1, 0);
			}
			else if (hour == grid.RowDefinitions.Count - 1)
			{
				return new Thickness(1, 1, 0, 1);
			}
			else
			{
				return new Thickness(1, 1, 0, 0);
			}
		}

		#endregion

		#endregion
	}
}
