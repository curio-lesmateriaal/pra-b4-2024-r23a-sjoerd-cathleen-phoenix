using PRA_B4_FOTOKIOSK.magie;
using PRA_B4_FOTOKIOSK.models;
using System;
using System.Collections.Generic;
using System.IO;

namespace PRA_B4_FOTOKIOSK.controller
{
    public class PictureController
    {
        // De window die we laten zien op het scherm
        public static Home Window { get; set; }

        // De lijst met fotos die we laten zien
        public List<KioskPhoto> PicturesToDisplay = new List<KioskPhoto>();

        //directories voor elke dag
        private readonly Dictionary<DayOfWeek, string> dayDirectories = new Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Sunday, @"../../../fotos/0_Zondag" },
            { DayOfWeek.Monday, @"../../../fotos/1_Maandag" },
            { DayOfWeek.Tuesday, @"../../../fotos/2_Dinsdag" },
            { DayOfWeek.Wednesday, @"../../../fotos/3_Woensdag" },
            { DayOfWeek.Thursday, @"../../../fotos/4_Donderdag" },
            { DayOfWeek.Friday, @"../../../fotos/5_vrijdag"},
            { DayOfWeek.Saturday, @"../../../fotos/6_Zaterdag" }
        };

        // Start methode die wordt aangeroepen wanneer de foto pagina opent.
        public void Start()
        {
            var now = DateTime.Now;
            var day = now.DayOfWeek;

            // Initializeer de lijst met fotos
            if (dayDirectories.TryGetValue(day, out string directory))
            {
                LoadPicturesFromDirectory(directory);
            }
            else
            {
                Console.WriteLine($"No directory assigned for {day}.");
            }

            // Update de fotos
            PictureManager.UpdatePictures(PicturesToDisplay);
        }

        private void LoadPicturesFromDirectory(string rootDir)
        {
            try
            {
                var now = DateTime.Now;
                var lowerBound = now.AddMinutes(-30);
                var upperBound = now.AddMinutes(-2);

                foreach (string file in Directory.GetFiles(rootDir, "*.jpg", SearchOption.AllDirectories))
                {
                    if (TryParseDateTimeFromFileName(file, out DateTime photoDateTime))
                    {
                        if (photoDateTime >= lowerBound && photoDateTime <= upperBound)
                        {
                            PicturesToDisplay.Add(new KioskPhoto() { Id = 0, Source = file });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the error
                Console.WriteLine($"Error loading pictures from directory '{rootDir}': {ex.Message}");
            }
        }

        private bool TryParseDateTimeFromFileName(string filePath, out DateTime photoDateTime)
        {
            photoDateTime = default;

            try
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var parts = fileName.Split('_');

                if (parts.Length >= 3)
                {
                    var timePart = $"{parts[0]}:{parts[1]}:{parts[2]}";
                    var currentDate = DateTime.Today; // Gebruik de huidige datum en voeg de tijd van de bestandsnaam toe
                    photoDateTime = DateTime.Parse($"{currentDate.ToShortDateString()} {timePart}");
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        // Wordt uitgevoerd wanneer er op de Refresh knop is geklikt
        public void RefreshButtonClick()
        {
            // Refresh logic, if any
        }
    }
}
