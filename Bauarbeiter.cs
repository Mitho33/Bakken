using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfAppVererben4
{
    class Bauarbeiter:Spieler
    {

        public Bauarbeiter():base()
        {
          spielerMalen.ImageSource = new BitmapImage(new Uri(@"bauarbeiter.png", UriKind.Relative));
        }
    }
}
