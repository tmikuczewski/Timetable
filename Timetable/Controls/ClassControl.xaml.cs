namespace Timetable.Controls
{
	/// <summary>
	/// Interaction logic for ClassControl.xaml</summary>
	public partial class ClassControl : System.Windows.Controls.UserControl
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>Controls.ClassControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
		/// <param name="oClass">Obiekt typu <c>Models.Base.Class</c> wypełniający danymi pola tekstowe kontrol.</param>
		public ClassControl(Models.Class oClass)
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
