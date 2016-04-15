using System.Windows;

using Timetable.Controls;
using Timetable.Models;
using Timetable.Utilities.Enums;

namespace Timetable
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
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
						stackPanel.Children.Add(new ExpanderControl(ExpanderControlType.Add.ToString(), ExpanderControlType.Add));
						stackPanel.Children.Add(new ExpanderControl(ExpanderControlType.Change.ToString(), ExpanderControlType.Change));
						stackPanel.Children.Add(new ExpanderControl(ExpanderControlType.Remove.ToString(), ExpanderControlType.Remove));
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
					this.comboBox.Items.Add(ComboBoxContent.Classes.ToString());
					this.comboBox.Items.Add(ComboBoxContent.Subjects.ToString());
					this.comboBox.SelectedIndex = 0;
					break;
			}
		}

		private void AddPersonToGrid(Models.Base.Person person)
		{
			var personControl = new PersonControl(person);

			this.scrollViewersGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new GridLength(PersonControl.HEIGHT) });
			System.Windows.Controls.Grid.SetRow(personControl, this.scrollViewersGrid.RowDefinitions.Count - 1);
			this.scrollViewersGrid.Children.Add(personControl);
		}

		#endregion

		#region Events

		private void mainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			this.FillComboBox();

			this.FillExpander();
		}

		private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			this.comboBoxContent = (ComboBoxContent)((sender as System.Windows.Controls.ComboBox).SelectedIndex + 1);

			switch (this.comboBoxContent)
			{
				case ComboBoxContent.Teachers:
					this.FillScrollViewer(ComboBoxContent.Teachers);
					this.FillExpander(ComboBoxContent.Teachers);
					break;
				case ComboBoxContent.Classes:
					this.FillScrollViewer(ComboBoxContent.Classes);
					this.FillExpander(ComboBoxContent.Classes);
					break;
				case ComboBoxContent.Subjects:
					this.FillScrollViewer(ComboBoxContent.Subjects);
					this.FillExpander(ComboBoxContent.Subjects);
					break;
				default:
					this.FillScrollViewer(ComboBoxContent.Students);
					this.FillExpander(ComboBoxContent.Students);
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
