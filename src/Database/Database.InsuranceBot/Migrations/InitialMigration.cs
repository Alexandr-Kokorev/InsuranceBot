using FluentMigrator;

namespace Database.InsuranceBot.Migrations;

[Migration(20250716120000)]
public class InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table("AuditLogs")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable()
            .WithColumn("Action").AsString().Nullable()
            .WithColumn("State").AsString().Nullable()
            .WithColumn("Timestamp").AsDateTime2().NotNullable();

        Create.Table("Conversations")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable()
            .WithColumn("Request").AsString(int.MaxValue).Nullable()
            .WithColumn("Response").AsString(int.MaxValue).Nullable()
            .WithColumn("Timestamp").AsDateTime2().NotNullable();

        Create.Table("Documents")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable()
            .WithColumn("Type").AsString(int.MaxValue).Nullable()
            .WithColumn("Path").AsString(int.MaxValue).Nullable()
            .WithColumn("UploadedAt").AsDateTime2().NotNullable()
            .WithColumn("Hash").AsString(int.MaxValue).Nullable();

        Create.Table("Errors")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Message").AsString(int.MaxValue).Nullable()
            .WithColumn("StackTrace").AsString(int.MaxValue).Nullable()
            .WithColumn("Timestamp").AsDateTime2().NotNullable();


        Create.Table("ExtractedFields")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("DocumentId").AsGuid().NotNullable()
            .WithColumn("FieldName").AsString(int.MaxValue).Nullable()
            .WithColumn("FieldValue").AsString(int.MaxValue).Nullable();

        Create.Table("Policies")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable()
            .WithColumn("FilePath").AsString(int.MaxValue).Nullable()
            .WithColumn("IssuedAt").AsDateTime2().NotNullable()
            .WithColumn("ExpiresAt").AsDateTime2().NotNullable()
            .WithColumn("Status").AsString(int.MaxValue).Nullable();


        Create.Table("PolicyEvents")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("PolicyId").AsGuid().NotNullable()
            .WithColumn("EventType").AsString(int.MaxValue).Nullable()
            .WithColumn("Timestamp").AsDateTime2().NotNullable();

        Create.Table("Users")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity().NotNullable()
            .WithColumn("TelegramUserId").AsInt64().NotNullable()
            .WithColumn("FirstName").AsString(int.MaxValue).Nullable()
            .WithColumn("LastName").AsString(int.MaxValue).Nullable()
            .WithColumn("Username").AsString(int.MaxValue).Nullable()
            .WithColumn("CurrentState").AsString(int.MaxValue).Nullable()
            .WithColumn("UploadAttempts").AsInt32().NotNullable()
            .WithColumn("CreatedAt").AsDateTime2().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime2().NotNullable()
            .WithColumn("IsAdmin").AsBoolean().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("AuditLogs");

        Delete.Table("Conversations");

        Delete.Table("Documents");

        Delete.Table("Errors");

        Delete.Table("ExtractedFields");

        Delete.Table("Policies");

        Delete.Table("PolicyEvents");

        Delete.Table("Users");
    }
}