using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Timetable.Controls;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>MainWindow</c>.
		/// </summary>
		public MainWindow()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		/// <summary>
		/// Metoda zwracająca informację o aktualnie wyświetlanej grupie encji.
		/// </summary>
		/// <returns></returns>
		public ComboBoxContent GetCurrentCoboBoxContent() => comboBoxContent;

		/// <summary>
		/// Metoda odświeżająca listę aktualnie wyświetlanych encji.
		/// </summary>
		public void RefreshCurrentView()
		{
			this.FillScrollViewer(comboBoxContent);
		}

		/// <summary>
		/// Metoda zwracająca listę numerów PESEL zaznaczonych osób.
		/// </summary>
		/// <returns></returns>
		public ICollection<string> GetPeselNumbersOfMarkedPeople()
		{
			var markedPesels = new List<string>();

			foreach (PersonControl personControl in this.scrollViewersGrid.Children)
			{
				if (personControl.IsChecked())
				{
					markedPesels.Add(personControl.Pesel.StringRepresentation);
				}
			}

			return markedPesels;
		}

		/// <summary>
		/// Metoda zwracająca listę numerów ID zaznaczonych klas.
		/// </summary>
		/// <returns></returns>
		public ICollection<string> GetIdNumbersOfMarkedClasses()
		{
			var markedIds = new List<string>();

			foreach (ClassControl classControl in this.scrollViewersGrid.Children)
			{
				if (classControl.IsChecked())
				{
					markedIds.Add(classControl.GetId());
				}
			}

			return markedIds;
		}

		/// <summary>
		/// Metoda zwracająca listę numerów ID zaznaczonych przedmiotów.
		/// </summary>
		/// <returns></returns>
		public ICollection<string> GetIdNumbersOfMarkedSubjects()
		{
			var markedIds = new List<string>();

			foreach (SubjectControl subjectControl in this.scrollViewersGrid.Children)
			{
				if (subjectControl.IsChecked())
				{
					markedIds.Add(subjectControl.GetId());
				}
			}

			return markedIds;
		}

		#endregion

		#region Properties

		#endregion

		#region Private methods

		private void InitDatabaseObjects()
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
		}

		private void FillScrollViewer(ComboBoxContent content = ComboBoxContent.Entities)
		{
			if (this.scrollViewersGrid.Children.Count > 0)
			{
				this.scrollViewersGrid.Children.Clear();
				this.scrollViewersGrid.RowDefinitions.Clear();
			}

			switch (content)
			{
				case ComboBoxContent.Teachers:
					teachersTableAdapter.Fill(timetableDataSet.Teachers);
					foreach (TimetableDataSet.TeachersRow teacherRow in timetableDataSet.Teachers.Rows)
					{
						this.AddPersonToGrid(teacherRow);
					}
					break;
				case ComboBoxContent.Classes:
					classesTableAdapter.Fill(timetableDataSet.Classes);
					foreach (TimetableDataSet.ClassesRow classRow in timetableDataSet.Classes.Rows)
					{
						this.AddClassToGrid(classRow);
					}
					break;
				case ComboBoxContent.Subjects:
					subjectsTableAdapter.Fill(timetableDataSet.Subjects);
					foreach (TimetableDataSet.SubjectsRow subjectRow in timetableDataSet.Subjects.Rows)
					{
						this.AddSubjectToGrid(subjectRow);
					}
					break;
				default:
					studentsTableAdapter.Fill(timetableDataSet.Students);
					foreach (TimetableDataSet.StudentsRow studentRow in timetableDataSet.Students.Rows)
					{
						this.AddPersonToGrid(studentRow);
					}
					break;
			}
		}

		private void FillExpander(ExpanderContent content)
		{
			var stackPanel = new StackPanel();
			switch (content)
			{
				case ExpanderContent.Management:
					stackPanel.Children.Add(new ExpanderControl(ExpanderControlType.Add.ToString(), ExpanderControlType.Add, this));
					stackPanel.Children.Add(new ExpanderControl(ExpanderControlType.Change.ToString(), ExpanderControlType.Change, this));
					stackPanel.Children.Add(new ExpanderControl(ExpanderControlType.Remove.ToString(), ExpanderControlType.Remove, this));
					this.expander.Content = stackPanel;
					this.expander.Header = "Operation";
					break;
				case ExpanderContent.Summary:
					stackPanel.Children.Add(new ExpanderControl("Excel", ExpanderControlType.XLSX, this));
					stackPanel.Children.Add(new ExpanderControl("PDF", ExpanderControlType.PDF, this));
					this.expander.Content = stackPanel;
					this.expander.Header = "Export";
					break;
			}
		}

		private void FillComboBoxes()
		{
			this.comboBox.Items.Add(ComboBoxContent.Students.ToString());
			this.comboBox.Items.Add(ComboBoxContent.Teachers.ToString());
			this.comboBox.Items.Add(ComboBoxContent.Classes.ToString());
			this.comboBox.Items.Add(ComboBoxContent.Subjects.ToString());
			this.comboBox.SelectedIndex = 0;

			this.comboBoxPlanning1.Items.Add(ComboBoxContent.Teachers.ToString());
			this.comboBoxPlanning1.Items.Add(ComboBoxContent.Classes.ToString());
			this.comboBoxPlanning1.SelectedIndex = 0;

			this.comboBoxContent = ComboBoxContent.Students;
			this.comboBoxPlanningContent = ComboBoxContent.Teachers;
		}

		private void FillTimetableGrid()
		{
			for (int i = 1; i <= timetableDataSet.Days.Count; i++)
			{
				for (int j = 1; j <= timetableDataSet.Hours.Count; j++)
				{
					var cellControl = new CellControl(diffColor: (j % 2) != 0);
					Grid.SetColumn(cellControl, i);
					Grid.SetRow(cellControl, j);
					this.gridPlanning.Children.Add(cellControl);
				}
			}
		}

		private void ClearTimetableGrids(TabType planning)
		{
			switch (planning)
			{
				case TabType.Planning:
					var tilesToSkip = timetableDataSet.Days.Count + timetableDataSet.Hours.Count;
					if (this.gridPlanning.Children.Count > tilesToSkip)
					{
						this.gridPlanning.Children.RemoveRange(tilesToSkip, this.gridPlanning.Children.Count - tilesToSkip);
					}
					if (this.gridPlanningRemainingLessons.Children.Count > 0)
					{
						this.gridPlanningRemainingLessons.Children.Clear();
						this.gridPlanningRemainingLessons.RowDefinitions.Clear();
					}
					this.FillTimetableGrid();
					break;
				case TabType.Summary:
					foreach (var children in this.gridSummary.Children)
					{
						if (children is CellControl)
						{
							this.gridSummary.Children.Remove(children as UIElement);
						}
					}
					break;
			}
		}

		private void AddPersonToGrid(TimetableDataSet.StudentsRow studentRow)
		{
			var personControl = new PersonControl(studentRow);

			this.scrollViewersGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(PersonControl.HEIGHT) });
			Grid.SetRow(personControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(personControl);
		}

		private void AddPersonToGrid(TimetableDataSet.TeachersRow teacherRow)
		{
			var personControl = new PersonControl(teacherRow);

			this.scrollViewersGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(PersonControl.HEIGHT) });
			Grid.SetRow(personControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(personControl);
		}

		private void AddClassToGrid(TimetableDataSet.ClassesRow classRow)
		{
			var classControl = new ClassControl(classRow);

			this.scrollViewersGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(ClassControl.HEIGHT) });
			Grid.SetRow(classControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(classControl);
		}

		private void AddSubjectToGrid(TimetableDataSet.SubjectsRow subjectRow)
		{
			var subjectControl = new SubjectControl(subjectRow);

			this.scrollViewersGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(ClassControl.HEIGHT) });
			Grid.SetRow(subjectControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(subjectControl);
		}

		private void AddLessonToGrid(TimetableDataSet.LessonsRow lesson)
		{
			var cellControl = new CellControl(
				lesson.SubjectsRow.Name,
				(lesson.ClassesRow.Year.ToString()) + (string.IsNullOrEmpty(lesson.ClassesRow.CodeName)
					? string.Empty
					: $" ({lesson.ClassesRow.CodeName})"),
				$"{lesson.TeachersRow.FirstName.First()}.{lesson.TeachersRow.LastName}",
				diffColor: (this.gridPlanningRemainingLessons.RowDefinitions.Count % 2) != 0
			);

			this.gridPlanningRemainingLessons.RowDefinitions.Add(new RowDefinition());
			Grid.SetRow(cellControl, this.gridPlanningRemainingLessons.RowDefinitions.Count - 1);
			this.gridPlanningRemainingLessons.Children.Add(cellControl);
		}

		private void AddLessonPlaceToGrid(
			TimetableDataSet.LessonsPlacesRow lessonPlace,
			TabType tabType,
			ComboBoxContent content)
		{
			var cellControl = new CellControl(
				lessonPlace.LessonsRow.SubjectsRow.Name,
				content == ComboBoxContent.Students
					? $"{lessonPlace.LessonsRow.TeachersRow.FirstName.First()}.{lessonPlace.LessonsRow.TeachersRow.LastName}"
					: ($"kl. {lessonPlace.LessonsRow.ClassesRow.Year}") +
						(string.IsNullOrEmpty(lessonPlace.LessonsRow.ClassesRow.CodeName)
							? string.Empty
							: $" ({lessonPlace.LessonsRow.ClassesRow.CodeName})"),
				$"s.{lessonPlace.ClassroomsRow.Name}",
				diffColor: (lessonPlace.HoursRow.Id % 2) != 0
			);
			Grid.SetColumn(cellControl, lessonPlace.DaysRow.Id);
			Grid.SetRow(cellControl, lessonPlace.HoursRow.Id);
			switch (tabType)
			{
				case TabType.Planning:
					this.gridPlanning.Children.Add(cellControl);
					break;
				case TabType.Summary:
					this.gridSummary.Children.Add(cellControl);
					break;
			}
		}

		#endregion

		#region Events

		private void mainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			this.InitDatabaseObjects();

			this.FillComboBoxes();

			this.FillExpander(ExpanderContent.Management);

			this.FillTimetableGrid();
		}

		private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			switch ((TabType)this.tabControl.SelectedIndex)
			{
				case TabType.Management:
					this.gridSummaryFilter.Visibility = Visibility.Hidden;
					this.gridOperations.Visibility = Visibility.Visible;
					this.gridOperationsComboBox.Visibility = Visibility.Visible;
					this.expander.Visibility = Visibility.Visible;
					this.FillExpander(ExpanderContent.Management);
					break;
				case TabType.Mapping:
				case TabType.Planning:
					this.gridSummaryFilter.Visibility = Visibility.Hidden;
					this.gridOperations.Visibility = Visibility.Hidden;
					break;
				case TabType.Summary:
					this.gridSummaryFilter.Visibility = Visibility.Visible;
					this.gridOperations.Visibility = Visibility.Visible;
					this.gridOperationsComboBox.Visibility = Visibility.Hidden;
					this.expander.Visibility = Visibility.Visible;
					this.FillExpander(ExpanderContent.Summary);
					break;
			}
		}

		private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.comboBoxContent = (ComboBoxContent)((sender as ComboBox).SelectedIndex + 1);
			this.FillExpander(ExpanderContent.Management);

			switch (this.comboBoxContent)
			{
				case ComboBoxContent.Students:
					this.FillScrollViewer(ComboBoxContent.Students);
					break;
				case ComboBoxContent.Teachers:
					this.FillScrollViewer(ComboBoxContent.Teachers);
					break;
				case ComboBoxContent.Classes:
					this.FillScrollViewer(ComboBoxContent.Classes);
					break;
				case ComboBoxContent.Subjects:
					this.FillScrollViewer(ComboBoxContent.Subjects);
					break;
			}
		}

		#region TabPlanning

		private void comboBoxPlanning1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.comboBoxPlanningContent = (ComboBoxContent)((sender as ComboBox).SelectedIndex + 2);
			switch (this.comboBoxPlanningContent)
			{
				case ComboBoxContent.Classes:
					var classesList = timetableDataSet.Classes.
						OrderBy(c => c.Year);
					this.comboBoxPlanning2.ItemsSource = classesList.
						Select(c => (c.Year.ToString()) + (string.IsNullOrEmpty(c.CodeName) ? string.Empty : $" ({c.CodeName})"));
					this.comboBoxPlanning2.SelectedIndex = this.comboBoxPlanning2.Items.Count > 0 ? 0 : -1;
					break;
				case ComboBoxContent.Teachers:
					var teachersList = timetableDataSet.Teachers.
						OrderBy(t => t.Pesel);
					this.comboBoxPlanning2.ItemsSource = teachersList.
						Select(t => $"{t.FirstName[0]}.{t.LastName} ({t.Pesel})");
					this.comboBoxPlanning2.SelectedIndex = this.comboBoxPlanning2.Items.Count > 0 ? 0 : -1;
					break;
			}
		}
		private void comboBoxPlanning2_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.comboBoxPlanning2.SelectedIndex != -1)
			{
				switch (this.comboBoxPlanningContent)
				{
					case ComboBoxContent.Teachers:
						string currentPlanningTeacherPesel = timetableDataSet.Teachers.
							OrderBy(t => t.Pesel).
							ElementAt(this.comboBoxPlanning2.SelectedIndex).Pesel;

						this.ClearTimetableGrids(TabType.Planning);

						var teachersLessonsPlacesLessonsIds = timetableDataSet.LessonsPlaces.
							Select(lp => lp.LessonId);
						foreach (var lesson in timetableDataSet.Lessons.
							Where(l => l.TeacherPesel == currentPlanningTeacherPesel).
							Where(l => !teachersLessonsPlacesLessonsIds.Contains(l.Id)))
						{
							this.AddLessonToGrid(lesson);
						}
						foreach (var lessonPlace in timetableDataSet.LessonsPlaces.
							Where(lp => lp.LessonsRow.TeacherPesel == currentPlanningTeacherPesel))
						{
							this.AddLessonPlaceToGrid(lessonPlace, TabType.Planning, ComboBoxContent.Teachers);
						}
						break;
					case ComboBoxContent.Classes:
						int currentPlanningClassId = timetableDataSet.Classes.
							OrderBy(c => c.Year).
							ElementAt(this.comboBoxPlanning2.SelectedIndex).Id;

						this.ClearTimetableGrids(TabType.Planning);

						var studentsLessonsPlacesLessonsIds = timetableDataSet.LessonsPlaces.
							Select(lp => lp.LessonId);
						foreach (var lesson in timetableDataSet.Lessons.
							Where(l => l.ClassId == currentPlanningClassId).
							Where(l => !studentsLessonsPlacesLessonsIds.Contains(l.Id)))
						{
							this.AddLessonToGrid(lesson);
						}
						foreach (var lessonPlace in timetableDataSet.LessonsPlaces.
							Where(lp => lp.LessonsRow.ClassId == currentPlanningClassId))
						{
							this.AddLessonPlaceToGrid(lessonPlace, TabType.Planning, ComboBoxContent.Students);
						}
						break;
				}
			}
		}

		#endregion

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#region Database

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

		#endregion

		private ComboBoxContent
			comboBoxContent,
			comboBoxPlanningContent;

		#endregion
	}
}
