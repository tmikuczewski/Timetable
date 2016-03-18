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
			InitializeComponent();
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		#endregion

		#region Private methods

		private void InitializeEpander()
		{
			var stackPanel = new System.Windows.Controls.StackPanel();

			stackPanel.Children.Add(new ExpanderControl(Utilities.Enums.ExpanderControlType.ADD_BTN));
			stackPanel.Children.Add(new ExpanderControl(Utilities.Enums.ExpanderControlType.CHANGE_BTN));
			stackPanel.Children.Add(new ExpanderControl(Utilities.Enums.ExpanderControlType.REMOVE_BTN));

			this.expander.Content = stackPanel;
		}

		#endregion

		#region Events

		private void mainWindow_Loaded(object sender, RoutedEventArgs e)
		{

		}

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
