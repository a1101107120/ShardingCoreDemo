using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using Pomelo.EntityFrameworkCore.MySql.Migrations;
using ShardingCore.Core.RuntimeContexts;
using ShardingCore.Helpers;

namespace ShardingProj.DbContext;

public class ShardingMysqlMigrationsSqlGenerator : MySqlMigrationsSqlGenerator
{
    private readonly IShardingRuntimeContext _shardingRuntimeContext;
    public ShardingMysqlMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider annotationProvider, IMySqlOptions options, IShardingRuntimeContext shardingRuntimeContext) : base(dependencies, annotationProvider, options)
    {
        _shardingRuntimeContext = shardingRuntimeContext;
    }

    protected override void Generate(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
    {
        var oldCmds = builder.GetCommandList().ToList();
        base.Generate(operation, model, builder);
        var newCmds = builder.GetCommandList().ToList();
        var addCmds = newCmds.Where(r => !oldCmds.Contains(r)).ToList();
        
        MigrationHelper.Generate(_shardingRuntimeContext,operation,builder,Dependencies.SqlGenerationHelper, addCmds);
    }
}