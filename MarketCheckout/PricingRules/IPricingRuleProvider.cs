using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketCheckout.PricingRules
{
    public interface IPricingRuleProvider
    {
        IReadOnlyCollection<PricingRule> GetRules();
    }
}
