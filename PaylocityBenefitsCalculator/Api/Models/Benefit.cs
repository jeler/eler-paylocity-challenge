namespace Api.Models;


// 26 per year
public class Benefit {

    public Benefit(decimal inputtedBaseCost = 1000, decimal inputtedAdditionalPartnerCost =  1000, decimal inputtedDependentCost = 600, int inputtedSalarySurcharge = 2, decimal inputtedOlderDepsSurcharge = 200, Employee inputtedEmp) {
        baseCost = inputtedBaseCost;
        additionalPartnerCost = inputtedAdditionalPartnerCost;
        dependentCost = inputtedDependentCost;
        salarySurcharge = inputtedSalarySurcharge;
        olderDepsSurcharge = inputtedOlderDepsSurcharge;
        employee = inputtedEmp;

        TotalCost = CalculateBenefits(inputtedEmp);
    }

    private static decimal baseCost {get; set;}

    private static decimal additionalPartnerCost {get; set;}
    private static decimal dependentCost  {get; set;}
    private static int salarySurcharge {get; set;}

    private static decimal olderDepsSurcharge {get; set;}

    private static Employee employee { get; set; }
    // per month
    public decimal TotalCost { get; set; }


    // divide by 26  = per paycheck

    // subtract this from salary
        public static decimal CalculateBenefits(Employee employee) {

        // employee added
        decimal additionalCost = calculateAdditionalSalaryCost(employee.Salary);
        decimal dependantsCost = calculalateDependentCost(employee.Dependents.ToList());
        decimal additionalPartnerCost = calculateAdditionalPartnerCost(employee.Dependents.ToList());
        return baseCost + additionalCost + dependantsCost + additionalPartnerCost;
     }

    public static decimal calculalateDependentCost(List<Dependent> dependents) {
        decimal childrenDepsCost = 0;
        decimal additionalPartnerCost = 0;
        var childrenDeps = dependents.Where(dep => dep.Relationship == Relationship.Child);

        if(childrenDeps.Count() > 0 ) {
            childrenDepsCost = childrenDeps.Count() * dependentCost;
        }

        return childrenDepsCost + additionalPartnerCost;
    }

    public static decimal calculateAdditionalPartnerCost(List<Dependent> dependents) {
        bool hasSpouseDomesticPartner = dependents.Any(dep => dep.Relationship == Relationship.Spouse || dep.Relationship == Relationship.DomesticPartner);
        return hasSpouseDomesticPartner ? additionalPartnerCost : 0;
    }
    
    public static decimal calculateAdditionalSalaryCost(decimal salary) {
        decimal additionalCost = salary * salarySurcharge/ 100;
        return salary > 80000m ? additionalCost: 0;
    }

    // would rather pass paramater as argument
    public static decimal calculateOlderDeps(List<Dependent> dependents) {
        DateTime today = DateTime.Now;
        DateTime olderPersonAdditionalChargeDate = today.AddYears(-50);
        var olderDeps = dependents.Where(dep => dep.DateOfBirth > olderPersonAdditionalChargeDate);
        int olderDepsAmount = olderDeps.Count();
        return olderDepsAmount> 0 ? olderDepsSurcharge * olderDepsAmount : 0;
    }
}

// should i just make these constants???
// where to put them