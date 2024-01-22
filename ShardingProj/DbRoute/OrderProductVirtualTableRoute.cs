using ShardingCore.Core.EntityMetadatas;
using ShardingCore.VirtualRoutes.Mods;
using ShardingProj.DbModel;

namespace ShardingProj.DbRoute;

public class OrderProductVirtualTableRoute : AbstractSimpleShardingModKeyStringVirtualTableRoute<OrderProduct>
{

    private const int _tailLength = 2;
    private const int _tableTotal = 10;
    public override void Configure(EntityMetadataTableBuilder<OrderProduct> builder)
    {
        //设置分表字段
        builder.ShardingProperty(o => o.CreateTime);
        //是否启动的时候创建表，仅在启动时判断该属性，
        //如果你是按时间分表的那么这个属性将不会在特定时间创建对应的表信息需要手动进行表创建和添加
        //当然您也可以使用系统默认的时间路由，通过重写AutoCreateByTime
        builder.AutoCreateTable(null);
        //分割符
        builder.TableSeparator("_");
    }

    public OrderProductVirtualTableRoute() : base(_tailLength, _tableTotal)
    {
    }
    public override string ShardingKeyToTail(object shardingKey)
    {
        //進入表的規則
        if (shardingKey is long dateTime)
        {
            return (dateTime % _tableTotal).ToString().PadLeft(_tailLength, '0');
        }

        throw new ArgumentException("Invalid sharding key type.");
    }


}