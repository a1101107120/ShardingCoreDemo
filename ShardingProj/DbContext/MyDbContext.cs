using Microsoft.EntityFrameworkCore;
using ShardingCore.Core.VirtualRoutes.TableRoutes.RouteTails.Abstractions;
using ShardingCore.Sharding;
using ShardingCore.Sharding.Abstractions;
using ShardingProj.DbModel;

namespace ShardingProj.DbContext;

public class MyDbContext:AbstractShardingDbContext,IShardingTableDbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).IsRequired().IsUnicode(false).HasMaxLength(50);
            entity.Property(o=>o.Payer).IsRequired().IsUnicode(false).HasMaxLength(50);
            entity.Property(o => o.Area).IsRequired().IsUnicode(false).HasMaxLength(50);
            entity.Property(o => o.OrderStatus).HasConversion<int>();
            //really table name is Order_202101,Order_202102,Order_202103.....

            entity.HasIndex(o => new { o.CreateTime, o.Payer }).HasDatabaseName("IX_Order_CreationTime_Player");
            entity.HasIndex(o => o.Id).HasDatabaseName("IX_Order_Area");

            entity.ToTable(nameof(Order));
        });
        
        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Id).IsRequired().IsUnicode(false).HasMaxLength(50);
            entity.Property(o => o.ProductId).IsRequired().IsUnicode(false).HasMaxLength(50);
            entity.Property(o => o.OrderId).IsRequired().IsUnicode(false).HasMaxLength(50);
            entity.ToTable(nameof(OrderProduct));
        });
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // 設定 Log 屬性為一個委託，可以用來輸出 SQL 指令
        optionsBuilder.LogTo(Console.WriteLine);
    }
    /// <summary>
    /// empty implment if use sharding table
    /// </summary>
    public IRouteTail RouteTail { get; set; }
    
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderProduct> OrderProduct { get; set; }
}

//Route constructor support dependency injection,that's life time scope is `Singleton`