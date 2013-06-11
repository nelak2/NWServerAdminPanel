namespace NWServerAdminPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        are = c.Binary(),
                        gic = c.Binary(),
                        git = c.Binary(),
                        Uploaded = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        Owner = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Areas");
        }
    }
}
