using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
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

		private TimetableDataSet timetableDataSet;
		private ClassesTableAdapter classesTableAdapter;
		private TeachersTableAdapter teachersTableAdapter;
		private SubjectsTableAdapter subjectsTableAdapter;
		private LessonsTableAdapter lessonsTableAdapter;

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
			timetableDataSet = new TimetableDataSet();
			classesTableAdapter = new ClassesTableAdapter();
			subjectsTableAdapter = new SubjectsTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();
			lessonsTableAdapter = new LessonsTableAdapter();

			classesTableAdapter.Fill(timetableDataSet.Classes);
			subjectsTableAdapter.Fill(timetableDataSet.Subjects);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);
			lessonsTableAdapter.Fill(timetableDataSet.Lessons);
		}

		private void FillComboBoxes()
		{
			comboBoxTeachers.ItemsSource = timetableDataSet.Teachers
				.OrderBy(t => t.LastName)
				.ThenBy(t => t.FirstName);
			comboBoxTeachers.SelectedValuePath = "Pesel";

			comboBoxClasses.ItemsSource = timetableDataSet.Classes
				.OrderBy(c => c.Year)
				.ThenBy(c => c.CodeName);
			comboBoxClasses.SelectedValuePath = "Id";

			comboBoxSubjects.ItemsSource = timetableDataSet.Subjects
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
						_currentLessonRow = timetableDataSet.Lessons.NewLessonsRow();
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

			var lessonsRow = timetableDataSet.Lessons.FindById(_currentLessonId);

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
				timetableDataSet.Lessons.Rows.Add(_currentLessonRow);
			}

			lessonsTableAdapter.Update(timetableDataSet.Lessons);

			_callingWindow.RefreshViews(EntityType.Lessons);

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
