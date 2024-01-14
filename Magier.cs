using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfAppVererben4
{
    class Magier:Spieler
    {
        

        public Magier():base()
        {
   
         spielerMalen.ImageSource = new BitmapImage(new Uri(@"magier.png", UriKind.Relative));
            speedUp = -200;
            speed = -50;
        }
    }
}

