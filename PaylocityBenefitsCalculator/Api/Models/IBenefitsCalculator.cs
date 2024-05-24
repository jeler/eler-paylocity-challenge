namespace Apis.Models;

public interface IBenefitsCalculator: IBenefit {
  // per pay period net pay with deductions
    public decimal NetPay {get; set;}
    
    // per pay period with no deductions
    public decimal GrossPay {get; set;}

    public decimal TotalPayPeriodDeductions {get; set;}

    public decimal TotalAdditionalPartnerCost {get; set; }
    public decimal TotalDependentCost {get; set;}
    public decimal TotalSalarySurcharge {get; set;}
    public decimal TotalOlderDepsSurcharge {get; set;}
}