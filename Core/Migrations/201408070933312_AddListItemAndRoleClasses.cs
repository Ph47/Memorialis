namespace Memorialis.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddListItemAndRoleClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        Weight = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name)
                .Index(t => t.Weight);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Roles", new[] { "Weight" });
            DropIndex("dbo.Roles", new[] { "Name" });
            DropTable("dbo.Roles");
        }
    }
}
