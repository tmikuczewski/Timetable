using System.ServiceModel;
using Timetable.DAL.ViewModels;

namespace Timetable.Service.Interfaces
{
	[ServiceContract]
	public interface ITimetableService
	{
		/// <summary>
		///     Metoda zwracająca listę dostępnych encji.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		TimetableViewModel GetTimetableEntities();

		/// <summary>
		///     Metoda zwracająca plan lekcji dla danej klasy.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		TimetableViewModel GetTimetableForClass(int id);

		/// <summary>
		///     Metoda zwracająca plan lekcji dla danego nauczyciela.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		TimetableViewModel GetTimetableForTeacher(string pesel);

		/// <summary>
		///     Metoda zwracająca plan lekcji dla danej sali.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		TimetableViewModel GetTimetableForClassroom(int id);
	}
}
