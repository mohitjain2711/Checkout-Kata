using MarketCheckout;
using MarketCheckout.PricingRules;
using Moq;

namespace MarketCheckoutTest
{
    public class CheckoutTests
    {
        private readonly Mock<IPricingRuleProvider> _pricingRuleProviderMock;
        private readonly Checkout _checkout;

        public CheckoutTests()
        {
            _pricingRuleProviderMock = new Mock<IPricingRuleProvider>();

            _pricingRuleProviderMock
                .Setup(p => p.GetRules())
                .Returns(new[]
                {
                new PricingRule("A", 50, 3, 130),
                new PricingRule("B", 30, 2, 45),
                new PricingRule("C", 20),
                new PricingRule("D", 15)
                });

            _checkout = new Checkout(_pricingRuleProviderMock.Object);
        }

        [Fact]
        public void GetTotalPrice_NoItems_ReturnsZero()
        {
            var total = _checkout.GetTotalPrice();

            Assert.Equal(0, total);
        }

        [Fact]
        public void Scan_SingleItem_NoSpecial_ReturnsUnitPrice()
        {
            _checkout.Scan("C");

            Assert.Equal(20, _checkout.GetTotalPrice());
        }

        [Fact]
        public void Scan_ThreeAs_AppliesSpecialPrice()
        {
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");

            Assert.Equal(130, _checkout.GetTotalPrice());
        }

        [Fact]
        public void Scan_SixAs_AppliesSpecialPriceTwice()
        {
            for (int i = 0; i < 6; i++)
                _checkout.Scan("A");

            Assert.Equal(260, _checkout.GetTotalPrice());
        }

        [Fact]
        public void Scan_MixedItems_AnyOrder_AppliesCorrectPricing()
        {
            _checkout.Scan("B");
            _checkout.Scan("A");
            _checkout.Scan("B");

            Assert.Equal(95, _checkout.GetTotalPrice());
        }

        [Fact]
        public void Scan_AllItems_Combination_ReturnsCorrectTotal()
        {
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("C");
            _checkout.Scan("D");

            Assert.Equal(210, _checkout.GetTotalPrice());
        }

        [Fact]
        public void Scan_UnknownSku_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _checkout.Scan("X"));

            Assert.Contains("Unknown SKU", ex.Message);
        }

        [Fact]
        public void PricingRules_AreLoadedOnce()
        {
            var mockProvider = new Mock<IPricingRuleProvider>();

            mockProvider.Setup(p => p.GetRules())
                .Returns(new[]
                {
            new PricingRule("A", 50, 3, 130)
                });

            var checkout = new Checkout(mockProvider.Object);

            checkout.Scan("A");
            checkout.GetTotalPrice();

            mockProvider.Verify(p => p.GetRules(), Times.Once);
        }
    }
}