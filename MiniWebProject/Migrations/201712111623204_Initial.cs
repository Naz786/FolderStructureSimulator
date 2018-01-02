namespace MiniWebProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentFolderId = c.Int(nullable: false),
                        Folder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folders", t => t.Folder_Id)
                .Index(t => t.Folder_Id);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentFolderId = c.Int(nullable: false),
                        Name = c.String(),
                        URI = c.String(),
                        Folder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folders", t => t.Folder_Id)
                .Index(t => t.Folder_Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentFolderId = c.Int(nullable: false),
                        Name = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Folder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folders", t => t.Folder_Id)
                .Index(t => t.Folder_Id);
            
            CreateTable(
                "dbo.TextFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentFolderId = c.Int(nullable: false),
                        Name = c.String(),
                        Text = c.String(),
                        Folder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folders", t => t.Folder_Id)
                .Index(t => t.Folder_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TextFiles", "Folder_Id", "dbo.Folders");
            DropForeignKey("dbo.Locations", "Folder_Id", "dbo.Folders");
            DropForeignKey("dbo.Links", "Folder_Id", "dbo.Folders");
            DropForeignKey("dbo.Folders", "Folder_Id", "dbo.Folders");
            DropIndex("dbo.TextFiles", new[] { "Folder_Id" });
            DropIndex("dbo.Locations", new[] { "Folder_Id" });
            DropIndex("dbo.Links", new[] { "Folder_Id" });
            DropIndex("dbo.Folders", new[] { "Folder_Id" });
            DropTable("dbo.TextFiles");
            DropTable("dbo.Locations");
            DropTable("dbo.Links");
            DropTable("dbo.Folders");
        }
    }
}
