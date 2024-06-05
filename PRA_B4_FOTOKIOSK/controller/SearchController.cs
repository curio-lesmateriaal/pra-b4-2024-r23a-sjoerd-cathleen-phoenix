using PRA_B4_FOTOKIOSK.magie;
using PRA_B4_FOTOKIOSK.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRA_B4_FOTOKIOSK.controller
{
    public class SearchController
    {
        // De window die we laten zien op het scherm
        public static Home Window { get; set; }
        

        // Start methode die wordt aangeroepen wanneer de zoek pagina opent.
        // very veel code smell "Django"
        public void Start()
        {
            SearchManager.Instance = Window;
        }

        // Wordt uitgevoerd wanneer er op de Zoeken knop is geklikt
        public void SearchButtonClick()
        {
            string picture;
            string search = SearchManager.GetSearchInput();
            var now = DateTime.Now;
            int day = (int)now.DayOfWeek;
            bool found = false;

            foreach (string dir in Directory.GetDirectories(@"../../../fotos"))
            {
                string[] parts = Path.GetFileName(dir).Split('_');
                int foldersDays = int.Parse(parts[0]);

                if (foldersDays == day)
                {
                    foreach (string file in Directory.GetFiles(dir))
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        string[] nameParts = fileName.Split('_');

                        int fileHour = int.Parse(nameParts[0]);
                        int fileMinute = int.Parse(nameParts[1]);
                        int fileSecond = int.Parse(nameParts[2]);

                        DateTime fileTime = new DateTime(now.Year, now.Month, now.Day, fileHour, fileMinute, fileSecond);
                        string[] searchParts = search.Split("_");

                        int searchHour = int.Parse(searchParts[0]);
                        int searchMinute = int.Parse(searchParts[1]);
                        int searchSecond = int.Parse(searchParts[2]);


                        if (searchHour == fileHour && searchMinute == fileMinute && searchSecond == fileSecond)
                        {
                            picture = file;
                            SearchManager.SetPicture(picture);
                            string[] id = nameParts[3].Split("d");
                            SearchManager.SetSearchImageInfo($"tijd: {fileTime} id: {id[1]}");
                            found = true;
                            break;
                        }  
                    }
                    if (found == true)
                    {
                        break;
                    }
                }

            }
            
        }

    }
}
