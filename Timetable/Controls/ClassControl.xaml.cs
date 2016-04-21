namespace Timetable.Controls
{
	/// <summary>
	/// Interaction logic for ClassControl.xaml
	/// </summary>
	public partial class ClassControl : System.Windows.Controls.UserControl
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>Controls.ClassControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
		/// <param name="classRow">Obiekt typu <c>TimetableDataSet.ClassesRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
		public ClassControl(TimetableDataSet.ClassesRow classRow)
		{
			InitializeComponent();

			this.textBlockId.Text = classRow.Id.ToString();
			this.textBlockYear.Text = classRow.Year.ToString();
			this.textBlockCodeName.Text = classRow.CodeName ?? string.Empty;
			this.textBlockTutorPesel.Text = classRow.TutorPesel ?? string.Empty;
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		/// <summary>
		/// Sprawdza, czy wybrana klasa jest zaznaczona.
		/// </summary>
		/// <returns></returns>
		public bool IsChecked()
		{
			return this.checkBox.IsChecked ?? false;
		}

		/// <summary>
		/// Zwraca numer id klasy w kontrolce.
		/// </summary>
		/// <returns></returns>
		public string GetId()
		{
			return this.textBlockId.Text;
		}

		#endregion

		#region Properties

		#endregion

		#region Private methods

		#endregion

		#region Events

		private void UserControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.checkBox.IsChecked = !this.checkBox.IsChecked;
		}

		#endregion

		#region Constants and Statics

		/// <summary>
		/// Wysokość kontrolki.</summary>
		public const int HEIGHT = 30;

		#endregion

		#region Fields

		#endregion
	}
}
