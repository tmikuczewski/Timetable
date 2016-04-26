namespace Timetable.Utilities
{
    /// <summary>
    /// Statyczna klasa przechowująca metody/klasy pomocniczne.</summary>
    public static class Utilities
    {
        #region Constructors

        #endregion

        #region Overridden methods

        #endregion

        #region Public methods

        /// <summary>
        /// Statyczna metoda konwertująca obiekt typu <c>System.Drawing.Bitmap</c> na obiekt 
        /// typu <c>System.Windows.Media.Imaging.BitmapImage</c>.
        /// </summary>
        /// <param name="bitmap">Obiekt typu <c>System.Drawing.Bitmap</c> mający ulec konwersji.</param>
        /// <returns>Obiekt przekonwertowany na typ <c>System.Windows.Media.Imaging.BitmapImage</c>.</returns>
        public static System.Windows.Media.Imaging.BitmapImage ToBitmapImage(this System.Drawing.Bitmap bitmap)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;

            System.Windows.Media.Imaging.BitmapImage bitmapImg = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = ms;
            bitmapImg.EndInit();

            return bitmapImg;
        }

        /// <summary>
        /// Statyczna metoda konwertująca obiekt typu <c>System.Drawing.Color</c> na obiekt 
        /// typu <c>System.Windows.Media.Color</c>.
        /// </summary>
        /// <param name="color">Obiekt typu <c>System.Drawing.Color</c> mający ulec konwersji.</param>
        /// <returns>Obiekt przekonwertowany na typ <c>System.Windows.Media.Color</c>.</returns>
        public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        #endregion

        #region Properties

        #endregion

        #region Private methods

        #endregion

        #region Constants and Statics

        #endregion

        #region Fields

        #endregion
    }
}
