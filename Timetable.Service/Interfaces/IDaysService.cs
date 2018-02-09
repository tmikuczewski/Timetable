using System.Collections.Generic;
using System.ServiceModel;
using Timetable.DAL.ViewModels;

namespace Timetable.Service.Interfaces
{
	[ServiceContract]
	public interface IDaysService
	{
		/// <summary>
		///     Metoda zwracająca listę dni.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		IList<DayViewModel> GetAllDays();
	}
}
