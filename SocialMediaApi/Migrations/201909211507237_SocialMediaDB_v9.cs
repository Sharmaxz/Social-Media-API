namespace SocialMediaApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SocialMediaDB_v9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Img = c.String(),
                        Favorite = c.Boolean(nullable: false),
                        Timestamp = c.String(),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Client", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Post", "Owner_Id", "dbo.Client");
            DropIndex("dbo.Post", new[] { "Owner_Id" });
            DropTable("dbo.Post");
        }
    }
}
