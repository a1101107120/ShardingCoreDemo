using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ShardingCore;
using ShardingCore.Sharding.ReadWriteConfigurations;
using ShardingProj.DbContext;
using ShardingProj.DbRoute;
using ShardingProj.Services;

namespace ShardingProj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<DataAccess>();

            var builderConfiguration = builder.Configuration;
            //sharding config
            builder.Services.AddShardingDbContext<MyDbContext>()
                .UseRouteConfig(op =>
                {
                    //添加order 路由
                    op.AddShardingTableRoute<OrderVirtualTableRoute>();
                    // 添加orderProduct 路由
                    op.AddShardingTableRoute<OrderProductVirtualTableRoute>();
                }).UseConfig(op =>
                {
                    ILoggerFactory factory = LoggerFactory.Create(config =>
                    {
                        config.AddConsole();
                    });
                    op.UseShardingMigrationConfigure(b =>
                    {
                        b.ReplaceService<IMigrationsSqlGenerator, ShardingMysqlMigrationsSqlGenerator>();
                    });
                    op.ThrowIfQueryRouteNotMatch = false;
                    op.UseShardingQuery((connStr, builder) =>
                    {
                        //connStr is delegate input param
                        builder.UseMySql(connStr, MySqlServerVersion.AutoDetect(connStr)).UseLoggerFactory(factory);
                    });
                    op.UseShardingTransaction((connection, builder) =>
                    {
                        //connection is delegate input param
                        builder.UseMySql(connection, MySqlServerVersion.AutoDetect(connection.ConnectionString)).UseLoggerFactory(factory);
                    });
                    //use your data base connection string
                    op.AddDefaultDataSource("ds0",
                        builderConfiguration["Appsettings:DbConnetion"]);
                    op.AddReadWriteSeparation(sp =>
                    {
                        return new Dictionary<string, IEnumerable<string>>()
                        {
                            {
                                "ds0", new List<string>()
                                {
                                    builderConfiguration["Appsettings:DbConnetion"]
                                }
                            }
                        };
                    }, ReadStrategyEnum.Loop, defaultEnable: true);
                }).AddShardingCore();
            
            var app = builder.Build();

            //建议补偿表在迁移后面
            using (var scope = app.Services.CreateScope())
            {
                var myDbContext = scope.ServiceProvider.GetService<MyDbContext>();
                //如果没有迁移那么就直接创建表和库
                //myDbContext.Database.EnsureCreated();
                //如果有迁移使用下面的
                myDbContext.Database.Migrate();
            }

//not required, enable check table missing and auto create,非必须  启动检查缺少的表并且创建
            app.Services.UseAutoTryCompensateTable();
            

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();
            
           


            app.MapControllers();

            app.Run();
        }
    }
}