using System.Linq;
using System.Collections.Generic;

using Timetable.Controls;
using Timetable.Models;
using Timetable.Code;

namespace Timetable
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : System.Windows.Window
	{
		#region Constructors

		/// <summary>Konstruktor tworzący obiekt typu <c>MainWindow</c>.
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

			foreach (PersonControl person in this.scrollViewersGrid.Children)
			{
				if (person.IsChecked())
				{
					markedPesels.Add(person.GetPesel());
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

			foreach (ClassControl oClass in this.scrollViewersGrid.Children)
			{
				if (oClass.IsChecked())
				{
					markedIds.Add(oClass.GetId());
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

			foreach (SubjectControl subject in this.scrollViewersGrid.Children)
			{
				if (subject.IsChecked())
				{
					markedIds.Add(subject.GetId());
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
					foreach (Teacher teacher in Utilities.Database.GetTeachers())
					{
						this.AddPersonToGrid(teacher);
					}
					break;
				case ComboBoxContent.Classes:
					foreach (Class oClass in Utilities.Database.GetClasses())
					{
						this.AddClassToGrid(oClass);
					}
					break;
				case ComboBoxContent.Subjects:
					foreach (Subject subject in Utilities.Database.GetSubjects())
					{
						this.AddSubjectToGrid(subject);
					}
					break;
				default:
					foreach (Student student in Utilities.Database.GetStudents())
					{
						this.AddPersonToGrid(student);
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

		private void AddPersonToGrid(Models.Base.Person person)
		{
			var personControl = new PersonControl(person);

			this.scrollViewersGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new System.Windows.GridLength(PersonControl.HEIGHT) });
			System.Windows.Controls.Grid.SetRow(personControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(personControl);
		}

		private void AddClassToGrid(Class oClass)
		{
			var classControl = new ClassControl(oClass);

			this.scrollViewersGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new System.Windows.GridLength(ClassControl.HEIGHT) });
			System.Windows.Controls.Grid.SetRow(classControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(classControl);
		}

		private void AddSubjectToGrid(Subject subject)
		{
			var subjectControl = new SubjectControl(subject);

			this.scrollViewersGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new System.Windows.GridLength(ClassControl.HEIGHT) });
			System.Windows.Controls.Grid.SetRow(subjectControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(subjectControl);
		}

		#endregion

		#region Events

		private void mainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
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
					this.comboBoxPlanning2.ItemsSource = Utilities.Database.GetClasses().
						OrderBy(c => c.Year).
						Select(c => (c.Year.ToString()) + (string.IsNullOrEmpty(c.CodeName) ? string.Empty : $" ({c.CodeName})"));
					this.comboBoxPlanning2.SelectedIndex = this.comboBoxPlanning2.Items.Count > 0 ? 0 : -1;
					break;
				case ComboBoxContent.Teachers:
					this.comboBoxPlanning2.ItemsSource = Utilities.Database.GetTeachers().
						OrderBy(t => t.Pesel.BirthDate).
						Select(t => $"{t.FirstName[0]}.{t.LastName} ({t.Pesel})");
					this.comboBoxPlanning2.SelectedIndex = this.comboBoxPlanning2.Items.Count > 0 ? 0 : -1;
					break;
			}
		}
		private void comboBoxPlanning2_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			// TODO: Wyświetlić plan danej klasy / danego nauczyciela.
		}

		#endregion

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		private ComboBoxContent comboBoxContent = ComboBoxContent.Entities;

		#endregion
	}
}
