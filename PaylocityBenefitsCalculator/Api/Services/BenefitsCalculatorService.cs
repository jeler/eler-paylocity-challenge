using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Apis.Models;


class BenefitsCalculatorService: IBenefitsCalculatorService
{

    public decimal BaseCost {get; private set;}
    public decimal SalaryCap {get; private set;}

    public decimal DependentCost { get; private set; }
    public decimal SalarySurcharge { get; private set; }
    public decimal OlderDepsSurcharge { get; private set; }
    public decimal AdditionalPartnerCost { get; private set; }

    public BenefitsCalculatorService()
    {
        Benefit b = new Benefit();
        BaseCost = b.BaseCost;
        AdditionalPartnerCost = b.AdditionalPartnerCost;
        DependentCost = b.DependentCost;
        SalarySurcharge = b.SalarySurcharge;
        SalaryCap = b.SalaryCap;
    }

    public BenefitResponse createBenefitsPackage(GetEmployeeDto _employee) {
        return new BenefitResponse {
            GrossPay = calculateGrossPaycheckPerPaycheck(_employee),
            TotalDependentCost = calculalateChildDependentDeduction(_employee.Dependents.ToList()),
            TotalAdditionalPartnerCost = calculateAdditionalPartnerDeduction(_employee.Dependents.ToList()),
            TotalOlderDepsSurcharge = calculateOlderDepsDeduction(_employee.Dependents.ToList()),
            TotalPayPeriodDeductions = calculateTotalPaycheckDeductions(_employee),
            NetPay = calculateNetPaycheck(_employee)
        };
    }


    private decimal calculateNetPaycheck(GetEmployeeDto employee) {
        // Salary surcharge not on a monthly basis which is why I separated it from the monthly benefit calculations
        decimal grossPayCheck = calculateGrossPaycheckPerPaycheck(employee);
        decimal salaryDeduction = calculateAdditionalSalaryDeduction(employee.Salary);
        decimal totalDeductions = calculateTotalPaycheckDeductions(employee) - salaryDeduction;
        return Math.Round(grossPayCheck - totalDeductions, 2);
    }

    private decimal calculateTotalPaycheckDeductions(GetEmployeeDto employee) {
        decimal totalDepsCost = calculalateChildDependentDeduction(employee.Dependents);
        decimal partnerDepsCost = calculateAdditionalPartnerDeduction(employee.Dependents);
        decimal olderDepsSurcharge = calculateOlderDepsDeduction(employee.Dependents);
        return BaseCost  + partnerDepsCost + totalDepsCost + olderDepsSurcharge;
    }

    private decimal calculateGrossPaycheckPerPaycheck(GetEmployeeDto employee) {
        return Math.Round(employee.Salary/26, 2);
    }

    // individual calculations
    private decimal calculalateChildDependentDeduction(ICollection<GetDependentDto> dependents) {
        var childrenDeps = dependents.Where(dep => dep.Relationship == Api.Models.Relationship.Child);
        decimal childrenDepsCostPerMonth = childrenDeps.Count() * DependentCost;
        return calculatePerPaycheckAmt(childrenDepsCostPerMonth);
    }

    private decimal calculateAdditionalPartnerDeduction(ICollection<GetDependentDto> dependents) {
        // I am using .Any instead of .Where here to avoid the issue of including both domestic partner and spouse
        bool hasSpouseDomesticPartner = dependents.Any(dep => dep.Relationship == Api.Models.Relationship.Spouse || dep.Relationship == Api.Models.Relationship.DomesticPartner);
        return hasSpouseDomesticPartner ? calculatePerPaycheckAmt(AdditionalPartnerCost) : 0;
    }
    

    // configurable or changes yearly 
    private decimal calculateAdditionalSalaryDeduction(decimal salary) {
        decimal additionalCost = salary * SalarySurcharge/ 100;
        return salary > SalaryCap ? calculatePerPaycheckAmt(additionalCost): 0;
    }

    // would rather pass paramater as argument
    private decimal calculateOlderDepsDeduction(ICollection<GetDependentDto> dependents) {
        DateTime today = DateTime.Now;
        DateTime olderPersonAdditionalChargeDate = today.AddYears(-50);
        var olderDeps = dependents.Where(dep => dep.DateOfBirth > olderPersonAdditionalChargeDate);
        int olderDepsAmount = olderDeps.Count();
        return olderDepsAmount> 0 ? calculatePerPaycheckAmt(OlderDepsSurcharge * olderDepsAmount) : 0;
    }

    private decimal calculatePerPaycheckAmt(decimal monthlyCost) {
        return Math.Round(monthlyCost*12/26);
    }
}