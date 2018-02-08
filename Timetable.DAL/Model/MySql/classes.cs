using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Model.MySql
{
	[Table("timetable.classes")]
	public partial class classes
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public classes()
		{
			lessons = new HashSet<lessons>();
			students = new HashSet<students>();
		}

		public int id { get; set; }

		public int year { get; set; }

		[StringLength(255)]
		public string code_name { get; set; }

		[StringLength(11)]
		public string tutor { get; set; }

		public virtual teachers teachers { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<lessons> lessons { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<students> students { get; set; }
	}
}
