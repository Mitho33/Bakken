using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfAppVererben4
{
    class Waffe:Spieler
    {
        public Waffe() : base()
        {
            spielerMalen.ImageSource = new BitmapImage(new Uri(@"pistole.png", UriKind.Relative));
            speed = -300;
            canvasSpieler.Width = 20;
            canvasSpieler.Height = 20;
        
        }
        
        public override void Zeichnen()
        {           
            Canvas.SetLeft(canvasSpieler, posx = 695);//Position im Canvas(Element,Position)
            Canvas.SetTop(canvasSpieler, posy = 230);          
        }

      
    }
}
