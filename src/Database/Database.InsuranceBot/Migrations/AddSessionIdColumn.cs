using FluentMigrator;

namespace Database.InsuranceBot.Migrations;

[Migration(20250717120000)]
public class AddSessionIdColumnToDocumentsTableMigration : Migration
{
    public override void Up()
    {
        Alter.Table("Documents")
            .AddColumn("SessionId").AsGuid().NotNullable().WithDefaultValue(Guid.Empty);
    }

    public override void Down()
    {
        Delete.Column("SessionId").FromTable("Documents");
    }
}