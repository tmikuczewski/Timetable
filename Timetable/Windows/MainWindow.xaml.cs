using System.Windows;

using Timetable.Controls;
using Timetable.Models;

namespace Timetable
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Constructors

		/// <summary>Konstruktor tworzący obiekt typu <c>MainWindow</c>.</summary>
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

		private void InitializeExpander()
		{
			var stackPanel = new System.Windows.Controls.StackPanel();

			stackPanel.Children.Add(new ExpanderControl("Add", Utilities.Enums.ExpanderControlType.ADD_BTN));
			stackPanel.Children.Add(new ExpanderControl("Change", Utilities.Enums.ExpanderControlType.CHANGE_BTN));
			stackPanel.Children.Add(new ExpanderControl("Remove", Utilities.Enums.ExpanderControlType.REMOVE_BTN));

			this.expander.Content = stackPanel;
		}

		private void AddPersonToGrid(Models.Base.Person person)
		{
			var personControl = new PersonControl(person);

			this.gridTabManage.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new GridLength(16) });
			System.Windows.Controls.Grid.SetRow(personControl, this.gridTabManage.RowDefinitions.Count - 1);
			this.gridTabManage.Children.Add(personControl);
		}

		#endregion

		#region Events

		private void mainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			this.InitializeExpander();

			foreach (Teacher teacher in Utilities.Database.GetTeachers())
			{
				this.AddPersonToGrid(teacher);
			}
		}

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
