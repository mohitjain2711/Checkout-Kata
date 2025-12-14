using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCheckout.PricingRules
{
    public class PricingRuleProvider : IPricingRuleProvider
    {
        public IReadOnlyCollection<PricingRule> GetRules()
        {
            return new List<PricingRule>
        {
            new PricingRule("A", 50, 3, 130),
            new PricingRule("B", 30, 2, 45),
            new PricingRule("C", 20),
            new PricingRule("D", 15)
        };
        }
    }
}
