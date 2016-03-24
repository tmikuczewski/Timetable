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

		public PersonControl(Models.Base.Person person)
		{
			InitializeComponent();

			this.textBlockPesel.Text = person.PESEL;
			this.textBlockFirstName.Text = person.FirstName;
			this.textBlockLastName.Text = person.LastName;

			if (person is Teacher)
			{
				//TODO: Dodać wewnątrz obiektu 'Teacher' informację o klasach jakie uczy.
				this.textBlockClassName.Text = 0.ToString(); // (person as Teacher).Classes.Count;
			}
			else if (person is Student)
			{
				this.textBlockClassName.Text = Utilities.Database.ClassesTable.FirstOrDefault(c => c.id == (person as Student).Class).code_name;
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
