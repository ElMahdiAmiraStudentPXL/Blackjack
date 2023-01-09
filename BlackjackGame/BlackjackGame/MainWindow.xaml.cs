using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
using System.Xml.Linq;

namespace BlackjackGame
{
    /// <summary>
    /// Alle nodige waarden worden aangemaakt, er bestaat een scoreSpelerOnberekend en een scoreBankOnberekend die
    /// geen rekening houden met de aas waarde. De reden hiervoor is om daarna de aantal azen af te trekken maal 10
    /// en die waarde door te geven aan de berekende waarden.
    /// </summary>
    public partial class MainWindow : Window
    {

        private int scoreSpelerOnberekend;
        private int scoreSpelerBerekend;
        private int scoreBankOnberekend;
        private int scoreBankBerekend;
        private int waardeKaart;
        private string soortKaart;
        private Random randomNummer = new Random();
        private string[] kaartEnScoreSpeler;
        private string[] kaartEnScoreBank;
        private int aasCounterSpeler;
        private int aasCounterBank;
        private Dictionary<string, int> soortKaarten;
        private List<string> imageKaarten;
        private double inzet;
        private BitmapImage imageKaart = new BitmapImage();
        private DispatcherTimer klok = new DispatcherTimer();
        private string Historiek;
        private int RondeCounter;

        /// <summary>
        /// Hier worden het nodig Tick event aangemaakt voor de klok en de buttons die nu niet nodig
        /// zijn worden uitgeschakeld. Er worden ook de List aangemaakt van de afbeeldingen en
        /// een dictionary van de kaarten en hun waarden.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            klok.Interval = new TimeSpan(1000);
            klok.Tick += Klok_Tick;
            klok.Start();
            TxtScoreBank.Text = "0";
            TxtScoreSpeler.Text = "0";
            TxtStatus.Text = String.Empty;
            BtnHit.IsEnabled = false;
            BtnStand.IsEnabled = false;
            BtnDoubleDown.IsEnabled = false;
            imageKaarten = new List<string>
            {
                "/Kaarten/Harten/2H.svg.png", "/Kaarten/Harten/3H.svg.png",
                "/Kaarten/Harten/4H.svg.png", "/Kaarten/Harten/5H.svg.png",
                "/Kaarten/Harten/6H.svg.png", "/Kaarten/Harten/7H.svg.png",
                "/Kaarten/Harten/8H.svg.png", "/Kaarten/Harten/9H.svg.png",
                "/Kaarten/Harten/10H.svg.png", "/Kaarten/Harten/AH.svg.png",
                "/Kaarten/Harten/JH.svg.png", "/Kaarten/Harten/QH.svg.png",
                "/Kaarten/Harten/KH.svg.png",

                "/Kaarten/Schoppen/2S.svg.png",
                "/Kaarten/Schoppen/3S.svg.png", "/Kaarten/Schoppen/4S.svg.png",
                "/Kaarten/Schoppen/5S.svg.png", "/Kaarten/Schoppen/6S.svg.png",
                "/Kaarten/Schoppen/7S.svg.png", "/Kaarten/Schoppen/8S.svg.png",
                "/Kaarten/Schoppen/9S.svg.png", "/Kaarten/Schoppen/10S.svg.png",
                "/Kaarten/Schoppen/AS.svg.png", "/Kaarten/Schoppen/JS.svg.png",
                "/Kaarten/Schoppen/QS.svg.png", "/Kaarten/Schoppen/KS.svg.png",

                "/Kaarten/Klaveren/2C.svg.png", "/Kaarten/Klaveren/3C.svg.png",
                "/Kaarten/Klaveren/4C.svg.png", "/Kaarten/Klaveren/5C.svg.png",
                "/Kaarten/Klaveren/6C.svg.png", "/Kaarten/Klaveren/7C.svg.png",
                "/Kaarten/Klaveren/8C.svg.png", "/Kaarten/Klaveren/9C.svg.png",
                "/Kaarten/Klaveren/10C.svg.png", "/Kaarten/Klaveren/QC.svg.png",
                "/Kaarten/Klaveren/AC.svg.png", "/Kaarten/Klaveren/JC.svg.png",
                "/Kaarten/Klaveren/KC.svg.png",

                "/Kaarten/Ruiten/2D.svg.png",
                "/Kaarten/Ruiten/3D.svg.png", "/Kaarten/Ruiten/4D.svg.png",
                "/Kaarten/Ruiten/5D.svg.png", "/Kaarten/Ruiten/6D.svg.png",
                "/Kaarten/Ruiten/7D.svg.png", "/Kaarten/Ruiten/8D.svg.png",
                "/Kaarten/Ruiten/9D.svg.png", "/Kaarten/Ruiten/10D.svg.png",
                "/Kaarten/Ruiten/QD.svg.png", "/Kaarten/Ruiten/KD.svg.png",
                "/Kaarten/Ruiten/JD.svg.png", "/Kaarten/Ruiten/AD.svg.png"
            };

