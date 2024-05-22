


using Microsoft.EntityFrameworkCore;
using Api.Dtos.Employee;
using Api.Dtos.Dependent;

namespace EFCoreInMemoryDb {
    public class CompanyContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // in memory database used for simplicity, change to a real db for production applications
            options.UseInMemoryDatabase(databaseName: "CompanyDB");
        }

        public DbSet<GetEmployeeDto> Employees { get; set; }
        public DbSet<GetDependentDto> Dependents {get; set; }

        public DbSet<EmployeeDependent> EmployeeDependents { get; set;}

        public class EmployeeDependent {
            public int EmployeeId { get; set;}
            public int DependentId { get; set; }
        }

        // public class Dependent {
        //     public int Id { get; set; }
        //     public string? FirstName { get; set; }
        //     public string? LastName { get; set; }
        //     public DateTime DateOfBirth { get; set; }
        //     public int Relationship { get; set; }
        // }
    }

}