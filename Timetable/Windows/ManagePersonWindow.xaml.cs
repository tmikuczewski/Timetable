using System;
using System.Linq;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows
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
		private readonly ExpanderControlType _controlType;
		private readonly ComboBoxContentType _contentType;

		private int _currentClassId;
		private string _currentPesel;
		private TimetableDataSet.StudentsRow _currentStudentRow;
		private TimetableDataSet.TeachersRow _currentTeacherRow;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>ManagePersonWindow</c>.
		/// </summary>
		public ManagePersonWindow(MainWindow mainWindow, ExpanderControlType controlType)
		{
			InitDatabaseObjects();

			InitializeComponent();

			_callingWindow = mainWindow;
			_controlType = controlType;
			_contentType = _callingWindow.GetCurrentContentType();
		}

		#endregion


		#region Events

		private void managementWindow_Loaded(object sender, RoutedEventArgs e)
		{
			FillComboBoxes();

			PrepareEntity();

			FillControls();
		}

		private void buttonOk_Click(object sender, RoutedEventArgs e)
		{
			SaveEntity();
		}

		private void buttonCancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
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
			switch (_contentType)
			{
				case ComboBoxContentType.Students:
					comboBoxClass.ItemsSource = timetableDataSet.Classes.OrderBy(c => c.Year).ThenBy(c => c.CodeName);
					comboBoxClass.SelectedValuePath = "Id";
					break;
				case ComboBoxContentType.Teachers:
					comboBoxClassLabel.Visibility = Visibility.Hidden;
					comboBoxClass.Visibility = Visibility.Hidden;
					break;
			}
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_controlType)
				{
					case ExpanderControlType.Add:
						if (_contentType == ComboBoxContentType.Students)
						{
							_currentStudentRow = timetableDataSet.Students.NewStudentsRow();
						}
						else if (_contentType == ComboBoxContentType.Teachers)
						{
							_currentTeacherRow = timetableDataSet.Teachers.NewTeachersRow();
						}
						break;
					case ExpanderControlType.Change:
						if (_contentType == ComboBoxContentType.Students)
						{
							_currentStudentRow = PrepareStudent();
						}
						else if (_contentType == ComboBoxContentType.Teachers)
						{
							_currentTeacherRow = PrepareTeachert();
						}
						break;
				}
			}
			catch (EntityDoesNotExistException)
			{
				MessageBox.Show(this, "Student with given PESEL number does not exist.", "Error",
					MessageBoxButton.OK, MessageBoxImage.Error);
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString(), "Error",
					MessageBoxButton.OK, MessageBoxImage.Error);
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
			switch (_controlType)
			{
				case ExpanderControlType.Change:
					if (_contentType == ComboBoxContentType.Students)
					{
						maskedTextBoxPesel.Text = _currentStudentRow.Pesel;
						textBoxFirstName.Text = _currentStudentRow.FirstName;
						textBoxLastName.Text = _currentStudentRow.LastName;
						comboBoxClass.SelectedValue = _currentStudentRow.ClassId;
					}
					else if (_contentType == ComboBoxContentType.Teachers)
					{
						maskedTextBoxPesel.Text = _currentTeacherRow.Pesel;
						textBoxFirstName.Text = _currentTeacherRow.FirstName;
						textBoxLastName.Text = _currentTeacherRow.LastName;
					}
					break;
			}
		}

		private void SaveEntity()
		{
			var peselString = maskedTextBoxPesel.Text;
			var firstName = textBoxFirstName.Text.Trim();
			var lastName = textBoxLastName.Text.Trim();

			try
			{
				if (_contentType == ComboBoxContentType.Students)
				{
					SaveStudent(firstName, lastName, peselString);
				}
				else if (_contentType == ComboBoxContentType.Teachers)
				{
					SaveTeacher(firstName, lastName, peselString);
				}
			}
			catch (FieldsNotFilledException)
			{
				MessageBox.Show(this, "All fields are required.", "Warning",
					MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			catch (InvalidPeselException)
			{
				MessageBox.Show(this, "PESEL number is invalid.", "Warning",
					MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			catch (DuplicateEntityException)
			{
				MessageBox.Show(this, "Person with given PESEL number has already existed.", "Error",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString(), "Error",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void SaveStudent(string firstName, string lastName, string peselString)
		{
			if (comboBoxClass.SelectedValue == null
				|| string.IsNullOrEmpty(firstName)
				|| string.IsNullOrEmpty(lastName))
			{
				throw new FieldsNotFilledException();
			}

			_currentStudentRow.FirstName = firstName;
			_currentStudentRow.LastName = lastName;

			if (int.TryParse(comboBoxClass.SelectedValue.ToString(), out _currentClassId))
			{
				_currentStudentRow.ClassId = _currentClassId;
			}
			else
			{
				throw new FieldsNotFilledException();
			}

			if (_controlType == ExpanderControlType.Add)
			{
				_currentStudentRow.Pesel = new Pesel(peselString).StringRepresentation;

				if (timetableDataSet.Students.FindByPesel(_currentStudentRow.Pesel) != null)
				{
					throw new DuplicateEntityException();
				}

				timetableDataSet.Students.Rows.Add(_currentStudentRow);
			}

			studentsTableAdapter.Update(timetableDataSet.Students);

			_callingWindow.RefreshCurrentView(ComboBoxContentType.Students);

			Close();
		}

		private void SaveTeacher(string firstName, string lastName, string peselString)
		{
			if (string.IsNullOrEmpty(firstName)
				|| string.IsNullOrEmpty(lastName))
			{
				throw new FieldsNotFilledException();
			}

			_currentTeacherRow.FirstName = firstName;
			_currentTeacherRow.LastName = lastName;

			if (_controlType == ExpanderControlType.Add)
			{
				_currentTeacherRow.Pesel = new Pesel(peselString).StringRepresentation;

				if (timetableDataSet.Teachers.FindByPesel(_currentTeacherRow.Pesel) != null)
				{
					throw new DuplicateEntityException();
				}

				timetableDataSet.Teachers.Rows.Add(_currentTeacherRow);
			}

			teachersTableAdapter.Update(timetableDataSet.Teachers);

			_callingWindow.RefreshCurrentView(ComboBoxContentType.Teachers);

			Close();
		}

		#endregion
	}
}