            soortKaarten = new Dictionary<string, int>()
            {
                {"Harten 2", 2}, {"Harten 3", 3}, {"Harten 4", 4 }, {"Harten 5", 5}, {"Harten 6", 6}, {"Harten 7", 7}, {"Harten 8", 8},
                {"Harten 9", 9 }, {"Harten 10", 10}, {"Harten Aas", 11}, {"Harten Boer", 10}, {"Harten Vrouw", 10}, {"Harten Heer", 10},

                {"Schoppen 2", 2 }, {"Schoppen 3", 3}, {"Schoppen 4", 4}, {"Schoppen 5", 5}, {"Schoppen 6", 6}, {"Schoppen 7", 7 }, {"Schoppen 8", 8},
                {"Schoppen 9", 9 }, {"Schoppen 10", 10}, {"Schoppen Aas", 11}, {"Schoppen Boer", 10},{ "Schoppen Vrouw", 10},{ "Schoppen Heer", 10},

                {"Klaveren 2", 2 }, {"Klaveren 3", 3}, {"Klaveren 4", 4}, {"Klaveren 5", 5}, {"Klaveren 6", 6}, {"Klaveren 7", 7}, {"Klaveren 8", 8}, {"Klaveren 9", 9},
                {"Klaveren 10", 10 }, {"Klaveren Vrouw", 10}, {"Klaveren Aas", 11}, {"Klaveren Boer", 10}, {"Klaveren Heer", 10},

                {"Ruiten 2", 2}, {"Ruiten 3", 3}, {"Ruiten 4", 4}, {"Ruiten 5", 5}, {"Ruiten 6", 6}, {"Ruiten 7", 7}, {"Ruiten 8", 8}, {"Ruiten 9", 9},
                {"Ruiten 10", 10 }, {"Ruiten Vrouw", 10}, {"Ruiten Heer", 10}, {"Ruiten Boer", 10}, {"Ruiten Aas", 11}
            };
            TxtHoeveelheidKaarten.Content = Convert.ToString(soortKaarten.Count);
        }
        // Tick event voor klok op statusbalk
        private void Klok_Tick(object sender, EventArgs e)
        {
            TxtKlok.Text = DateTime.Now.ToLongTimeString();
        }

        /// <summary>
        /// Hier wordt de kaart die gegeven werd afgeprint op de TextBox
        /// en een Image wordt getoond van de kaart.
        /// </summary>
        private void PrintKaart(bool isSpeler, bool isDoubleDown)
        {
            if (isSpeler == true && isDoubleDown == false)
            {
                TxtKaartSpeler.Text += kaartEnScoreSpeler[0] + "\n";
                imageKaart = new BitmapImage();
                imageKaart.BeginInit();
                imageKaart.UriSource = new Uri(kaartEnScoreSpeler[2], UriKind.Relative);
                imageKaart.EndInit();
                ImageKaartSpeler.Source = imageKaart;

            }
            else if (isSpeler == true)
            {
                TxtKaartSpeler.Text += kaartEnScoreSpeler[0] + "\n";
                imageKaart = new BitmapImage();
                imageKaart.BeginInit();
                imageKaart.UriSource = new Uri(kaartEnScoreSpeler[2], UriKind.Relative);
                imageKaart.Rotation = Rotation.Rotate90;
                imageKaart.EndInit();
                ImageKaartSpeler.Source = imageKaart;
            }

            else
            {
                TxtKaartBank.Text += kaartEnScoreBank[0] + "\n";
                imageKaart = new BitmapImage();
                imageKaart.BeginInit();
                imageKaart.UriSource = new Uri(kaartEnScoreBank[2], UriKind.Relative);
                imageKaart.EndInit();
                ImageKaartBank.Source = imageKaart;
            }
        }

