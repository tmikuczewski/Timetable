namespace Timetable.Controls
{
	/// <summary>
	/// Interaction logic for PersonControl.xaml
	/// </summary>
	public partial class PersonControl : System.Windows.Controls.UserControl
	{
		#region Constructors

		public PersonControl(string pesel, string firstName, string lastName)
		{
			InitializeComponent();

			this.textBlockPesel.Text = pesel;
			this.textBlockFirstName.Text = firstName;
			this.textBlockLastName.Text = lastName;
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

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
