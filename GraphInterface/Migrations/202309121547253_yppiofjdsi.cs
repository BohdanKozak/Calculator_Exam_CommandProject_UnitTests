namespace GraphInterface.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yppiofjdsi : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CalculatorValues", "TypeOfTest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CalculatorValues", "TypeOfTest", c => c.Int(nullable: false));
        }
    }
}
