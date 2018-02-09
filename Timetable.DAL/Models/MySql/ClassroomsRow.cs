using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Models.MySql
{
	[Table("timetable.classrooms")]
	public partial class ClassroomsRow
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ClassroomsRow()
		{
			LessonsPlaces = new HashSet<LessonsPlacesRow>();
		}

		[Column("id")]
		public int Id { get; set; }

		[Column("name")]
		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[Column("administrator")]
		[StringLength(11)]
		public string AdministratorPesel { get; set; }

		public virtual TeachersRow Administrator { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LessonsPlacesRow> LessonsPlaces { get; set; }
	}
}
