using System;
using System.Linq;
using System.Windows;
using Timetable.Code;

namespace Timetable.Windows
{
	/// <summary>
	/// Interaction logic for ManageWindow.xaml</summary>
	public partial class ManageWindow : System.Windows.Window
	{
		#region Constructors

		/// <summary>Konstruktor tworzący obiekt typu <c>ManageWindow</c>.
		/// </summary>
		public ManageWindow(MainWindow window, ExpanderControlType type)
		{
			this.InitializeComponent();
			this.callingWindow = window;
			this.controlType = type;
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

		private void managementWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (this.controlType == ExpanderControlType.Change)
			{
				this.currentPesel = this.callingWindow.MarkedPeople().FirstOrDefault();

				try
				{
					Models.Student student = Utilities.Database.GetStudentByPesel(this.currentPesel);

					this.maskedTextBoxPesel.Text = student.Pesel.StringRepresentation;
					this.textBoxFirstName.Text = student.FirstName;
					this.textBoxLastName.Text = student.LastName;
				}
				catch (Utilities.EntityDoesNotExistException)
				{
					MessageBox.Show("Student with given PESEL number does not existed.", "Error");
					this.Close();
				}
			}
		}

		private void buttonOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			string peselString = this.maskedTextBoxPesel.Text;
			string firstName = this.textBoxFirstName.Text.Trim();
			string lastName = this.textBoxLastName.Text.Trim();

			try
			{
				if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
				{
					MessageBox.Show("All fields are required.", "Error");
				}
				else
				{
					if (controlType == ExpanderControlType.Add)
					{
						Pesel pesel = new Pesel(peselString);
						Utilities.Database.AddStudent(pesel.ToString(), firstName, lastName);
					}

					if (controlType == ExpanderControlType.Change)
					{
						Utilities.Database.EditStudent(currentPesel, firstName, lastName);
					}

					this.callingWindow.RefreshStudents();
					this.Close();
				}
			}
			catch (Utilities.InvalidPeselException)
			{
				MessageBox.Show("PESEL number is invalid.", "Error");
			}
			catch (Utilities.DuplicateEntityException)
			{
				MessageBox.Show("Student with given PESEL number has already existed.", "Error");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error");
			}
		}

		private void buttonCancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Close();
		}

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		private readonly MainWindow callingWindow;

		private readonly ExpanderControlType controlType;

		private string currentPesel;

		#endregion
	}
}
