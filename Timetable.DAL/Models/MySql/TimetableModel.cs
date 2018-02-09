using System.Data.Entity;
using MySql.Data.Entity;

namespace Timetable.DAL.Models.MySql
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public partial class TimetableModel : DbContext
	{
		public TimetableModel()
			: base("name=Timetable.DAL.Properties.Settings.TimetableConnectionStringMySql")
		{
		}

		public virtual DbSet<ClassesRow> Classes { get; set; }
		public virtual DbSet<ClassroomsRow> Classrooms { get; set; }
		public virtual DbSet<DaysRow> Days { get; set; }
		public virtual DbSet<HoursRow> Hours { get; set; }
		public virtual DbSet<LessonsRow> Lessons { get; set; }
		public virtual DbSet<LessonsPlacesRow> LessonsPlaces { get; set; }
		public virtual DbSet<StudentsRow> Students { get; set; }
		public virtual DbSet<SubjectsRow> Subjects { get; set; }
		public virtual DbSet<TeachersRow> Teachers { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ClassesRow>()
				.Property(e => e.CodeName)
				.IsUnicode(false);

			modelBuilder.Entity<ClassesRow>()
				.Property(e => e.TutorPesel)
				.IsUnicode(false);

			modelBuilder.Entity<ClassesRow>()
				.HasMany(e => e.Lessons)
				.WithRequired(e => e.Class)
				.HasForeignKey(e => e.ClassId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ClassesRow>()
				.HasMany(e => e.Students)
				.WithOptional(e => e.Class)
				.HasForeignKey(e => e.ClassId);

			modelBuilder.Entity<ClassroomsRow>()
				.Property(e => e.Name)
				.IsUnicode(false);

			modelBuilder.Entity<ClassroomsRow>()
				.Property(e => e.AdministratorPesel)
				.IsUnicode(false);

			modelBuilder.Entity<ClassroomsRow>()
				.HasMany(e => e.LessonsPlaces)
				.WithRequired(e => e.Classroom)
				.HasForeignKey(e => e.ClassroomId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<DaysRow>()
				.Property(e => e.Name)
				.IsUnicode(false);

			modelBuilder.Entity<DaysRow>()
				.HasMany(e => e.LessonsPlaces)
				.WithRequired(e => e.Day)
				.HasForeignKey(e => e.DayId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<HoursRow>()
				.HasMany(e => e.LessonsPlaces)
				.WithRequired(e => e.Hour)
				.HasForeignKey(e => e.HourId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<LessonsRow>()
				.Property(e => e.TeacherPesel)
				.IsUnicode(false);

			modelBuilder.Entity<LessonsRow>()
				.HasMany(e => e.LessonsPlaces)
				.WithRequired(e => e.Lesson)
				.HasForeignKey(e => e.LessonId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<StudentsRow>()
				.Property(e => e.Pesel)
				.IsUnicode(false);

			modelBuilder.Entity<StudentsRow>()
				.Property(e => e.FirstName)
				.IsUnicode(false);

			modelBuilder.Entity<StudentsRow>()
				.Property(e => e.LastName)
				.IsUnicode(false);

			modelBuilder.Entity<SubjectsRow>()
				.Property(e => e.Name)
				.IsUnicode(false);

			modelBuilder.Entity<SubjectsRow>()
				.HasMany(e => e.Lessons)
				.WithRequired(e => e.Subject)
				.HasForeignKey(e => e.SubjectId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TeachersRow>()
				.Property(e => e.Pesel)
				.IsUnicode(false);

			modelBuilder.Entity<TeachersRow>()
				.Property(e => e.FirstName)
				.IsUnicode(false);

			modelBuilder.Entity<TeachersRow>()
				.Property(e => e.LastName)
				.IsUnicode(false);

			modelBuilder.Entity<TeachersRow>()
				.HasMany(e => e.Classes)
				.WithOptional(e => e.Tutor)
				.HasForeignKey(e => e.TutorPesel);

			modelBuilder.Entity<TeachersRow>()
				.HasMany(e => e.Classrooms)
				.WithOptional(e => e.Administrator)
				.HasForeignKey(e => e.AdministratorPesel);

			modelBuilder.Entity<TeachersRow>()
				.HasMany(e => e.Lessons)
				.WithRequired(e => e.Teacher)
				.HasForeignKey(e => e.TeacherPesel)
				.WillCascadeOnDelete(false);
		}
	}
}
