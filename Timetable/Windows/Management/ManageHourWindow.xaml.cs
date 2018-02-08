using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.DAL.DataSet.MySql;
using Timetable.DAL.DataSet.MySql.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows.Management
{
	/// <summary>
	///     Interaction logic for ManageHourWindow.xaml
	/// </summary>
	public partial class ManageHourWindow : Window
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private TimetableDataSet _timetableDataSet;
		private HoursTableAdapter _hoursTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly ActionType _actionType;

		private int _currentHourId;
		private TimetableDataSet.HoursRow _currentHourRow;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>ManageHourWindow</c>.
		/// </summary>
		public ManageHourWindow(MainWindow mainWindow, ActionType actionType)
		{
			InitializeComponent();

			_callingWindow = mainWindow;
			_actionType = actionType;
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
			_timetableDataSet = new TimetableDataSet();
			_hoursTableAdapter = new HoursTableAdapter();

			_hoursTableAdapter.Fill(_timetableDataSet.Hours);
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_actionType)
				{
					case ActionType.Add:
						_currentHourRow = _timetableDataSet.Hours.NewHoursRow();
						break;
					case ActionType.Change:
						_currentHourRow = PrepareHour();
						break;
				}
			}
			catch (EntityDoesNotExistException)
			{
				ShowErrorMessageBox("Hour with given ID number does not exist.");
				Close();
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
				Close();
			}
		}

		private TimetableDataSet.HoursRow PrepareHour()
		{
			if (!int.TryParse(_callingWindow.GetIdNumbersOfMarkedHours().FirstOrDefault(), out _currentHourId))
			{
				throw new EntityDoesNotExistException();
			}

			var hourRow = _timetableDataSet.Hours.FindById(_currentHourId);

			if (hourRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return hourRow;
		}

		private void FillControls()
		{
			switch (_actionType)
			{
				case ActionType.Change:
					if (_currentHourRow == null)
						return;

					textBoxId.Text = _currentHourRow.Id.ToString();
					textBoxNumber.Text = _currentHourRow.Number.ToString();
					textBoxBegin.Text = _currentHourRow.Begin.ToString(@"hh\:mm");
					textBoxEnd.Text = _currentHourRow.End.ToString(@"hh\:mm");
					break;
			}

			buttonOk.IsEnabled = true;
			buttonCancel.IsEnabled = true;
		}

		private void SaveEntity()
		{
			var numberString = textBoxNumber.Text.Trim();
			var beginString = textBoxBegin.Text.Trim();
			var endString = textBoxEnd.Text.Trim();

			try
			{
				SaveDay(numberString, beginString, endString);
			}
			catch (FieldsNotFilledException)
			{
				ShowWarningMessageBox("All fields are required.");
			}
			catch (FormatException)
			{
				ShowWarningMessageBox("Some fields are invalid.");
			}
			catch (InvalidOperationException)
			{
				ShowWarningMessageBox("Begin is not before End.");
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void SaveDay(string numberString, string beginString, string endString)
		{
			if (string.IsNullOrEmpty(numberString)
				|| string.IsNullOrEmpty(beginString)
				|| string.IsNullOrEmpty(endString))
			{
				throw new FieldsNotFilledException();
			}

			int number = int.Parse(numberString);
			TimeSpan begin = TimeSpan.Parse(beginString);
			TimeSpan end = TimeSpan.Parse(endString);

			if (end <= begin)
			{
				throw new InvalidOperationException();
			}

			_currentHourRow.Number = number;
			_currentHourRow.Begin = begin;
			_currentHourRow.End = end;

			if (_actionType == ActionType.Add)
			{
				_timetableDataSet.Hours.Rows.Add(_currentHourRow);
			}

			_hoursTableAdapter.Update(_timetableDataSet.Hours);

			_callingWindow.RefreshViews(EntityType.Hour);

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
