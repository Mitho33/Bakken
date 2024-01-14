using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfAppVererben4
{
    class Spieler
    {
        protected double posx = 700;//Platzhalter
        protected double posy = 200;
        protected Canvas canvasSpieler;//using System.Windows.Controls hinzufügen
        protected ImageBrush spielerMalen;//using System.Windows.Media; hinzufügen
        protected int speed;//Schrittgröße pro Key.Down
//nicht Fliegen
        protected int speedUp;//Schrittgröße pro Key.Up
        protected int gravity;//Schwerkraft um, die der Spieler fällt per Tick
     
        public Canvas CanvasSpieler
        {
            get { return canvasSpieler; }
            set { canvasSpieler = value; }
        }

        public double Posx
        {
            get { return posx; }
            set { posx = value; }
        }

        public double Posy
        {
            get { return posy; }
            set { posy = value; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public int SpeedUp
        {
            get { return speedUp; }
            set { speedUp = value; }
        }

        public int Gravity
        {
            get { return gravity; }

        }

        public Spieler()
        {
            //Die Bilddateien müssen in den Ordner bin/Debug und dann mit RMT
            //ProjektmappenExplorer auf Dateiname/Hinzufügen/Vorhandens Element
            canvasSpieler = new Canvas();
           spielerMalen = new ImageBrush();
           spielerMalen.ImageSource = new BitmapImage(new Uri(@"einstein.png", UriKind.Relative));
            canvasSpieler.Background = spielerMalen;
            speed = -5;//sonst sind die Pfeiltasten vertauscht,da links oben Punkt 0
            speedUp = -5;
            gravity = 1;
            canvasSpieler.Width = 50;//hier wird das bild in der Größe festgelegt, der Rahmen passt sich automatisch an
            canvasSpieler.Height = 100;
        }

        public virtual void Zeichnen()
        {            
            Canvas.SetLeft(canvasSpieler, posx);//Position im Canvas(Element,Position)
            Canvas.SetTop(canvasSpieler, posy);
           
        }
    }
}

