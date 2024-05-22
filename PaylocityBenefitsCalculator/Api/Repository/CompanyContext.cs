


using Microsoft.EntityFrameworkCore;
using Api.Dtos.Employee;
using Api.Dtos.Dependent;

namespace EFCoreInMemoryDb {
    public class CompanyContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // in memory database used for simplicity, would change to a real db for production applications
            options.UseInMemoryDatabase(databaseName: "CompanyDB");
        }

        public DbSet<GetEmployeeDto> Employees { get; set; }
        public DbSet<GetDependentDto> Dependents {get; set; }
    }

}