        /// <summary>
        /// Hier wordt gecontroleerd of de inzet 10% is van de kapitaal
        /// en/of de kapitaal niet onder 0 zal gaan bij een gegeven inzet.
        /// </summary>
        private bool MinInzetIs10PcOfKapitaalOp()
        {
            if (double.TryParse(TxtInzet.Text, out inzet) == true && inzet != 0)
            {
                if (Math.Ceiling(Convert.ToDouble(TxtInzet.Text)) >= Math.Ceiling(Convert.ToDouble(TxtKapitaal.Text) * 0.1))
                {
                    if (Math.Ceiling(Convert.ToDouble(TxtKapitaal.Text)) - Math.Ceiling(Convert.ToDouble(TxtInzet.Text)) < 0 && Math.Ceiling(Convert.ToDouble(TxtKapitaal.Text)) != 0)
                    {
                        MessageBox.Show("U kunt geen inzet geven hoger dan de kapitaal", "Te hoge inzet", MessageBoxButton.OK, MessageBoxImage.Error);
                        SliderAmount.IsEnabled = true;
                        BtnDeel.IsEnabled = true;
                        TxtInzet.IsReadOnly = false;
                        return false;
                    }
                    else if (Math.Ceiling(Convert.ToDouble(TxtKapitaal.Text)) > 0)
                    {
                        TxtKapitaal.Text = Convert.ToString(Math.Ceiling(Convert.ToDouble(TxtKapitaal.Text)) - Math.Ceiling(Convert.ToDouble(TxtInzet.Text)));
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Kapitaal is op, een andere keer beter.");
                        SliderAmount.IsEnabled = true;
                        BtnDeel.IsEnabled = false;
                        TxtInzet.IsReadOnly = false;
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Inzet moet minstens 10% van de kapitaal zijn.", "Te lage inzet", MessageBoxButton.OK, MessageBoxImage.Error);
                    BtnDeel.IsEnabled = true;
                    TxtInzet.IsReadOnly = false;
                    SliderAmount.IsEnabled = true;
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Inzet is geen numerieke waarde of 0.", "Inzet fout", MessageBoxButton.OK, MessageBoxImage.Error);
                BtnDeel.IsEnabled= true;
                TxtInzet.IsReadOnly = false;
                SliderAmount.IsEnabled= true;
                return false;
            }
        }
        /// <summary>
        /// Hier zal twee kaarten gegeven worden voor de speler en een voor de bank.
        /// De waarden die terug op nul gezet moeten worden gebeurt hier ook.
        /// De nodige knoppen worden enabled en de andere disabled.
        /// </summary>
        private async void BtnDeel_Click(object sender, RoutedEventArgs e)
        {
            SliderAmount.IsEnabled = false;
            TxtKaartSpeler.Text = "";
            TxtKaartBank.Text = "";
            scoreSpelerBerekend = 0;
            scoreSpelerOnberekend = 0;
            scoreBankBerekend = 0;
            scoreBankOnberekend = 0;
            aasCounterSpeler = 0;
            aasCounterBank = 0;
            TxtScoreSpeler.Text = "0";
            TxtScoreBank.Text = "0";
            TxtStatus.Text = "";
            BtnDeel.IsEnabled = false;
            TxtInzet.IsReadOnly = true;
            if (TxtKapitaal.Text != "")
            {
                if (MinInzetIs10PcOfKapitaalOp() == true)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        GeefKaart(true);
                        if (kaartEnScoreSpeler != null)
                        {
                            await Task.Delay(1000);
                            ScoreGeven(kaartEnScoreSpeler, kaartEnScoreBank, true);
                            PrintKaart(true, false);
                        }


                    }
                    GeefKaart(false);
                    if (kaartEnScoreBank != null)
                    {
                        await Task.Delay(1000);
                        ScoreGeven(kaartEnScoreSpeler, kaartEnScoreBank, false);
                        PrintKaart(false, false);
                    }
                    BtnStand.IsEnabled = true;
                    BtnDoubleDown.IsEnabled = true;
                    if (scoreSpelerBerekend < 21)
                    {
                        BtnHit.IsEnabled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Druk eerst op de knop 'Nieuw Spel' om het spel te spelen.", "Nieuw Spel", MessageBoxButton.OK, MessageBoxImage.Error);
                BtnNieuwSpel.IsEnabled = true;
                TxtInzet.IsReadOnly = false;
                SliderAmount.IsEnabled = true;
            }
        }
        /// <summary>
        /// Hier wordt een kaart gegeven aan de Speler of de Bank aan de hand
        /// de bool isSpeler en als de kaarten opgebruikt zijn, worden de List en Dictionary
        /// geladen met de nodige waarden
        /// </summary>
        private void GeefKaart(bool isSpeler)
        {

            {
                if (soortKaarten.Count != 0)
                {
                    int nummerKaart = randomNummer.Next(0, soortKaarten.Count);
                    waardeKaart = soortKaarten.ElementAt(nummerKaart).Value;
                    soortKaart = soortKaarten.ElementAt(nummerKaart).Key;
                    if (isSpeler == true)
                    {
                        kaartEnScoreSpeler = new string[] { soortKaart, Convert.ToString(waardeKaart), imageKaarten[nummerKaart] };
                    }
                    else
                    {
                        kaartEnScoreBank = new string[] { soortKaart, Convert.ToString(waardeKaart), imageKaarten[nummerKaart] };
                    }
                    soortKaarten.Remove(soortKaarten.ElementAt(nummerKaart).Key);
                    imageKaarten.RemoveAt(nummerKaart);
                    TxtHoeveelheidKaarten.Content = Convert.ToString(soortKaarten.Count);
                }
                else if (soortKaarten.Count == 0 && imageKaarten.Count == 0)
                {
                    MessageBox.Show("De kaarten zijn allemaal uitgedeeld en worden opnieuw geschud en verdeeld", "Deck opgebruikt", MessageBoxButton.OK);
                    soortKaarten = new Dictionary<string, int>
                           {
                                { "Harten 2", 2}, {"Harten 3", 3}, {"Harten 4", 4 }, {"Harten 5", 5}, {"Harten 6", 6}, {"Harten 7", 7}, {"Harten 8", 8},
                                {"Harten 9", 9 }, {"Harten 10", 10}, {"Harten Aas", 11}, {"Harten Boer", 10}, {"Harten Vrouw", 10}, {"Harten Heer", 10},
                                {"Schoppen 2", 2 }, {"Schoppen 3", 3}, {"Schoppen 4", 4}, {"Schoppen 5", 5}, {"Schoppen 6", 6}, {"Schoppen 7", 7 }, {"Schoppen 8", 8},
                                {"Schoppen 9", 9 }, {"Schoppen 10", 10}, {"Schoppen Aas", 11}, {"Schoppen Boer", 10},{ "Schoppen Vrouw", 10},{ "Schoppen Heer", 10},
                                {"Klaveren 2", 2 }, {"Klaveren 3", 3}, {"Klaveren 4", 4}, {"Klaveren 5", 5}, {"Klaveren 6", 6}, {"Klaveren 7", 7}, {"Klaveren 8", 8}, {"Klaveren 9", 9},
                                {"Klaveren 10", 10 }, {"Klaveren Vrouw", 10}, {"Klaveren Aas", 11}, {"Klaveren Boer", 10}, {"Klaveren Heer", 10},
                                {"Ruiten 2", 2}, {"Ruiten 3", 3}, {"Ruiten 4", 4}, {"Ruiten 5", 5}, {"Ruiten 6", 6}, {"Ruiten 7", 7}, {"Ruiten 8", 8}, {"Ruiten 9", 9},
                                {"Ruiten 10", 10 }, {"Ruiten Vrouw", 10}, {"Ruiten Heer", 10}, {"Ruiten Boer", 10}, {"Ruiten Aas", 11}
                            };
                    imageKaarten = new List<string>
                        {
                                "/Kaarten/Harten/2H.svg.png", "/Kaarten/Harten/3H.svg.png",
                                "/Kaarten/Harten/4H.svg.png", "/Kaarten/Harten/5H.svg.png",
                                "/Kaarten/Harten/6H.svg.png", "/Kaarten/Harten/7H.svg.png",
                                "/Kaarten/Harten/8H.svg.png", "/Kaarten/Harten/9H.svg.png",
                                "/Kaarten/Harten/10H.svg.png", "/Kaarten/Harten/AH.svg.png",
                                "/Kaarten/Harten/JH.svg.png", "/Kaarten/Harten/QH.svg.png",
                                "/Kaarten/Harten/KH.svg.png",

                                "/Kaarten/Schoppen/2S.svg.png",
                                "/Kaarten/Schoppen/3S.svg.png", "/Kaarten/Schoppen/4S.svg.png",
                                "/Kaarten/Schoppen/5S.svg.png", "/Kaarten/Schoppen/6S.svg.png",
                                "/Kaarten/Schoppen/7S.svg.png", "/Kaarten/Schoppen/8S.svg.png",
                                "/Kaarten/Schoppen/9S.svg.png", "/Kaarten/Schoppen/10S.svg.png",
                                "/Kaarten/Schoppen/AS.svg.png", "/Kaarten/Schoppen/JS.svg.png",
                                "/Kaarten/Schoppen/QS.svg.png", "/Kaarten/Schoppen/KS.svg.png",

                                "/Kaarten/Klaveren/2C.svg.png", "/Kaarten/Klaveren/3C.svg.png",
                                "/Kaarten/Klaveren/4C.svg.png", "/Kaarten/Klaveren/5C.svg.png",
                                "/Kaarten/Klaveren/6C.svg.png", "/Kaarten/Klaveren/7C.svg.png",
                                "/Kaarten/Klaveren/8C.svg.png", "/Kaarten/Klaveren/9C.svg.png",
                                "/Kaarten/Klaveren/10C.svg.png", "/Kaarten/Klaveren/QC.svg.png",
                                "/Kaarten/Klaveren/AC.svg.png", "/Kaarten/Klaveren/JC.svg.png",
                                "/Kaarten/Klaveren/KC.svg.png",

                                "/Kaarten/Ruiten/2D.svg.png",
                                "/Kaarten/Ruiten/3D.svg.png", "/Kaarten/Ruiten/4D.svg.png",
                                "/Kaarten/Ruiten/5D.svg.png", "/Kaarten/Ruiten/6D.svg.png",
                                "/Kaarten/Ruiten/7D.svg.png", "/Kaarten/Ruiten/8D.svg.png",
                                "/Kaarten/Ruiten/9D.svg.png", "/Kaarten/Ruiten/10D.svg.png",
                                "/Kaarten/Ruiten/QD.svg.png", "/Kaarten/Ruiten/KD.svg.png",
                                "/Kaarten/Ruiten/JD.svg.png", "/Kaarten/Ruiten/AD.svg.png"
                        };
                    TxtHoeveelheidKaarten.Content = Convert.ToString(soortKaarten.Count);
                    if (isSpeler == true)
                    {
                        GeefKaart(true);
                    }
                    else
                    {
                        GeefKaart(false);
                    }
                }
            }

        }

        /// <summary>
        /// Hier wordt de score berekent, er wordt rekening gehouden met azen. Aan de hand van de hoeveelheid azen
        /// zal de juiste berekening uitgevoerd worden om de juiste score af te beelden.
        /// </summary>
        private void ScoreGeven(string[] kaartEnScoreSpeler, string[] kaartEnScoreBank, bool isSpeler)
        {
                if (isSpeler == true)
                {
                    if (kaartEnScoreSpeler[1] != "11")
                    {
                        scoreSpelerOnberekend += Convert.ToInt32(kaartEnScoreSpeler[1]);
                        if (aasCounterSpeler >= 1 && scoreSpelerOnberekend + Convert.ToInt32(kaartEnScoreSpeler[1]) > 21)
                        {
                            scoreSpelerBerekend = scoreSpelerOnberekend - aasCounterSpeler * 10;
                            TxtScoreSpeler.Text = Convert.ToString(scoreSpelerBerekend);
                        }
                        else
                        {
                            scoreSpelerBerekend += Convert.ToInt32(kaartEnScoreSpeler[1]);
                            TxtScoreSpeler.Text = Convert.ToString(scoreSpelerBerekend);
                        }
                    }
                    else
                    {
                        aasCounterSpeler++;
                        if (scoreSpelerOnberekend + Convert.ToInt32(kaartEnScoreSpeler[1]) <= 21)
                        {
                            scoreSpelerOnberekend += Convert.ToInt32(kaartEnScoreSpeler[1]);
                            scoreSpelerBerekend += Convert.ToInt32(kaartEnScoreSpeler[1]);
                            TxtScoreSpeler.Text = Convert.ToString(scoreSpelerBerekend);
                        }
                        else
                        {
                            scoreSpelerOnberekend += Convert.ToInt32(kaartEnScoreSpeler[1]);
                            scoreSpelerBerekend += scoreSpelerOnberekend - aasCounterSpeler * 10;
                            TxtScoreSpeler.Text = Convert.ToString(scoreSpelerBerekend);
                        }
                    }
                }
                else
                {
                    if (kaartEnScoreBank[1] != "11")
                    {
                        scoreBankOnberekend += Convert.ToInt32(kaartEnScoreBank[1]);
                        if (aasCounterBank >= 1 && scoreBankOnberekend + Convert.ToInt32(kaartEnScoreBank[1]) > 21)
                        {
                            scoreBankBerekend = scoreBankOnberekend - aasCounterBank * 10;
                            TxtScoreBank.Text = Convert.ToString(scoreBankBerekend);
                        }
                        else
                        {
                            scoreBankBerekend += Convert.ToInt32(kaartEnScoreBank[1]);
                            TxtScoreBank.Text = Convert.ToString(scoreBankBerekend);
                        }

                    }
                    else
                    {
                        aasCounterBank++;
                        if (scoreBankOnberekend + Convert.ToInt32(kaartEnScoreBank[1]) <= 21)
                        {
                            scoreBankOnberekend += Convert.ToInt32(kaartEnScoreBank[1]);
                            scoreBankBerekend += Convert.ToInt32(kaartEnScoreBank[1]);
                            TxtScoreBank.Text = Convert.ToString(scoreBankBerekend);
                        }
                        else
                        {
                            scoreBankOnberekend += Convert.ToInt32(kaartEnScoreBank[1]);
                            scoreBankBerekend = scoreBankOnberekend - aasCounterBank * 10;
                            TxtScoreBank.Text = Convert.ToString(scoreBankBerekend);
                        }
                    }

                }
        }
        /// <summary>
        /// Hij wordt gebruikt gemaakt van de code die kaart geeft, de kaart toont en een score geeft
        /// om een kaart aan de speler te geven, als de speler bust, dan wordt de knop disabled
        /// </summary>
        private async void BtnHit_Click(object sender, RoutedEventArgs e)
        {
            GeefKaart(true);
            if (scoreSpelerBerekend >= 21)
            {
                BtnHit.IsEnabled = false;
                BtnDoubleDown.IsEnabled = false;
            }
            await Task.Delay(1000);
            PrintKaart(true, false);
            ScoreGeven(kaartEnScoreSpeler, kaartEnScoreBank, true);


        }
        /// <summary>
        /// Functie die kapitaal toevoegt aan de speler, als de speler pusht, dan krijgt
        /// hij de inzet gewoon terug. Als hij wint, dan krijgt hij het dubbele inzet
        /// terug.
        /// </summary>
        private void VoegKapitaalToe()
        {
            if(!(scoreBankBerekend == scoreSpelerBerekend))
            {
                int som = 0;
                som = Convert.ToInt32(TxtKapitaal.Text) + Convert.ToInt32(TxtInzet.Text) * 2;
                TxtKapitaal.Text = Convert.ToString(som);
            }
            else
            {
                int som = 0;
                som = Convert.ToInt32(TxtKapitaal.Text) + Convert.ToInt32(TxtInzet.Text);
                TxtKapitaal.Text = Convert.ToString(som);
            }
            
        }


        /// <summary>
        /// Hier wordt de historiek toegevoegd aan private string Historiek en de laatste hand
        /// wordt toegevoegd aan LaatsteHand.Text, er is rondeCounter die aangeeft welke ronde het is
        /// </summary>
        private void HistoriekBijhouden()
        {
            if (TxtStatus.Text == "Gewonnen")
            {
                TxtLaatsteHand.Text = "Bedrag: +" + TxtInzet.Text + " punten Speler: " + TxtScoreSpeler.Text + " Punten Bank: " + TxtScoreBank.Text;
                RondeCounter++;
                Historiek += "Ronde " + RondeCounter + ": " + TxtLaatsteHand.Text + "\n";
            }
            else if (TxtStatus.Text != "Push")
            {
                TxtLaatsteHand.Text = "Bedrag: -" + TxtInzet.Text + " punten Speler: " + TxtScoreSpeler.Text + " Punten Bank: " + TxtScoreBank.Text;
                RondeCounter++;
                Historiek += "Ronde " + RondeCounter + ": " + TxtLaatsteHand.Text + "\n";
            }
            else
            {
                TxtLaatsteHand.Text = "Bedrag: " + TxtInzet.Text + " punten Speler: " + TxtScoreSpeler.Text + " Punten Bank: " + TxtScoreBank.Text;
                RondeCounter++;
                Historiek += "Ronde " + RondeCounter + ": " + TxtLaatsteHand.Text + "\n";
            }
        }
        /// <summary>
        /// Hier wordt gekeken of de Speler gewonnen, push of verloren heeft
        /// aan de hand van de score die de Speler heeft behaald. Er wordt hier ook kaarten
        /// uitgedeeld aan de bank tot het een score heeft hoger dan 16.
        /// </summary>
        private async void BtnStand_Click(object sender, RoutedEventArgs e)
        {
            BtnHit.IsEnabled = false;
            BtnStand.IsEnabled = false;
            BtnDoubleDown.IsEnabled = false;
            while (!(scoreBankBerekend > 16))
            {
                GeefKaart(false);
                if (kaartEnScoreBank != null)
                {
                    await Task.Delay(1000);
                    PrintKaart(false, false);
                    ScoreGeven(kaartEnScoreSpeler, kaartEnScoreBank, false);
                }

            }

            if (scoreSpelerBerekend == 21 && scoreBankBerekend != 21)
            {
                TxtStatus.Text = "Gewonnen";
                TxtStatus.Foreground = Brushes.Green;
                VoegKapitaalToe();
            }
            else if (scoreSpelerBerekend > 21)
            {
                TxtStatus.Text = "Verloren";
                TxtStatus.Foreground = Brushes.Red;
            }
            else if (scoreSpelerBerekend > scoreBankBerekend && scoreSpelerBerekend <= 21)
            {
                TxtStatus.Text = "Gewonnen";
                TxtStatus.Foreground = Brushes.Green;
                VoegKapitaalToe();
            }
            else if (scoreSpelerBerekend < 21 && scoreSpelerBerekend > scoreBankBerekend)
            {
                TxtStatus.Text = "Gewonnen";
                TxtStatus.Foreground = Brushes.Green;
                VoegKapitaalToe();
            }

            else if (scoreSpelerBerekend == scoreBankBerekend)
            {
                TxtStatus.Text = "Push";
                TxtStatus.Foreground = Brushes.Black;
                VoegKapitaalToe();
            }
            else if (scoreBankBerekend == 21 && scoreSpelerBerekend < 21)
            {
                TxtStatus.Text = "Verloren";
                TxtStatus.Foreground = Brushes.Red;
            }
            else if (scoreBankBerekend > scoreSpelerBerekend && scoreBankBerekend <= 21)
            {
                TxtStatus.Text = "Verloren";
                TxtStatus.Foreground = Brushes.Red;
            }
            else if (scoreBankBerekend > scoreSpelerBerekend && (scoreSpelerBerekend <= 21 && scoreBankBerekend > 21))
            {
                TxtStatus.Text = "Gewonnen";
                TxtStatus.Foreground = Brushes.Green;
                VoegKapitaalToe();
            }
            BtnDeel.IsEnabled = true;
            TxtInzet.IsReadOnly = false;
            SliderAmount.IsEnabled = true;
            HistoriekBijhouden();
        }

        /// <summary>
        /// Hier wordt de nodige waarden terug op 0 gezet en kapitaal op 100 op het
        /// spel weer te kunnen spelen, de nodige knoppen wordt geactiveerd en de andere
        /// uitgeschakeld, de List van afbeeldingen en de Dictionary van kaarten en waarden
        /// worden terug naar hun standaardwaarden gezet.
        /// </summary>
        private void BtnNieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            scoreSpelerBerekend = 0;
            scoreSpelerOnberekend = 0;
            scoreBankBerekend = 0;
            scoreBankOnberekend = 0;
            TxtKaartBank.Clear();
            TxtKaartSpeler.Clear();
            TxtStatus.Text = String.Empty;
            TxtScoreSpeler.Text = "0";
            TxtScoreBank.Text = "0";
            BtnDeel.IsEnabled = true;
            aasCounterSpeler = 0;
            aasCounterBank = 0;
            imageKaart = new BitmapImage();
            imageKaart.BeginInit();
            imageKaart.UriSource = new Uri("/Kaarten/LegeKaart/EmptyCard.svg.png", UriKind.Relative);
            imageKaart.EndInit();
            ImageKaartSpeler.Source = imageKaart;
            imageKaart = new BitmapImage();
            imageKaart.BeginInit();
            imageKaart.UriSource = new Uri("/Kaarten/LegeKaart/EmptyCard.svg.png", UriKind.Relative);
            imageKaart.EndInit();
            ImageKaartBank.Source = imageKaart;
            TxtInzet.IsReadOnly = false;
            SliderAmount.IsEnabled = true;
            TxtKapitaal.Text = "100";
            Historiek = "";
            RondeCounter = 0;
            TxtLaatsteHand.Text = "Laatste hand";
            soortKaarten = new Dictionary<string, int>
                           {
                                { "Harten 2", 2}, {"Harten 3", 3}, {"Harten 4", 4 }, {"Harten 5", 5}, {"Harten 6", 6}, {"Harten 7", 7}, {"Harten 8", 8},
                                {"Harten 9", 9 }, {"Harten 10", 10}, {"Harten Aas", 11}, {"Harten Boer", 10}, {"Harten Vrouw", 10}, {"Harten Heer", 10},
                                {"Schoppen 2", 2 }, {"Schoppen 3", 3}, {"Schoppen 4", 4}, {"Schoppen 5", 5}, {"Schoppen 6", 6}, {"Schoppen 7", 7 }, {"Schoppen 8", 8},
                                {"Schoppen 9", 9 }, {"Schoppen 10", 10}, {"Schoppen Aas", 11}, {"Schoppen Boer", 10},{ "Schoppen Vrouw", 10},{ "Schoppen Heer", 10},
                                {"Klaveren 2", 2 }, {"Klaveren 3", 3}, {"Klaveren 4", 4}, {"Klaveren 5", 5}, {"Klaveren 6", 6}, {"Klaveren 7", 7}, {"Klaveren 8", 8}, {"Klaveren 9", 9},
                                {"Klaveren 10", 10 }, {"Klaveren Vrouw", 10}, {"Klaveren Aas", 11}, {"Klaveren Boer", 10}, {"Klaveren Heer", 10},
                                {"Ruiten 2", 2}, {"Ruiten 3", 3}, {"Ruiten 4", 4}, {"Ruiten 5", 5}, {"Ruiten 6", 6}, {"Ruiten 7", 7}, {"Ruiten 8", 8}, {"Ruiten 9", 9},
                                {"Ruiten 10", 10 }, {"Ruiten Vrouw", 10}, {"Ruiten Heer", 10}, {"Ruiten Boer", 10}, {"Ruiten Aas", 11}
                            };
            imageKaarten = new List<string>
                        {
                                "/Kaarten/Harten/2H.svg.png", "/Kaarten/Harten/3H.svg.png",
                                "/Kaarten/Harten/4H.svg.png", "/Kaarten/Harten/5H.svg.png",
                                "/Kaarten/Harten/6H.svg.png", "/Kaarten/Harten/7H.svg.png",
                                "/Kaarten/Harten/8H.svg.png", "/Kaarten/Harten/9H.svg.png",
                                "/Kaarten/Harten/10H.svg.png", "/Kaarten/Harten/AH.svg.png",
                                "/Kaarten/Harten/JH.svg.png", "/Kaarten/Harten/QH.svg.png",
                                "/Kaarten/Harten/KH.svg.png",

                                "/Kaarten/Schoppen/2S.svg.png",
                                "/Kaarten/Schoppen/3S.svg.png", "/Kaarten/Schoppen/4S.svg.png",
                                "/Kaarten/Schoppen/5S.svg.png", "/Kaarten/Schoppen/6S.svg.png",
                                "/Kaarten/Schoppen/7S.svg.png", "/Kaarten/Schoppen/8S.svg.png",
                                "/Kaarten/Schoppen/9S.svg.png", "/Kaarten/Schoppen/10S.svg.png",
                                "/Kaarten/Schoppen/AS.svg.png", "/Kaarten/Schoppen/JS.svg.png",
                                "/Kaarten/Schoppen/QS.svg.png", "/Kaarten/Schoppen/KS.svg.png",

                                "/Kaarten/Klaveren/2C.svg.png", "/Kaarten/Klaveren/3C.svg.png",
                                "/Kaarten/Klaveren/4C.svg.png", "/Kaarten/Klaveren/5C.svg.png",
                                "/Kaarten/Klaveren/6C.svg.png", "/Kaarten/Klaveren/7C.svg.png",
                                "/Kaarten/Klaveren/8C.svg.png", "/Kaarten/Klaveren/9C.svg.png",
                                "/Kaarten/Klaveren/10C.svg.png", "/Kaarten/Klaveren/QC.svg.png",
                                "/Kaarten/Klaveren/AC.svg.png", "/Kaarten/Klaveren/JC.svg.png",
                                "/Kaarten/Klaveren/KC.svg.png",

                                "/Kaarten/Ruiten/2D.svg.png",
                                "/Kaarten/Ruiten/3D.svg.png", "/Kaarten/Ruiten/4D.svg.png",
                                "/Kaarten/Ruiten/5D.svg.png", "/Kaarten/Ruiten/6D.svg.png",
                                "/Kaarten/Ruiten/7D.svg.png", "/Kaarten/Ruiten/8D.svg.png",
                                "/Kaarten/Ruiten/9D.svg.png", "/Kaarten/Ruiten/10D.svg.png",
                                "/Kaarten/Ruiten/QD.svg.png", "/Kaarten/Ruiten/KD.svg.png",
                                "/Kaarten/Ruiten/JD.svg.png", "/Kaarten/Ruiten/AD.svg.png"
                        };

            TxtHoeveelheidKaarten.Content = Convert.ToString(imageKaarten.Count);

        }

        // Wanneer de slider bewogen wordt, zal de waarde van inzet veranderen.
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            TxtInzet.Text = Convert.ToString(Convert.ToInt32(SliderAmount.Value));
        }

