namespace Timetable.Utilities
{
	/// <summary>
	///     Rodzaj wybranej zakładki oraz zawartości kontrolki <c>Expander</c>.
	/// </summary>
	public enum MainTabControlType
	{
		/// <summary>
		///     Zakładka 'Zarządzanie'.
		/// </summary>
		Management,

		/// <summary>
		///     Zakładka 'Łączenie'.
		/// </summary>
		Mapping,

		/// <summary>
		///     Zakładka 'Planowanie'.
		/// </summary>
		Planning,

		/// <summary>
		///     Zakładka 'Podsumowanie'.
		/// </summary>
		Summary
	}

	/// <summary>
	///     Rodzaj wybranej encji oraz zawartość kontrolki <c>ComboBox</c>.
	/// </summary>
	public enum ComboBoxContentType
	{
		/// <summary>
		///     Wszystkie encje.
		/// </summary>
		Entities,

		/// <summary>
		///     Lista studentów.
		/// </summary>
		Students,

		/// <summary>
		///     Lista nauczycieli.
		/// </summary>
		Teachers,

		/// <summary>
		///     Lista klas.
		/// </summary>
		Classes,

		/// <summary>
		///     Lista sal.
		/// </summary>
		Classrooms,

		/// <summary>
		///     Lista przedmiotów.
		/// </summary>
		Subjects,

		/// <summary>
		///     Lista lekcji.
		/// </summary>
		Lessons,

		/// <summary>
		///     Lista zajęć.
		/// </summary>
		LessonsPlaces
	}

	/// <summary>
	///     Rodzaj wybranej akcji oraz zawartość kontrolki <c>ExpanderControl</c>.
	/// </summary>
	public enum ExpanderControlType
	{
		/// <summary>
		///     Akcja 'Dodaj'.
		/// </summary>
		Add,

		/// <summary>
		///     Akcja 'Zmień'.
		/// </summary>
		Change,

		/// <summary>
		///     Akcja 'Usuń'.
		/// </summary>
		Remove,

		/// <summary>
		///     Akcja 'XLS'.
		/// </summary>
		XLS,

		/// <summary>
		///     Akcja 'PDF'.
		/// </summary>
		PDF
	}

	/// <summary>
	///     Typ pliku do jakiego eksportowany jest plan
	/// </summary>
	public enum ExportFileType
	{
		/// <summary>
		///     XLS (Excel)
		/// </summary>
		XLS,

		/// <summary>
		///     PDF
		/// </summary>
		PDF
	}

	/// <summary>
	///     Płeć.
	/// </summary>
	public enum Sex
	{
		/// <summary>
		///     Kobieta.
		/// </summary>
		Female,

		/// <summary>
		///     Mężczyzna.
		/// </summary>
		Male
	}
}
