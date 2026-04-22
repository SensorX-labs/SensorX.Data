using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;

public record ShippingInfo
{
    public WardId WardId { get; init; }
    public string ShippingAddress { get; init; }
    public string ReceiverName { get; init; }
    public Phone ReceiverPhone { get; init; }

    private ShippingInfo(WardId wardId, string shippingAddress, string receiverName, Phone receiverPhone)
    {
        WardId = wardId;
        ShippingAddress = shippingAddress;
        ReceiverName = receiverName;
        ReceiverPhone = receiverPhone;
    }

    public static ShippingInfo Create(WardId wardId, string shippingAddress, string receiverName, Phone receiverPhone)
    {
        if (string.IsNullOrWhiteSpace(shippingAddress))
            throw new ArgumentException("Địa chỉ giao hàng không được để trống", nameof(shippingAddress));
        if (string.IsNullOrWhiteSpace(receiverName))
            throw new ArgumentException("Tên người nhận không được để trống", nameof(receiverName));
        return new ShippingInfo(wardId, shippingAddress, receiverName, receiverPhone);
    }
}