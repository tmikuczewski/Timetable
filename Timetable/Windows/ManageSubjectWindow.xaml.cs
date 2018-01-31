using System;
using System.Linq;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows
{
	/// <summary>
	///     Interaction logic for ManageSubjectWindow.xaml
	/// </summary>
	public partial class ManageSubjectWindow : Window
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private TimetableDataSet timetableDataSet;
		private SubjectsTableAdapter subjectsTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly ExpanderControlType _controlType;

		private int _currentSubjectId;
		private TimetableDataSet.SubjectsRow _currentSubjectRow;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>ManageClassWindow</c>.
		/// </summary>
		public ManageSubjectWindow(MainWindow mainWindow, ExpanderControlType controlType)
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
			subjectsTableAdapter = new SubjectsTableAdapter();

			subjectsTableAdapter.Fill(timetableDataSet.Subjects);
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_controlType)
				{
					case ExpanderControlType.Add:
						_currentSubjectRow = timetableDataSet.Subjects.NewSubjectsRow();
						break;
					case ExpanderControlType.Change:
						_currentSubjectRow = PrepareSubject();
						break;
				}
			}
			catch (EntityDoesNotExistException)
			{
				MessageBox.Show(this, "Subject with given ID number does not exist.", "Error",
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

		private TimetableDataSet.SubjectsRow PrepareSubject()
		{
			if (!int.TryParse(_callingWindow.GetIdNumbersOfMarkedSubjects().FirstOrDefault(), out _currentSubjectId))
			{
				throw new EntityDoesNotExistException();
			}

			var subjectRowe = timetableDataSet.Subjects.FindById(_currentSubjectId);

			if (subjectRowe == null)
			{
				throw new EntityDoesNotExistException();
			}

			return subjectRowe;
		}

		private void FillControls()
		{
			switch (_controlType)
			{
				case ExpanderControlType.Change:
					textBoxId.Text = _currentSubjectRow.Id.ToString();
					textBoxName.Text = _currentSubjectRow.Name;
					break;
			}
		}

		private void SaveEntity()
		{
			var name = textBoxName.Text.Trim();

			try
			{
				SaveSubject(name);
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

		private void SaveSubject(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new FieldsNotFilledException();
			}

			_currentSubjectRow.Name = name;

			if (_controlType == ExpanderControlType.Add)
			{
				timetableDataSet.Subjects.Rows.Add(_currentSubjectRow);
			}

			subjectsTableAdapter.Update(timetableDataSet.Subjects);

			_callingWindow.RefreshCurrentView(ComboBoxContentType.Subjects);

			Close();
		}

		#endregion
	}
}
