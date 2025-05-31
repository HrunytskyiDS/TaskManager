using FluentMigrator;

namespace TaskManager.Migrations;

[Migration(2024053101)]
public class CreateTasksTable : Migration
{
    public override void Up()
    {
        Console.WriteLine("Up: Створення таблиці");
        Create.Table("Tasks")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("Description").AsString().Nullable()
            .WithColumn("IsCompleted").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
    }

    public override void Down()
    {
        Console.WriteLine("Down: Видалення таблиці");
        Delete.Table("Tasks");
    }
}
