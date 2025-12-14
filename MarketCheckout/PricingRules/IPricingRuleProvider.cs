namespace MarketCheckout.PricingRules
{
    public interface IPricingRuleProvider
    {
        IReadOnlyCollection<PricingRule> GetRules();
    }
}
