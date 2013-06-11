namespace NWServerAdminPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Areas", "Name", c => c.String());
            AddColumn("dbo.Areas", "Tags", c => c.String());
            AddColumn("dbo.Areas", "resref", c => c.String());
            AddColumn("dbo.Areas", "oldresref", c => c.String());
            DropColumn("dbo.Areas", "Owner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Areas", "Owner", c => c.String());
            DropColumn("dbo.Areas", "oldresref");
            DropColumn("dbo.Areas", "resref");
            DropColumn("dbo.Areas", "Tags");
            DropColumn("dbo.Areas", "Name");
        }
    }
}
