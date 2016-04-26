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

        public void RefreshMapping()
        {
            this.FillScrollViewer(ComboBoxContent.Mapping);
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

        /// <summary>
        /// Metoda zwracająca listę numerów ID zaznaczonych lekcji.
        /// </summary>
        /// <returns></returns>
        public ICollection<string> GetIdNumbersOfMarkedLessons()
        {
            var markedIds = new List<string>();

            foreach (LessonControl lessonControl in this.scrollViewersMappingGrid.Children)
            {
                if (lessonControl.IsChecked())
                {
                    markedIds.Add(lessonControl.GetId());
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
            if (this.scrollViewersMappingGrid.Children.Count > 0)
            {
                this.scrollViewersMappingGrid.Children.Clear();
                this.scrollViewersMappingGrid.RowDefinitions.Clear();
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
                case ComboBoxContent.Mapping:
                    lessonsTableAdapter.Fill(timetableDataSet.Lessons);
                    foreach (TimetableDataSet.LessonsRow lessonRow in timetableDataSet.Lessons.Rows)
                    {
                        this.AddLessonToGrid(lessonRow);
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

        private void AddLessonToGrid(TimetableDataSet.LessonsRow lessonRow)
        {
            var lessonControl = new LessonControl(lessonRow);

            this.scrollViewersMappingGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new System.Windows.GridLength(LessonControl.HEIGHT) });
            System.Windows.Controls.Grid.SetRow(lessonControl, this.scrollViewersMappingGrid.RowDefinitions.Count - 1);
            this.scrollViewersMappingGrid.Children.Add(lessonControl);
        }

        #endregion

        #region Events

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitDatabaseObjects();

            this.FillComboBoxes();

			this.FillExpander(ExpanderContent.Management);
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
                    this.gridSummaryFilter.Visibility =Visibility.Hidden;
                    this.gridOperations.Visibility =Visibility.Visible;
                    this.gridOperationsComboBox.Visibility = Visibility.Hidden;                    this.expander.Visibility =Visibility.Visible;
                    this.FillExpander(ExpanderContent.Management);
                    this.FillScrollViewer(ComboBoxContent.Mapping);
                    break;
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
