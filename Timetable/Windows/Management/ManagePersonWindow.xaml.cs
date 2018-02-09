using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.DAL.DataSet.MySql;
using Timetable.DAL.DataSet.MySql.TimetableDataSetTableAdapters;
using Timetable.DAL.Utilities;
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

		private TimetableDataSet _timetableDataSet;
		private ClassesTableAdapter _classesTableAdapter;
		private StudentsTableAdapter _studentsTableAdapter;
		private TeachersTableAdapter _teachersTableAdapter;

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
			_timetableDataSet = new TimetableDataSet();
			_classesTableAdapter = new ClassesTableAdapter();
			_studentsTableAdapter = new StudentsTableAdapter();
			_teachersTableAdapter = new TeachersTableAdapter();

			_classesTableAdapter.Fill(_timetableDataSet.Classes);
			_studentsTableAdapter.Fill(_timetableDataSet.Students);
			_teachersTableAdapter.Fill(_timetableDataSet.Teachers);
		}

		private void FillComboBoxes()
		{
			switch (_entityType)
			{
				case EntityType.Student:
					labelClass.Visibility = Visibility.Visible;
					comboBoxClass.Visibility = Visibility.Visible;

					_classesItemsSource = _timetableDataSet.Classes
						.OrderBy(c => c.ToFriendlyString())
						.ToList();

					var emptyClassRow = _timetableDataSet.Classes.NewClassesRow();
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
						if (_entityType == EntityType.Student)
						{
							_currentStudentRow = _timetableDataSet.Students.NewStudentsRow();
						}
						else if (_entityType == EntityType.Teacher)
						{
							_currentTeacherRow = _timetableDataSet.Teachers.NewTeachersRow();
						}
						break;
					case ActionType.Change:
						if (_entityType == EntityType.Student)
						{
							_currentStudentRow = PrepareStudent();
						}
						else if (_entityType == EntityType.Teacher)
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

			var studentRow = _timetableDataSet.Students.FindByPesel(_currentPesel);

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

			var teacherRow = _timetableDataSet.Teachers.FindByPesel(_currentPesel);

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
					if (_entityType == EntityType.Student)
					{
						if (_currentStudentRow == null)
							return;

						maskedTextBoxPesel.Text = _currentStudentRow.Pesel;
						textBoxFirstName.Text = _currentStudentRow.FirstName;
						textBoxLastName.Text = _currentStudentRow.LastName;
						comboBoxClass.SelectedValue = _currentStudentRow.ClassId;
					}
					else if (_entityType == EntityType.Teacher)
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
				if (_entityType == EntityType.Student)
				{
					SaveStudent(peselString, firstName, lastName);
				}
				else if (_entityType == EntityType.Teacher)
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

				if (_timetableDataSet.Students.FindByPesel(_currentStudentRow.Pesel) != null)
				{
					throw new DuplicateEntityException();
				}

				_timetableDataSet.Students.Rows.Add(_currentStudentRow);
			}

			_studentsTableAdapter.Update(_timetableDataSet.Students);

			_callingWindow.RefreshViews(EntityType.Student);

			Close();
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

				if (_timetableDataSet.Teachers.FindByPesel(_currentTeacherRow.Pesel) != null)
				{
					throw new DuplicateEntityException();
				}

				_timetableDataSet.Teachers.Rows.Add(_currentTeacherRow);
			}

			_teachersTableAdapter.Update(_timetableDataSet.Teachers);

			_callingWindow.RefreshViews(EntityType.Teacher);

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
