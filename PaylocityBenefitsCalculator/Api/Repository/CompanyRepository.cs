using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreInMemoryDb {
public class CompanyRepository: ICompanyRepository
    {
        public CompanyRepository() 
        {
            {
                using (var context = new CompanyContext()) 
                {
                    if (context.Employees.Any() && context.Dependents.Any())
                    {
                        return;   // Data was already seeded
                    }
                    var employees = new List<GetEmployeeDto>
                    {
                        new GetEmployeeDto
                        {
                            Id = 1,
                            FirstName = "LeBron",
                            LastName = "James",
                            Salary = 75420.99m,
                            DateOfBirth = new DateTime(1984, 12, 30) 
                        },
                        new GetEmployeeDto
                        {
                            Id = 2,
                            FirstName = "Ja",
                            LastName = "Morant",
                            Salary = 92365.22m,
                            DateOfBirth = new DateTime(1999, 8, 10),
                            // Dependents = new List<GetDependentDto> 
                            // {
                            //     new GetDependentDto 
                            //     {
                            //         Id = 1,
                            //         FirstName = "Spouse",
                            //         LastName = "Morant",
                            //         Relationship = Relationship.Spouse,
                            //         DateOfBirth = new DateTime(1998, 3, 3)
                            //     },
                            //     new GetDependentDto 
                            //     {
                            //         Id = 2,
                            //         FirstName = "Child1",
                            //         LastName = "Morant",
                            //         Relationship = Relationship.Child,
                            //         DateOfBirth = new DateTime(2020, 6, 23)
                            //     },
                            //     new GetDependentDto
                            //     {
                            //         Id = 3,
                            //         FirstName = "Child2",
                            //         LastName = "Morant",
                            //         Relationship = Relationship.Child,
                            //         DateOfBirth = new DateTime(2021, 5, 18)
                            //     }
                            // }
                        },
                        new GetEmployeeDto
                        {
                            Id = 3,
                            FirstName = "Michael",
                            LastName = "Jordan",
                            Salary = 143211.12m,
                            DateOfBirth = new DateTime(1963, 2, 17),
                            // Dependents = new List<GetDependentDto>
                            // {
                            //     new()
                            //     {
                            //         Id = 4,
                            //         FirstName = "DP",
                            //         LastName = "Jordan",
                            //         Relationship = Relationship.DomesticPartner,
                            //         DateOfBirth = new DateTime(1974, 1, 2)
                            //     }
                            // }
                        }
                    };

                    var joinTableData = new List<CompanyContext.EmployeeDependent>
                    {
                        new() {
                            EmployeeId = 1,
                            DependentId = 1
                        },
                        new() {
                            EmployeeId = 1,
                            DependentId = 2
                        },
                        new() {
                            EmployeeId = 1,
                            DependentId = 3
                        },
                        new() {
                            EmployeeId = 2,
                            DependentId = 4
                        },
                        
                    };

                    var dependents = new List<GetDependentDto>
                    {
                        new() {
                            Id = 1,
                            FirstName = "Spouse",
                            LastName = "Morant",
                            Relationship = Relationship.Spouse,
                            DateOfBirth = new DateTime(1999, 8, 10)
                        },
                        new() {
                            Id = 2,
                            FirstName = "Child1",
                            LastName = "Morant",
                            Relationship = Relationship.Child,
                            DateOfBirth = new DateTime(1998, 3, 3)
                        },
                        new() {
                            Id = 3,
                            FirstName = "Child2",
                            LastName = "Morant",
                            Relationship = Relationship.Child,
                            DateOfBirth = new DateTime(2020, 6, 23)
                        },
                        new() {
                            Id = 4,
                            FirstName = "DP",
                            LastName = "Jordan",
                            Relationship = Relationship.DomesticPartner,
                            DateOfBirth = new DateTime(1974, 1, 2)
                        }
                    };
                    context.Employees.AddRange(employees);
                    context.Dependents.AddRange(dependents);
                    context.EmployeeDependents.AddRange(joinTableData);
                    context.SaveChangesAsync();
                }
            }
        }

        public async Task<GetDependentDto> GetDependentById(int id)
        {
            using (var context = new CompanyContext())
            {
                var dependent = await context.Dependents
                    .FindAsync(id);
                return dependent;
            }
        }

        public async Task<List<GetDependentDto>> GetDependents()
        {
            using (var context = new CompanyContext())
            {
                var dependents = await context.Dependents
                    .ToListAsync();
                return dependents;
            }
        }

        public async Task<GetEmployeeDto> GetEmployeeById(int id)
        {
            using (var context = new CompanyContext())
            {
                var employee = await context.Employees
                    .FindAsync(id);
                return employee;
            }
        }

        public async Task<List<GetEmployeeDto>> GetEmployees()
        {
            using (var context = new CompanyContext())
            {
                var list = await context.Employees
                    // .Include(a => a.Dependents)
                    .ToListAsync();
                    
                return list;
            }
        }
    }
}