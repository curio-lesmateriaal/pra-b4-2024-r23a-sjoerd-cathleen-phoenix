using PRA_B4_FOTOKIOSK.magie;
using PRA_B4_FOTOKIOSK.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PRA_B4_FOTOKIOSK.controller
{
    public class PictureController
    {
        // De window die we laten zien op het scherm
        public static Home Window { get; set; }

        // De lijst met fotos die we laten zien
        public List<KioskPhotoPair> PicturesToDisplay = new List<KioskPhotoPair>();

        // Map to hold directories for each day
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
                LoadAndPairPictures(directory);
            }
            else
            {
                Console.WriteLine($"No directory assigned for {day}.");
            }

            // Update de fotos
            PictureManager.UpdatePictures(PicturesToDisplay.SelectMany(pair => new[] { pair.FirstPhoto, pair.SecondPhoto }).ToList());
        }

        private void LoadAndPairPictures(string rootDir)
        {
            try
            {
                var now = DateTime.Now;
                var lowerBound = now.AddMinutes(-30);
                var upperBound = now.AddMinutes(-2);

                var allPhotos = new List<(DateTime PhotoTime, string FilePath)>();

                foreach (string file in Directory.GetFiles(rootDir, "*.jpg", SearchOption.AllDirectories))
                {
                    if (TryParseDateTimeFromFileName(file, out DateTime photoDateTime))
                    {
                        if (photoDateTime >= lowerBound && photoDateTime <= upperBound)
                        {
                            allPhotos.Add((photoDateTime, file));
                        }
                    }
                }

                // Sorteer alle foto's op tijdstempel
                var sortedPhotos = allPhotos.OrderBy(photo => photo.PhotoTime).ToList();

                // Koppel foto's
                for (int i = 0; i < sortedPhotos.Count - 1; i++)
                {
                    var currentPhoto = sortedPhotos[i];
                    var nextPhoto = sortedPhotos[i + 1];

                    if ((nextPhoto.PhotoTime - currentPhoto.PhotoTime).TotalSeconds == 60)
                    {
                        PicturesToDisplay.Add(new KioskPhotoPair
                        {
                            FirstPhoto = new KioskPhoto { Id = 0, Source = currentPhoto.FilePath },
                            SecondPhoto = new KioskPhoto { Id = 0, Source = nextPhoto.FilePath }
                        });

                        i++; // Skip the next photo as it's already paired
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

    // A class to hold paired photos
    public class KioskPhotoPair
    {
        public KioskPhoto FirstPhoto { get; set; }
        public KioskPhoto SecondPhoto { get; set; }
    }
}
