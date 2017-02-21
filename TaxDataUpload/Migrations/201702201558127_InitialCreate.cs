namespace TaxDataUpload.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ISO4217Currency",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3),
                        Country = c.String(maxLength: 40),
                        Currency = c.String(maxLength: 40),
                        NumericCode = c.Byte(nullable: false),
                        MinorUnits = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.TransactionDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        CurrencyCode = c.String(nullable: false, maxLength: 3),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransactionDatas");
            DropTable("dbo.ISO4217Currency");
        }
    }
}
