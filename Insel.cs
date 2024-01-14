using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfAppVererben4
{
    class Insel : Spieler
    {
        Rectangle rectangle1 = new Rectangle();
        public Insel() : base()
        {
            //Canvas ist eine leere Leinwand, deshalb Rectangle eingefügt
            rectangle1.Stroke = Brushes.DarkRed;
            rectangle1.Fill = Brushes.DarkRed;
            rectangle1.Width = 100;
            rectangle1.Height = 10;
            canvasSpieler.Width = 100;
            canvasSpieler.Height = 10;
            canvasSpieler.Children.Add(rectangle1);
        }
        public override void Zeichnen()
        {

            Canvas.SetLeft(canvasSpieler, posx = 500);//Position im Canvas(Element,Position)
            Canvas.SetTop(canvasSpieler, posy = 200);

        }
    }
}
