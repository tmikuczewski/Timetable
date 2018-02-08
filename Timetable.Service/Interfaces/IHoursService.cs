using System.Collections.Generic;
using System.ServiceModel;
using Timetable.Service.ViewModels;

namespace Timetable.Service.Interfaces
{
	[ServiceContract]
	public interface IHoursService
	{
		/// <summary>
		///     Metoda zwracająca listę godzin.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		IList<HoursViewModel> GetAllHours();
	}
}
