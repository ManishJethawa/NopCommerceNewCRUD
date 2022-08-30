using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;

namespace XcellenceIT.Plugin.Misc.NewCRUD.Domain
{
    public class NewCRUDBuilder : NopEntityBuilder<NewCRUDDomain>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(NewCRUDDomain.FirstName)).AsString(100).NotNullable()
                .WithColumn(nameof(NewCRUDDomain.LastName)).AsString(50).NotNullable()
                .WithColumn(nameof(NewCRUDDomain.Age)).AsInt32().NotNullable()
                .WithColumn(nameof(NewCRUDDomain.City)).AsString(100).NotNullable()
                ;
        }
    }
}
