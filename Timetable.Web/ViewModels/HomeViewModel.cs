using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Timetable.DAL.Model;

namespace Timetable.Web.ViewModels
{
	public class HomeViewModel
	{
		public IList<classes> Classess { get; set; }
		public IList<teachers> Teachers { get; set; }
		public IList<classrooms> Classrooms { get; set; }
	}
}
