using System.Windows.Controls;

namespace Timetable.Windows
{
	/// <summary>
	///     Interaction logic for OperationsStackPanel.xaml
	/// </summary>
	public partial class OperationsStackPanel : UserControl
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private MainWindow _callingWindow;

		#endregion


		#region Properties

		/// <summary>
		///     Metoda zwracająca referencję do okna rodzica.
		/// </summary>
		public MainWindow CallingWindow
		{
			get { return _callingWindow; }
			set
			{
				if (_callingWindow == null)
					_callingWindow = value;
			}
		}

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>OperationsStackPanel</c>.
		/// </summary>
		public OperationsStackPanel()
		{
			InitializeComponent();
		}

		#endregion


		#region Events

		private void comboBoxManagementFilterEntityType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_callingWindow?.comboBoxManagementFilterEntityType_SelectionChanged(sender, e);
		}

		private void comboBoxPlanningFilterEntityType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_callingWindow?.comboBoxPlanningFilterEntityType_SelectionChanged(sender, e);
		}

		private void comboBoxPlanningFilterEntity_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_callingWindow?.comboBoxPlanningFilterEntity_SelectionChanged(sender, e);
		}

		private void comboBoxSummaryFilterEntityType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_callingWindow?.comboBoxSummaryFilterEntityType_SelectionChanged(sender, e);
		}

		private void comboBoxSummaryFilterEntity_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_callingWindow?.comboBoxSummaryFilterEntity_SelectionChanged(sender, e);
		}

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		#endregion


		#region Private methods

		#endregion
	}
}
