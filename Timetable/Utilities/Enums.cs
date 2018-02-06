namespace Timetable.Utilities
{
	/// <summary>
	///     Rodzaj wybranej zakładki oraz zawartości kontrolki <c>TabControl</c>.
	/// </summary>
	public enum MainWindowTabType
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
		///     Zakładka 'Podgląd'.
		/// </summary>
		Summary
	}

	/// <summary>
	///     Rodzaj wybranej encji oraz zawartość kontrolki <c>ComboBox</c>.
	/// </summary>
	public enum EntityType
	{
		/// <summary>
		///     Brak encji.
		/// </summary>
		None,

		/// <summary>
		///     Klasa.
		/// </summary>
		Class,

		/// <summary>
		///     Sala.
		/// </summary>
		Classroom,

		/// <summary>
		///     Dzień.
		/// </summary>
		Day,

		/// <summary>
		///     Godzina.
		/// </summary>
		Hour,

		/// <summary>
		///     Lekcja.
		/// </summary>
		Lesson,

		/// <summary>
		///     Zajęcia.
		/// </summary>
		LessonsPlace,

		/// <summary>
		///     Student.
		/// </summary>
		Student,

		/// <summary>
		///     Przedmiot.
		/// </summary>
		Subject,

		/// <summary>
		///     Nauczyciel.
		/// </summary>
		Teacher,
	}

	/// <summary>
	///     Rodzaj wybranego podglądu planu lekcji oraz zawartość kontrolki <c>CellControl</c>.
	/// </summary>
	public enum TimetableType
	{
		/// <summary>
		///     Brak podglądu.
		/// </summary>
		None,

		/// <summary>
		///     Podgląd dla klasy.
		/// </summary>
		Class,

		/// <summary>
		///     Podgląd dla nauczyciela.
		/// </summary>
		Teacher,

		/// <summary>
		///     Podgląda dla sali.
		/// </summary>
		Classroom,

		/// <summary>
		///     Podgląd dla lekcji.
		/// </summary>
		Lesson
	}

	/// <summary>
	///     Rodzaj wybranej akcji oraz zawartość kontrolki <c>ExpanderControl</c>.
	/// </summary>
	public enum ActionType
	{
		/// <summary>
		///     Brak akcji.
		/// </summary>
		None,

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
		///     Akcja 'Usuń'.
		/// </summary>
		RemoveLessonsPlace,

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
