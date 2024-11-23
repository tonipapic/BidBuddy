using EntitiesLayer.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF;
using System.Drawing;
namespace BusinessLogicLayer.Services
{
    public class StatisticsService
    {
        /// <summary>
        /// Creates PDF document for a given auction
        /// PDF contains information about auction, table of all bidders and images of auction
        /// Uses QuestPDF library
        /// 
        /// </summary>
        /// <remarks>Toni Papić</remarks>
        /// <param name="filePath"></param>
        /// <param name="auction"></param>
        public static void GeneratePDF(string filePath, Auction auction)
        {
            Settings.EnableCaching = true;
            Settings.License=LicenseType.Professional;
            Settings.CheckIfAllTextGlyphsAreAvailable = false;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Izvjestaj za aukciju #"+auction.AuctionId+" "+auction.Name)
                        
                        .SemiBold().FontSize(26);
                    page.Content()
                    .Text(PrepareContent(auction));
                    
              
                    
                });
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Tablica svih ponuditelja")

                        .SemiBold().FontSize(26);



                    if (auction.AuctionBids.Count > 0)
                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(column =>
                            {
                                column.RelativeColumn();
                                column.RelativeColumn();
                            });


                            foreach (var bid in auction.AuctionBids.Where(p => p.Value > 0).OrderByDescending(p => p.Value).ToList())
                            {
                                table.Cell().Text(bid.User.FullName);

                                table.Cell().Text(bid.Value.ToString("N0",CultureInfo.CurrentCulture) + " €");

                            }
                        }
                        );
                });
                if (auction.AuctionImages.Count > 0) { 
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(20));

                        page.Header()
                            .Text("Slike aukcije")

                            .SemiBold().FontSize(26);
                    
                        page.Content().Column(column =>
                        {
                            
                            foreach(var image in auction.AuctionImages)
                            column.Item().Image(image.ImageData).UseOriginalImage(true); 
                        });
                    });
                }
            })
            .GeneratePdf(filePath+"\\Izvještaj_aukcija_#"+auction.AuctionId+"_"+auction.Name+".pdf");
        }

        /// <summary>
        /// Prepares auction information for PDF
        /// </summary>
        /// <remarks>Toni Papić</remarks>
        /// <param name="auction"></param>
        /// <returns>String of information</returns>

        private static string PrepareContent(Auction auction)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Naziv aukcije: " + auction.Name + "\n");
            sb.Append("Opis aukcije: " + auction.Description + "\n");
            sb.Append("Datum kreiranja: " + auction.CreationDate.ToString("dd.MM.yyyy. HH:mm") + "\n");
            sb.Append("Datum završetka: " + auction.EndDate.ToString("dd.MM.yyyy. HH:mm") + "\n");
            sb.Append("Pocetna cijena: " + auction.MinimalBidPrice + "\n");
            sb.Append("Instant cijena: " + auction.InstantBuyPrice ?? "nema"+ "\n");
            sb.Append("Kategorija: " + auction.Category.Name + "\n");
            sb.Append("Regija: " + auction.Region.Name + "\n");
            sb.Append("Stanje proizvoda: " + auction.ProductState.Name + "\n");
            sb.Append("Stanje aukcije: " + auction.AuctionState.Name + "\n");
            sb.Append("Broj ponuda: " + auction.AuctionBids.Count + "\n");
            sb.Append("Pobjednik: " + auction.AuctionBids.Where(x => x.Selected).FirstOrDefault()?.User.FullName);

            
            return sb.ToString();
        }

    }
}