        /// <summary>
        /// De inzet wordt verdubbeld en de GeefKaart, ScoreGeven, BtnStand_Click functies worden aangeroepen
        /// om een kaart te geven en aan te geven of de speler gewonnen of verloren heeft tegen de bank.
        /// Er wordt ook aangegeven als deze actie niet mogelijk is.
        /// </summary>
        private async void BtnDoubleDown_Click(object sender, RoutedEventArgs e)
        {
            BtnStand.IsEnabled = false;
            BtnHit.IsEnabled = false;
            BtnDoubleDown.IsEnabled = false;
            if (Math.Ceiling(Convert.ToDouble(TxtKapitaal.Text)) - Math.Ceiling(Convert.ToDouble(TxtInzet.Text))*2 > 0)
            {
                int dubbelInzet = Convert.ToInt32(TxtInzet.Text) * 2;
                TxtInzet.Text = Convert.ToString(dubbelInzet);
                GeefKaart(true);
                if (kaartEnScoreSpeler != null)
                {
                    TxtKapitaal.Text = Convert.ToString(Math.Ceiling(Convert.ToDouble(TxtKapitaal.Text)) - Math.Ceiling(Math.Ceiling(Convert.ToDouble(TxtInzet.Text)) / 2));
                    await Task.Delay(1000);
                    PrintKaart(true, true);
                    ScoreGeven(kaartEnScoreSpeler, kaartEnScoreBank, true);
                    BtnStand_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Deze actie is niet mogelijk want uw kapitaal is te laag.",
                    "Te lage kapitaal", MessageBoxButton.OK, MessageBoxImage.Error);
                BtnStand.IsEnabled = true;
                BtnHit.IsEnabled = true;
            }

        }


        /// <summary>
        /// Wanneer er op de Historiek label geklikt wordt, zal de Historiek getoond worden,
        /// als de Historiek leeg is zal dat ook getoond worden.
        /// </summary>
        private void LblHistoriek_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Historiek == "")
            {
                MessageBox.Show("Historiek is leeg", "Lege historiek", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show(Historiek, "Historiek", MessageBoxButton.OK);
            }
        }
    }
}