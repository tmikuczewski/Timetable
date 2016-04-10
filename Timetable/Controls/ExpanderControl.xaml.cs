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
		public ExpanderControl(string text, ExpanderControlType ect, ComboBoxContent dataType)
		{
			InitializeComponent();

            this.dataType = dataType;
            this.type = ect;
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
        ExpanderControlType type { get; set; }
        ComboBoxContent dataType { get; set; }
        #endregion

        #region Private methods

        #endregion

        #region Events
        private void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Console.WriteLine(this.dataType);
            System.Console.WriteLine(this.type);
            if (this.type.ToString().Equals("Add"))
            {
                if (dataType.Equals(ComboBoxContent.Students) || dataType.Equals(ComboBoxContent.Teachers))
                {
                    if (window == null || window.CanFocus == false)
                    {
                        window = new Windows.PersonWindow(dataType);
                    }
                    if(((Windows.PersonWindow)window).dataType != dataType){
                        window.Close();
                        window = new Windows.PersonWindow(dataType);
                    }
                    window.Focus();
                    window.Show();
                }
                /// miejsce na inne okna do dodawania
            }
        }
        #endregion

        #region Constants and Statics

        #endregion

        #region Fields
        private static System.Windows.Forms.Form window;
        #endregion


    }
}
