using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using Timetable.DAL.Model;

namespace Timetable.Service.Services
{
	[ServiceContract]
	public interface IHourService
	{
		/// <summary>
		/// Metoda zwracająca listę godzin.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		IList<hours> GetAllHours();
	}
}
