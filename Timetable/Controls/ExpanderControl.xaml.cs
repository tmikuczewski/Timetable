using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;
using Timetable.Windows;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for ExpanderControl.xaml
	/// </summary>
	public partial class ExpanderControl : UserControl
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private TimetableDataSet timetableDataSet;
		private StudentsTableAdapter studentsTableAdapter;
		private TeachersTableAdapter teachersTableAdapter;
		private ClassesTableAdapter classesTableAdapter;
		private SubjectsTableAdapter subjectsTableAdapter;
		private LessonsTableAdapter lessonsTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly Export _exportEngine;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.ExpanderControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="mainWindow">Uchwyt do wywołującego okna.</param>
		/// <param name="controlType">Rodzaj wybranej akcji.</param>
		/// <param name="text">Tekst przycisku <c>button</c>.</param>
		public ExpanderControl(MainWindow mainWindow, ExpanderControlType controlType, string text)
		{
			InitDatabaseObjects();

			InitializeComponent();

			_callingWindow = mainWindow;

			button.Content = text;

			switch (controlType)
			{
				case ExpanderControlType.Add:
					image.Source = Properties.Resources.add.ToBitmapImage();
					button.Click += AddButton_Click;
					break;
				case ExpanderControlType.Change:
					image.Source = Properties.Resources.manage.ToBitmapImage();
					button.Click += ChangeButton_Click;
					break;
				case ExpanderControlType.Remove:
					image.Source = Properties.Resources.delete.ToBitmapImage();
					button.Click += RemoveButton_Click;
					break;
				case ExpanderControlType.XLS:
					image.Source = Properties.Resources.excel.ToBitmapImage();
					button.Click += XlsButton_Click;
					break;
				case ExpanderControlType.PDF:
					image.Source = Properties.Resources.pdf.ToBitmapImage();
					button.Click += PdfButton_Click;
					break;
			}

			_exportEngine = new Export();
			_exportEngine.ExportFinishedEvent += ExportFinishedConfirmation;
		}

		#endregion


		#region Events

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.expander.IsExpanded = false;

			ModifyEntities(ExpanderControlType.Add);
		}

		private void ChangeButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.expander.IsExpanded = false;

			ModifyEntities(ExpanderControlType.Change);
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.expander.IsExpanded = false;

			switch (_callingWindow.GetCurrentContentType())
			{
				case ComboBoxContentType.Students:
					RemoveStudents();
					break;
				case ComboBoxContentType.Teachers:
					RemoveTeachers();
					break;
				case ComboBoxContentType.Classes:
					RemoveClasses();
					break;
				case ComboBoxContentType.Subjects:
					RemoveSubjects();
					break;
				case ComboBoxContentType.Lessons:
					RemoveLessons();
					break;
			}

		}

		private void PdfButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.expander.IsExpanded = false;

			ExportToFile(ExportFileType.PDF);
		}

		private void XlsButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.expander.IsExpanded = false;

			ExportToFile(ExportFileType.XLS);
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
			studentsTableAdapter = new StudentsTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();
			classesTableAdapter = new ClassesTableAdapter();
			subjectsTableAdapter = new SubjectsTableAdapter();
			lessonsTableAdapter = new LessonsTableAdapter();

			studentsTableAdapter.Fill(timetableDataSet.Students);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);
			classesTableAdapter.Fill(timetableDataSet.Classes);
			subjectsTableAdapter.Fill(timetableDataSet.Subjects);
			lessonsTableAdapter.Fill(timetableDataSet.Lessons);
		}

		private void ModifyEntities(ExpanderControlType controlType)
		{
			switch (_callingWindow.GetCurrentContentType())
			{
				case ComboBoxContentType.Students:
				case ComboBoxContentType.Teachers:
					var manageWindow = new ManagePersonWindow(_callingWindow, controlType);
					manageWindow.Owner = _callingWindow;
					manageWindow.Show();
					break;
				case ComboBoxContentType.Classes:
					var manageClassWindow = new ManageClassWindow(_callingWindow, controlType);
					manageClassWindow.Owner = _callingWindow;
					manageClassWindow.Show();
					break;
				case ComboBoxContentType.Subjects:
					var manageSubjectWindow = new ManageSubjectWindow(_callingWindow, controlType);
					manageSubjectWindow.Owner = _callingWindow;
					manageSubjectWindow.Show();
					break;
				case ComboBoxContentType.Lessons:
					var mappingWindow = new MappingWindow(_callingWindow, controlType);
					mappingWindow.Owner = _callingWindow;
					mappingWindow.Show();
					break;
			}
		}

		private MessageBoxResult ShowRemoveConfirmationMessageBox()
		{
			return MessageBox.Show("Are you sure you want to remove these objects?", "Confirmation",
				MessageBoxButton.YesNo, MessageBoxImage.Question);
		}

		private void RemoveStudents()
		{
			try
			{
				if (ShowRemoveConfirmationMessageBox() != MessageBoxResult.Yes)
					return;

				studentsTableAdapter.Fill(timetableDataSet.Students);

				foreach (var pesel in _callingWindow.GetPeselsOfMarkedPeople())
				{
					timetableDataSet.Students.FindByPesel(pesel).Delete();
					studentsTableAdapter.Update(timetableDataSet.Students);
				}

				_callingWindow.RefreshCurrentView(ComboBoxContentType.Students);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void RemoveTeachers()
		{
			try
			{
				if (ShowRemoveConfirmationMessageBox() != MessageBoxResult.Yes)
					return;

				teachersTableAdapter.Fill(timetableDataSet.Teachers);

				foreach (var pesel in _callingWindow.GetPeselsOfMarkedPeople())
				{
					timetableDataSet.Teachers.FindByPesel(pesel).Delete();
					teachersTableAdapter.Update(timetableDataSet.Teachers);
				}

				_callingWindow.RefreshCurrentView(ComboBoxContentType.Teachers);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void RemoveClasses()
		{
			try
			{
				if (ShowRemoveConfirmationMessageBox() != MessageBoxResult.Yes)
					return;

				classesTableAdapter.Fill(timetableDataSet.Classes);

				foreach (var id in _callingWindow.GetIdNumbersOfMarkedClasses())
				{
					int idNumber;
					int.TryParse(id, out idNumber);
					timetableDataSet.Classes.FindById(idNumber).Delete();
					classesTableAdapter.Update(timetableDataSet.Classes);
				}

				_callingWindow.RefreshCurrentView(ComboBoxContentType.Classes);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void RemoveSubjects()
		{
			try
			{
				if (ShowRemoveConfirmationMessageBox() != MessageBoxResult.Yes)
					return;

				subjectsTableAdapter.Fill(timetableDataSet.Subjects);

				foreach (var id in _callingWindow.GetIdNumbersOfMarkedSubjects())
				{
					int idNumber;
					int.TryParse(id, out idNumber);
					timetableDataSet.Subjects.FindById(idNumber).Delete();
					subjectsTableAdapter.Update(timetableDataSet.Subjects);
				}

				_callingWindow.RefreshCurrentView(ComboBoxContentType.Subjects);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void RemoveLessons()
		{
			try
			{
				if (ShowRemoveConfirmationMessageBox() != MessageBoxResult.Yes)
					return;

				lessonsTableAdapter.Fill(timetableDataSet.Lessons);

				foreach (var id in _callingWindow.GetIdNumbersOfMarkedLessons())
				{
					int idNumber;
					int.TryParse(id, out idNumber);
					timetableDataSet.Lessons.FindById(idNumber).Delete();
					lessonsTableAdapter.Update(timetableDataSet.Lessons);
				}

				_callingWindow.RefreshCurrentView(ComboBoxContentType.Lessons);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private TimetableDataSet.ClassesRow GetCurrentClass()
		{
			int? classId = _callingWindow.GetSummaryClassId();

			if (classId == null)
				throw new EntityDoesNotExistException("Class with id=" + classId.Value + " does not exists");

			var classRow = timetableDataSet.Classes.FirstOrDefault(c => c.Id == classId.Value);

			if (classRow == null)
				throw new EntityDoesNotExistException("Class with id=" + classId.Value + " does not exists");

			return classRow;
		}

		private TimetableDataSet.TeachersRow GetCurrentTeacher()
		{
			string teacherPesel = _callingWindow.GetSummaryTeacherPesel();

			if (string.IsNullOrEmpty(teacherPesel))
				throw new EntityDoesNotExistException("Teacher with PESEL=" + teacherPesel + " does not exists");

			var teacherRow = timetableDataSet.Teachers.FirstOrDefault(t => t.Pesel == teacherPesel);

			if (teacherRow == null)
				throw new EntityDoesNotExistException("Teacher with PESEL=" + teacherPesel + " does not exists");

			return teacherRow;
		}

		private TimetableDataSet.ClassroomsRow GetCurrentClassroom()
		{
			int? classroomId = -1; //callingWindow.GetSummaryClassroomId();

			if (classroomId == null)
				throw new EntityDoesNotExistException("Classroom with id=" + classroomId.Value + " does not exists");

			var classroomRow = timetableDataSet.Classrooms.FirstOrDefault(cr => cr.Id == classroomId.Value);

			if (classroomRow == null)
			{
				throw new EntityDoesNotExistException("Classroom with id=" + classroomId.Value + " does not exists");
			}

			return classroomRow;
		}

		private async void ExportToFile(ExportFileType fileType)
		{
			TimetableDataSet.ClassesRow classRow = null;
			TimetableDataSet.TeachersRow teacherRow = null;
			TimetableDataSet.ClassroomsRow classroomRow = null;

			var contentType = _callingWindow.GetSummaryContentType();
			var date = DateTime.Now.ToString("yyyyMMddTHHmmss");

			var saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			try
			{
				switch (contentType)
				{
					case ComboBoxContentType.Classes:
						classRow = GetCurrentClass();
						saveFileDialog.FileName = $"Klasa {classRow.ToFriendlyString()} ({date})";
						break;
					case ComboBoxContentType.Teachers:
						teacherRow = GetCurrentTeacher();
						saveFileDialog.FileName = $"{teacherRow.LastName} {teacherRow.FirstName} ({date})";
						break;
					case ComboBoxContentType.Classrooms:
						classroomRow = GetCurrentClassroom();
						saveFileDialog.FileName = $"Sala {classroomRow.Name} ({date})";
						break;
				}

				switch (fileType)
				{
					case ExportFileType.XLS:
						saveFileDialog.Filter = "Excel Files|*.xls";
						break;
					case ExportFileType.PDF:
						saveFileDialog.Filter = "PDF Files|*.pdf";
						break;
				}

				if (saveFileDialog.ShowDialog() == false
					|| string.IsNullOrEmpty(saveFileDialog.FileName))
				{
					return;
				}


				await Task.Factory.StartNew(() =>
				{
					switch (contentType)
					{
						case ComboBoxContentType.Classes:
							_exportEngine.SaveTimeTableForClass(classRow, saveFileDialog.FileName, fileType);
							break;
						case ComboBoxContentType.Teachers:
							_exportEngine.SaveTimeTableForTeacher(teacherRow, saveFileDialog.FileName, fileType);
							break;
						case ComboBoxContentType.Classrooms:
							_exportEngine.SaveTimeTableForClassroom(classroomRow, saveFileDialog.FileName, fileType);
							break;
					}
				});

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ExportFinishedConfirmation()
		{
			Dispatcher.Invoke(() =>
			{
				MessageBox.Show(_callingWindow, "Timetable exported successfully.", "Confirmation", MessageBoxButton.OK,
					MessageBoxImage.Information);
			});
		}

		#endregion
	}
}
