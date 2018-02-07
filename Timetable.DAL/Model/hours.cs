namespace Timetable.DAL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("timetable.hours")]
    public partial class hours
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hours()
        {
            lessons_places = new HashSet<lessons_places>();
        }

        public int id { get; set; }

        public TimeSpan begin { get; set; }

        public TimeSpan end { get; set; }

        public int number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<lessons_places> lessons_places { get; set; }
    }
}
