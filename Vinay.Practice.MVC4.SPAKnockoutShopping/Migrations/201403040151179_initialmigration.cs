namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoppingItems",
                c => new
                    {
                        ShoppingItemId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        IsDone = c.Boolean(nullable: false),
                        Price = c.Double(nullable: false),
                        ShoppingCategoryListId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ShoppingItemId)
                .ForeignKey("dbo.ShoppingCategoryLists", t => t.ShoppingCategoryListId, cascadeDelete: true)
                .Index(t => t.ShoppingCategoryListId);
            
            CreateTable(
                "dbo.ShoppingCategoryLists",
                c => new
                    {
                        ShoppingCategoryListId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ShoppingCategoryListId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ShoppingItems", new[] { "ShoppingCategoryListId" });
            DropForeignKey("dbo.ShoppingItems", "ShoppingCategoryListId", "dbo.ShoppingCategoryLists");
            DropTable("dbo.ShoppingCategoryLists");
            DropTable("dbo.ShoppingItems");
        }
    }
}
