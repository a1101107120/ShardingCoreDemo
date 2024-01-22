using System;
using System.Text.Json.Serialization;
using ShardingProj.Entities;
using ShardingProj.Enum;
using ShardingProj.Func;

namespace ShardingProj.DbModel;

/// <summary>
/// order table
/// </summary>
public class Order
{
    /// <summary>
    /// order Id
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// payer id
    /// </summary>
    public string Payer { get; set; }
    /// <summary>
    /// pay money cent
    /// </summary>
    public long Money { get; set; }
    /// <summary>
    /// area
    /// </summary>
    public string Area { get; set; }
    /// <summary>
    /// order status
    /// </summary>
    public OrderStatusEnum OrderStatus { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    [JsonIgnore]
    public long CreateTime { get; set; } = DateTimeHelper.ConvertDateTimeToLong(DateTime.Now);
}

public class OrderProduct
{
    public string Id { get; set; }

    public string ProductId { get; set; }

    public string OrderId { get; set; }

    [JsonIgnore]
    public long CreateTime { get; set; } = DateTimeHelper.ConvertDateTimeToLong(DateTime.Now);
}