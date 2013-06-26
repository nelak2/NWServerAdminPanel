namespace NWServerAdminPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Areas", "Resref", c => c.String());
            AlterColumn("dbo.Areas", "Oldresref", c => c.String());
            AlterColumn("dbo.Areas", "Are", c => c.Binary());
            AlterColumn("dbo.Areas", "Gic", c => c.Binary());
            AlterColumn("dbo.Areas", "Git", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Areas", "git", c => c.Binary());
            AlterColumn("dbo.Areas", "gic", c => c.Binary());
            AlterColumn("dbo.Areas", "are", c => c.Binary());
            AlterColumn("dbo.Areas", "oldresref", c => c.String());
            AlterColumn("dbo.Areas", "resref", c => c.String());
        }
    }
}
