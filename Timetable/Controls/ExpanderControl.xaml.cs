using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Timetable.DAL.DataSets.MySql;
using Timetable.DAL.DataSets.MySql.TimetableDataSetTableAdapters;
using Timetable.DAL.Utilities;
using Timetable.Utilities;
using Timetable.Windows;
using Timetable.Windows.Management;
using Timetable.Windows.Mapping;

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

		private async void RemoveLessonsPlaceButton_Click(object sender, RoutedEventArgs e)
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
			_callingWindow.stackPanelOperations.expanderSummary.IsExpanded = false;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					ExportToFile(ActionType.XLS);
				});
			});
		}

		private async void PdfButton_Click(object sender, RoutedEventArgs e)
		{
			_callingWindow.stackPanelOperations.expanderSummary.IsExpanded = false;

			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					ExportToFile(ActionType.PDF);
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
				case ActionType.RemoveLessonsPlace:
					image.Source = Properties.Resources.delete.ToBitmapImage();
					button.Click += RemoveLessonsPlaceButton_Click;
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
				case EntityType.Class:
					return _callingWindow.GetIdNumbersOfMarkedClasses().Any();
				case EntityType.Student:
				case EntityType.Teacher:
					return _callingWindow.GetPeselsOfMarkedPeople().Any();
				case EntityType.Classroom:
					return _callingWindow.GetIdNumbersOfMarkedClassrooms().Any();
				case EntityType.Subject:
					return _callingWindow.GetIdNumbersOfMarkedSubjects().Any();
				case EntityType.Day:
					return _callingWindow.GetIdNumbersOfMarkedDays().Any();
				case EntityType.Hour:
					return _callingWindow.GetIdNumbersOfMarkedHours().Any();
				case EntityType.Lesson:
					return _callingWindow.GetIdNumbersOfMarkedLessons().Any();
				case EntityType.LessonsPlace:
					return _callingWindow.GetCollectionOfMarkedLessonsPlaces().Any();
			}

			return false;
		}

		private void ModifyEntities(ActionType actionType)
		{
			Window window;

			switch (_callingWindow.GetCurrentEntityType())
			{
				case EntityType.Class:
					window = new ManageClassWindow(_callingWindow, actionType);
					break;
				case EntityType.Student:
				case EntityType.Teacher:
					window = new ManagePersonWindow(_callingWindow, actionType);
					break;
				case EntityType.Classroom:
					window = new ManageClassroomWindow(_callingWindow, actionType);
					break;
				case EntityType.Subject:
					window = new ManageSubjectWindow(_callingWindow, actionType);
					break;
				case EntityType.Day:
					window = new ManageDayWindow(_callingWindow, actionType);
					break;
				case EntityType.Hour:
					window = new ManageHourWindow(_callingWindow, actionType);
					break;
				case EntityType.Lesson:
					window = new MappingWindow(_callingWindow, actionType);
					break;
				default:
					return;
			}

			window.Owner = _callingWindow;
			window.Show();
		}

		private void RemoveEntities()
		{
			switch (_callingWindow.GetCurrentEntityType())
			{
				case EntityType.Class:
					RemoveClasses();
					break;
				case EntityType.Student:
					RemoveStudents();
					break;
				case EntityType.Teacher:
					RemoveTeachers();
					break;
				case EntityType.Classroom:
					RemoveClassrooms();
					break;
				case EntityType.Subject:
					RemoveSubjects();
					break;
				case EntityType.Day:
					RemoveDays();
					break;
				case EntityType.Hour:
					RemoveHours();
					break;
				case EntityType.Lesson:
					RemoveLessons();
					break;
				case EntityType.LessonsPlace:
					RemoveLessonsPlaces();
					break;
			}
		}

		private void RemoveClasses()
		{
			try
			{
				if (ShowConfirmationMessageBox("Are you sure you want to remove marked classes?") != MessageBoxResult.Yes)
					return;

				_classesTableAdapter.Fill(_timetableDataSet.Classes);
				_lessonsTableAdapter.Fill(_timetableDataSet.Lessons);
				_studentsTableAdapter.Fill(_timetableDataSet.Students);

				foreach (var id in _callingWindow.GetIdNumbersOfMarkedClasses())
				{
					int classId;

					if (!int.TryParse(id, out classId))
						continue;

					var classRow = _timetableDataSet.Classes.FindById(classId);

					if (classRow == null)
						continue;

					var lessons = _timetableDataSet.Lessons
						.Where(s => s.ClassId == classId)
						.ToList();

					if (lessons.Any())
					{
						ShowErrorMessageBox($"Class {classRow.ToFriendlyString()} " +
											$"is assigned to {lessons.Count} lesson" +
											$"{((lessons.Count == 1) ? string.Empty : "s")}.");
						continue;
					}

					var students = _timetableDataSet.Students
						.Where(s => s.ClassId == classId)
						.ToList();

					if (students.Any())
					{
						ShowErrorMessageBox($"Class {classRow.ToFriendlyString()} " +
											$"is assigned to {students.Count} student" +
											$"{((students.Count == 1) ? string.Empty : "s")}.");
						continue;
					}

					classRow.Delete();
				}

				_classesTableAdapter.Update(_timetableDataSet.Classes);

				_callingWindow.RefreshViews(EntityType.Class);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
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

					studentRow?.Delete();
				}

				_studentsTableAdapter.Update(_timetableDataSet.Students);

				_callingWindow.RefreshViews(EntityType.Student);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
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
						ShowErrorMessageBox($"{teacherRow.ToFriendlyString()} is the tutor of the following classes:{SEPARATOR}" +
											$"{string.Join(SEPARATOR, classes.Select(c => c.ToFriendlyString()))}");
						continue;
					}

					var classrooms = _timetableDataSet.Classrooms
						.Where(cr => cr.AdministratorPesel == pesel)
						.OrderBy(cr => cr.Name)
						.ToList();

					if (classrooms.Any())
					{
						ShowErrorMessageBox($"{teacherRow.ToFriendlyString()} is the administrator of the following classrooms:{SEPARATOR}" +
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
											$"{string.Join(SEPARATOR, lessons.Select(l => l.SubjectsRow.Name + " - " + l.ClassesRow.ToFriendlyString()))}");
						continue;
					}

					teacherRow.Delete();
				}

				_teachersTableAdapter.Update(_timetableDataSet.Teachers);

				_callingWindow.RefreshViews(EntityType.Teacher);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void RemoveClassrooms()
		{
			try
			{
				if (ShowConfirmationMessageBox("Are you sure you want to remove marked classrooms?") != MessageBoxResult.Yes)
					return;

				_classroomsTableAdapter.Fill(_timetableDataSet.Classrooms);
				_lessonsPlacesTableAdapter.Fill(_timetableDataSet.LessonsPlaces);

				foreach (var id in _callingWindow.GetIdNumbersOfMarkedClassrooms())
				{
					int classroomId;

					if (!int.TryParse(id, out classroomId))
						continue;

					var classroomRow = _timetableDataSet.Classrooms.FindById(classroomId);

					if (classroomRow == null)
						continue;

					var lessonsPlaces = _timetableDataSet.LessonsPlaces
						.Where(lp => lp.ClassroomId == classroomId)
						.ToList();

					if (lessonsPlaces.Any())
					{
						ShowErrorMessageBox($"Classroom {classroomRow.Name} " +
											$"is assigned to {lessonsPlaces.Count} lesson" +
											$"{((lessonsPlaces.Count == 1) ? string.Empty : "s")} on the timetable.");
						continue;
					}

					classroomRow.Delete();
				}

				_classroomsTableAdapter.Update(_timetableDataSet.Classrooms);

				_callingWindow.RefreshViews(EntityType.Classroom);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
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
											$"is assigned to {lessons.Count} lesson" +
											$"{((lessons.Count == 1) ? string.Empty : "s")}.");
						continue;
					}

					subjectRow.Delete();
				}

				_subjectsTableAdapter.Update(_timetableDataSet.Subjects);

				_callingWindow.RefreshViews(EntityType.Subject);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void RemoveDays()
		{
			try
			{
				if (ShowConfirmationMessageBox("Are you sure you want to remove marked days?") != MessageBoxResult.Yes)
					return;

				_daysTableAdapter.Fill(_timetableDataSet.Days);
				_lessonsPlacesTableAdapter.Fill(_timetableDataSet.LessonsPlaces);

				foreach (var id in _callingWindow.GetIdNumbersOfMarkedDays())
				{
					int dayId;

					if (!int.TryParse(id, out dayId))
						continue;

					var dayRow = _timetableDataSet.Days.FindById(dayId);

					if (dayRow == null)
						continue;

					var lessonsPlaces = _timetableDataSet.LessonsPlaces
						.Where(lp => lp.DayId == dayId)
						.ToList();

					if (lessonsPlaces.Any())
					{
						ShowErrorMessageBox($"Day {dayRow.Name} " +
											$"is assigned to {lessonsPlaces.Count} lesson" +
											$"{((lessonsPlaces.Count == 1) ? string.Empty : "s")} on the timetable.");
						continue;
					}

					dayRow.Delete();
				}

				_daysTableAdapter.Update(_timetableDataSet.Days);

				_callingWindow.RefreshViews(EntityType.Day);
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void RemoveHours()
		{
			try
			{
				if (ShowConfirmationMessageBox("Are you sure you want to remove marked hours?") != MessageBoxResult.Yes)
					return;

				_hoursTableAdapter.Fill(_timetableDataSet.Hours);
				_lessonsPlacesTableAdapter.Fill(_timetableDataSet.LessonsPlaces);

				foreach (var id in _callingWindow.GetIdNumbersOfMarkedHours())
				{
					int hourId;

					if (!int.TryParse(id, out hourId))
						continue;

					var hourRow = _timetableDataSet.Hours.FindById(hourId);

					if (hourRow == null)
						continue;

					var lessonsPlaces = _timetableDataSet.LessonsPlaces
						.Where(lp => lp.HourId == hourId)
						.ToList();

					if (lessonsPlaces.Any())
					{
						ShowErrorMessageBox($"Hour {hourRow.Begin:hh\\:mm} – {hourRow.End:hh\\:mm} " +
						                    $"is assigned to {lessonsPlaces.Count} lesson" +
						                    $"{((lessonsPlaces.Count == 1) ? string.Empty : "s")} on the timetable.");
						continue;
					}

					hourRow.Delete();
				}

				_hoursTableAdapter.Update(_timetableDataSet.Hours);

				_callingWindow.RefreshViews(EntityType.Hour);
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
											$"subject:\t{lessonRow.SubjectsRow.Name}{SEPARATOR}" +
											$"class:\t{lessonRow.ClassesRow.ToFriendlyString()}\n" +
											$"teacher:\t{lessonRow.TeachersRow.ToFriendlyString()}{SEPARATOR}" +
											$"has {lessonsPlaces.Count} occurrence" +
											$"{((lessonsPlaces.Count == 1) ? string.Empty : "s")} on the timetable.");
						continue;
					}

					lessonRow.Delete();
				}

				_lessonsTableAdapter.Update(_timetableDataSet.Lessons);

				_callingWindow.RefreshViews(EntityType.Lesson);
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

					var lessonsPlaceRow = _timetableDataSet.LessonsPlaces
						.FindByLessonIdClassroomIdDayIdHourId(
						(int) cellViewModel.LessonId,
						(int) cellViewModel.ClassroomId,
						cellViewModel.DayId,
						cellViewModel.HourId);

					if (lessonsPlaceRow == null)
						continue;

					lessonsPlaceRow.Delete();

					_lessonsPlacesTableAdapter.Update(_timetableDataSet.LessonsPlaces);
				}

				_callingWindow.RefreshViews(EntityType.LessonsPlace);
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
				throw new EntityDoesNotExistException("Class with ID = " + classId.Value + " does not exists");

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
				throw new EntityDoesNotExistException("Teacher with PESEL = " + teacherPesel + " does not exists");

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
				throw new EntityDoesNotExistException("Classroom with ID  = " + classroomId.Value + " does not exists");
			}

			return classroomRow;
		}

		private async void ExportToFile(ActionType fileType)
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
					case EntityType.Class:
						classRow = GetCurrentClass();
						saveFileDialog.FileName = $"Klasa {classRow.ToFriendlyString()} ({date})";
						break;
					case EntityType.Teacher:
						teacherRow = GetCurrentTeacher();
						saveFileDialog.FileName = $"{teacherRow.LastName} {teacherRow.FirstName} ({date})";
						break;
					case EntityType.Classroom:
						classroomRow = GetCurrentClassroom();
						saveFileDialog.FileName = $"Sala {classroomRow.Name} ({date})";
						break;
				}

				switch (fileType)
				{
					case ActionType.XLS:
						saveFileDialog.Filter = "Excel Files|*.xls";
						break;
					case ActionType.PDF:
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
						case EntityType.Class:
							_exportEngine.SaveTimetableForClass(classRow, saveFileDialog.FileName, fileType);
							break;
						case EntityType.Teacher:
							_exportEngine.SaveTimetableForTeacher(teacherRow, saveFileDialog.FileName, fileType);
							break;
						case EntityType.Classroom:
							_exportEngine.SaveTimetableForClassroom(classroomRow, saveFileDialog.FileName, fileType);
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
