using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using Timetable.DAL.Model;

namespace Timetable.Service.Services
{
	[ServiceContract]
	public interface IDayService
	{
		/// <summary>
		/// Metoda zwracająca listę dni.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		IList<days> GetAllDays();
	}
}
