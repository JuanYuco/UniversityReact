//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UniversityReact.API.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UniversityReactEntities : DbContext
    {
        public UniversityReactEntities()
            : base("name=UniversityReactEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Instructors> Instructors { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<OfficesAssignment> OfficesAssignment { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<CoursesInstructor> CoursesInstructor { get; set; }
        public virtual DbSet<Enrollments> Enrollments { get; set; }
    }
}
