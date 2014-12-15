namespace Memorialis.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ClientId = c.String(),
                        Secret = c.String(),
                        RedirectUrl = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DevLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Title = c.String(),
                        Text = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 32),
                        Value = c.String(nullable: false),
                        Group = c.Int(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 32),
                        Ticket = c.String(),
                        UserId = c.Guid(nullable: false),
                        Issued = c.DateTime(nullable: false),
                        Expired = c.DateTime(nullable: false),
                        Used = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Code)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 32),
                        PasswordHash = c.String(maxLength: 128),
                        FullName_Name = c.String(nullable: false, maxLength: 32),
                        FullName_Surename = c.String(nullable: false, maxLength: 32),
                        FullName_Patronym = c.String(maxLength: 32),
                        FullName_Nick = c.String(maxLength: 32),
                        Email = c.String(maxLength: 128),
                        EmailConfirmed = c.Boolean(nullable: false),
                        Phone = c.String(maxLength: 16),
                        PhoneConfirmed = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName)
                .Index(t => t.Email)
                .Index(t => t.Phone);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tokens", "UserId", "dbo.Users");
            DropIndex("dbo.Users", new[] { "Phone" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.Tokens", new[] { "UserId" });
            DropIndex("dbo.Tokens", new[] { "Code" });
            DropIndex("dbo.Settings", new[] { "Name" });
            DropTable("dbo.Users");
            DropTable("dbo.Tokens");
            DropTable("dbo.Settings");
            DropTable("dbo.DevLogs");
            DropTable("dbo.Clients");
        }

        
    }
}
