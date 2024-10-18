

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

namespace MauiAppMapTest.DTO;

public class Order
{
    public Guid Id { get; set; }
    public double TargetPosLati { get; set; }
    public double TargetPosLong { get; set; }
    public string? ToAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? StartDeliveryAt { get; set; }
    public DateTime? OrderClosedAt { get; set; }
    public Guid ClientId { get; set; }
    [JsonIgnore]
    public GDUser Client { get; set; }
    /// <summary>
    /// Наличка, Карта, Онлайн
    /// </summary>
    public string? PayMethod { get; set; }
    public double TotalPrice { get; set; }
    public string? Status { get; set; }
    public Guid? CourierId { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }

    public static implicit operator bool(Order order)
    {
        return order != null && order.Id != Guid.Empty;
    }

    public bool HasItems => OrderItems != null && OrderItems.Any();

}

public class GDUser : IdentityUser<Guid>
{
    public string Address { get; set; } = "";

    public double Balance { get; set; }

    public double PosLati { get; set; }
    public double PosLong { get; set; }
}

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid OrderId { get; set; }
    public Product Product { get; set; }
    [JsonIgnore]
    public Order Order { get; set; }
    public int Amount { get; set; }
}

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageValue { get; set; }
    public double Price { get; set; }
    public string Tags { get; set; }
    public int Amount { get; set; }

    public List<Feedback> Feedbacks { get; set; }
}

public class Feedback
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    [JsonIgnore]
    public Product Product { get; set; }

    public int Stars { get; set; }

    public Guid ClientId { get; set; }

    public GDUser Client { get; set; }

    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
}
