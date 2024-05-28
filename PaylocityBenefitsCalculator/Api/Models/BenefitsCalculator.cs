using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Apis.Models;
// design teir => C-tier suite get better benefits

// don't have older dependents or not extra 

// configuration outside of runtime 
// dot env file?
// config from database
public class BenefitsCalculator : IBenefitsCalculator {

    private GetEmployeeDto _employee { get; set; }


    // per pay period net pay with deductions
    public decimal NetPay {get; set;}
    
    // per pay period with no deductions
    public decimal GrossPay {get; set;}

    public decimal BaseCost {get; set;}

    public decimal TotalPayPeriodDeductions {get; set;}

    public decimal TotalAdditionalPartnerCost {get; set; }
    public decimal TotalDependentCost {get; set;}
    public decimal TotalSalarySurcharge {get; set;}
    public decimal TotalOlderDepsSurcharge {get; set;}
    public decimal TotalMonthlyDeductions {get; set;}
    public decimal AdditionalPartnerCost { get; set; }
    public decimal DependentCost { get; set; }
    public decimal SalarySurcharge { get; set; }
    public decimal OlderDepsSurcharge { get; set; }
    public decimal SalaryCap { get; set; }

    public BenefitsCalculator(GetEmployeeDto employee) {
        // benefit is a dependency
        // unit tests would provide different benefit setups 
        Benefit b = new Benefit();
        BaseCost = b.BaseCost;
        AdditionalPartnerCost = b.AdditionalPartnerCost;
        DependentCost = b.DependentCost;
        SalarySurcharge = b.SalarySurcharge;

        _employee = employee;
        GrossPay = calculateGrossPaycheckPerPaycheck();
        TotalDependentCost = calculalateChildDependentDeduction(_employee.Dependents.ToList());
        TotalAdditionalPartnerCost = calculateAdditionalPartnerDeduction(_employee.Dependents.ToList());
        TotalOlderDepsSurcharge = calculateOlderDepsDeduction(_employee.Dependents.ToList());
        TotalPayPeriodDeductions = calculatePerPaycheckDeduction();
        NetPay = calculatePaycheckWithDeductions();

    }

    public decimal calculatePaycheckWithDeductions() {
        // Salary surcharge not on a monthly basis which is why I separated it from the monthly benefit calculations
        return Math.Round(GrossPay - TotalPayPeriodDeductions, 2);
    }

    public decimal calculateTotalMonthlyDeductions() {
        return BaseCost  + TotalDependentCost + TotalAdditionalPartnerCost + TotalOlderDepsSurcharge;
    }

    public decimal calculateGrossPaycheckPerPaycheck() {
        return Math.Round(_employee.Salary/26, 2);
    }
    
    // Salary surcharge not on a monthly basis which is why I separated it from the monthly benefit calculations
    public decimal calculatePerPaycheckDeduction() {
        decimal totalMonthlyDeductons = calculateTotalMonthlyDeductions();
        decimal totalPayPeriodDeductions = ((totalMonthlyDeductons * 12) + TotalSalarySurcharge)/26;
        return Math.Round(totalPayPeriodDeductions, 2);
    }

    // individual calculations
    public decimal calculalateChildDependentDeduction(List<GetDependentDto> dependents) {
        decimal childrenDepsCost = 0;
        var childrenDeps = dependents.Where(dep => dep.Relationship == Relationship.Child);

        if(childrenDeps.Count() > 0 ) {
            childrenDepsCost = childrenDeps.Count() * DependentCost;
        }

        return Math.Round(childrenDepsCost, 2);
    }

    public decimal calculateAdditionalPartnerDeduction(List<GetDependentDto> dependents) {
        // I am using .Any instead of .Where here to avoid the issue of including both domestic partner and spouse
        bool hasSpouseDomesticPartner = dependents.Any(dep => dep.Relationship == Relationship.Spouse || dep.Relationship == Relationship.DomesticPartner);
        return hasSpouseDomesticPartner ? AdditionalPartnerCost : 0;
    }
    

    // configurable or changes yearly 
    public decimal calculateAdditionalSalaryDeduction(decimal salary) {
        decimal additionalCost = salary * SalarySurcharge/ 100;
        return salary > 80000 ? additionalCost: 0;
    }

    // would rather pass paramater as argument
    public decimal calculateOlderDepsDeduction(List<GetDependentDto> dependents) {
        DateTime today = DateTime.Now;
        DateTime olderPersonAdditionalChargeDate = today.AddYears(-50);
        var olderDeps = dependents.Where(dep => dep.DateOfBirth > olderPersonAdditionalChargeDate);
        int olderDepsAmount = olderDeps.Count();
        return olderDepsAmount> 0 ? OlderDepsSurcharge * olderDepsAmount : 0;
    }

}