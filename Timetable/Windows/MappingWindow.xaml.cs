using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows
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
		private readonly ExpanderControlType _controlType;

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
		public MappingWindow(MainWindow mainWindow, ExpanderControlType controlType)
		{
			InitDatabaseObjects();

			InitializeComponent();

			_callingWindow = mainWindow;
			_controlType = controlType;
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
			comboBoxTeachers.ItemsSource = timetableDataSet.Teachers.OrderBy(t => new Pesel(t.Pesel).BirthDate);
			comboBoxTeachers.SelectedValuePath = "Pesel";

			comboBoxClasses.ItemsSource = timetableDataSet.Classes.OrderBy(c => c.Year).ThenBy(c => c.CodeName);
			comboBoxClasses.SelectedValuePath = "Id";

			comboBoxSubjects.ItemsSource = timetableDataSet.Subjects.OrderBy(s => s.Name);
			comboBoxSubjects.SelectedValuePath = "Id";
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_controlType)
				{
					case ExpanderControlType.Add:
						_currentLessonRow = timetableDataSet.Lessons.NewLessonsRow();
						break;
					case ExpanderControlType.Change:
						_currentLessonRow = PrepareLesson();
						break;
				}
			}
			catch (EntityDoesNotExistException)
			{
				MessageBox.Show(this, "Lesson with given ID number does not exist.", "Error",
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
			switch (_controlType)
			{
				case ExpanderControlType.Change:
					comboBoxClasses.SelectedValue = _currentLessonRow.ClassId;
					comboBoxSubjects.SelectedValue = _currentLessonRow.SubjectId;
					comboBoxTeachers.SelectedValue = _currentLessonRow.TeacherPesel;
					break;
			}
		}

		private void SaveEntity()
		{
			try
			{
				SaveLesson();
			}
			catch (FieldsNotFilledException)
			{
				MessageBox.Show(this, "All fields are required.", "Warning",
					MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString(), "Error",
					MessageBoxButton.OK, MessageBoxImage.Error);
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


			if (_controlType == ExpanderControlType.Add)
			{
				timetableDataSet.Lessons.Rows.Add(_currentLessonRow);
			}

			lessonsTableAdapter.Update(timetableDataSet.Lessons);

			_callingWindow.RefreshCurrentView(ComboBoxContentType.Lessons);

			Close();
		}

		#endregion
	}
}
