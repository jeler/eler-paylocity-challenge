using Api.Dtos.Dependent;
using Api.Dtos.Employee;

namespace Api.Models;


public class Benefit {

    // This class is doing a whole lot. In the future, i would split it up into a "Benefits" model and a "Benefits Calculator" one

    private decimal _baseCost {get; set;}

    private decimal _additionalPartnerCost {get; set;}
    private decimal _dependentCost  {get; set;}
    private int _salarySurcharge {get; set;}

    private decimal _olderDepsSurcharge {get; set;}

    private static GetEmployeeDto _employee { get; set; }
    // per month

    public decimal PaycheckAmountWithDeductions {get; set;}

    // public decimal NetPaycheckAmount {get; set;}

    // public decimal OlderDependentCharge {get; set;}

    // public decimal SalarySurcharge {get; set;}
    // I am setting default amounts based on the prompt but the user can also pass in their own values
    public Benefit(GetEmployeeDto employee, decimal baseCost = 1000, decimal additionalPartnerCost =  1000, decimal dependentCost = 600, int salarySurcharge = 2, decimal olderDepsSurcharge = 200) {
        _baseCost = baseCost;
        _additionalPartnerCost = additionalPartnerCost;
        _dependentCost = dependentCost;
        _salarySurcharge = salarySurcharge;
        _olderDepsSurcharge = olderDepsSurcharge;
        _employee = employee;
        PaycheckAmountWithDeductions = calculatePaycheckWithDeductions();
        // NetPaycheckAmount = Math.Round(_employee.Salary/26, 2);
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
        return _baseCost  + dependantsCost  + additionalPartnerCost + olderDepsSurcharge;
    }

    private decimal calculalateChildDependentCost(List<GetDependentDto> dependents) {
        decimal childrenDepsCost = 0;
        var childrenDeps = dependents.Where(dep => dep.Relationship == Relationship.Child);

        if(childrenDeps.Count() > 0 ) {
            childrenDepsCost = childrenDeps.Count() * _dependentCost;
        }

        return Math.Round(childrenDepsCost, 2);
    }

    private decimal calculateAdditionalPartnerCost(List<GetDependentDto> dependents) {
        // I am using .Any instead of .Where here to avoid the issue of including both domestic partner and spouse
        bool hasSpouseDomesticPartner = dependents.Any(dep => dep.Relationship == Relationship.Spouse || dep.Relationship == Relationship.DomesticPartner);
        return hasSpouseDomesticPartner ? _additionalPartnerCost : 0;
    }
    
    private decimal calculateAdditionalSalaryCost(decimal salary) {
        decimal additionalCost = salary * _salarySurcharge/ 100;
        return salary > 80000 ? additionalCost: 0;
    }

    // would rather pass paramater as argument
    private decimal calculateOlderDeps(List<GetDependentDto> dependents) {
        DateTime today = DateTime.Now;
        DateTime olderPersonAdditionalChargeDate = today.AddYears(-50);
        var olderDeps = dependents.Where(dep => dep.DateOfBirth > olderPersonAdditionalChargeDate);
        int olderDepsAmount = olderDeps.Count();
        return olderDepsAmount> 0 ? _olderDepsSurcharge * olderDepsAmount : 0;
    }
}