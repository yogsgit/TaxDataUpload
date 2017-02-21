namespace TaxDataUpload.Migrations
{
    using DB;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    internal sealed class Configuration : DbMigrationsConfiguration<TaxDataUpload.DB.TaxDataUploadDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TaxDataUpload.DB.TaxDataUploadDB";
        }

        protected override void Seed(TaxDataUpload.DB.TaxDataUploadDB context)
        {
            //  This method will be called after migrating to the latest version.

            XDocument doc = XDocument.Load(Path.Combine("c:\\ISO4217CurrencyList.xml"));
            var countries = doc.Descendants("CcyNtry");
            foreach(var country in countries)
            {
                var ISO4217 = new ISO4217Currency();
                ISO4217.Country = country.Element("CtryNm") != null ? country.Element("CtryNm").Value : null;
                ISO4217.Currency = country.Element("CcyNm") != null ? country.Element("CcyNm").Value : null;
                ISO4217.Code = country.Element("Ccy") != null ? country.Element("Ccy").Value : null;
                ISO4217.NumericCode = country.Element("CcyNbr") != null ? Convert.ToInt32(country.Element("CcyNbr").Value) : -999;
                ISO4217.MinorUnits = country.Element("CcyMnrUnts") != null && country.Element("CcyMnrUnts").Value != "N.A." ? Convert.ToInt32(country.Element("CcyMnrUnts").Value) : -999;
                context.ISO4217Currency.Add(ISO4217);
            }
            context.SaveChanges();
        }
    }
}
