namespace SocialMediaApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SocialMediaDBv11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Timestamp = c.String(),
                        Recepient_Id = c.Int(),
                        Sender_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Client", t => t.Recepient_Id)
                .ForeignKey("dbo.Client", t => t.Sender_Id)
                .Index(t => t.Recepient_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.Like",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Owner_Id = c.Int(),
                        Comment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Client", t => t.Owner_Id)
                .ForeignKey("dbo.Comment", t => t.Comment_Id)
                .Index(t => t.Owner_Id)
                .Index(t => t.Comment_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "Sender_Id", "dbo.Client");
            DropForeignKey("dbo.Comment", "Recepient_Id", "dbo.Client");
            DropForeignKey("dbo.Like", "Comment_Id", "dbo.Comment");
            DropForeignKey("dbo.Like", "Owner_Id", "dbo.Client");
            DropIndex("dbo.Like", new[] { "Comment_Id" });
            DropIndex("dbo.Like", new[] { "Owner_Id" });
            DropIndex("dbo.Comment", new[] { "Sender_Id" });
            DropIndex("dbo.Comment", new[] { "Recepient_Id" });
            DropTable("dbo.Like");
            DropTable("dbo.Comment");
        }
    }
}
