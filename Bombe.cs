using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;//für Canvas
using System.Windows.Media.Imaging;

namespace WpfAppVererben4
{
    class Bombe:Spieler
    {

        public Bombe() : base()
        {
            canvasSpieler.Width = 50;//hier wird das bild in der Größe festgelegt, der Rahmen passt sich automatisch an
            canvasSpieler.Height = 50;        
        
        }

        public override void Zeichnen()
        {
            spielerMalen.ImageSource = new BitmapImage(new Uri(@"bombe.png", UriKind.Relative));
            Random  randomBombe = new Random();//Zufallsgenerator wird instanziiert
            double multi1= randomBombe.Next(1,10);//1. Zahl kleinste Zufallszahl, 2. Zahl größte Zahl minus erste Zahl
        Canvas.SetLeft(canvasSpieler, posx=100);//Position im Canvas(Element,Position)
            Canvas.SetTop(canvasSpieler, posy = 40 * multi1);
            gravity = 10;
        }
    }
}
