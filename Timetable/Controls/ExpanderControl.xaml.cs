using System.Windows.Controls;

using Timetable.Utilities.Enums;

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
		public ExpanderControl(string text, ExpanderControlType ect)
		{
			InitializeComponent();

			this.button.Content = text;

			switch (ect)
			{
				case ExpanderControlType.Add:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.plus);
					break;
				case ExpanderControlType.Change:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.pen);
					break;
				case ExpanderControlType.Remove:
				default:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.recycleBin);
					break;
			}
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		#endregion

		#region Private methods

		#endregion

		#region Events

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
