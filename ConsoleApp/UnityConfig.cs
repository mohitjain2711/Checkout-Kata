using MarketCheckout;
using MarketCheckout.PricingRules;
using Unity;

namespace Kata
{
    class UnityConfig
    {
        public static IUnityContainer RegisterComponents()
        {
            var container = new UnityContainer();

            // Register pricing rule provider
            container.RegisterType<IPricingRuleProvider, PricingRuleProvider>();

            // Register checkout
            container.RegisterType<ICheckout, Checkout>();

            return container;
        }
    }
}
