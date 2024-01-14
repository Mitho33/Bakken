using System;
using System.Collections.Generic;
using System.Data.OleDb;//für die OleDb Objekte
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfAppVererben4
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Magier magier= new Magier();//Instanziierung muss hier erfolgen, da sonst if Abfrage Fehler wegen nicht vorhandener Objekte anzeigt
        private Mumie mumie = new Mumie();
        private Einstein einstein = new Einstein();
        private Bauarbeiter bauarbeiter = new Bauarbeiter();
        private Feuerwerk feuerwerk = new Feuerwerk();
       private Bombe bombe = new Bombe();
        private Waffe waffe = new Waffe();
        private Insel insel1 = new Insel();
        DispatcherTimer timerSpieler = new DispatcherTimer();
        DispatcherTimer timerBombe = new DispatcherTimer();//erst nach dem Konstruktor wird das Objekt  Dispatcher Timer erzeugt, sonst wird unten den timer.Start nicht erkannt
        double counter;                                            //using System.Windows.Threading; muss hinzugefügt werden

     //____________________________________________________________________________________________________________________________________________________________________     
        SpeechSynthesizer speechSynthesizer;//Deklarierung eines Sprachwiedergabeobjekts
        TextBox txtSpeech1;//Textbox zur Speicherung des Inhalts für die Sprachwiedergabe
        private int counterBombe;//Zähler für die Bombe, damit nach der Zerstörung einer Bombe neue hinzukommen
        private int counterWaffe;//Zähler für die Waffe, begrenzt die Anzahl der Schüsse
        //Deklaration und Initialisierung eines Musikwiedergabeobjekts, einmal absolut und einmal relativ
        //SoundPlayer snd = new SoundPlayer("C:\\Users\\Michael Thomas\\Desktop\\WpfAppVererben11\\WpfAppVererben4\\gmwalking.wav");
        SoundPlayer snd = new SoundPlayer(@"Video Game 1.wav");
        SoundPlayer sndSchuss = new SoundPlayer(@"9_mm_gunshot-mike-koenig-123.wav");
        SoundPlayer sndFeuerwerk = new SoundPlayer(@"Bomb_Exploding-Sound_Explorer-68256487.wav");
        //______________________________________________________________________________________________________________________________________________
        OleDbConnection con = new OleDbConnection();//Verbindung
        OleDbCommand cmd = new OleDbCommand();//Sprache

        public MainWindow()
        {
            InitializeComponent();

            timerSpieler.Tick += new EventHandler(TimerSpieler_Tick);//Event Handler sorgt dafür das jedes Ereignis von MainWindow bei jedem Tick die Funktion neu ausführt
            timerSpieler.Interval = TimeSpan.FromSeconds(0.1);//Intervalldauer
            timerSpieler.Tick += TimerSpieler_Tick; // Methode für den Timer wird für die Bombe benötigt

            timerBombe.Tick += new EventHandler(TimerBombe_Tick);//Event Handler sorgt dafür das jedes Ereignis von MainWindow bei jedem Tick die Funktion neu ausführt
            timerBombe.Interval = TimeSpan.FromSeconds(0.1);//Intervalldauer
            timerBombe.Tick += TimerBombe_Tick; // Methode für den Timer wird für die Bombe benötigt

             snd.Play();//Während des Aufruf des Konstruktors wird das Musikobjekt gestartet
 
        }

        //AccessDatenBank Primärschlüssel Datenformat in Text ändern: Ansicht/Entwurfsansicht/Primärschlüssel/Datenformat
        //SQL Anweisungen in Großbuchschstaben
        private void CbSpeichern_Click(object sender, RoutedEventArgs e)
        {
            int zeitSpieler = Convert.ToInt32(LblZeitSpieler.Content);    
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=C:\\Users\\Michael Thomas\\Desktop\\2020_2\\ITA\\Norwegen C#\\11polymorphismus\\WpfAppVererben11\\WpfAppVererben4\\Personen_1.accdb";
            cmd.Connection = con;
            cmd.CommandText = "DELETE  FROM Personen_1" ;
            con.Open();//Verb indung öffnen
            cmd.ExecuteNonQuery();//Anweisung ausführen
            con.Close();//Verbindung schließen
            cmd.CommandText = "INSERT INTO Personen_1 (Zeit) Values(@param1)";
            cmd.Parameters.AddWithValue ("@param1", zeitSpieler);

            try
            {
              con.Open();
             cmd.ExecuteNonQuery();    //Vorgang, um beliebige SQL-Anweisungen in SQL Server auszuführen   Query=Datenabfrage  
                MessageBox.Show("Daten geändert");
                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CbAktualisieren_Click(object sender, RoutedEventArgs e)
        {

            OleDbDataReader reader;//Leser
            con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=C:\\Users\\Michael Thomas\\Desktop\\Vererben9\\WpfAppVererben9\\WpfAppVererben4\\Personen_1.accdb";//Verbindung
            cmd.Connection = con;//Verbindung wird hergestellt
            cmd.CommandText = "SELECT * FROM Personen_1";//alle werden ausgewählt aus der Tabelle mit 

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();//Methode, um das Lesen auszuführen
                LblAnzeige.Content="";
                while (reader.Read())
                {
                    LblAnzeige.Content=(reader["Zeit"]);
                }
                reader.Close();
                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Methoden für die Auswahl der Spieler_________________________________________________________________________________________________________
        private void LblMagier_MD(object sender, MouseButtonEventArgs e)
        {
            magier.Zeichnen();//hier bekommt der Magier seine  Pos. zu gewiesen
            CanvasSpielerContainer.Children.Clear();//würde man das Canvas Spielfläche nehmen würde die gesamte Spielfäche glöscht, so nur der Container
            CanvasSpielerContainer.Children.Add(magier.CanvasSpieler);//in den Container Canvas wird der Magier in einem Canvas aus der Klasse Spieler hinzugefügt
        }

        private void LblMumie_MD(object sender, MouseButtonEventArgs e)
        {
            mumie.Zeichnen();
            CanvasSpielerContainer.Children.Clear();
            CanvasSpielerContainer.Children.Add(mumie.CanvasSpieler);
        }

        private void LblEinstein_MD(object sender, MouseButtonEventArgs e)
        {
            einstein.Zeichnen();
            CanvasSpielerContainer.Children.Clear();
            CanvasSpielerContainer.Children.Add(einstein.CanvasSpieler);
        }

        private void LblBauarbeiter_MD(object sender, MouseButtonEventArgs e)
        {
            bauarbeiter.Zeichnen();
            CanvasSpielerContainer.Children.Clear();
            CanvasSpielerContainer.Children.Add(bauarbeiter.CanvasSpieler);
        }

        //Methode für den Timer der Bombe____________________________________________________________________________________________
        private void TimerBombe_Tick(object sender, EventArgs e)
    
            {
                counter++;//Zählt bei jedem Tick einen mehr
               // LblZeitSpieler.Content = Convert.ToString(counter);//wird umgewandelt als Zähler für die Bomben

                //Ticker für Bombe
                bombe.Posx = (double)bombe.CanvasSpieler.GetValue(Canvas.LeftProperty);//Ortung + Initialisierung
                bombe.Posy = (double)bombe.CanvasSpieler.GetValue(Canvas.TopProperty);
                bombe.CanvasSpieler.SetValue(Canvas.LeftProperty, bombe.Posx + bombe.Gravity);//
                bombe.CanvasSpieler.SetValue(Canvas.TopProperty, bombe.Posy);

                //sorgt für rechts raus und links wieder rein
                if (bombe.Posx > 900)//Rechts canvas ist 1000 breit + bereite Figur
                {
                    bombe.CanvasSpieler.SetValue(Canvas.LeftProperty, bombe.Posx = 0);
                }
                //Kollission mit Magier
                if ((bombe.Posx + bombe.CanvasSpieler.Width == Canvas.GetLeft(magier.CanvasSpieler)) &&//X-Achse identische Position
                    (bombe.Posy + bombe.CanvasSpieler.Height >= Canvas.GetTop(magier.CanvasSpieler)) &&
                    //Bombe rechts oben ist größer gleich Spieler links oben, Bombe height =50, Spieler height =100
                    (bombe.Posy + bombe.CanvasSpieler.Height <= Canvas.GetTop(magier.CanvasSpieler) + magier.CanvasSpieler.Height))
                //Bombe rechts oben ist kleiner gleich als Spieler unten links
                {
                    Spielflaeche.Children.Clear();
                    Spielflaeche.Children.Add(feuerwerk.CanvasSpieler);
                 sndFeuerwerk.Play();
                //Der Label, der vorher die Zeit angezeigt hat, wird umgewandelt zur Anzeige der Treffer
                //muss aber neu instanziiert werden, da er aus der Spielfläche gelöscht wurde
                Label LblZeitSpieler = new Label();
                Spielflaeche.Children.Add(LblZeitSpieler);
                LblZeitSpieler.Width = 300;
                LblZeitSpieler.Height = 50;
                LblZeitSpieler.FontSize = 16;
                LblZeitSpieler.Foreground = Brushes.White;
                Canvas.SetLeft(LblZeitSpieler, 600);
                Canvas.SetTop(LblZeitSpieler, 150);
                LblZeitSpieler.Content = Convert.ToString("Anzahl der Bomben: " + counterBombe);
            }

                //Kollission mit Mumie
                if ((bombe.Posx + bombe.CanvasSpieler.Width == Canvas.GetLeft(mumie.CanvasSpieler)) &&
                    (bombe.Posy + bombe.CanvasSpieler.Height >= Canvas.GetTop(mumie.CanvasSpieler)) &&
                    (bombe.Posy + bombe.CanvasSpieler.Height <= Canvas.GetTop(mumie.CanvasSpieler) + mumie.CanvasSpieler.Height))
                {
                    Spielflaeche.Children.Clear();
                    Spielflaeche.Children.Add(feuerwerk.CanvasSpieler);
                }

                //Kollission mit Einstein
                if ((bombe.Posx + bombe.CanvasSpieler.Width == Canvas.GetLeft(einstein.CanvasSpieler)) &&
                    (bombe.Posy + bombe.CanvasSpieler.Height >= Canvas.GetTop(einstein.CanvasSpieler)) &&
                    (bombe.Posy + bombe.CanvasSpieler.Height <= Canvas.GetTop(einstein.CanvasSpieler) + einstein.CanvasSpieler.Height))
                {
                    Spielflaeche.Children.Clear();
                    Spielflaeche.Children.Add(feuerwerk.CanvasSpieler);
                }
                //Kollission mit Bauarbeiter
                if ((bombe.Posx + bombe.CanvasSpieler.Width == Canvas.GetLeft(bauarbeiter.CanvasSpieler)) &&
                    (bombe.Posy + bombe.CanvasSpieler.Height >= Canvas.GetTop(bauarbeiter.CanvasSpieler)) &&
                    (bombe.Posy + bombe.CanvasSpieler.Height <= Canvas.GetTop(bauarbeiter.CanvasSpieler) + bauarbeiter.CanvasSpieler.Height))
                {
                    Spielflaeche.Children.Clear();
                    Spielflaeche.Children.Add(feuerwerk.CanvasSpieler);
                }
            }
        
        // Methode für den Timer der Spieler__________________________________________________________________________________________________
        private void TimerSpieler_Tick(object sender, EventArgs e)
        {
           
                //Ticker für Magier und seine Waffe
                magier.Posx = (double)magier.CanvasSpieler.GetValue(Canvas.LeftProperty);//Initialisiert Variable
                magier.Posy = (double)magier.CanvasSpieler.GetValue(Canvas.TopProperty);
                magier.CanvasSpieler.SetValue(Canvas.LeftProperty, magier.Posx);//notwendig zur aktuellen Positionsbestimmung
                magier.CanvasSpieler.SetValue(Canvas.TopProperty, magier.Posy + magier.Gravity);

                waffe.Posx = (double)waffe.CanvasSpieler.GetValue(Canvas.LeftProperty);//Initialisiert Variable
                waffe.Posy = (double)waffe.CanvasSpieler.GetValue(Canvas.TopProperty);
                waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy + magier.Gravity);
                waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx);
                //***Fliegen
                if (magier.Posy > 300)//unten Canvas ist 450 hoch - Höhe Spieler
                {
                    magier.CanvasSpieler.SetValue(Canvas.TopProperty, magier.Posy = 300);
                    waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy = 330);
                    magier.SpeedUp = -200;
                }

                //***Insel
                //rutscht nicht durch die Insel und kriegt wieder Speed
                if ((magier.Posy + magier.CanvasSpieler.Height) > Canvas.GetTop(insel1.CanvasSpieler) && (magier.Posy + magier.CanvasSpieler.Height) < (Canvas.GetTop(insel1.CanvasSpieler) + insel1.CanvasSpieler.Height) && magier.Posx >= Canvas.GetLeft(insel1.CanvasSpieler) && magier.Posx <= (Canvas.GetLeft(insel1.CanvasSpieler) + 50))
                //Obere Seite vom Spieler größer als obere Seite von der Insel+Höhe Spieler, damit er auf der Insel zum Stehen kommt, zweiter Posy-Bereich damit nach unten eingegrenzt wird, sonst kann man nicht unter der Insel durch, Posx für Breite der Insel 100-50 Spielerbreite
                {
                    magier.CanvasSpieler.SetValue(Canvas.TopProperty, magier.Posy = (Canvas.GetTop(insel1.CanvasSpieler) - magier.CanvasSpieler.Height));
                    waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy = (Canvas.GetTop(insel1.CanvasSpieler) - magier.CanvasSpieler.Height) + 30);
                    magier.SpeedUp = -100;
                }

                //Insel1 nicht von unten durchbrechen
                if (magier.Posy == 300 && magier.Posx >= Canvas.GetLeft(insel1.CanvasSpieler) && magier.Posx <= (Canvas.GetLeft(insel1.CanvasSpieler) + 50))
                {
                    magier.SpeedUp = -100;
                }

                if (magier.Posx < 365 && magier.Posy > 215)//Loch x=15++350 y=15+200
                {
                Spielflaeche.Children.Clear();
                Spielflaeche.Children.Add(feuerwerk.CanvasSpieler);
                timerSpieler.Stop();
                sndFeuerwerk.Play();
                Label LblZeitSpieler = new Label();
                Spielflaeche.Children.Add(LblZeitSpieler);
                LblZeitSpieler.Width = 300;
                LblZeitSpieler.Height = 50;
                LblZeitSpieler.FontSize = 16;
                LblZeitSpieler.Foreground = Brushes.White;
                Canvas.SetLeft(LblZeitSpieler, 600);
                Canvas.SetTop(LblZeitSpieler, 150);
                LblZeitSpieler.Content = Convert.ToString("Anzahl der Bomben: " + counterBombe);

            }

                //Ticker für Mumie
                mumie.Posx = (double)mumie.CanvasSpieler.GetValue(Canvas.LeftProperty);
                mumie.Posy = (double)mumie.CanvasSpieler.GetValue(Canvas.TopProperty);
                mumie.CanvasSpieler.SetValue(Canvas.TopProperty, mumie.Posy + mumie.Gravity);
                mumie.CanvasSpieler.SetValue(Canvas.LeftProperty, mumie.Posx);//notwendig zur aktuellen Positionsbestimmung


                if (mumie.Posy > 300)//unten Canvas ist 450 hoch - höhe spieler
                {
                    mumie.CanvasSpieler.SetValue(Canvas.TopProperty, mumie.Posy = 300);
                    waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy = 330);
                }
                if (mumie.Posx < 365 && mumie.Posy > 215)//Loch x=15++350 y=15+200
                {
                    CanvasSpielerContainer.Children.Clear();

                    CanvasSpielerContainer.Children.Add(feuerwerk.CanvasSpieler);
                }

                //Ticker für Einstein
                einstein.Posx = (double)einstein.CanvasSpieler.GetValue(Canvas.LeftProperty);
                einstein.Posy = (double)einstein.CanvasSpieler.GetValue(Canvas.TopProperty);
                einstein.CanvasSpieler.SetValue(Canvas.TopProperty, einstein.Posy + einstein.Gravity);
                einstein.CanvasSpieler.SetValue(Canvas.LeftProperty, einstein.Posx);

                if (einstein.Posy > 300)//unten Canvas ist 450 hoch - höhe spieler
                {
                    einstein.CanvasSpieler.SetValue(Canvas.TopProperty, einstein.Posy = 300);
                    waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy = 330);
                }
                if (einstein.Posx < 365 && einstein.Posy > 215)//Loch x=15++350 y=15+200
                {
                    CanvasSpielerContainer.Children.Clear();

                    CanvasSpielerContainer.Children.Add(feuerwerk.CanvasSpieler);
                }

                //Ticker für Bauarbeiter
                bauarbeiter.Posx = (double)bauarbeiter.CanvasSpieler.GetValue(Canvas.LeftProperty);
                bauarbeiter.Posy = (double)bauarbeiter.CanvasSpieler.GetValue(Canvas.TopProperty);
                bauarbeiter.CanvasSpieler.SetValue(Canvas.TopProperty, bauarbeiter.Posy + bauarbeiter.Gravity);
                bauarbeiter.CanvasSpieler.SetValue(Canvas.LeftProperty, bauarbeiter.Posx);


                if (bauarbeiter.Posy > 300)//unten Canvas ist 450 hoch - höhe spieler
                {
                    bauarbeiter.CanvasSpieler.SetValue(Canvas.TopProperty, bauarbeiter.Posy = 300);
                    waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy = 330);
                }
                if (bauarbeiter.Posx < 365 && bauarbeiter.Posy > 215)//Loch x=15++350 y=15+200
                {
                    CanvasSpielerContainer.Children.Clear();

                    CanvasSpielerContainer.Children.Add(feuerwerk.CanvasSpieler);
                }
            }//Klammer für Methode Timer

            //Methode zum Bewegen der Spieler___________________________________________________________________________________________________
        
        private void KeyDown_Tastatur(object sender, KeyEventArgs e)
        {
        
                //********Insel  bzw. nicht Fliegen       
                if (CanvasSpielerContainer.Children.Contains(magier.CanvasSpieler))
                {
                    if (e.Key == Key.Up)
                    //Speed wird bei Up auf 0 gesetzt, dadurch kein Fliegen möglicht
                    //und beim Ende Timer gibt es neuen Speed;)
                    {
                        //****fliegen
                        magier.CanvasSpieler.SetValue(Canvas.TopProperty, magier.Posy + magier.SpeedUp);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy + magier.SpeedUp);
                        magier.SpeedUp = 0;//Speed geht weg, sonst könnte Spieler fliegen
                    }
                    if (e.Key == Key.Left)
                    {

                        magier.CanvasSpieler.SetValue(Canvas.LeftProperty, magier.Posx + magier.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx + magier.Speed);
                    }
                    if (e.Key == Key.Right)
                    {

                        magier.CanvasSpieler.SetValue(Canvas.LeftProperty, magier.Posx - magier.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx - magier.Speed);
                    }
                    if (e.Key == Key.Down)
                    {

                        magier.CanvasSpieler.SetValue(Canvas.TopProperty, magier.Posy - magier.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy - magier.Speed);
                    }

                    if (e.Key == Key.W)

                    {
                        counterWaffe++;
                    //SystemSounds.Beep.Play();//Alternative aber nicht so echt
            
                    sndSchuss.Play();
           

                    waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx + waffe.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy);
                    
                        if ((bombe.Posx + bombe.CanvasSpieler.Width >= Canvas.GetLeft(waffe.CanvasSpieler)) &&
                             (bombe.Posy + bombe.CanvasSpieler.Height >= Canvas.GetTop(waffe.CanvasSpieler)) &&
                             (bombe.Posy + bombe.CanvasSpieler.Height <= Canvas.GetTop(waffe.CanvasSpieler) + magier.CanvasSpieler.Height) &&
                           counterBombe <= 10)
                    {
                        //Wenn Waffe Bombe getroffen hat, dann wir Bombe entfernt, Waffe bekommt alte Werte, ist wieder am Mann, es wird ein Bombe addiert
                        //und dem label hinzugefügt
                        CanvasBombenContainer.Children.Clear();
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy + magier.Gravity);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx);
                        counterBombe++;
                        LblZeitSpieler.Content = Convert.ToString(counterBombe);
                        {//zweites if
                            if (counterBombe <= 10)
                            //wenn der Zähler =5 ist dann wird der Timer gestoppt,der Container geleert
                            {
                                timerBombe.Stop();
                                CanvasBombenContainer.Children.Clear();
                                //neue Bombe erscheint
                                bombe.Zeichnen();
                                CanvasBombenContainer.Children.Add(bombe.CanvasSpieler);
                                timerBombe.Start();
                            }
                            else
                            {
                                timerBombe.Stop();
                                CanvasBombenContainer.Children.Clear();
                            }
                        }//zweites if
                    }

                    else if (counterWaffe <= 10)//notwendig falls nicht getroffen wird muss Pistole auch am Mann bleiben
                    {
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy + magier.Gravity);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx);
                     
                    }
                    else //hierdurch hat man max. 10 Schuss
                    {
                        CanvasWaffenContainer.Children.Clear();
                        sndSchuss.Stop();
                        snd.Play();
                    }
                }

                    //Positionen

                    if (magier.Posx < 10)//Links
                    {
                    magier.CanvasSpieler.SetValue(Canvas.LeftProperty, magier.Posx = 10);
                    waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx = 5);
                
                    }
                    if (magier.Posx > 900)//Rechts canvas ist 1000 breit + bereite Figur
                    {
                        magier.CanvasSpieler.SetValue(Canvas.LeftProperty, magier.Posx = 900);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx = 895);
                    }

                    if (magier.Posy < 0)//oben
                    {
                        magier.CanvasSpieler.SetValue(Canvas.TopProperty, magier.Posy = 0);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy = 30);
                    }

                    if (magier.Posy > 300)//unten Canvas ist 450 hoch - Höhe spieler
                    {
                        magier.CanvasSpieler.SetValue(Canvas.TopProperty, magier.Posy = 300);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy = 330);
                    }
                    if (magier.Posx < 365 && magier.Posy > 215)//Loch x=15++350 y=15+200
                    {
                    Spielflaeche.Children.Clear();
                    Spielflaeche.Children.Add(feuerwerk.CanvasSpieler);

                    sndFeuerwerk.Play();
                    Label LblZeitSpieler = new Label();
                    Spielflaeche.Children.Add(LblZeitSpieler);
                    LblZeitSpieler.Width = 300;
                    LblZeitSpieler.Height = 50;
                    LblZeitSpieler.FontSize = 16;
                    LblZeitSpieler.Foreground = Brushes.White;
                    Canvas.SetLeft(LblZeitSpieler, 600);
                    Canvas.SetTop(LblZeitSpieler, 150);
                    LblZeitSpieler.Content = Convert.ToString("Anzahl der Bomben: " + counterBombe);

                }

                     }

                else if (CanvasSpielerContainer.Children.Contains(mumie.CanvasSpieler))
                {
                    if (e.Key == Key.Up)
                    {
                        mumie.CanvasSpieler.SetValue(Canvas.TopProperty, mumie.Posy + mumie.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy + mumie.Speed);
                    }
                    if (e.Key == Key.Left)
                    {
                        mumie.CanvasSpieler.SetValue(Canvas.LeftProperty, mumie.Posx + mumie.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx + mumie.Speed);
                    }
                    if (e.Key == Key.Right)
                    {
                        mumie.CanvasSpieler.SetValue(Canvas.LeftProperty, mumie.Posx - mumie.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx - mumie.Speed);
                    }
                    if (e.Key == Key.Down)
                    {
                        mumie.CanvasSpieler.SetValue(Canvas.TopProperty, mumie.Posy - mumie.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy - mumie.Speed);
                    }

                    if (e.Key == Key.W)

                    {
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx + waffe.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy);

                        if ((bombe.Posx + bombe.CanvasSpieler.Width >= Canvas.GetLeft(waffe.CanvasSpieler)) &&
                             (bombe.Posy + bombe.CanvasSpieler.Height >= Canvas.GetTop(waffe.CanvasSpieler)) &&
                             (bombe.Posy + bombe.CanvasSpieler.Height <= Canvas.GetTop(waffe.CanvasSpieler) + magier.CanvasSpieler.Height))
                        {
                            CanvasBombenContainer.Children.Clear();
                            CanvasWaffenContainer.Children.Clear();
                            timerBombe.Stop();
                            //Spielstart(magier.CanvasSpieler, e);
                        }

                        else
                        {
                            CanvasWaffenContainer.Children.Clear();
                        }
                    }

                    //Positionen

                    if (mumie.Posx < 10)//Links
                    {
                        mumie.CanvasSpieler.SetValue(Canvas.LeftProperty, mumie.Posx = 10);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx = 5);
                    }
                    if (mumie.Posx > 900)//Rechts canvas ist 1000 breit + bereite Figur
                    {
                        mumie.CanvasSpieler.SetValue(Canvas.LeftProperty, mumie.Posx = 900);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx = 895);
                    }

                    if (mumie.Posy < 0)//oben
                    {
                        mumie.CanvasSpieler.SetValue(Canvas.TopProperty, mumie.Posy = 0);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy = 30);
                    }

                    if (mumie.Posy > 300)//unten Canvas ist 450 hoch - höhe spieler
                    {
                        mumie.CanvasSpieler.SetValue(Canvas.TopProperty, mumie.Posy = 300);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, mumie.Posy = 330);
                    }
                    if (mumie.Posx < 365 && mumie.Posy > 215)//Loch x=15++350 y=15+200
                    {
                        Spielflaeche.Children.Clear();
                        Spielflaeche.Children.Add(feuerwerk.CanvasSpieler);
                    }
                }

                else if (CanvasSpielerContainer.Children.Contains(einstein.CanvasSpieler))
                {
                    if (e.Key == Key.Up)
                    {
                        einstein.CanvasSpieler.SetValue(Canvas.TopProperty, einstein.Posy + einstein.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy + einstein.Speed);
                    }
                    if (e.Key == Key.Left)
                    {
                        einstein.CanvasSpieler.SetValue(Canvas.LeftProperty, einstein.Posx + einstein.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx + einstein.Speed);
                    }
                    if (e.Key == Key.Right)
                    {
                        einstein.CanvasSpieler.SetValue(Canvas.LeftProperty, einstein.Posx - einstein.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx - einstein.Speed);
                    }
                    if (e.Key == Key.Down)
                    {
                        einstein.CanvasSpieler.SetValue(Canvas.TopProperty, einstein.Posy - einstein.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy - einstein.Speed);
                    }
                    if (e.Key == Key.W)

                    {
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx + waffe.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy);

                        if ((bombe.Posx + bombe.CanvasSpieler.Width >= Canvas.GetLeft(waffe.CanvasSpieler)) &&
                             (bombe.Posy + bombe.CanvasSpieler.Height >= Canvas.GetTop(waffe.CanvasSpieler)) &&
                             (bombe.Posy + bombe.CanvasSpieler.Height <= Canvas.GetTop(waffe.CanvasSpieler) + magier.CanvasSpieler.Height))

                        {
                            CanvasBombenContainer.Children.Clear();
                            CanvasWaffenContainer.Children.Clear();
                            timerBombe.Stop();
                            //Spielstart(magier.CanvasSpieler, e);
                        }

                        else
                        {
                            CanvasWaffenContainer.Children.Clear();
                        }
                    }

                    //Positionen
                    if (einstein.Posx < 10)//Links
                    {
                        einstein.CanvasSpieler.SetValue(Canvas.LeftProperty, einstein.Posx = 10);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx = 5);

                    }
                    if (einstein.Posx > 900)//Rechts canvas ist 1000 breit + bereite Figur
                    {
                        einstein.CanvasSpieler.SetValue(Canvas.LeftProperty, einstein.Posx = 900);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx = 895);
                    }

                    if (einstein.Posy < 0)//oben
                    {
                        einstein.CanvasSpieler.SetValue(Canvas.TopProperty, einstein.Posy = 0);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy = 30);
                    }

                    if (einstein.Posy > 300)//unten Canvas ist 450 hoch - höhe Spieler
                    {
                        einstein.CanvasSpieler.SetValue(Canvas.TopProperty, einstein.Posy = 300);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, mumie.Posy = 330);
                    }
                    if (einstein.Posx < 365 && einstein.Posy > 215)//Loch x=15++350 y=15+200
                    {
                        Spielflaeche.Children.Clear();
                        Spielflaeche.Children.Add(feuerwerk.CanvasSpieler);
                    }
                }

                else if (CanvasSpielerContainer.Children.Contains(bauarbeiter.CanvasSpieler))
                {
                    if (e.Key == Key.Up)
                    {
                        bauarbeiter.CanvasSpieler.SetValue(Canvas.TopProperty, bauarbeiter.Posy + bauarbeiter.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy + bauarbeiter.Speed);
                    }
                    if (e.Key == Key.Left)
                    {
                        bauarbeiter.CanvasSpieler.SetValue(Canvas.LeftProperty, bauarbeiter.Posx + bauarbeiter.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx + bauarbeiter.Speed);
                    }
                    if (e.Key == Key.Right)
                    {
                        bauarbeiter.CanvasSpieler.SetValue(Canvas.LeftProperty, bauarbeiter.Posx - bauarbeiter.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx - bauarbeiter.Speed);
                    }
                    if (e.Key == Key.Down)
                    {
                        bauarbeiter.CanvasSpieler.SetValue(Canvas.TopProperty, bauarbeiter.Posy - bauarbeiter.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy - bauarbeiter.Speed);
                    }
                    if (e.Key == Key.W)

                    {
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx + waffe.Speed);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy);

                        if ((bombe.Posx + bombe.CanvasSpieler.Width >= Canvas.GetLeft(waffe.CanvasSpieler)) &&
                             (bombe.Posy + bombe.CanvasSpieler.Height >= Canvas.GetTop(waffe.CanvasSpieler)) &&
                             (bombe.Posy + bombe.CanvasSpieler.Height <= Canvas.GetTop(waffe.CanvasSpieler) + magier.CanvasSpieler.Height))

                        {
                            CanvasBombenContainer.Children.Clear();
                            CanvasWaffenContainer.Children.Clear();
                            timerBombe.Stop();
                            //Spielstart(magier.CanvasSpieler, e);
                        }

                        else
                        {
                            CanvasWaffenContainer.Children.Clear();
                        }
                    }

                    //Positionen
                    if (bauarbeiter.Posx < 10)//Links
                    {
                        bauarbeiter.CanvasSpieler.SetValue(Canvas.LeftProperty, bauarbeiter.Posx = 10);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx = 5);
                    }
                    if (bauarbeiter.Posx > 900)//Rechts canvas ist 1000 breit + bereite Figur
                    {
                        bauarbeiter.CanvasSpieler.SetValue(Canvas.LeftProperty, bauarbeiter.Posx = 900);
                        waffe.CanvasSpieler.SetValue(Canvas.LeftProperty, waffe.Posx = 895);
                    }

                    if (bauarbeiter.Posy < 0)//oben
                    {
                        bauarbeiter.CanvasSpieler.SetValue(Canvas.TopProperty, bauarbeiter.Posy = 0);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, waffe.Posy = 30);
                    }

                    if (bauarbeiter.Posy > 300)//unten Canvas ist 450 hoch - Höhe Spieler
                    {
                        bauarbeiter.CanvasSpieler.SetValue(Canvas.TopProperty, bauarbeiter.Posy = 300);
                        waffe.CanvasSpieler.SetValue(Canvas.TopProperty, mumie.Posy = 330);
                    }
                    if (bauarbeiter.Posx < 365 && bauarbeiter.Posy > 215)//Loch x=15++350 y=15+200
                    {
                        Spielflaeche.Children.Clear();
                        Spielflaeche.Children.Add(feuerwerk.CanvasSpieler);
                    }
                }
            }
    
            //Methode für den Spielstart und Methode für das Ende_________________________________________________________________________________
        private void Spielstart(object sender, RoutedEventArgs e)
        {

            //***Insel
            //hier werden Insel, Bombe und Waffe in separate Container innerhalb des Canvas hinzugefügt
            insel1.Zeichnen();
            CanvasInselContainer.Children.Add(insel1.CanvasSpieler);

         
            waffe.Zeichnen();
            CanvasWaffenContainer.Children.Add(waffe.CanvasSpieler);


            //Methode Bombe für Positionierung und hinzufügen Canvas Spielfläche
  
             
                bombe.Zeichnen();
                CanvasBombenContainer.Children.Add(bombe.CanvasSpieler);

                timerSpieler.Start();//startet Timer
                timerBombe.Start();


                if (CanvasSpielerContainer.Children.Contains(magier.CanvasSpieler))
                {              
                Container.Children.Remove(LblMagier);
                Container.Children.Remove(LblMumie);
                Container.Children.Remove(LblEinstein);
                Container.Children.Remove(LblBauarbeiter);
                 }
                else if (CanvasSpielerContainer.Children.Contains(mumie.CanvasSpieler))
                {
                Container.Children.Remove(LblMagier);
                Container.Children.Remove(LblMumie);
                Container.Children.Remove(LblEinstein);
                Container.Children.Remove(LblBauarbeiter);
                }

                else   if (CanvasSpielerContainer.Children.Contains(einstein.CanvasSpieler))
                {
                Container.Children.Remove(LblMagier);
                Container.Children.Remove(LblMumie);
                Container.Children.Remove(LblEinstein);
                Container.Children.Remove(LblBauarbeiter);
                 }

                else   if (CanvasSpielerContainer.Children.Contains(bauarbeiter.CanvasSpieler))
                 {
                Container.Children.Remove(LblMagier);
                Container.Children.Remove(LblMumie);
                Container.Children.Remove(LblEinstein);
                Container.Children.Remove(LblBauarbeiter);
                    }

                else

            {
                snd.Stop();
                TextToSpeech1();
                Close();
            }
        }

                private void Ende(object sender, RoutedEventArgs e)
                {
                 Close();
                }
//Ansagezum Spielbeginn
        private void TextToSpeech1()
        {
            txtSpeech1 = new TextBox();
            txtSpeech1.Text = "Hallo Herzlich Willkommen, wählen Sie einen Spieler aus und Starten sie das Spiel. Sie haben 10 Schuss, um die Bombe zu treffen. Starten Sie das Spiel nun erneut";
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.Speak(txtSpeech1.Text);
        }



    }
}

    

