using OrderService.Domain.Buyers.Entities;

namespace OrderService.Domain.Buyers;
public class Buyer : Aggregate
{
    public string IdentityGuild { get; private set; }

    public string Name { get; private set; }

    private List<PaymentMethod> _paymentMethods;
    public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

    protected Buyer()
    {
        _paymentMethods = new List<PaymentMethod>();
    }

    public Buyer(string identity, string name) : this()
    {
        IdentityGuild = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
    }
}
