namespace Apis.Models;

public class Benefit: IBenefit {
    public decimal BaseCost { get ; set;}
    public decimal AdditionalPartnerCost {get; set;}
    public decimal DependentCost {get; set;}
    public decimal SalarySurcharge {get; set;}
    public decimal OlderDepsSurcharge {get; set;}

    public decimal SalaryCap {get; set;}

    public Benefit(decimal baseCost = 1000, decimal additionalPartnerCost =  1000, decimal dependentCost = 600, int salarySurcharge = 2, decimal olderDepsSurcharge = 200, decimal salaryCap = 80000m) {
        BaseCost = baseCost;
        AdditionalPartnerCost = additionalPartnerCost;
        DependentCost = dependentCost;
        SalarySurcharge = salarySurcharge;
        OlderDepsSurcharge = olderDepsSurcharge;
        SalaryCap = salaryCap;
    }

}