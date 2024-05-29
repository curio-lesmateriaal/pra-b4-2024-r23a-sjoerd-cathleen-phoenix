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
    public class PictureController
    {
        public static Home Window { get; set; }

        public List<KioskPhoto> PicturesToDisplay = new List<KioskPhoto>();

        public void Start()
        {
            var now = DateTime.Now;
            int day = (int)now.DayOfWeek;
            DateTime lowerBound = now.AddMinutes(-30);
            DateTime upperBound = now.AddMinutes(-2);

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


                        if (fileTime >= lowerBound && fileTime <= upperBound)
                        {
                            PicturesToDisplay.Add(new KioskPhoto() { Id = 0, Source = file });
                        }
                    }
                }
            }
            PictureManager.UpdatePictures(PicturesToDisplay);
        }

        public void RefreshButtonClick()
        {
        }
    }
}
