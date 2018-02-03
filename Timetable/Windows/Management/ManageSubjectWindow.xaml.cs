using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows.Management
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
			InitializeComponent();

			_callingWindow = mainWindow;
			_controlType = controlType;
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
				ShowErrorMessageBox("Subject with given ID number does not exist.");
				Close();
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
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
					if (_currentSubjectRow == null)
						return;

					textBoxId.Text = _currentSubjectRow.Id.ToString();
					textBoxName.Text = _currentSubjectRow.Name;
					break;
			}

			buttonOk.IsEnabled = true;
			buttonCancel.IsEnabled = true;
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
				ShowWarningMessageBox("All fields are required.");
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
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

			_callingWindow.RefreshViews(ComboBoxContentType.Subjects);

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
