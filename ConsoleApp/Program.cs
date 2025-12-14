using Kata;
using MarketCheckout;
using Unity;

var container = UnityConfig.RegisterComponents();

var checkout = container.Resolve<ICheckout>();

checkout.Scan("B");
checkout.Scan("A");
checkout.Scan("B");

Console.WriteLine($"Total: {checkout.GetTotalPrice()}");
Console.Read();