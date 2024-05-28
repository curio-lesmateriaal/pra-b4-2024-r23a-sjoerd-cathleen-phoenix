using PRA_B4_FOTOKIOSK.magie;
using PRA_B4_FOTOKIOSK.models;
using System;
using System.IO;
using System.Text;

namespace PRA_B4_FOTOKIOSK.controller
{
    public class ShopController
    {
        public static Home Window { get; set; }

        public void Start()
        {
            // Stel de prijslijst in aan de rechterkant.
            ShopManager.SetShopPriceList("Prijs:\n");

            // Stel de bon in onderaan het scherm
            ShopManager.SetShopReceipt("Eindbedrag:\n");

            // Vul de productlijst met producten
            ShopManager.Products.Add(new KioskProduct() { Name = "Foto 10x15", Price = 2.55f });
            ShopManager.Products.Add(new KioskProduct() { Name = "Foto 20x30", Price = 4.95f });
            ShopManager.Products.Add(new KioskProduct() { Name = "Mok met foto", Price = 9.95f });
            ShopManager.Products.Add(new KioskProduct() { Name = "Sleutelhanger met foto", Price = 6.12f });
            ShopManager.Products.Add(new KioskProduct() { Name = "T-shirt met foto", Price = 11.99f });

            foreach (KioskProduct product in ShopManager.Products)
            {
                ShopManager.AddShopPriceList($"{product.Name}: €{product.Price}\n");
            }

            // Update dropdown met producten
            ShopManager.UpdateDropDownProducts();
        }

        // Wordt uitgevoerd wanneer er op de Toevoegen knop is geklikt
        public void AddButtonClick()
        {
            ShopManager.AddShopReceipt(ShopManager.GetCheckout());
        }

        // Wordt uitgevoerd wanneer er op de Resetten knop is geklikt
        public void ResetButtonClick()
        {
            ShopManager.SetShopReceipt("Eindbedrag:\n");
        }

        // Wordt uitgevoerd wanneer er op de Save knop is geklikt
        public void SaveButtonClick()
        {
            string receiptContent = GenerateReceiptContent();
            SaveReceiptToFile(receiptContent);
            try
            {
                SaveReceiptToFile(receiptContent);
                Console.WriteLine("De bon is opgeslagen in bon.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het opslaan van de bon: {ex.Message}");
            }
        }

        private string GenerateReceiptContent()
        {
            StringBuilder receiptBuilder = new StringBuilder();
            receiptBuilder.AppendLine("Bon inhoud:");

            float total = 0;

            foreach (var product in ShopManager.Products)
            {
                receiptBuilder.AppendLine($"{product.Name}: €{product.Price}");
                total += product.Price;
            }

            receiptBuilder.AppendLine($"Totaal: €{total}");
            return receiptBuilder.ToString();
        }

        private void SaveReceiptToFile(string receiptContent)
        {
            string filePath = "bon.txt";
            File.WriteAllText(filePath, receiptContent);
        }
    }
}
