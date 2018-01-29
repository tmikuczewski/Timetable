namespace Timetable.Utilities
{
	/// <summary>
	/// Rodzaj wyświetlanych danych w kontrolce <c>ExpanderControl</c>.</summary>
	public enum ExpanderControlType
	{
		/// <summary>
		/// Rodzaj 'dodaj' kontrolki <c>ExpanderControl</c>.</summary>
		Add,
		/// <summary>
		/// Rodzaj 'zmień' kontrolki <c>ExpanderControl</c>.</summary>
		Change,
		/// <summary>
		/// Rodzaj 'usuń' kontrolki <c>ExpanderControl</c>.</summary>
		Remove,
		/// <summary>
		/// Rodzaj 'XLSX' kontrolki <c>ExpanderControl</c>.</summary>
		XLSX,
		/// <summary>
		/// Rodzaj 'PDF' kontrolki <c>ExpanderControl</c>.</summary>
		PDF
	}

	/// <summary>
	/// Rodzaj wybranej zakładki.</summary>
	public enum TabType
	{
		/// <summary>
		/// Zakładka 'Zarządzanie'.</summary>
		Management,
		/// <summary>
		/// Zakładka 'Powiązania'.</summary>
		Mapping,
		/// <summary>
		/// Zakładka 'Ustalanie planu'.</summary>
		Planning,
		/// <summary>
		/// Zakładka 'Podsumowanie'.</summary>
		Summary
	}

	/// <summary>
	/// Zawartość kontrolki <c>ComboBox</c>.</summary>
	public enum ComboBoxContent
	{
		/// <summary>
		/// Wszystkie nazwy encji.</summary>
		Entities,
		/// <summary>
		/// Lista studentów.</summary>
		Students,
		/// <summary>
		/// Lista nauczycieli.</summary>
		Teachers,
		/// <summary>
		/// Lista klas.</summary>
		Classes,
		/// <summary>
		/// Lista przedmiotów.</summary>
		Subjects,
		/// <summary>
		/// Mapping.</summary>
		Mapping,
		/// <summary>
		/// Planning.</summary>
		Planning,
	}

	/// <summary>
	/// Zawartość kontrolki <c>Expander</c>.</summary>
	public enum ExpanderContent
	{
		/// <summary>
		/// Zawartość dla zakładki 'Zarządzanie'.</summary>
		Management,
		/// <summary>
		/// Zawartość dla zakładki 'Podsumowanie'.</summary>
		Summary
	}

	/// <summary>
	/// Płeć.</summary>
	public enum Sex
	{
		/// <summary>
		/// Kobieta.</summary>
		Female,
		/// <summary>
		/// Mężczyzna.</summary>
		Male
	}
}
