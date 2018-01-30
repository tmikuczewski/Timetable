using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
	/// Interaction logic for ExpanderControl.xaml
	/// </summary>
	public partial class ExpanderControl : UserControl
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>Controls.ExpanderControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
		/// <param name="text">Tekst przycisku <c>button</c>.</param>
		/// <param name="ect"></param>
		/// <param name="window"></param>
		public ExpanderControl(string text, ExpanderControlType ect, MainWindow window)
		{
			InitializeComponent();

			this.button.Content = text;
			this.callingWindow = window;
			this.exportEngine = new Export();
			exportEngine.ExportFinishedEvent += ExportFinishedConfirmation;

			switch (ect)
			{
				case ExpanderControlType.Add:
					this.image.Source = Properties.Resources.add.ToBitmapImage();
					this.button.Click += AddButton_Click;
					break;
				case ExpanderControlType.Change:
					this.image.Source = Properties.Resources.manage.ToBitmapImage();
					this.button.Click += ChangeButton_Click;
					break;
				case ExpanderControlType.Remove:
					this.image.Source = Properties.Resources.delete.ToBitmapImage();
					this.button.Click += RemoveButton_Click;
					break;
				case ExpanderControlType.XLS:
					this.image.Source = Properties.Resources.excel.ToBitmapImage();
					this.button.Click += XlsButton_Click;
					break;
				case ExpanderControlType.PDF:
					this.image.Source = Properties.Resources.pdf.ToBitmapImage();
					this.button.Click += PdfButton_Click;
					break;
			}

			timetableDataSet = new TimetableDataSet();

			classesTableAdapter = new ClassesTableAdapter();
			studentsTableAdapter = new StudentsTableAdapter();
			subjectsTableAdapter = new SubjectsTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();
			lessonsTableAdapter = new LessonsTableAdapter();

			classesTableAdapter.Fill(timetableDataSet.Classes);
			studentsTableAdapter.Fill(timetableDataSet.Students);
			subjectsTableAdapter.Fill(timetableDataSet.Subjects);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);
			lessonsTableAdapter.Fill(timetableDataSet.Lessons);
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		#endregion

		#region Private methods

		private void ExportFinishedConfirmation()
		{
			Dispatcher.Invoke(() =>
			{
				MessageBox.Show(callingWindow, "Timetable exported successfully.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
			});
		}

		#endregion

		#region Events

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			if (callingWindow.GetCurrentContentType() == ComboBoxContent.Students
				|| callingWindow.GetCurrentContentType() == ComboBoxContent.Teachers)
			{
				ManagePersonWindow manageWindow = new ManagePersonWindow(callingWindow, ExpanderControlType.Add);
				manageWindow.Owner = callingWindow;
				manageWindow.Show();
			}
			if (callingWindow.GetCurrentContentType() == ComboBoxContent.Classes)
			{
				ManageClassWindow manageClassWindow = new ManageClassWindow(callingWindow, ExpanderControlType.Add);
				manageClassWindow.Owner = callingWindow;
				manageClassWindow.Show();
			}
			if (callingWindow.GetCurrentContentType() == ComboBoxContent.Subjects)
			{
				ManageSubjectWindow manageSubjectWindow = new ManageSubjectWindow(callingWindow, ExpanderControlType.Add);
				manageSubjectWindow.Owner = callingWindow;
				manageSubjectWindow.Show();
			}
			if (callingWindow.GetCurrentContentType() == ComboBoxContent.Mapping)
			{
				MappingWindow mappingWindow = new MappingWindow(callingWindow, ExpanderControlType.Add);
				mappingWindow.Owner = callingWindow;
				mappingWindow.Show();
			}
		}

		private void ChangeButton_Click(object sender, RoutedEventArgs e)
		{
			if (callingWindow.GetCurrentContentType() == ComboBoxContent.Students
				|| callingWindow.GetCurrentContentType() == ComboBoxContent.Teachers)
			{
				ManagePersonWindow manageWindow = new ManagePersonWindow(callingWindow, ExpanderControlType.Change);
				manageWindow.Owner = callingWindow;
				manageWindow.Show();
			}
			if (callingWindow.GetCurrentContentType() == ComboBoxContent.Classes)
			{
				ManageClassWindow manageClassWindow = new ManageClassWindow(callingWindow, ExpanderControlType.Change);
				manageClassWindow.Owner = callingWindow;
				manageClassWindow.Show();
			}
			if (callingWindow.GetCurrentContentType() == ComboBoxContent.Subjects)
			{
				ManageSubjectWindow manageSubjectWindow = new ManageSubjectWindow(callingWindow, ExpanderControlType.Change);
				manageSubjectWindow.Owner = callingWindow;
				manageSubjectWindow.Show();
			}
			if (callingWindow.GetCurrentContentType() == ComboBoxContent.Mapping)
			{
				MappingWindow mappingWindow = new MappingWindow(callingWindow, ExpanderControlType.Change);
				mappingWindow.Owner = callingWindow;
				mappingWindow.Show();
			}
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (callingWindow.GetCurrentContentType() == ComboBoxContent.Students)
				{
					studentsTableAdapter.Fill(timetableDataSet.Students);

					foreach (string pesel in callingWindow.GetPeselsOfMarkedPeople())
					{
						timetableDataSet.Students.FindByPesel(pesel).Delete();
						studentsTableAdapter.Update(timetableDataSet.Students);
					}
				}
				if (callingWindow.GetCurrentContentType() == ComboBoxContent.Teachers)
				{
					teachersTableAdapter.Fill(timetableDataSet.Teachers);

					foreach (string pesel in callingWindow.GetPeselsOfMarkedPeople())
					{
						timetableDataSet.Teachers.FindByPesel(pesel).Delete();
						teachersTableAdapter.Update(timetableDataSet.Teachers);
					}
				}
				if (callingWindow.GetCurrentContentType() == ComboBoxContent.Classes)
				{
					classesTableAdapter.Fill(timetableDataSet.Classes);

					foreach (string id in callingWindow.GetIdNumbersOfMarkedClasses())
					{
						int idNumber;
						int.TryParse(id, out idNumber);
						timetableDataSet.Classes.FindById(idNumber).Delete();
						classesTableAdapter.Update(timetableDataSet.Classes);
					}
				}
				if (callingWindow.GetCurrentContentType() == ComboBoxContent.Subjects)
				{
					subjectsTableAdapter.Fill(timetableDataSet.Subjects);

					foreach (string id in callingWindow.GetIdNumbersOfMarkedSubjects())
					{
						int idNumber;
						int.TryParse(id, out idNumber);
						timetableDataSet.Subjects.FindById(idNumber).Delete();
						subjectsTableAdapter.Update(timetableDataSet.Subjects);
					}
				}
				if (callingWindow.GetCurrentContentType() == ComboBoxContent.Mapping)
				{
					lessonsTableAdapter.Fill(timetableDataSet.Lessons);

					foreach (string id in callingWindow.GetIdNumbersOfMarkedLessons())
					{
						int idNumber;
						int.TryParse(id, out idNumber);
						timetableDataSet.Lessons.FindById(idNumber).Delete();
						lessonsTableAdapter.Update(timetableDataSet.Lessons);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error");
			}

			this.callingWindow.RefreshCurrentView();
		}

		private void PdfButton_Click(object sender, RoutedEventArgs e)
		{
			ExportToFile(ExportFileType.PDF);
		}

		private void XlsButton_Click(object sender, RoutedEventArgs e)
		{
			ExportToFile(ExportFileType.XLS);
		}

		private async void ExportToFile(ExportFileType fileType)
		{
			this.callingWindow.expander.IsExpanded = false;

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			var date = DateTime.Now.ToString("yyyyMMddTHHmmss");
			ComboBoxContent contentType = callingWindow.GetSummaryContentType();
			int? classId = null;
			string teacherPesel = null;
			int? classroomId = null;

			switch (contentType)
			{
				case ComboBoxContent.Classes:
					classId = callingWindow.GetSummaryClassId();

					if (classId == null)
					{
						return;
					}

					var schoolClass = timetableDataSet.Classes.FirstOrDefault(c => c.Id == classId.Value);

					if (schoolClass == null)
					{
						throw new EntityDoesNotExistException("Class with id=" + classId.Value + " does not exists");
					}

					saveFileDialog.FileName = $"Klasa {schoolClass.ToFriendlyString()} ({date})";
					break;

				case ComboBoxContent.Teachers:
					teacherPesel = callingWindow.GetSummaryTeacherPesel();

					if (string.IsNullOrEmpty(teacherPesel))
					{
						return;
					}

					var teacher = timetableDataSet.Teachers.FirstOrDefault(t => t.Pesel == teacherPesel);

					if (teacher == null)
					{
						throw new EntityDoesNotExistException("Teacher with PESEL=" + teacherPesel + " does not exists");
					}

					saveFileDialog.FileName = $"{teacher.LastName} {teacher.FirstName} ({date})";
					break;

				/*case ComboBoxContent.Classrooms:
					classroomId = callingWindow.GetSummaryClassroomId();

					if (classroomId == null)
					{
						return;
					}

					var classroom = timetableDataSet.Classrooms.FirstOrDefault(cr => cr.Id == classroomId.Value);

					if (classroom == null)
					{
						throw new EntityDoesNotExistException("Classroom with id=" + classroomId.Value + " does not exists");
					}

					saveFileDialog.FileName = $"Sala {classroom.Name} ({date})";
					break;*/

				default:
					return;
			}

			switch (fileType)
			{
				case ExportFileType.XLS:
					saveFileDialog.Filter = "Excel Files|*.xls";
					break;
				case ExportFileType.PDF:
					saveFileDialog.Filter = "PDF Files|*.pdf";
					break;
				default:
					return;
			}

			if (saveFileDialog.ShowDialog() == true
				&& !string.IsNullOrEmpty(saveFileDialog.FileName))
			{
				switch (contentType)
				{
					case ComboBoxContent.Classes:
						await Task.Factory.StartNew(() => exportEngine.SaveTimeTableForClass(classId.Value, saveFileDialog.FileName, fileType));
						break;
					case ComboBoxContent.Teachers:
						await Task.Factory.StartNew(() => exportEngine.SaveTimeTableForTeacher(teacherPesel, saveFileDialog.FileName, fileType));
						break;
				}
			}
		}

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		private readonly MainWindow callingWindow;

		private Export exportEngine;

		private TimetableDataSet timetableDataSet;

		private ClassesTableAdapter classesTableAdapter;
		private StudentsTableAdapter studentsTableAdapter;
		private SubjectsTableAdapter subjectsTableAdapter;
		private TeachersTableAdapter teachersTableAdapter;
		private LessonsTableAdapter lessonsTableAdapter;

		#endregion
	}
}
