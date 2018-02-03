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

		private void comboBoxManagement_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_callingWindow?.comboBoxManagement_SelectionChanged(sender, e);
		}

		private void comboBoxSummary1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_callingWindow?.comboBoxSummary1_SelectionChanged(sender, e);
		}

		private void comboBoxSummary2_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_callingWindow?.comboBoxSummary2_SelectionChanged(sender, e);
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
