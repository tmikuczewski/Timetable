using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.DAL.DataSet.MySql;
using Timetable.DAL.DataSet.MySql.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows.Mapping
{
	/// <summary>
	///     Interaction logic for MappingWindow.xaml
	/// </summary>
	public partial class MappingWindow : Window
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private TimetableDataSet _timetableDataSet;
		private ClassesTableAdapter _classesTableAdapter;
		private TeachersTableAdapter _teachersTableAdapter;
		private SubjectsTableAdapter _subjectsTableAdapter;
		private LessonsTableAdapter _lessonsTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly ActionType _actionType;

		private int _currentClassId;
		private int _currentSubjectId;
		private int _currentLessonId;
		private TimetableDataSet.LessonsRow _currentLessonRow;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>MappingWindow</c>.
		/// </summary>
		public MappingWindow(MainWindow mainWindow, ActionType actionType)
		{
			InitializeComponent();

			_callingWindow = mainWindow;
			_actionType = actionType;
		}

		#endregion


		#region Events

		private async void mappingWindow_Loaded(object sender, RoutedEventArgs e)
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
			_subjectsTableAdapter = new SubjectsTableAdapter();
			_teachersTableAdapter = new TeachersTableAdapter();
			_lessonsTableAdapter = new LessonsTableAdapter();

			_classesTableAdapter.Fill(_timetableDataSet.Classes);
			_subjectsTableAdapter.Fill(_timetableDataSet.Subjects);
			_teachersTableAdapter.Fill(_timetableDataSet.Teachers);
			_lessonsTableAdapter.Fill(_timetableDataSet.Lessons);
		}

		private void FillComboBoxes()
		{
			comboBoxTeachers.ItemsSource = _timetableDataSet.Teachers
				.OrderBy(t => t.LastName)
				.ThenBy(t => t.FirstName);
			comboBoxTeachers.SelectedValuePath = "Pesel";

			comboBoxClasses.ItemsSource = _timetableDataSet.Classes
				.OrderBy(c => c.Year)
				.ThenBy(c => c.CodeName);
			comboBoxClasses.SelectedValuePath = "Id";

			comboBoxSubjects.ItemsSource = _timetableDataSet.Subjects
				.OrderBy(s => s.Name);
			comboBoxSubjects.SelectedValuePath = "Id";
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_actionType)
				{
					case ActionType.Add:
						_currentLessonRow = _timetableDataSet.Lessons.NewLessonsRow();
						break;
					case ActionType.Change:
						_currentLessonRow = PrepareLesson();
						break;
				}
			}
			catch (EntityDoesNotExistException)
			{
				ShowErrorMessageBox("Lesson with given ID number does not exist.");
				Close();
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
				Close();
			}
		}

		private TimetableDataSet.LessonsRow PrepareLesson()
		{
			if (!int.TryParse(_callingWindow.GetIdNumbersOfMarkedLessons().FirstOrDefault(), out _currentLessonId))
			{
				throw new EntityDoesNotExistException();
			}

			var lessonsRow = _timetableDataSet.Lessons.FindById(_currentLessonId);

			if (lessonsRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return lessonsRow;
		}

		private void FillControls()
		{
			switch (_actionType)
			{
				case ActionType.Change:
					if (_currentLessonRow == null)
						return;

					comboBoxClasses.SelectedValue = _currentLessonRow.ClassId;
					comboBoxSubjects.SelectedValue = _currentLessonRow.SubjectId;
					comboBoxTeachers.SelectedValue = _currentLessonRow.TeacherPesel;
					break;
			}

			buttonOk.IsEnabled = true;
			buttonCancel.IsEnabled = true;
		}

		private void SaveEntity()
		{
			try
			{
				SaveLesson();
			}
			catch (FieldsNotFilledException)
			{
				ShowWarningMessageBox("All fields are required.");
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void SaveLesson()
		{
			if (comboBoxClasses.SelectedValue == null
				|| comboBoxSubjects.SelectedValue == null
				|| comboBoxTeachers.SelectedValue == null)
			{
				throw new FieldsNotFilledException();
			}

			if (int.TryParse(comboBoxClasses.SelectedValue.ToString(), out _currentClassId)
				&& int.TryParse(comboBoxSubjects.SelectedValue.ToString(), out _currentSubjectId))
			{
				_currentLessonRow.ClassId = _currentClassId;
				_currentLessonRow.SubjectId = _currentSubjectId;
			}
			else
			{
				throw new FieldsNotFilledException();
			}

			_currentLessonRow.TeacherPesel = comboBoxTeachers.SelectedValue.ToString();


			if (_actionType == ActionType.Add)
			{
				_timetableDataSet.Lessons.Rows.Add(_currentLessonRow);
			}

			_lessonsTableAdapter.Update(_timetableDataSet.Lessons);

			_callingWindow.RefreshViews(EntityType.Lesson);

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
