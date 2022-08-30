using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using XcellenceIT.Plugin.Misc.NewCRUD.Domain;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Data
{
    [NopMigration("2022/08/24 05:55:08:9037677", "Misc.NewCRUD base schema")]
    public class SchemaMigrationFile : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<NewCRUDDomain>();
        }
    }
}
