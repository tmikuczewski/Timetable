using System;
using System.Linq;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows
{
	/// <summary>
	/// Interaction logic for ManagePersonWindow.xaml
	/// </summary>
	public partial class ManagePersonWindow : System.Windows.Window
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>ManagePersonWindow</c>.
		/// </summary>
		public ManagePersonWindow(MainWindow window, ExpanderControlType type)
		{
			this.InitializeComponent();
			this.callingWindow = window;
			this.controlType = type;
			this.contentType = window.GetCurrentContentType();
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
			timetableDataSet = new TimetableDataSet();

			classesTableAdapter = new ClassesTableAdapter();
			studentsTableAdapter = new StudentsTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();

			classesTableAdapter.Fill(timetableDataSet.Classes);
			studentsTableAdapter.Fill(timetableDataSet.Students);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);

			if (contentType == ComboBoxContent.Students)
			{
				comboBoxClass.ItemsSource = timetableDataSet.Classes.DefaultView;
				comboBoxClass.SelectedValuePath = "Id";
			}

			if (contentType == ComboBoxContent.Teachers)
			{
				comboBoxClassLabel.Visibility = Visibility.Hidden;
				comboBoxClass.Visibility = Visibility.Hidden;
			}

			if (this.controlType == ExpanderControlType.Add)
			{
				if (contentType == ComboBoxContent.Students)
				{
					currentStudentRow = timetableDataSet.Students.NewStudentsRow();
				}

				if (contentType == ComboBoxContent.Teachers)
				{
					currentTeacherRow = timetableDataSet.Teachers.NewTeachersRow();
				}
			}

			if (this.controlType == ExpanderControlType.Change)
			{
				this.currentPesel = this.callingWindow.GetPeselsOfMarkedPeople().FirstOrDefault();

				if (contentType == ComboBoxContent.Students)
				{
					currentStudentRow = timetableDataSet.Students.FindByPesel(this.currentPesel);

					if (currentStudentRow != null)
					{
						this.maskedTextBoxPesel.Text = currentStudentRow.Pesel;
						this.textBoxFirstName.Text = currentStudentRow.FirstName;
						this.textBoxLastName.Text = currentStudentRow.LastName;
						this.comboBoxClass.SelectedValue = currentStudentRow.ClassId;
					}
					else
					{
						MessageBox.Show("Student with given PESEL number does not existed.", "Error");
						this.Close();
					}
				}

				if (contentType == ComboBoxContent.Teachers)
				{
					currentTeacherRow = timetableDataSet.Teachers.FindByPesel(this.currentPesel);

					if (currentTeacherRow != null)
					{
						this.maskedTextBoxPesel.Text = currentTeacherRow.Pesel;
						this.textBoxFirstName.Text = currentTeacherRow.FirstName;
						this.textBoxLastName.Text = currentTeacherRow.LastName;
					}
					else
					{
						MessageBox.Show("Teacher with given PESEL number does not existed.", "Error");
						this.Close();
					}
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
				if (contentType == ComboBoxContent.Students)
				{
					if (this.comboBoxClass.SelectedValue == null
						|| string.IsNullOrEmpty(firstName)
						|| string.IsNullOrEmpty(lastName))
					{
						MessageBox.Show("All fields are required.", "Error");
						return;
					}
					else
					{
						currentStudentRow.FirstName = firstName;
						currentStudentRow.LastName = lastName;

						if (this.comboBoxClass.SelectedValue != null)
						{
							int classId;

							if (int.TryParse(this.comboBoxClass.SelectedValue.ToString(), out classId))
							{
								currentStudentRow.ClassId = classId;
							}
						}

						if (this.controlType == ExpanderControlType.Add)
						{
							Pesel pesel = new Pesel(peselString);
							currentStudentRow.Pesel = pesel.StringRepresentation;

							if (timetableDataSet.Students.FindByPesel(currentStudentRow.Pesel) != null)
							{
								throw new Utilities.DuplicateEntityException();
							}

							timetableDataSet.Students.Rows.Add(currentStudentRow);
						}

						studentsTableAdapter.Update(timetableDataSet.Students);

						this.callingWindow.RefreshCurrentView();
						this.Close();
					}
				}

				if (contentType == ComboBoxContent.Teachers)
				{
					if (string.IsNullOrEmpty(firstName)
						|| string.IsNullOrEmpty(lastName))
					{
						MessageBox.Show("All fields are required.", "Error");
						return;
					}
					else
					{
						currentTeacherRow.FirstName = firstName;
						currentTeacherRow.LastName = lastName;

						if (this.controlType == ExpanderControlType.Add)
						{
							Pesel pesel = new Pesel(peselString);
							currentTeacherRow.Pesel = pesel.StringRepresentation;

							if (timetableDataSet.Teachers.FindByPesel(currentTeacherRow.Pesel) != null)
							{
								throw new Utilities.DuplicateEntityException();
							}

							timetableDataSet.Teachers.Rows.Add(currentTeacherRow);
						}

						teachersTableAdapter.Update(timetableDataSet.Teachers);

						this.callingWindow.RefreshCurrentView();
						this.Close();
					}
				}
			}
			catch (Utilities.InvalidPeselException)
			{
				MessageBox.Show("PESEL number is invalid.", "Error");
			}
			catch (Utilities.DuplicateEntityException)
			{
				MessageBox.Show("Person with given PESEL number has already existed.", "Error");
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

		private readonly ComboBoxContent contentType;

		private string currentPesel;

		private TimetableDataSet timetableDataSet;

		private ClassesTableAdapter classesTableAdapter;
		private StudentsTableAdapter studentsTableAdapter;
		private TeachersTableAdapter teachersTableAdapter;

		private TimetableDataSet.StudentsRow currentStudentRow;
		private TimetableDataSet.TeachersRow currentTeacherRow;

		#endregion
	}
}
