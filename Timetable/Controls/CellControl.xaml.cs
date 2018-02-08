using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Timetable.Utilities;
using Timetable.Windows;
using Timetable.Windows.Planning;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for CellControl.xaml
	/// </summary>
	public partial class CellControl : UserControl
	{
		#region Constants and Statics

		/// <summary>
		///     Wysokość kontrolki planowanych lekcji.
		/// </summary>
		public const int HEIGHT = 80;

		private const int PIXELS_PER_LETTER = 7;

		#endregion


		#region Fields

		private readonly string
			_originalFirstRow,
			_originalSecondRow,
			_originalThirdRow;

		private readonly CellViewModel _cellViewModel;
		private readonly ActionType _actionType;
		private readonly EntityType _entityType;
		private readonly MainWindow _callingWindow;

		#endregion


		#region Properties

		/// <summary>
		///     Ustawienie marginesów umożliwiające zachowanie odstępu między kontrolkami.
		/// </summary>
		public static Thickness SEPARATOR_MARGIN => new Thickness(0, 20, 0, 0);

		/// <summary>
		///     Pierwszy wiersz kontrolki.
		/// </summary>
		public string FirstRow
		{
			get { return textBlockFirstRow.Text; }
			set
			{
				if (value != null)
					textBlockFirstRow.Text = value;
			}
		}

		/// <summary>
		///     Drugi wiersz kontrolki.
		/// </summary>
		public string SecondRow
		{
			get { return textBlockSecondRow.Text; }
			set { textBlockSecondRow.Text = value; }
		}

		/// <summary>
		///     Trzeci wiersz kontroli.
		/// </summary>
		public string ThirdRow
		{
			get { return textBlockThirdRow.Text; }
			set { textBlockThirdRow.Text = value; }
		}

		/// <summary>
		///     Zwraca obiekt typu <c>CellViewModel</c> zaznaczonej lekcji.
		/// </summary>
		/// <returns></returns>
		public CellViewModel CellViewModel => _cellViewModel;

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>CellControl</c>.
		/// </summary>
		/// <param name="mainWindow">Uchwyt do wywołującego okna.</param>
		/// <param name="diffColor">Parametr sterujący zmianą koloru kontrolki.</param>
		public CellControl(MainWindow mainWindow, bool diffColor = false)
		{
			InitializeComponent();

			_callingWindow = mainWindow;

			if (diffColor)
				Background = new SolidColorBrush(System.Drawing.SystemColors.InactiveBorder.ToMediaColor());
			else
				Background = new SolidColorBrush(System.Drawing.SystemColors.Info.ToMediaColor());

			FirstRow = _originalFirstRow = string.Empty;
			SecondRow = _originalSecondRow = string.Empty;
			ThirdRow = _originalThirdRow = string.Empty;
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>CellControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="firstRow">Tekst wypełniający pierwszy rząd.</param>
		/// <param name="secondRow">Tekst wypełniający drugi rząd.</param>
		/// <param name="thirdRow">Tekst wypełniający pierwszy rząd.</param>
		/// <param name="mainWindow">Uchwyt do wywołującego okna.</param>
		/// <param name="diffColor">Parametr sterujący zmianą koloru kontrolki.</param>
		public CellControl(string firstRow, string secondRow, string thirdRow, MainWindow mainWindow, bool diffColor = false)
			: this(mainWindow, diffColor)
		{
			FirstRow = _originalFirstRow = firstRow;
			SecondRow = _originalSecondRow = secondRow;
			ThirdRow = _originalThirdRow = thirdRow;
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>CellControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="cellViewModel">Obiekt przechowujący informacje o zaplanowanej lekcji.</param>
		/// <param name="actionType">Rodzaj wybranej akcji.</param>
		/// <param name="entityType">Rodzaj wybranej encji.</param>
		/// <param name="timetableType">Rodzaj wybranego podglądu.</param>
		/// <param name="mainWindow">Uchwyt do wywołującego okna.</param>
		/// <param name="diffColor">Parametr sterujący zmianą koloru kontrolki.</param>
		public CellControl(CellViewModel cellViewModel, ActionType actionType, EntityType entityType,
			TimetableType timetableType, MainWindow mainWindow, bool diffColor = false)
					: this(mainWindow, diffColor)
		{
			_cellViewModel = cellViewModel;

			_entityType = entityType;

			string className = (_cellViewModel.ClassFriendlyName != null) ? $"kl. {_cellViewModel.ClassFriendlyName}" : string.Empty;
			string classroom = (_cellViewModel.ClassroomName != null) ? $"s. {_cellViewModel.ClassroomName}" : string.Empty;
			string teacher = _cellViewModel.TeacherFriendlyName ?? string.Empty;
			string subject = _cellViewModel.SubjectName ?? string.Empty;

			switch (timetableType)
			{
				case TimetableType.Class:
					FirstRow = _originalFirstRow = subject;
					SecondRow = _originalSecondRow = teacher;
					ThirdRow = _originalThirdRow = classroom;
					break;
				case TimetableType.Teacher:
					FirstRow = _originalFirstRow = subject;
					SecondRow = _originalSecondRow = className;
					ThirdRow = _originalThirdRow = classroom;
					break;
				case TimetableType.Classroom:
					FirstRow = _originalFirstRow = subject;
					SecondRow = _originalSecondRow = className;
					ThirdRow = _originalThirdRow = teacher;
					break;
				case TimetableType.Lesson:
					FirstRow = _originalFirstRow = subject;
					SecondRow = _originalSecondRow = className;
					ThirdRow = _originalThirdRow = teacher;
					break;
			}

			switch (actionType)
			{
				case ActionType.Add:
					_actionType = actionType;
					break;
				case ActionType.Change:
					checkBox.Visibility = Visibility.Visible;
					borderFirstRow.Padding = new Thickness(22, 0, 22, 0);
					_actionType = actionType;
					break;
			}
		}

		#endregion


		#region Events

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.NewSize.Width < _originalFirstRow.Length * PIXELS_PER_LETTER)
			{
				var a = (int) Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				textBlockFirstRow.Text = $"{_originalFirstRow.Substring(0, a - 4)} ...";
			}
			else
			{
				textBlockFirstRow.Text = _originalFirstRow;
			}

			if (e.NewSize.Width < _originalSecondRow.Length * PIXELS_PER_LETTER)
			{
				var a = (int) Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				textBlockSecondRow.Text = $"{_originalSecondRow.Substring(0, a - 4)} ...";
			}
			else
			{
				textBlockSecondRow.Text = _originalSecondRow;
			}

			if (e.NewSize.Width < _originalThirdRow.Length * PIXELS_PER_LETTER)
			{
				var a = (int) Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				textBlockThirdRow.Text = $"{_originalThirdRow.Substring(0, a - 4)} ...";
			}
			else
			{
				textBlockThirdRow.Text = _originalThirdRow;
			}
		}

		private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (_actionType == ActionType.Add
				|| _actionType == ActionType.Change)
			{
				var planningWindow = new PlanningWindow(_callingWindow, _actionType, _entityType,
					_cellViewModel.ClassId, _cellViewModel.TeacherPesel, _cellViewModel.DayId, _cellViewModel.HourId);
				planningWindow.Owner = _callingWindow;
				planningWindow.Show();
			}
		}

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		/// <summary>
		///     Sprawdza, czy wybrana lekcja jest zaznaczona.
		/// </summary>
		/// <returns></returns>
		public bool IsChecked()
		{
			if ((checkBox.IsChecked ?? false)
				&& _actionType == ActionType.Change)
				return true;

			return false;
		}

		#endregion


		#region Private methods

		#endregion
	}
}
