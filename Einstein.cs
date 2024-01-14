using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfAppVererben4
{
    class Einstein:Spieler
    {
        public Einstein()
        {
      spielerMalen.ImageSource = new BitmapImage(new Uri(@"einstein.png", UriKind.Relative));
            speed = -20;
        }
    }
}
