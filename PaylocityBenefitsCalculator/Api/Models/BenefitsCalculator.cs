using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Apis.Models;

public class BenefitsCalculator : Benefit {

    public GetEmployeeDto _employee { get; set; }

    public decimal PaycheckAmountWithDeductions {get; set;}

    public BenefitsCalculator(GetEmployeeDto employee) {
        _employee = employee;
        PaycheckAmountWithDeductions = calculatePaycheckWithDeductions();
    }

    private decimal calculatePaycheckWithDeductions() {
        decimal monthlyBenefitCost = calculateMonthlyBenefits();
        // Salary surcharge not on a monthly basis which is why I separated it from the monthly benefit calculations
        decimal salarySurcharge = calculateAdditionalSalaryCost(_employee.Salary);
        // In the future, I would store these number variables instead of putting them here without context
        decimal paycheckDeductions = ((monthlyBenefitCost * 12) + salarySurcharge)/26;
        decimal finalPaycheckAmount = (_employee.Salary/26) - paycheckDeductions;
        // I was waiting until the very end to round 
        return Math.Round(finalPaycheckAmount, 2);
    }

    private decimal calculateMonthlyBenefits() {

        // employee added
        decimal dependantsCost = calculalateChildDependentCost(_employee.Dependents.ToList());
        decimal additionalPartnerCost = calculateAdditionalPartnerCost(_employee.Dependents.ToList());
        decimal olderDepsSurcharge = calculateOlderDeps(_employee.Dependents.ToList());
        return BaseCost  + dependantsCost  + additionalPartnerCost + olderDepsSurcharge;
    }

    public decimal calculalateChildDependentCost(List<GetDependentDto> dependents) {
        decimal childrenDepsCost = 0;
        var childrenDeps = dependents.Where(dep => dep.Relationship == Relationship.Child);

        if(childrenDeps.Count() > 0 ) {
            childrenDepsCost = childrenDeps.Count() * DependentCost;
        }

        return Math.Round(childrenDepsCost, 2);
    }

    public decimal calculateAdditionalPartnerCost(List<GetDependentDto> dependents) {
        // I am using .Any instead of .Where here to avoid the issue of including both domestic partner and spouse
        bool hasSpouseDomesticPartner = dependents.Any(dep => dep.Relationship == Relationship.Spouse || dep.Relationship == Relationship.DomesticPartner);
        return hasSpouseDomesticPartner ? AdditionalPartnerCost : 0;
    }
    
    public decimal calculateAdditionalSalaryCost(decimal salary) {
        decimal additionalCost = salary * SalarySurcharge/ 100;
        return salary > 80000 ? additionalCost: 0;
    }

    // would rather pass paramater as argument
    public decimal calculateOlderDeps(List<GetDependentDto> dependents) {
        DateTime today = DateTime.Now;
        DateTime olderPersonAdditionalChargeDate = today.AddYears(-50);
        var olderDeps = dependents.Where(dep => dep.DateOfBirth > olderPersonAdditionalChargeDate);
        int olderDepsAmount = olderDeps.Count();
        return olderDepsAmount> 0 ? OlderDepsSurcharge * olderDepsAmount : 0;
    }

}