namespace Api.Models;
public class BenefitResponse
{
    public decimal NetPay {get; set;}
    
    // per pay period with no deductions
    public decimal GrossPay {get; set;}

    public decimal TotalAdditionalPartnerCost {get; set; }
    public decimal TotalDependentCost {get; set;}
    public decimal TotalSalarySurcharge {get; set;}
    public decimal TotalOlderDepsSurcharge {get; set;}
    
    public decimal TotalPayPeriodDeductions {get; set;}

}
