using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;

using Timetable.Utilities;

namespace Timetable.Controls
{
	/// <summary>
	/// Interaction logic for CellControl.xaml
	/// </summary>
	public partial class CellControl : UserControl
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>CellControl</c>.</summary>
		/// <param name="diffColor">Parametr sterujący zmianą koloru kontrolki.</param>
		public CellControl(bool diffColor = false)
		{
			InitializeComponent();
			if (diffColor)
			{
				this.Background = new SolidColorBrush(SystemColors.InactiveBorder.ToMediaColor());
			}

			this.FirstRow = this.OriginalFirstRow = string.Empty;
			this.SecondRow = this.OriginalSecondRow = string.Empty;
			this.ThirdRow = this.OriginalThirdRow = string.Empty;
		}
		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>CellControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
		/// <param name="firstRow">Tekst wypełniający pierwszy rząd.</param>
		/// <param name="secondRow">Tekst wypełniający drugi rząd.</param>
		/// <param name="thirdRow">Tekst wypełniający pierwszy rząd.</param>
		/// <param name="diffColor">Parametr sterujący zmianą koloru kontrolki.</param>
		public CellControl(string firstRow, string secondRow, string thirdRow, bool diffColor = false)
			: this(diffColor)
		{
			this.FirstRow = this.OriginalFirstRow = firstRow;
			this.SecondRow = this.OriginalSecondRow = secondRow;
			this.ThirdRow = this.OriginalThirdRow = thirdRow;
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		/// <summary>
		/// Pierwszy wiersz kontrolki.</summary>
		public string FirstRow { get { return this.textBlockFirstRow.Text; } set { this.textBlockFirstRow.Text = value; } }
		/// <summary>
		/// Drugi wiersz kontrolki.</summary>
		public string SecondRow { get { return this.textBlockSecondRow.Text; } set { this.textBlockSecondRow.Text = value; } }
		/// <summary>
		/// Trzeci wiersz kontroli.</summary>
		public string ThirdRow { get { return this.textBlockThirdRow.Text; } set { this.textBlockThirdRow.Text = value; } }

		#endregion

		#region Private methods

		#endregion

		#region Events

		private void UserControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{

			if (e.NewSize.Width < (this.OriginalFirstRow.Length * PIXELS_PER_LETTER))
			{
				var a = (int)Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				this.textBlockFirstRow.Text = $"{this.OriginalFirstRow.Substring(0, a - 4)} ...";
			}
			else
			{
				this.textBlockFirstRow.Text = this.OriginalFirstRow;
			}

			if (e.NewSize.Width < (this.OriginalSecondRow.Length * PIXELS_PER_LETTER))
			{
				var a = (int)Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				this.textBlockSecondRow.Text = $"{this.OriginalSecondRow.Substring(0, a - 4)} ...";
			}
			else
			{
				this.textBlockSecondRow.Text = this.OriginalSecondRow;
			}

			if (e.NewSize.Width < (this.OriginalThirdRow.Length * PIXELS_PER_LETTER))
			{
				var a = (int)Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				this.textBlockThirdRow.Text = $"{this.OriginalThirdRow.Substring(0, a - 4)} ...";
			}
			else
			{
				this.textBlockThirdRow.Text = this.OriginalThirdRow;
			}
		}

		#endregion

		#region Constants and Statics

		private static readonly int PIXELS_PER_LETTER = 7;
		
		#endregion

		#region Fields

		private string
			OriginalFirstRow,
			OriginalSecondRow,
			OriginalThirdRow;

		#endregion
	}
}
