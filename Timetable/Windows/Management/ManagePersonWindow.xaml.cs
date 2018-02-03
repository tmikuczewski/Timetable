using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows.Management
{
	/// <summary>
	///     Interaction logic for ManagePersonWindow.xaml
	/// </summary>
	public partial class ManagePersonWindow : Window
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private TimetableDataSet timetableDataSet;
		private ClassesTableAdapter classesTableAdapter;
		private StudentsTableAdapter studentsTableAdapter;
		private TeachersTableAdapter teachersTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly ActionType _actionType;
		private readonly EntityType _entityType;

		private string _currentPesel;
		private TimetableDataSet.StudentsRow _currentStudentRow;
		private TimetableDataSet.TeachersRow _currentTeacherRow;
		private IList<TimetableDataSet.ClassesRow> _classesItemsSource;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>ManagePersonWindow</c>.
		/// </summary>
		public ManagePersonWindow(MainWindow mainWindow, ActionType actionType)
		{
			InitializeComponent();

			_callingWindow = mainWindow;
			_actionType = actionType;
			_entityType = _callingWindow.GetCurrentEntityType();
		}

		#endregion


		#region Events

		private async void managementWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					InitDatabaseObjects();

					FillComboBoxes();

					PrepareEntity();

					FillControls();
				});
			});
		}

		private async void buttonOk_Click(object sender, RoutedEventArgs e)
		{
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(SaveEntity);
			});
		}

		private async void buttonCancel_Click(object sender, RoutedEventArgs e)
		{
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(Close);
			});
		}

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		#endregion


		#region Private methods

		private void InitDatabaseObjects()
		{
			timetableDataSet = new TimetableDataSet();
			classesTableAdapter = new ClassesTableAdapter();
			studentsTableAdapter = new StudentsTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();

			classesTableAdapter.Fill(timetableDataSet.Classes);
			studentsTableAdapter.Fill(timetableDataSet.Students);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);
		}

		private void FillComboBoxes()
		{
			switch (_entityType)
			{
				case EntityType.Students:
					labelClass.Visibility = Visibility.Visible;
					comboBoxClass.Visibility = Visibility.Visible;

					_classesItemsSource = timetableDataSet.Classes
						.OrderBy(c => c.ToFriendlyString())
						.ToList();

					var emptyClassRow = timetableDataSet.Classes.NewClassesRow();
					emptyClassRow["Id"] = DBNull.Value;
					_classesItemsSource.Insert(0, emptyClassRow);

					comboBoxClass.ItemsSource = _classesItemsSource;
					comboBoxClass.SelectedValuePath = "Id";
					break;
			}
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_actionType)
				{
					case ActionType.Add:
						if (_entityType == EntityType.Students)
						{
							_currentStudentRow = timetableDataSet.Students.NewStudentsRow();
						}
						else if (_entityType == EntityType.Teachers)
						{
							_currentTeacherRow = timetableDataSet.Teachers.NewTeachersRow();
						}
						break;
					case ActionType.Change:
						if (_entityType == EntityType.Students)
						{
							_currentStudentRow = PrepareStudent();
						}
						else if (_entityType == EntityType.Teachers)
						{
							_currentTeacherRow = PrepareTeachert();
						}
						break;
				}
			}
			catch (EntityDoesNotExistException)
			{
				ShowErrorMessageBox("Person with given PESEL number does not exist.");
				Close();
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
				Close();
			}
		}

		private TimetableDataSet.StudentsRow PrepareStudent()
		{
			_currentPesel = _callingWindow.GetPeselsOfMarkedPeople().FirstOrDefault();

			if (string.IsNullOrEmpty(_currentPesel))
			{
				throw new EntityDoesNotExistException();
			}

			var studentRow = timetableDataSet.Students.FindByPesel(_currentPesel);

			if (studentRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return studentRow;
		}

		private TimetableDataSet.TeachersRow PrepareTeachert()
		{
			_currentPesel = _callingWindow.GetPeselsOfMarkedPeople().FirstOrDefault();

			if (string.IsNullOrEmpty(_currentPesel))
			{
				throw new EntityDoesNotExistException();
			}

			var teacherRow = timetableDataSet.Teachers.FindByPesel(_currentPesel);

			if (teacherRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return teacherRow;
		}

		private void FillControls()
		{
			switch (_actionType)
			{
				case ActionType.Change:
					if (_entityType == EntityType.Students)
					{
						if (_currentStudentRow == null)
							return;

						maskedTextBoxPesel.Text = _currentStudentRow.Pesel;
						textBoxFirstName.Text = _currentStudentRow.FirstName;
						textBoxLastName.Text = _currentStudentRow.LastName;
						comboBoxClass.SelectedValue = _currentStudentRow.ClassId;
					}
					else if (_entityType == EntityType.Teachers)
					{
						if (_currentTeacherRow == null)
							return;

						maskedTextBoxPesel.Text = _currentTeacherRow.Pesel;
						textBoxFirstName.Text = _currentTeacherRow.FirstName;
						textBoxLastName.Text = _currentTeacherRow.LastName;

						var classes = _currentTeacherRow.GetClassesRows()
							.OrderBy(c => c.ToFriendlyString())
							.Select(c => c.ToFriendlyString())
							.ToList();

						if (classes.Any())
						{
							labelClass.Visibility = Visibility.Visible;
							textBoxClasses.Visibility = Visibility.Visible;
							textBoxClasses.Text = string.Join(", ", classes);
						}
					}
					break;
			}

			buttonOk.IsEnabled = true;
			buttonCancel.IsEnabled = true;
		}

		private void SaveEntity()
		{
			var peselString = maskedTextBoxPesel.Text;
			var firstName = textBoxFirstName.Text.Trim();
			var lastName = textBoxLastName.Text.Trim();

			try
			{
				if (_entityType == EntityType.Students)
				{
					SaveStudent(peselString, firstName, lastName);
				}
				else if (_entityType == EntityType.Teachers)
				{
					SaveTeacher(peselString, firstName, lastName);
				}
			}
			catch (FieldsNotFilledException)
			{
				ShowWarningMessageBox("All fields are required.");
			}
			catch (InvalidPeselException)
			{
				ShowWarningMessageBox("PESEL number is invalid.");
			}
			catch (DuplicateEntityException)
			{
				ShowErrorMessageBox("Person with given PESEL number has already existed.");
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void SaveStudent(string peselString, string firstName, string lastName)
		{
			if (string.IsNullOrEmpty(firstName)
				|| string.IsNullOrEmpty(lastName))
			{
				throw new FieldsNotFilledException();
			}

			_currentStudentRow.FirstName = firstName;
			_currentStudentRow.LastName = lastName;
			_currentStudentRow["ClassId"] = comboBoxClass.SelectedValue ?? DBNull.Value;

			if (_actionType == ActionType.Add)
			{
				_currentStudentRow.Pesel = new Pesel(peselString).StringRepresentation;

				if (timetableDataSet.Students.FindByPesel(_currentStudentRow.Pesel) != null)
				{
					throw new DuplicateEntityException();
				}

				timetableDataSet.Students.Rows.Add(_currentStudentRow);
			}

			SetOdbcUpdateStudentCommand(peselString, firstName, lastName);

			studentsTableAdapter.Update(timetableDataSet.Students);

			_callingWindow.RefreshViews(EntityType.Students);

			Close();
		}

		private void SetOdbcUpdateStudentCommand(string pesel, string firstName, string lastName)
		{
			OdbcConnection conn = new OdbcConnection(System.Configuration.ConfigurationManager
				.ConnectionStrings["Timetable.Properties.Settings.ConnectionString"].ConnectionString);

			OdbcCommand cmd = conn.CreateCommand();

			cmd.CommandText = "UPDATE students " +
			                  "SET first_name = ?, last_name = ?, class = ? " +
			                  "WHERE pesel = ?";

			cmd.Parameters.Add("first_name", OdbcType.Text).Value = firstName;
			cmd.Parameters.Add("last_name", OdbcType.Text).Value = lastName;
			cmd.Parameters.Add("class", OdbcType.Int).Value = comboBoxClass.SelectedValue ?? DBNull.Value;
			cmd.Parameters.Add("pesel", OdbcType.VarChar).Value = pesel;

			studentsTableAdapter.Adapter.UpdateCommand = cmd;
		}

		private void SaveTeacher(string peselString, string firstName, string lastName)
		{
			if (string.IsNullOrEmpty(firstName)
				|| string.IsNullOrEmpty(lastName))
			{
				throw new FieldsNotFilledException();
			}

			_currentTeacherRow.FirstName = firstName;
			_currentTeacherRow.LastName = lastName;

			if (_actionType == ActionType.Add)
			{
				_currentTeacherRow.Pesel = new Pesel(peselString).StringRepresentation;

				if (timetableDataSet.Teachers.FindByPesel(_currentTeacherRow.Pesel) != null)
				{
					throw new DuplicateEntityException();
				}

				timetableDataSet.Teachers.Rows.Add(_currentTeacherRow);
			}

			teachersTableAdapter.Update(timetableDataSet.Teachers);

			_callingWindow.RefreshViews(EntityType.Teachers);

			Close();
		}

		private MessageBoxResult ShowErrorMessageBox(string message)
		{
			return MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private MessageBoxResult ShowWarningMessageBox(string message)
		{
			return MessageBox.Show(this, message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		#endregion
	}
}
