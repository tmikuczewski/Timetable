namespace Timetable.Controls
{
	/// <summary>
	/// Interaction logic for CellControl.xaml
	/// </summary>
	public partial class CellControl : System.Windows.Controls.UserControl
	{
		#region Constructors

		/// <summary>Konstruktor tworzący obiekt typu <c>CellControl</c>.
		/// </summary>
		public CellControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>CellControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
		/// <param name="firstRow">Tekst wypełniający pierwszy rząd.</param>
		/// <param name="secondRow">Tekst wypełniający drugi rząd.</param>
		/// <param name="thirdRow">Tekst wypełniający pierwszy rząd.</param>
		public CellControl(string firstRow, string secondRow, string thirdRow) 
			: this()
		{
			this.FirstRow = firstRow;
			this.SecondRow = secondRow;
			this.ThirdRow = thirdRow;
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		/// <summary>
		/// Pierwszy wiersz kontrolki.</summary>
		public string FirstRow { get; set; }
		/// <summary>
		/// Drugi wiersz kontrolki.</summary>
		public string SecondRow { get; set; }
		/// <summary>
		/// Trzeci wiersz kontroli.</summary>
		public string ThirdRow { get; set; }

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
