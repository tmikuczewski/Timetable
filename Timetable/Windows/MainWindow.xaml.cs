using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Timetable.Controls;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : System.Windows.Window
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
		public ComboBoxContent GetCurrentCoboBoxContent()
		{
			return comboBoxContent;
		}

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
					markedPesels.Add(personControl.GetPesel());
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
			var stackPanel = new System.Windows.Controls.StackPanel();
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
		}

		private void AddPersonToGrid(TimetableDataSet.StudentsRow studentRow)
		{
			var personControl = new PersonControl(studentRow);

			this.scrollViewersGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new System.Windows.GridLength(PersonControl.HEIGHT) });
			System.Windows.Controls.Grid.SetRow(personControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(personControl);
		}

		private void AddPersonToGrid(TimetableDataSet.TeachersRow teacherRow)
		{
			var personControl = new PersonControl(teacherRow);

			this.scrollViewersGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new System.Windows.GridLength(PersonControl.HEIGHT) });
			System.Windows.Controls.Grid.SetRow(personControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(personControl);
		}

		private void AddClassToGrid(TimetableDataSet.ClassesRow classRow)
		{
			var classControl = new ClassControl(classRow);

			this.scrollViewersGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new System.Windows.GridLength(ClassControl.HEIGHT) });
			System.Windows.Controls.Grid.SetRow(classControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(classControl);
		}

		private void AddSubjectToGrid(TimetableDataSet.SubjectsRow subjectRow)
		{
			var subjectControl = new SubjectControl(subjectRow);

			this.scrollViewersGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new System.Windows.GridLength(ClassControl.HEIGHT) });
			System.Windows.Controls.Grid.SetRow(subjectControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(subjectControl);
		}

		#endregion

		#region Events

		private void mainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
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

			this.FillComboBoxes();

			this.FillExpander(ExpanderContent.Management);
		}

		private void tabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			switch (this.tabControl.SelectedIndex)
			{
				case 0:
					this.gridSummaryFilter.Visibility = System.Windows.Visibility.Hidden;
					this.gridOperations.Visibility = System.Windows.Visibility.Visible;
					this.gridOperationsComboBox.Visibility = System.Windows.Visibility.Visible;
					this.expander.Visibility = System.Windows.Visibility.Visible;
					this.FillExpander(ExpanderContent.Management);
					break;
				case 1:
				case 2:
					this.gridSummaryFilter.Visibility = System.Windows.Visibility.Hidden;
					this.gridOperations.Visibility = System.Windows.Visibility.Hidden;
					break;
				case 3:
					this.gridSummaryFilter.Visibility = System.Windows.Visibility.Visible;
					this.gridOperations.Visibility = System.Windows.Visibility.Visible;
					this.gridOperationsComboBox.Visibility = System.Windows.Visibility.Hidden;
					this.expander.Visibility = System.Windows.Visibility.Visible;
					this.FillExpander(ExpanderContent.Summary);
					break;
			}
		}

		private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			this.comboBoxContent = (ComboBoxContent)((sender as System.Windows.Controls.ComboBox).SelectedIndex + 1);
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

		private void comboBoxPlanning1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			this.comboBoxContent = (ComboBoxContent)((sender as System.Windows.Controls.ComboBox).SelectedIndex + 2);
			switch (comboBoxContent)
			{
				case ComboBoxContent.Classes:
					List<TimetableDataSet.ClassesRow> classesList = timetableDataSet.Classes
						.OrderBy(c => c.Year)
						.ToList();
					this.comboBoxPlanning2.ItemsSource = classesList
						.Select(c => (c.Year.ToString()) + (string.IsNullOrEmpty(c.CodeName) ? string.Empty : $" ({c.CodeName})"));
					this.comboBoxPlanning2.SelectedIndex = this.comboBoxPlanning2.Items.Count > 0 ? 0 : -1;
					this.currentClassId = classesList.ElementAt(this.comboBoxPlanning2.SelectedIndex).Id;
					break;
				case ComboBoxContent.Teachers:
					this.comboBoxPlanning2.ItemsSource = timetableDataSet.Teachers
						.OrderBy(t => t.Pesel)
						.Select(t => $"{t.FirstName[0]}.{t.LastName} ({t.Pesel})");
					this.comboBoxPlanning2.SelectedIndex = this.comboBoxPlanning2.Items.Count > 0 ? 0 : -1;
					break;
			}
		}
		private void comboBoxPlanning2_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			List<TimetableDataSet.ClassesRow> classesList = timetableDataSet.Classes
				.OrderBy(c => c.Year)
				.ToList();
			int index = this.comboBoxPlanning2.Items.Count > 0 ? this.comboBoxPlanning2.SelectedIndex : -1;

			if (index >= 0)
			{
				this.currentClassId = classesList.ElementAt(index).Id;

				foreach (TimetableDataSet.LessonsPlacesRow lp
					in timetableDataSet.LessonsPlaces.Where(lp => lp.LessonsRow.ClassId == currentClassId))
				{
					MessageBox.Show(lp.LessonsRow.Id + " - "
					                + lp.LessonsRow.SubjectsRow.Name + " - "
					                + lp.LessonsRow.ClassesRow.Year + " "
					                + lp.LessonsRow.ClassesRow.CodeName + " - "
					                + lp.LessonsRow.TeachersRow.FirstName + " "
					                + lp.LessonsRow.TeachersRow.LastName + " - "
					                + lp.ClassroomsRow.Name + " - "
					                + lp.DaysRow.Name + " - "
					                + lp.HoursRow.Hour);
				}
			}

			// TODO: Wyświetlić plan danej klasy / danego nauczyciela.
		}

		#endregion

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		private ComboBoxContent comboBoxContent = ComboBoxContent.Entities;

		private int currentClassId;

		private int currentTeacherId;

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
	}
}
