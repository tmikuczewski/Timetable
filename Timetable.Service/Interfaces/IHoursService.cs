using System.Collections.Generic;
using System.ServiceModel;
using Timetable.DAL.ViewModels;

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
		IList<HourViewModel> GetAllHours();
	}
}
