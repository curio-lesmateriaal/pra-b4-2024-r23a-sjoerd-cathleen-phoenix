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


            foreach (string dir in Directory.GetDirectories(@"../../../fotos"))
            {
                string[] parts = Path.GetFileName(dir).Split('_');
                int foldersDays = int.Parse(parts[0]);

                if (foldersDays == day)
                {
                    foreach (string file in Directory.GetFiles(dir))
                    {
                        PicturesToDisplay.Add(new KioskPhoto() { Id = 0, Source = file });
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
