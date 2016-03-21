using System.Windows;

using Timetable.Controls;

namespace Timetable
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Constructors

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

			stackPanel.Children.Add(new ExpanderControl("Add", Utilities.Enums.ExpanderControlImgType.ADD_BTN));
			stackPanel.Children.Add(new ExpanderControl("Change", Utilities.Enums.ExpanderControlImgType.CHANGE_BTN));
			stackPanel.Children.Add(new ExpanderControl("Remove", Utilities.Enums.ExpanderControlImgType.REMOVE_BTN));

			this.expander.Content = stackPanel;
		}

		private void AddPersonToGrid(string pesel, string firstName, string lastName)
		{
			var personControl = new PersonControl(pesel, firstName, lastName);

			this.gridTabManage.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new GridLength(16) });
			System.Windows.Controls.Grid.SetRow(personControl, this.gridTabManage.RowDefinitions.Count - 1);
			this.gridTabManage.Children.Add(personControl);
		}

		#endregion

		#region Events

		private void mainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			this.InitializeExpander();

			this.AddPersonToGrid("62342987320", "Arkadiusz", "Robak");
			this.AddPersonToGrid("98419823477", "Jan", "Kowalski");
			this.AddPersonToGrid("53252132523", "Alicja", "Wróbel");
			this.AddPersonToGrid("94972948331", "Tomasz", "Mikuczewski");
			this.AddPersonToGrid("62342987320", "Roman", "Gula");
			this.AddPersonToGrid("98419823477", "Wojtek", "Marianek");
		}

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
