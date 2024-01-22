using ShardingCore.Core.EntityMetadatas;
using ShardingCore.VirtualRoutes.Months;
using ShardingProj.DbModel;

namespace ShardingProj.DbRoute;

public class OrderVirtualTableRoute:AbstractSimpleShardingMonthKeyDateTimeVirtualTableRoute<Order>
{
    //private readonly IServiceProvider _serviceProvider;

    //public OrderVirtualTableRoute(IServiceProvider serviceProvider)
    //{
    //_serviceProvider = serviceProvider;
    //}
    /// <summary>
    /// fixed value don't use DateTime.Now because if  if application restart this value where change
    /// </summary>
    /// <returns></returns>
    public override DateTime GetBeginTime()
    {
        return new DateTime(2021, 1, 1);
    }
    /// <summary>
    /// configure sharding property
    /// </summary>
    /// <param name="builder"></param>

    public override void Configure(EntityMetadataTableBuilder<Order> builder)
    {
        builder.ShardingProperty(o => o.CreateTime);
    }
    
    /// <summary>
    /// enable auto create table job
    /// </summary>
    /// <returns></returns>

    public override bool AutoCreateTableByTime()
    {
        return true;
    }
}