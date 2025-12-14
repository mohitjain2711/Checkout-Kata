using MarketCheckout.PricingRules;

namespace MarketCheckout
{
    public class Checkout : ICheckout
    {
        private readonly Dictionary<string, PricingRule> _pricingRules;
        private readonly Dictionary<string, int> _scanned = new();

        public Checkout(IPricingRuleProvider pricingRuleProvider)
        {
            _pricingRules = pricingRuleProvider
            .GetRules()
            .ToDictionary(r => r.Sku);
        }

        public void Scan(string item)
        {
            if (string.IsNullOrWhiteSpace(item)) throw new ArgumentException("item cannot be null or empty", nameof(item));
            if (!_pricingRules.ContainsKey(item)) throw new ArgumentException($"Unknown SKU: '{item}'", nameof(item));

            if (!_scanned.ContainsKey(item)) _scanned[item] = 0;
            _scanned[item]++;
        }

        public int GetTotalPrice()
        {
            int total = 0;
            foreach (var kv in _scanned)
            {
                var sku = kv.Key;
                var count = kv.Value;
                var rule = _pricingRules[sku];

                if (rule.SpecialQuantity.HasValue && rule.SpecialPrice.HasValue)
                {
                    int q = rule.SpecialQuantity.Value;
                    int groups = count / q;
                    int rem = count % q;
                    total += groups * rule.SpecialPrice.Value + rem * rule.UnitPrice;
                }
                else
                {
                    total += count * rule.UnitPrice;
                }
            }
            return total;
        }
    }
}
