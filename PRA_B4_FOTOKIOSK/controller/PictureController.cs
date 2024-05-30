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

            List<KioskPhoto> camera1Photos = new List<KioskPhoto>();

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
                            camera1Photos.Add(new KioskPhoto() { Id = 0, Source = file });
                        }
                    }
                }
            }

            camera1Photos.Sort((x, y) => DateTime.Compare(GetDateTimeFromFileName(x.Source), GetDateTimeFromFileName(y.Source)));

            List<KioskPhoto> camera2Photos = new List<KioskPhoto>();

            foreach (var photo in camera1Photos)
            {
                DateTime photoTime = GetDateTimeFromFileName(photo.Source);
                DateTime matchingTime = photoTime.AddSeconds(60);

                var matchingPhoto = camera1Photos.FirstOrDefault(p => GetDateTimeFromFileName(p.Source) == matchingTime);

                if (matchingPhoto != null)
                {

                    PicturesToDisplay.Add(photo);
                    PicturesToDisplay.Add(matchingPhoto);
                }
            }

            PictureManager.UpdatePictures(PicturesToDisplay);
        }


        private DateTime GetDateTimeFromFileName(string fileName)
        {
            string[] nameParts = Path.GetFileNameWithoutExtension(fileName).Split('_');
            int fileHour = int.Parse(nameParts[0]);
            int fileMinute = int.Parse(nameParts[1]);
            int fileSecond = int.Parse(nameParts[2]);
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, fileHour, fileMinute, fileSecond);
        }

        public void RefreshButtonClick()
        {

        }
    }
}
