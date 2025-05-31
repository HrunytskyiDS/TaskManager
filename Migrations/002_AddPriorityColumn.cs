using FluentMigrator;

namespace TaskManager.Migrations;

[Migration(2)]
public class AddPriorityColumn : Migration
{
    public override void Up()
    {
        Console.WriteLine("Up: Додавання стовпця Priority");
        Alter.Table("Tasks")
            .AddColumn("Priority").AsInt32().WithDefaultValue(0);
    }

    public override void Down()
    {
        Console.WriteLine("Down: Видалення стовпця Priority");
        Delete.Column("Priority").FromTable("Tasks");
    }
}
