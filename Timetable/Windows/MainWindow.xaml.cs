using Timetable.Controls;
using Timetable.Models;
using Timetable.Code;
using System;
using System.Collections.Generic;

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
				default:
					foreach (Student student in Utilities.Database.GetStudents())
					{
						this.AddPersonToGrid(student);
					}
					break;
			}
		}

		private void FillExpander(ComboBoxContent content = ComboBoxContent.Entities)
		{
			switch (content)
			{
				default:
					{
						var stackPanel = new System.Windows.Controls.StackPanel();
						stackPanel.Children.Add(new ExpanderControl(ExpanderControlType.Add.ToString(), ExpanderControlType.Add, this));
						stackPanel.Children.Add(new ExpanderControl(ExpanderControlType.Change.ToString(), ExpanderControlType.Change, this));
						stackPanel.Children.Add(new ExpanderControl(ExpanderControlType.Remove.ToString(), ExpanderControlType.Remove, this));
						this.expander.Content = stackPanel;
					}
					break;
			}
		}

		private void FillComboBox(ComboBoxContent content = ComboBoxContent.Entities)
		{
			switch (content)
			{
				default:
					this.comboBox.Items.Add(ComboBoxContent.Students.ToString());
					this.comboBox.Items.Add(ComboBoxContent.Teachers.ToString());
					// this.comboBox.Items.Add(ComboBoxContent.Classes.ToString());
					// this.comboBox.Items.Add(ComboBoxContent.Subjects.ToString());
					this.comboBox.SelectedIndex = 0;
					break;
			}
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

		#endregion

		#region Events

		private void mainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			this.FillComboBox();

			this.FillExpander();
		}

		private void tabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			switch (this.tabControl.SelectedIndex)
			{
				case 0:
					this.gridOperations.Visibility = System.Windows.Visibility.Visible;
					this.buttonExport.Visibility = System.Windows.Visibility.Hidden;
					this.expander.Visibility = System.Windows.Visibility.Visible;
					break;
				case 3:
					this.gridOperations.Visibility = System.Windows.Visibility.Visible;
					this.expander.Visibility = System.Windows.Visibility.Hidden;
					this.buttonExport.Visibility = System.Windows.Visibility.Visible;
					break;
				case 1:
				case 2:
				default:
					this.gridOperations.Visibility = System.Windows.Visibility.Hidden;
					break;
			}
		}

		private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			this.comboBoxContent = (ComboBoxContent)((sender as System.Windows.Controls.ComboBox).SelectedIndex + 1);
			this.FillExpander(ComboBoxContent.Students);
			
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

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		private ComboBoxContent comboBoxContent = ComboBoxContent.Entities;

		#endregion
	}
}
