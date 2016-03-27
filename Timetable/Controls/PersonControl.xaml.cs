using System.Linq;

using Timetable.Models;

namespace Timetable.Controls
{
	/// <summary>
	/// Interaction logic for PersonControl.xaml
	/// </summary>
	public partial class PersonControl : System.Windows.Controls.UserControl
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>Controls.PersonControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
		/// <param name="person">Obiekt typu <c>Models.Base.Person</c> wypełniający danymi pola tekstowe kontrol.</param>
		public PersonControl(Models.Base.Person person)
		{
			InitializeComponent();

			this.textBlockPesel.Text = person.PESEL;
			this.textBlockFirstName.Text = person.FirstName;
			this.textBlockLastName.Text = person.LastName;

			if (person is Teacher)
			{
				this.textBlockClassName.Text = 0.ToString(); // Utilities.Database.GetClasses().Where(c => c.TeacherPesel == person.Pesel).Count;
			}
			else if (person is Student)
			{
				this.textBlockClassName.Text = 0.ToString(); // Utilities.Database.GetClasses().FirstOrDefault(c => c.Id == (person as Student).ClassId).CodeName;
			}
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

		#endregion

		#region Fields

		#endregion
	}
}
