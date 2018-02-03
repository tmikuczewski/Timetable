using System.Windows.Controls;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for TermCellControl.xaml
	/// </summary>
	public partial class TermCellControl : UserControl
	{
		#region Constants and Statics

		/// <summary>
		///     Wysokość wiersza z listą dni.
		/// </summary>
		public const int HEIGHT = 40;

		/// <summary>
		///     Szerokość kolumny z listą godzin.
		/// </summary>
		public const int WIDTH = 40;

		#endregion


		#region Fields

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>TermCellControl</c>.
		/// </summary>
		/// <param name="description">Tekst wypełniający opis.</param>
		public TermCellControl(string description)
		{
			InitializeComponent();

			textBlock.Text = description;
		}

		#endregion


		#region Events

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		#endregion


		#region Private methods

		#endregion
	}
}