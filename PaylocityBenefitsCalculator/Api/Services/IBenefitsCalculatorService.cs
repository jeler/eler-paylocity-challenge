using Api.Dtos.Employee;
using Api.Models;

public interface IBenefitsCalculatorService 
{
   BenefitResponse createBenefitsPackage(GetEmployeeDto employee);
}