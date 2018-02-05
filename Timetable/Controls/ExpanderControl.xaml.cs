using System;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;
using Timetable.Windows;
using ManageClassWindow = Timetable.Windows.Management.ManageClassWindow;
using ManagePersonWindow = Timetable.Windows.Management.ManagePersonWindow;
using ManageSubjectWindow = Timetable.Windows.Management.ManageSubjectWindow;
using MappingWindow = Timetable.Windows.Mapping.MappingWindow;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for ExpanderControl.xaml
	/// </summary>
	public partial class ExpanderControl : UserControl
	{
		#region Constants and Statics

		private const string SEPARATOR = "\n\u2022 ";

		#endregion


		#region Fields

		private static TimetableDataSet _timetableDataSet;
		private static ClassesTableAdapter _classesTableAdapter;
		private static ClassroomsTableAdapter _classroomsTableAdapter;
		private static DaysTableAdapter _daysTableAdapter;
		private static HoursTableAdapter _hoursTableAdapter;
		private static LessonsPlacesTableAdapter _lessonsPlacesTableAdapter;
		private static LessonsTableAdapter _lessonsTableAdapter;
		private static StudentsTableAdapter _studentsTableAdapter;
		private static SubjectsTableAdapter _subjectsTableAdapter;
		private static TeachersTableAdapter _teachersTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly Export _exportEngine;
		private static bool _exportEngineWorking;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.ExpanderControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="mainWindow">Uchwyt do wywołującego okna.</param>
		/// <param name="actionType">Rodzaj wybranej akcji.</param>
		/// <param name="text">Tekst przycisku <c>button</c>.</param>
		public ExpanderControl(MainWindow mainWindow, ActionType actionType, string text)
		{
			InitializeComponent();

			_callingWindow = mainWindow;

			FillControls(actionType, text);

			InitDatabaseObjects();

			switch (actionType)
			{
				case ActionType.XLS:
				case ActionType.PDF:
					_exportEngine = new Export();
					_exportEngine.ExportFinishedEvent += ExportFinishedConfirmation;
					break;
			}
		}

		#endregion


		#region Events

		private async void AddButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.stackPanelOperations.expanderOperation.IsExpanded = false;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					ModifyEntities(ActionType.Add);
				});
			});
		}

		private async void ChangeButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.stackPanelOperations.expanderOperation.IsExpanded = false;

			if (!AreAnyEntitiesMarked())
				return;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					ModifyEntities(ActionType.Change);
				});
			});
		}

		private async void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.stackPanelOperations.expanderOperation.IsExpanded = false;

			if (!AreAnyEntitiesMarked())
				return;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(RemoveEntities);
			});
		}

		private async void RemoveLessonPlaceButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.stackPanelOperations.expanderPlanning.IsExpanded = false;

			if (!AreAnyEntitiesMarked())
				return;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(RemoveEntities);
			});
		}

		private async void XlsButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.stackPanelOperations.expanderExport.IsExpanded = false;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					ExportToFile(ExportFileType.XLS);
				});
			});
		}

		private async void PdfButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.stackPanelOperations.expanderExport.IsExpanded = false;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					ExportToFile(ExportFileType.PDF);
				});
			});
		}

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		#endregion


		#region Private methods

		private void FillControls(ActionType actionType, string text)
		{
			switch (actionType)
			{
				case ActionType.Add:
					image.Source = Properties.Resources.add.ToBitmapImage();
					button.Click += AddButton_Click;
					break;
				case ActionType.Change:
					image.Source = Properties.Resources.manage.ToBitmapImage();
					button.Click += ChangeButton_Click;
					break;
				case ActionType.Remove:
					image.Source = Properties.Resources.delete.ToBitmapImage();
					button.Click += RemoveButton_Click;
					break;
				case ActionType.RemoveLessonPlace:
					image.Source = Properties.Resources.delete.ToBitmapImage();
					button.Click += RemoveLessonPlaceButton_Click;
					break;
				case ActionType.XLS:
					image.Source = Properties.Resources.excel.ToBitmapImage();
					button.Click += XlsButton_Click;
					break;
				case ActionType.PDF:
					image.Source = Properties.Resources.pdf.ToBitmapImage();
					button.Click += PdfButton_Click;
					break;
			}

			button.Content = text;
		}

		private static void InitDatabaseObjects()
		{
			_timetableDataSet = new TimetableDataSet();
			_classesTableAdapter = new ClassesTableAdapter();
			_classroomsTableAdapter = new ClassroomsTableAdapter();
			_daysTableAdapter = new DaysTableAdapter();
			_hoursTableAdapter = new HoursTableAdapter();
			_lessonsPlacesTableAdapter = new LessonsPlacesTableAdapter();
			_lessonsTableAdapter = new LessonsTableAdapter();
			_studentsTableAdapter = new StudentsTableAdapter();
			_subjectsTableAdapter = new SubjectsTableAdapter();
			_teachersTableAdapter = new TeachersTableAdapter();
		}

		private bool AreAnyEntitiesMarked()
		{
			switch (_callingWindow.GetCurrentEntityType())
			{
				case EntityType.Students:
				case EntityType.Teachers:
					return _callingWindow.GetPeselsOfMarkedPeople().Any();
				case EntityType.Classes:
					return _callingWindow.GetIdNumbersOfMarkedClasses().Any();
				case EntityType.Subjects:
					return _callingWindow.GetIdNumbersOfMarkedSubjects().Any();
				case EntityType.Lessons:
					return _callingWindow.GetIdNumbersOfMarkedLessons().Any();
				case EntityType.LessonsPlaces:
					return _callingWindow.GetCollectionOfMarkedLessonsPlaces().Any();
			}

			return false;
		}

		private void ModifyEntities(ActionType actionType)
		{
			switch (_callingWindow.GetCurrentEntityType())
			{
				case EntityType.Students:
				case EntityType.Teachers:
					var manageWindow = new ManagePersonWindow(_callingWindow, actionType);
					manageWindow.Owner = _callingWindow;
					manageWindow.Show();
					break;
				case EntityType.Classes:
					var manageClassWindow = new ManageClassWindow(_callingWindow, actionType);
					manageClassWindow.Owner = _callingWindow;
					manageClassWindow.Show();
					break;
				case EntityType.Subjects:
					var manageSubjectWindow = new ManageSubjectWindow(_callingWindow, actionType);
					manageSubjectWindow.Owner = _callingWindow;
					manageSubjectWindow.Show();
					break;
				case EntityType.Lessons:
					var mappingWindow = new MappingWindow(_callingWindow, actionType);
					mappingWindow.Owner = _callingWindow;
					mappingWindow.Show();
					break;
			}
		}

		private void RemoveEntities()
		{
			switch (_callingWindow.GetCurrentEntityType())
			{
				case EntityType.Students:
					RemoveStudents();
					break;
				case EntityType.Teachers:
					RemoveTeachers();
					break;
				case EntityType.Classes:
					RemoveClasses();
					break;
				case EntityType.Subjects:
					RemoveSubjects();
					break;
				case EntityType.Lessons:
					RemoveLessons();
					break;
				case EntityType.LessonsPlaces:
					RemoveLessonsPlaces();
					break;
			}
		}

		private void RemoveStudents()
		{
			try
			{
				if (ShowConfirmationMessageBox("Are you sure you want to remove marked students?") != MessageBoxResult.Yes)
					return;

				_studentsTableAdapter.Fill(_timetableDataSet.Students);

				foreach (var pesel in _callingWindow.GetPeselsOfMarkedPeople())
				{
					var studentRow = _timetableDataSet.Students.FindByPesel(pesel);

					if (studentRow == null)
						continue;

					studentRow.Delete();

					SetOdbcDeleteStudentCommand(pesel);

					_studentsTableAdapter.Update(_timetableDataSet.Students);
				}

				_callingWindow.RefreshViews(EntityType.Students);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void SetOdbcDeleteStudentCommand(string pesel)
		{
			OdbcConnection conn = new OdbcConnection(System.Configuration.ConfigurationManager
				.ConnectionStrings["Timetable.Properties.Settings.ConnectionString"].ConnectionString);

			OdbcCommand cmd = conn.CreateCommand();

			cmd.CommandText = "DELETE FROM students " +
							  "WHERE pesel = ?";

			cmd.Parameters.Add("pesel", OdbcType.VarChar).Value = pesel;

			_studentsTableAdapter.Adapter.DeleteCommand = cmd;
		}

		private void RemoveTeachers()
		{
			try
			{
				if (ShowConfirmationMessageBox("Are you sure you want to remove marked teachers?") != MessageBoxResult.Yes)
					return;

				_classesTableAdapter.Fill(_timetableDataSet.Classes);
				_classroomsTableAdapter.Fill(_timetableDataSet.Classrooms);
				_lessonsTableAdapter.Fill(_timetableDataSet.Lessons);
				_subjectsTableAdapter.Fill(_timetableDataSet.Subjects);
				_teachersTableAdapter.Fill(_timetableDataSet.Teachers);

				foreach (var pesel in _callingWindow.GetPeselsOfMarkedPeople())
				{
					var teacherRow = _timetableDataSet.Teachers.FindByPesel(pesel);

					if (teacherRow == null)
						continue;

					var classes = _timetableDataSet.Classes
						.Where(c => c.TutorPesel == pesel)
						.OrderBy(c => c.ToFriendlyString())
						.ToList();

					if (classes.Any())
					{
						ShowErrorMessageBox($"{teacherRow.ToFriendlyString()} is the tutor of classes:{SEPARATOR}" +
											$"{string.Join(SEPARATOR, classes.Select(c => c.ToFriendlyString()))}");
						continue;
					}

					var classrooms = _timetableDataSet.Classrooms
						.Where(cr => cr.AdministratorPesel == pesel)
						.OrderBy(cr => cr.Name)
						.ToList();

					if (classrooms.Any())
					{
						ShowErrorMessageBox($"{teacherRow.ToFriendlyString()} is the administrator of classrooms:{SEPARATOR}" +
											$"{string.Join(SEPARATOR, classrooms.Select(cr => cr.Name))}");
						continue;
					}

					var lessons = _timetableDataSet.Lessons
						.Where(l => l.TeacherPesel == pesel)
						.OrderBy(l => l.SubjectsRow.Name)
						.ThenBy(l => l.ClassesRow.ToFriendlyString())
						.ToList();

					if (lessons.Any())
					{
						ShowErrorMessageBox($"{teacherRow.ToFriendlyString()} has the following lessons:{SEPARATOR}" +
											$"{string.Join(SEPARATOR, lessons.Select(l => l.SubjectsRow.Name))}");
						continue;
					}

					teacherRow.Delete();

					_teachersTableAdapter.Update(_timetableDataSet.Teachers);
				}

				_callingWindow.RefreshViews(EntityType.Teachers);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void RemoveClasses()
		{
			try
			{
				if (ShowConfirmationMessageBox("Are you sure you want to remove marked classes?") != MessageBoxResult.Yes)
					return;

				_classesTableAdapter.Fill(_timetableDataSet.Classes);
				_studentsTableAdapter.Fill(_timetableDataSet.Students);
				_teachersTableAdapter.Fill(_timetableDataSet.Teachers);

				foreach (var id in _callingWindow.GetIdNumbersOfMarkedClasses())
				{
					int classId;

					if (!int.TryParse(id, out classId))
						continue;

					var classRow = _timetableDataSet.Classes.FindById(classId);

					if (classRow == null)
						continue;

					var students = _timetableDataSet.Students
						.Where(s => s.ClassId == classId)
						.ToList();

					if (students.Any())
					{
						ShowErrorMessageBox($"Class {classRow.ToFriendlyString()} " +
											$"has {students.Count} assigned students.");
						continue;
					}

					var tutor = _timetableDataSet.Teachers.FindByPesel(classRow.TutorPesel);

					if (tutor != null)
					{
						ShowErrorMessageBox($"Class {classRow.ToFriendlyString()} " +
											$"is assigned to the teacher: {tutor.ToFriendlyString()}.");
						continue;
					}

					classRow.Delete();

					SetOdbcDeleteClassCommand(classId);

					_classesTableAdapter.Update(_timetableDataSet.Classes);
				}

				_callingWindow.RefreshViews(EntityType.Classes);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void SetOdbcDeleteClassCommand(int id)
		{
			OdbcConnection conn = new OdbcConnection(System.Configuration.ConfigurationManager
				.ConnectionStrings["Timetable.Properties.Settings.ConnectionString"].ConnectionString);

			OdbcCommand cmd = conn.CreateCommand();

			cmd.CommandText = "DELETE FROM classes " +
							  "WHERE id = ?";

			cmd.Parameters.Add("id", OdbcType.Int).Value = id;

			_classesTableAdapter.Adapter.DeleteCommand = cmd;
		}

		private void RemoveSubjects()
		{
			try
			{
				if (ShowConfirmationMessageBox("Are you sure you want to remove marked subjects?") != MessageBoxResult.Yes)
					return;

				_lessonsTableAdapter.Fill(_timetableDataSet.Lessons);
				_subjectsTableAdapter.Fill(_timetableDataSet.Subjects);

				foreach (var id in _callingWindow.GetIdNumbersOfMarkedSubjects())
				{
					int subjectId;

					if (!int.TryParse(id, out subjectId))
						continue;

					var subjectRow = _timetableDataSet.Subjects.FindById(subjectId);

					if (subjectRow == null)
						continue;

					var lessons = _timetableDataSet.Lessons
						.Where(l => l.SubjectId == subjectId)
						.ToList();

					if (lessons.Any())
					{
						ShowErrorMessageBox($"Subject {subjectRow.Name} " +
											$"is assigned to {lessons.Count} lessons.");
						continue;
					}

					subjectRow.Delete();

					_subjectsTableAdapter.Update(_timetableDataSet.Subjects);
				}

				_callingWindow.RefreshViews(EntityType.Subjects);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void RemoveLessons()
		{
			try
			{
				if (ShowConfirmationMessageBox("Are you sure you want to remove marked lessons?") != MessageBoxResult.Yes)
					return;

				_classesTableAdapter.Fill(_timetableDataSet.Classes);
				_lessonsPlacesTableAdapter.Fill(_timetableDataSet.LessonsPlaces);
				_lessonsTableAdapter.Fill(_timetableDataSet.Lessons);
				_subjectsTableAdapter.Fill(_timetableDataSet.Subjects);
				_teachersTableAdapter.Fill(_timetableDataSet.Teachers);

				foreach (var id in _callingWindow.GetIdNumbersOfMarkedLessons())
				{
					int lessonId;

					if (!int.TryParse(id, out lessonId))
						continue;

					var lessonRow = _timetableDataSet.Lessons.FindById(lessonId);

					if (lessonRow == null)
						continue;

					var lessonsPlaces = _timetableDataSet.LessonsPlaces
						.Where(lp => lp.LessonId == lessonId)
						.ToList();

					if (lessonsPlaces.Any())
					{
						ShowErrorMessageBox($"Lesson with the following attributes:{SEPARATOR}" +
											$"teacher:\t{lessonRow.TeachersRow.ToFriendlyString()}{SEPARATOR}" +
											$"subject:\t{lessonRow.SubjectsRow.Name}{SEPARATOR}" +
											$"class:\t{lessonRow.ClassesRow.ToFriendlyString()}\n" +
											$"has {lessonsPlaces.Count} occurrences on the timetable.");
						continue;
					}

					lessonRow.Delete();

					_lessonsTableAdapter.Update(_timetableDataSet.Lessons);
				}

				_callingWindow.RefreshViews(EntityType.Lessons);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void RemoveLessonsPlaces()
		{
			try
			{
				if (ShowConfirmationMessageBox("Are you sure you want to remove marked lessons?") != MessageBoxResult.Yes)
					return;

				_classesTableAdapter.Fill(_timetableDataSet.Classes);
				_classroomsTableAdapter.Fill(_timetableDataSet.Classrooms);
				_daysTableAdapter.Fill(_timetableDataSet.Days);
				_hoursTableAdapter.Fill(_timetableDataSet.Hours);
				_lessonsPlacesTableAdapter.Fill(_timetableDataSet.LessonsPlaces);
				_lessonsTableAdapter.Fill(_timetableDataSet.Lessons);
				_subjectsTableAdapter.Fill(_timetableDataSet.Subjects);
				_teachersTableAdapter.Fill(_timetableDataSet.Teachers);

				foreach (var cellViewModel in _callingWindow.GetCollectionOfMarkedLessonsPlaces())
				{
					if (cellViewModel.LessonId == null || cellViewModel.ClassroomId == null)
						continue;

					var lessonPlaceRow = _timetableDataSet.LessonsPlaces
						.FindByLessonIdClassroomIdDayIdHourId(
						(int) cellViewModel.LessonId,
						(int) cellViewModel.ClassroomId,
						cellViewModel.DayId,
						cellViewModel.HourId);

					if (lessonPlaceRow == null)
						continue;

					lessonPlaceRow.Delete();

					_lessonsPlacesTableAdapter.Update(_timetableDataSet.LessonsPlaces);
				}

				_callingWindow.RefreshViews(EntityType.LessonsPlaces);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private TimetableDataSet.ClassesRow GetCurrentClass()
		{
			int? classId = _callingWindow.GetSummaryClassId();

			if (classId == null)
				throw new EntityDoesNotExistException("Class does not exists");

			_classesTableAdapter.Fill(_timetableDataSet.Classes);

			var classRow = _timetableDataSet.Classes.FirstOrDefault(c => c.Id == classId.Value);

			if (classRow == null)
				throw new EntityDoesNotExistException("Class with id=" + classId.Value + " does not exists");

			return classRow;
		}

		private TimetableDataSet.TeachersRow GetCurrentTeacher()
		{
			string teacherPesel = _callingWindow.GetSummaryTeacherPesel();

			if (string.IsNullOrEmpty(teacherPesel))
				throw new EntityDoesNotExistException("Teacher does not exists");

			_teachersTableAdapter.Fill(_timetableDataSet.Teachers);

			var teacherRow = _timetableDataSet.Teachers.FirstOrDefault(t => t.Pesel == teacherPesel);

			if (teacherRow == null)
				throw new EntityDoesNotExistException("Teacher with PESEL=" + teacherPesel + " does not exists");

			return teacherRow;
		}

		private TimetableDataSet.ClassroomsRow GetCurrentClassroom()
		{
			int? classroomId = _callingWindow.GetSummaryClassroomId();

			if (classroomId == null)
				throw new EntityDoesNotExistException("Classroom does not exists");

			_classroomsTableAdapter.Fill(_timetableDataSet.Classrooms);

			var classroomRow = _timetableDataSet.Classrooms.FirstOrDefault(cr => cr.Id == classroomId.Value);

			if (classroomRow == null)
			{
				throw new EntityDoesNotExistException("Classroom with id=" + classroomId.Value + " does not exists");
			}

			return classroomRow;
		}

		private async void ExportToFile(ExportFileType fileType)
		{
			if (_exportEngineWorking)
				return;

			TimetableDataSet.ClassesRow classRow = null;
			TimetableDataSet.TeachersRow teacherRow = null;
			TimetableDataSet.ClassroomsRow classroomRow = null;

			var entityType = _callingWindow.GetSummaryEntityType();
			var date = DateTime.Now.ToString("yyyyMMddTHHmmss");

			var saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			try
			{
				switch (entityType)
				{
					case EntityType.Classes:
						classRow = GetCurrentClass();
						saveFileDialog.FileName = $"Klasa {classRow.ToFriendlyString()} ({date})";
						break;
					case EntityType.Teachers:
						teacherRow = GetCurrentTeacher();
						saveFileDialog.FileName = $"{teacherRow.LastName} {teacherRow.FirstName} ({date})";
						break;
					case EntityType.Classrooms:
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
					_exportEngineWorking = true;

					switch (entityType)
					{
						case EntityType.Classes:
							_exportEngine.SaveTimeTableForClass(classRow, saveFileDialog.FileName, fileType);
							break;
						case EntityType.Teachers:
							_exportEngine.SaveTimeTableForTeacher(teacherRow, saveFileDialog.FileName, fileType);
							break;
						case EntityType.Classrooms:
							_exportEngine.SaveTimeTableForClassroom(classroomRow, saveFileDialog.FileName, fileType);
							break;
					}
				});

			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void ExportFinishedConfirmation()
		{
			Dispatcher.Invoke(() =>
			{
				_exportEngineWorking = false;

				ShowInformationMessageBox("Timetable exported successfully.");
			});
		}

		private MessageBoxResult ShowConfirmationMessageBox(string message)
		{
			return MessageBox.Show(_callingWindow, message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
		}

		private MessageBoxResult ShowErrorMessageBox(string message)
		{
			return MessageBox.Show(_callingWindow, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private MessageBoxResult ShowInformationMessageBox(string message)
		{
			return MessageBox.Show(_callingWindow, message, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private MessageBoxResult ShowWarningMessageBox(string message)
		{
			return MessageBox.Show(_callingWindow, message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		#endregion
	}
}
