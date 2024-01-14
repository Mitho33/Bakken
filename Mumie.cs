using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfAppVererben4
{
    class Mumie:Spieler
    {
        public Mumie():base()
        {
     
          spielerMalen.ImageSource = new BitmapImage(new Uri(@"mumie.png", UriKind.Relative));
            speed = -6;
        }
    }
}
