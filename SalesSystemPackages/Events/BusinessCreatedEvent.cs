namespace SalesSystemPackages.Events;

public class BusinessCreatedEvent 
{
    public string ProductId { get; set; }
    public string PackageId { get; set; }
    public string BusinessId { get; set; }

    public override string ToString()
    {
        return $@"***************************
                  productId: {ProductId},
                  packageId: {PackageId},
                  businessId: {BusinessId}";
    }
}