public interface IBenefit {
    public decimal BaseCost { get; set; }
    public decimal AdditionalPartnerCost {get; set; }
    public decimal DependentCost {get; set;}
    public decimal SalarySurcharge {get; set;}
    public decimal OlderDepsSurcharge {get; set;}
}