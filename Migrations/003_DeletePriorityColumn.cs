using FluentMigrator;

namespace TaskManager.Migrations;

[Migration(3)]
public class RemovePriorityColumn : Migration
{
    public override void Up()
    {
        Console.WriteLine("Up: Видалення стовпця Priority");
        Delete.Column("Priority").FromTable("Tasks");
    }

    public override void Down()
    {
        Console.WriteLine("Down: Додавання стовпця Priority");
        Alter.Table("Tasks")
            .AddColumn("Priority").AsString().Nullable();
    }
}
