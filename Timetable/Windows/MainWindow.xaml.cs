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

		private MainTabControlType _mainTabControlType;

		private ComboBoxContentType
			_comboBoxContentType,
			_comboBoxPlanningContentType,
			_comboBoxSummaryContentType;

		private bool _windowLoaded;

		#endregion


		#region Properties

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
					_windowLoaded = true;

					tabControl.SelectedIndex = (int) MainTabControlType.Management;
					comboBox.SelectedIndex = 0;

					RefreshMainCategoryView();
				});
			});
		}

		private async void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			switch (tabControl.SelectedIndex)
			{
				case 0:
					_mainTabControlType = MainTabControlType.Management;
					_comboBoxContentType = GetComboBoxContentType();
					break;
				case 1:
					_mainTabControlType = MainTabControlType.Mapping;
					_comboBoxContentType = ComboBoxContentType.Lessons;
					break;
				case 2:
					_mainTabControlType = MainTabControlType.Planning;
					_comboBoxContentType = ComboBoxContentType.LessonsPlaces;
					break;
				case 3:
					_mainTabControlType = MainTabControlType.Summary;
					_comboBoxContentType = ComboBoxContentType.LessonsPlaces;
					break;
			}

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(RefreshMainCategoryView);
			});
		}

		private async void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			_comboBoxContentType = GetComboBoxContentType();

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(RefreshManagementCategoryView);
			});
		}

		private async void comboBoxPlanning1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			if (sender == null)
				return;

			_comboBoxPlanningContentType = GetComboBoxPlanningContentType();

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					RefreshPlanningCategoryView();
				});
			});
		}

		private async void comboBoxPlanning2_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			if (sender == null)
				return;

			if (comboBoxPlanning2.SelectedIndex != -1)
			{
				await Task.Factory.StartNew(() =>
				{
					Dispatcher.Invoke(RefreshPlanningEntityView);
				});
			}
		}

		private async void comboBoxSummary1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			if (sender == null)
				return;

			_comboBoxSummaryContentType = GetComboBoxSummaryContentType();

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					RefreshSummaryCategoryView();
				});
			});
		}

		private async void comboBoxSummary2_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.Handled = true;

			if (sender == null)
				return;

			if (comboBoxSummary2.SelectedIndex != -1)
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
		/// <param name="changedDataType">Informacja o rodzaju zmienionych danych.</param>
		public async void RefreshViews(ComboBoxContentType changedDataType = ComboBoxContentType.Entities)
		{
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					RefreshDatabaseObjects(changedDataType);

					RefreshMainCategoryView();

					RefreshOtherCategoriesViews(changedDataType);
				});
			});
		}

		/// <summary>
		///     Metoda zwracająca informację o aktualnie wyświetlanym widoku.
		/// </summary>
		/// <returns></returns>
		public MainTabControlType GetCurrentControlType()
		{
			return _mainTabControlType;
		}

		/// <summary>
		///     Metoda zwracająca informację o aktualnie wyświetlanej grupie encji w widoku zarządzania.
		/// </summary>
		/// <returns></returns>
		public ComboBoxContentType GetCurrentContentType()
		{
			return _comboBoxContentType;
		}

		/// <summary>
		///     Metoda zwracająca informację o aktualnie wyświetlanej grupie encji w widoku planowania.
		/// </summary>
		/// <returns></returns>
		public ComboBoxContentType GetPlanningContentType()
		{
			return _comboBoxPlanningContentType;
		}

		/// <summary>
		///     Metoda zwracająca informację o aktualnie wyświetlanej grupie encji w widoku podsumowania.
		/// </summary>
		/// <returns></returns>
		public ComboBoxContentType GetSummaryContentType()
		{
			return _comboBoxSummaryContentType;
		}

		/// <summary>
		///     Metoda zwracająca listę numerów PESEL zaznaczonych osób.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetPeselsOfMarkedPeople()
		{
			foreach (PersonControl personControl in scrollViewersManagementGrid.Children)
				if (personControl.IsChecked())
					yield return personControl.Pesel.StringRepresentation;
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych klas.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedClasses()
		{
			foreach (ClassControl classControl in scrollViewersManagementGrid.Children)
				if (classControl.IsChecked())
					yield return classControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych przedmiotów.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedSubjects()
		{
			foreach (SubjectControl subjectControl in scrollViewersManagementGrid.Children)
				if (subjectControl.IsChecked())
					yield return subjectControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca listę numerów ID zaznaczonych lekcji.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetIdNumbersOfMarkedLessons()
		{
			foreach (LessonControl lessonControl in scrollViewersMappingGrid.Children)
				if (lessonControl.IsChecked())
					yield return lessonControl.GetId();
		}

		/// <summary>
		///     Metoda zwracająca ID klasy wyświetlanej w widoku podsumowania.
		/// </summary>
		/// <returns></returns>
		public int? GetSummaryClassId()
		{
			int? currentSummaryClassId = null;

			if (_comboBoxSummaryContentType == ComboBoxContentType.Classes
				&& comboBoxSummary2.SelectedIndex != -1)
				currentSummaryClassId = ClassesEnumerable
					.ElementAt(comboBoxSummary2.SelectedIndex).Id;

			return currentSummaryClassId;
		}

		/// <summary>
		///     Metoda zwracająca Pesel nauczyciela wyświetlanego w widoku podsumowania.
		/// </summary>
		/// <returns></returns>
		public string GetSummaryTeacherPesel()
		{
			string currentSummaryTeacherPesel = null;

			if (_comboBoxSummaryContentType == ComboBoxContentType.Teachers
				&& comboBoxSummary2.SelectedIndex != -1)
				currentSummaryTeacherPesel = TeachersEnumerable.ElementAt(comboBoxSummary2.SelectedIndex).Pesel;

			return currentSummaryTeacherPesel;
		}

		#endregion


		#region Private methods

		#region Entities operations

		private IEnumerable<TimetableDataSet.LessonsRow> GetRemainingLessons(int classId)
		{
			return LessonsEnumerable
				.Where(l => l.ClassId == classId)
				.Where(l => !LessonsDistinctIdsEnumerable.Contains(l.Id));
		}

		private IEnumerable<TimetableDataSet.LessonsRow> GetRemainingLessons(string teacherPesel)
		{
			return LessonsEnumerable
				.Where(l => l.TeacherPesel == teacherPesel)
				.Where(l => !LessonsDistinctIdsEnumerable.Contains(l.Id));
		}

		private IEnumerable<TimetableDataSet.LessonsPlacesRow> GetLessonsPlaces(int classId)
		{
			return LessonsPlacesEnumerable
				.Where(lp => lp.LessonsRow.ClassId == classId);
		}

		private IEnumerable<TimetableDataSet.LessonsPlacesRow> GetLessonsPlaces(string teacherPesel)
		{
			return LessonsPlacesEnumerable
				.Where(lp => lp.LessonsRow.TeacherPesel == teacherPesel);
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

		private void RefreshDatabaseObjects(ComboBoxContentType changedDataType)
		{
			switch (changedDataType)
			{
				case ComboBoxContentType.Students:
					studentsTableAdapter.Fill(timetableDataSet.Students);
					break;
				case ComboBoxContentType.Teachers:
					teachersTableAdapter.Fill(timetableDataSet.Teachers);
					break;
				case ComboBoxContentType.Classes:
					classesTableAdapter.Fill(timetableDataSet.Classes);
					break;
				case ComboBoxContentType.Classrooms:
					classroomsTableAdapter.Fill(timetableDataSet.Classrooms);
					break;
				case ComboBoxContentType.Subjects:
					subjectsTableAdapter.Fill(timetableDataSet.Subjects);
					break;
				case ComboBoxContentType.Lessons:
					lessonsTableAdapter.Fill(timetableDataSet.Lessons);
					break;
				case ComboBoxContentType.LessonsPlaces:
					lessonsPlacesTableAdapter.Fill(timetableDataSet.LessonsPlaces);
					break;
			}
		}

		#endregion

		#region Main view controls

		private void FillComboBoxes()
		{
			comboBox.Items.Add(ComboBoxContentType.Students.ToString());
			comboBox.Items.Add(ComboBoxContentType.Teachers.ToString());
			comboBox.Items.Add(ComboBoxContentType.Classes.ToString());
			comboBox.Items.Add(ComboBoxContentType.Subjects.ToString());
			comboBox.SelectedIndex = 0;

			comboBoxPlanning1.Items.Add(ComboBoxContentType.Classes.ToString());
			comboBoxPlanning1.Items.Add(ComboBoxContentType.Teachers.ToString());
			comboBoxPlanning1.SelectedIndex = 0;

			comboBoxSummary1.Items.Add(ComboBoxContentType.Classes.ToString());
			comboBoxSummary1.Items.Add(ComboBoxContentType.Teachers.ToString());
			comboBoxSummary1.SelectedIndex = 0;

			_comboBoxContentType = ComboBoxContentType.Students;
			_comboBoxPlanningContentType = ComboBoxContentType.Classes;
			_comboBoxSummaryContentType = ComboBoxContentType.Classes;
		}

		private void FillExpanders()
		{
			var stackPanel = new StackPanel();
			stackPanel.Children.Add(new ExpanderControl(this, ExpanderControlType.Add, ExpanderControlType.Add.ToString()));
			stackPanel.Children.Add(new ExpanderControl(this, ExpanderControlType.Change, ExpanderControlType.Change.ToString()));
			stackPanel.Children.Add(new ExpanderControl(this, ExpanderControlType.Remove, ExpanderControlType.Remove.ToString()));
			expander.Content = stackPanel;
			expander.Header = "Operation";

			var stackPanelExport = new StackPanel();
			stackPanelExport.Children.Add(new ExpanderControl(this, ExpanderControlType.XLS, "Excel"));
			stackPanelExport.Children.Add(new ExpanderControl(this, ExpanderControlType.PDF, "PDF"));
			expanderExport.Content = stackPanelExport;
			expanderExport.Header = "Export";
		}

		private ComboBoxContentType GetComboBoxContentType()
		{
			ComboBoxContentType contentType = ComboBoxContentType.Entities;

			switch (comboBox.SelectedIndex)
			{
				case 0:
					contentType = ComboBoxContentType.Students;
					break;
				case 1:
					contentType = ComboBoxContentType.Teachers;
					break;
				case 2:
					contentType = ComboBoxContentType.Classes;
					break;
				case 3:
					contentType = ComboBoxContentType.Subjects;
					break;
			}

			return contentType;
		}

		private ComboBoxContentType GetComboBoxPlanningContentType()
		{
			ComboBoxContentType contentType = ComboBoxContentType.Entities;

			switch (comboBoxPlanning1.SelectedIndex)
			{
				case 0:
					contentType = ComboBoxContentType.Classes;
					break;
				case 1:
					contentType = ComboBoxContentType.Teachers;
					break;
			}

			return contentType;
		}

		private ComboBoxContentType GetComboBoxSummaryContentType()
		{
			ComboBoxContentType contentType = ComboBoxContentType.Entities;

			switch (comboBoxSummary1.SelectedIndex)
			{
				case 0:
					contentType = ComboBoxContentType.Classes;
					break;
				case 1:
					contentType = ComboBoxContentType.Teachers;
					break;
			}

			return contentType;
		}

		private void RefreshMainCategoryView()
		{
			if (!_windowLoaded)
				return;

			switch (_mainTabControlType)
			{
				case MainTabControlType.Management:
					gridOperations.Visibility = Visibility.Visible;
					gridOperationsComboBox.Visibility = Visibility.Visible;
					expander.Visibility = Visibility.Visible;
					expanderExport.Visibility = Visibility.Hidden;
					gridSummaryFilter.Visibility = Visibility.Hidden;
					RefreshManagementCategoryView();
					break;
				case MainTabControlType.Mapping:
					gridOperations.Visibility = Visibility.Visible;
					gridOperationsComboBox.Visibility = Visibility.Hidden;
					expander.Visibility = Visibility.Visible;
					expanderExport.Visibility = Visibility.Hidden;
					gridSummaryFilter.Visibility = Visibility.Hidden;
					RefreshMappingCategoryView();
					break;
				case MainTabControlType.Planning:
					gridOperations.Visibility = Visibility.Hidden;
					gridSummaryFilter.Visibility = Visibility.Hidden;
					RefreshPlanningCategoryView();
					break;
				case MainTabControlType.Summary:
					gridOperations.Visibility = Visibility.Visible;
					gridOperationsComboBox.Visibility = Visibility.Hidden;
					expander.Visibility = Visibility.Hidden;
					expanderExport.Visibility = Visibility.Visible;
					gridSummaryFilter.Visibility = Visibility.Visible;
					RefreshSummaryCategoryView();
					break;
			}
		}

		private void RefreshOtherCategoriesViews(ComboBoxContentType changedDataType)
		{
			if (!_windowLoaded)
				return;

			switch (changedDataType)
			{
				case ComboBoxContentType.Teachers:
				case ComboBoxContentType.Classes:
				case ComboBoxContentType.Classrooms:
				case ComboBoxContentType.Subjects:
				case ComboBoxContentType.Lessons:
				case ComboBoxContentType.LessonsPlaces:
					if (_mainTabControlType != MainTabControlType.Planning)
					{
						RefreshPlanningCategoryView(true);

					}
					if (_mainTabControlType != MainTabControlType.Summary)
					{
						RefreshSummaryCategoryView(true);

					}
					break;
			}
		}

		private void RefreshManagementCategoryView()
		{
			if (!_windowLoaded)
				return;

			FillScrollViewerGrid(_comboBoxContentType);
		}

		private void RefreshMappingCategoryView()
		{
			if (!_windowLoaded)
				return;

			FillScrollViewerGrid(ComboBoxContentType.Lessons);
		}

		private void RefreshPlanningCategoryView(bool forceRefresh = false)
		{
			if (!_windowLoaded)
				return;

			FillTimetableComboBox(MainTabControlType.Planning);

			if (forceRefresh)
				RefreshPlanningEntityView();
		}

		private void RefreshPlanningEntityView()
		{
			if (!_windowLoaded)
				return;

			FillTimetableGrid(MainTabControlType.Planning);
		}

		private void RefreshSummaryCategoryView(bool forceRefresh = false)
		{
			if (!_windowLoaded)
				return;

			FillTimetableComboBox(MainTabControlType.Summary);

			if (forceRefresh)
				RefreshSummaryEntityView();
		}

		private void RefreshSummaryEntityView()
		{
			if (!_windowLoaded)
				return;

			FillTimetableGrid(MainTabControlType.Summary);
		}

		#endregion

		#region Management & Mapping

		private Grid GetCurrentScrollViewersGrid()
		{
			Grid grid = null;

			switch (_mainTabControlType)
			{
				case MainTabControlType.Management:
					grid = scrollViewersManagementGrid;
					break;
				case MainTabControlType.Mapping:
					grid = scrollViewersMappingGrid;
					break;
			}

			return grid;
		}

		private void ClearScrollViewerGrid()
		{
			Grid grid = GetCurrentScrollViewersGrid();

			if (grid.Children.Count > 0)
			{
				grid.Children.Clear();
				grid.RowDefinitions.Clear();
			}
		}

		private void FillScrollViewerGrid(ComboBoxContentType contentType = ComboBoxContentType.Entities)
		{
			ClearScrollViewerGrid();

			switch (contentType)
			{
				case ComboBoxContentType.Students:
					AddAllStudentsToGrid();
					break;
				case ComboBoxContentType.Teachers:
					AddAllTeachersToGrid();
					break;
				case ComboBoxContentType.Classes:
					AddAllClassesToGrid();
					break;
				case ComboBoxContentType.Subjects:
					AddAllSubjectsToGrid();
					break;
				case ComboBoxContentType.Lessons:
					AddAllLessonsToGrid();
					break;
			}
		}

		#endregion

		#region Planning & Summary

		private ComboBox GetEntityComboBox(MainTabControlType controlType)
		{
			ComboBox entityComboBox = null;

			switch (controlType)
			{
				case MainTabControlType.Planning:
					entityComboBox = comboBoxPlanning2;
					break;
				case MainTabControlType.Summary:
					entityComboBox = comboBoxSummary2;
					break;
			}

			return entityComboBox;
		}

		private ComboBoxContentType GetEntityComboBoxContentType(MainTabControlType controlType)
		{
			ComboBoxContentType contentType = ComboBoxContentType.Entities;

			switch (controlType)
			{
				case MainTabControlType.Planning:
					contentType = _comboBoxPlanningContentType;
					break;
				case MainTabControlType.Summary:
					contentType = _comboBoxSummaryContentType;
					break;
			}

			return contentType;
		}

		private void FillTimetableComboBox(MainTabControlType controlType)
		{
			ComboBox entityComboBox = GetEntityComboBox(controlType);
			ComboBoxContentType contentType = GetEntityComboBoxContentType(controlType);

			switch (contentType)
			{
				case ComboBoxContentType.Classes:
					entityComboBox.ItemsSource = ClassessFriendlyNamesEnumerable;
					entityComboBox.SelectedIndex = entityComboBox.Items.Count > 0 ? 0 : -1;
					break;
				case ComboBoxContentType.Teachers:
					entityComboBox.ItemsSource = TeachersFriendlyNamesEnumerable;
					entityComboBox.SelectedIndex = entityComboBox.Items.Count > 0 ? 0 : -1;
					break;
			}
		}

		private void ClearRemainingLessonsGrid(Grid grid)
		{
			if (grid.Children.Count > 0)
			{
				grid.Children.Clear();
				grid.RowDefinitions.Clear();
			}
		}
		private void ClearLessonsPlacesGrid(Grid grid, int tilesToSkip)
		{
			if (grid.Children.Count > tilesToSkip)
			{
				grid.Children.RemoveRange(tilesToSkip, grid.Children.Count - tilesToSkip);
			}
		}

		private void ClearTimetableGrid(MainTabControlType controlType)
		{
			var tilesToSkip = timetableDataSet.Days.Count + timetableDataSet.Hours.Count;

			switch (controlType)
			{
				case MainTabControlType.Planning:
					ClearRemainingLessonsGrid(gridPlanningRemainingLessons);
					ClearLessonsPlacesGrid(gridPlanning, tilesToSkip);
					break;
				case MainTabControlType.Summary:
					ClearLessonsPlacesGrid(gridSummary, tilesToSkip);
					break;
			}

			PrepareTimetableGrid(controlType);
		}

		private void PrepareTimetableGrid(MainTabControlType controlType)
		{
			ComboBox entityComboBox = GetEntityComboBox(controlType);
			ComboBoxContentType contentType = GetEntityComboBoxContentType(controlType);

			switch (contentType)
			{
				case ComboBoxContentType.Classes:
					var currentPlanningClassId = ClassesEnumerable.ElementAt(entityComboBox.SelectedIndex).Id;
					AddAllEmptyLessonsToGrid(currentPlanningClassId, controlType);
					break;
				case ComboBoxContentType.Teachers:
					var currentPlanningTeacherPesel = TeachersEnumerable.ElementAt(entityComboBox.SelectedIndex).Pesel;
					AddAllEmptyLessonsToGrid(currentPlanningTeacherPesel, controlType);
					break;
			}
		}

		private void FillTimetableGrid(MainTabControlType controlType)
		{
			ClearTimetableGrid(controlType);

			ComboBox entityComboBox = GetEntityComboBox(controlType);
			ComboBoxContentType contentType = GetEntityComboBoxContentType(controlType);

			int selectedIndex = entityComboBox.SelectedIndex;

			if (selectedIndex < 0)
				return;

			switch (contentType)
			{
				case ComboBoxContentType.Classes:
					var currentClassId = ClassesEnumerable.ElementAt(selectedIndex).Id;

					if (controlType == MainTabControlType.Planning)
						AddAllRemainingLessonsToGrid(currentClassId, controlType);

					AddAllLessonsPlacesToGrid(currentClassId, controlType);
					break;
				case ComboBoxContentType.Teachers:
					var currentPlanningTeacherPesel = TeachersEnumerable.ElementAt(selectedIndex).Pesel;

					if (controlType == MainTabControlType.Planning)
						AddAllRemainingLessonsToGrid(currentPlanningTeacherPesel, controlType);

					AddAllLessonsPlacesToGrid(currentPlanningTeacherPesel, controlType);
					break;
			}
		}

		#endregion

		#region Management & Mapping - subcontrols

		private void AddControlToGrid<TControl, TRow>(TRow dataRow)
		{
			Control control = (Control) Activator.CreateInstance(typeof(TControl), dataRow);
			int height = (int) typeof(TControl).GetField("HEIGHT").GetValue(null);

			Grid grid = GetCurrentScrollViewersGrid();
			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(height) });
			Grid.SetRow(control, grid.RowDefinitions.Count - 1);
			grid.Children.Add(control);
		}

		private void AddAllStudentsToGrid()
		{
			foreach (TimetableDataSet.StudentsRow studentRow in StudentsEnumerable)
				AddControlToGrid<PersonControl, TimetableDataSet.StudentsRow>(studentRow);
		}

		private void AddAllTeachersToGrid()
		{
			foreach (TimetableDataSet.TeachersRow teacherRow in TeachersEnumerable)
				AddControlToGrid<PersonControl, TimetableDataSet.TeachersRow>(teacherRow);
		}

		private void AddAllClassesToGrid()
		{
			foreach (TimetableDataSet.ClassesRow classRow in ClassesEnumerable)
				AddControlToGrid<ClassControl, TimetableDataSet.ClassesRow>(classRow);
		}

		private void AddAllSubjectsToGrid()
		{
			foreach (TimetableDataSet.SubjectsRow subjectRow in SubjectsEnumerable)
				AddControlToGrid<SubjectControl, TimetableDataSet.SubjectsRow>(subjectRow);
		}

		private void AddAllLessonsToGrid()
		{
			foreach (TimetableDataSet.LessonsRow lessonRow in LessonsEnumerable)
				AddControlToGrid<LessonControl, TimetableDataSet.LessonsRow>(lessonRow);
		}

		#endregion

		#region Planning & Summary - subcontrols

		private void AddAllEmptyLessonsToGrid(object objectId, MainTabControlType controlType)
		{
			for (var i = 1; i <= timetableDataSet.Days.Count; i++)
			{
				for (var j = 1; j <= timetableDataSet.Hours.Count; j++)
				{
					var cellControl = new CellControl(this, j % 2 != 0);
					Grid.SetColumn(cellControl, i);
					Grid.SetRow(cellControl, j);

					if (controlType == MainTabControlType.Planning)
					{
						if (objectId is int)
						{
							cellControl.SetLessonData(ExpanderControlType.Add, (int) objectId, i, j);
						}
						else if (objectId is string)
						{
							cellControl.SetLessonData(ExpanderControlType.Add, (string) objectId, i, j);
						}

						gridPlanning.Children.Add(cellControl);
					}
					else
					{
						gridSummary.Children.Add(cellControl);
					}
				}
			}
		}

		private void AddAllRemainingLessonsToGrid(int classId, MainTabControlType controlType)
		{
			var remainingLessons = GetRemainingLessons(classId);

			foreach (var lesson in remainingLessons)
				AddLessonToGrid(lesson, controlType, ComboBoxContentType.Classes);
		}

		private void AddAllRemainingLessonsToGrid(string teacherPesel, MainTabControlType controlType)
		{
			var remainingLessons = GetRemainingLessons(teacherPesel);

			foreach (var lesson in remainingLessons)
				AddLessonToGrid(lesson, controlType, ComboBoxContentType.Classes);
		}

		private void AddAllLessonsPlacesToGrid(int classId, MainTabControlType controlType)
		{
			var allLessonsPlaces = GetLessonsPlaces(classId);

			foreach (var lessonPlace in allLessonsPlaces)
				AddLessonPlaceToGrid(lessonPlace, controlType, ComboBoxContentType.Classes);
		}

		private void AddAllLessonsPlacesToGrid(string teacherPesel, MainTabControlType controlType)
		{
			var allLessonsPlaces = GetLessonsPlaces(teacherPesel);

			foreach (var lessonPlace in allLessonsPlaces)
				AddLessonPlaceToGrid(lessonPlace, controlType, ComboBoxContentType.Teachers);
		}

		private void AddLessonToGrid(TimetableDataSet.LessonsRow lessonRow, MainTabControlType controlType, ComboBoxContentType contentType)
		{
			var cellControl = new CellControl(
				firstRow: lessonRow.SubjectsRow.Name,
				secondRow: lessonRow.ClassesRow.ToFriendlyString(),
				thirdRow: lessonRow.TeachersRow.ToFriendlyString(),
				mainWindow: this,
				diffColor: gridPlanningRemainingLessons.RowDefinitions.Count % 2 != 0
			);

			gridPlanningRemainingLessons.RowDefinitions.Add(new RowDefinition());
			Grid.SetRow(cellControl, gridPlanningRemainingLessons.RowDefinitions.Count - 1);
			gridPlanningRemainingLessons.Children.Add(cellControl);
		}

		private void AddLessonPlaceToGrid(TimetableDataSet.LessonsPlacesRow lessonPlace, MainTabControlType controlType, ComboBoxContentType contentType)
		{
			var cellControl = new CellControl(
				firstRow: lessonPlace.LessonsRow.SubjectsRow.Name,
				secondRow: (contentType == ComboBoxContentType.Classes)
								? lessonPlace.LessonsRow.TeachersRow.ToFriendlyString()
								: $"kl. {lessonPlace.LessonsRow.ClassesRow.ToFriendlyString()}",
				thirdRow: $"s. {lessonPlace.ClassroomsRow.Name}",
				mainWindow: this,
				diffColor: lessonPlace.HoursRow.Id % 2 != 0
			);

			Grid.SetColumn(cellControl, lessonPlace.DaysRow.Id);
			Grid.SetRow(cellControl, lessonPlace.HoursRow.Id);

			switch (controlType)
			{
				case MainTabControlType.Planning:
					if (_comboBoxPlanningContentType == ComboBoxContentType.Classes)
						cellControl.SetLessonData(ExpanderControlType.Change, lessonPlace.LessonsRow.ClassId, lessonPlace.DayId, lessonPlace.HourId);
					else if (_comboBoxPlanningContentType == ComboBoxContentType.Teachers)
						cellControl.SetLessonData(ExpanderControlType.Change, lessonPlace.LessonsRow.TeacherPesel, lessonPlace.DayId, lessonPlace.HourId);

					gridPlanning.Children.Add(cellControl);
					break;
				case MainTabControlType.Summary:
					gridSummary.Children.Add(cellControl);
					break;
			}
		}

		#endregion

		#endregion
	}
}
