namespace GraphInterface.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewRow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalculatorValues", "TypeOfTest", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CalculatorValues", "TypeOfTest");
        }
    }
}
