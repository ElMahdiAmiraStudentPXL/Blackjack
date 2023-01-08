using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace BlackjackGame
{
    /// <summary>
    /// Nodige waarden worden aangemaakt
    /// </summary>
    public partial class MainWindow : Window
    {
        private int scoreSpeler;
        private int scoreBank;
        private int waardeKaart;
        private string soortKaart;
        private Random randomNummer = new Random();
        private string[] kaartEnScoreSpeler;
        private string[] kaartEnScoreBank;
        public MainWindow()
        {
            InitializeComponent();
            TxtScoreBank.Text = "0";
            TxtScoreSpeler.Text = "0";
            TxtStatus.Text = String.Empty;
            BtnHit.IsEnabled = false;
            BtnStand.IsEnabled = false;
        }

        // De kaarten worden getoond van de Speler en Bank
        private void PrintKaart(int beurtNummer)
        {
            if (beurtNummer == 0)
            {
                if (kaartEnScoreSpeler[0].Substring(kaartEnScoreSpeler[0].Length - 3) == "Aas" || kaartEnScoreSpeler[0].Substring(kaartEnScoreSpeler[0].Length - 4) == "Boer" ||
                kaartEnScoreSpeler[0].Substring(kaartEnScoreSpeler[0].Length - 4) == "Heer" ||
                kaartEnScoreSpeler[0].Substring(kaartEnScoreSpeler[0].Length - 5) == "Vrouw")
                {
                    TxtKaartSpeler.Text += kaartEnScoreSpeler[0] + "\n";
                }
                else
                {
                    TxtKaartSpeler.Text += kaartEnScoreSpeler[0] + " " + kaartEnScoreSpeler[1] + "\n";
                }
            }
            else
            {
                if (kaartEnScoreBank[0].Substring(kaartEnScoreBank[0].Length - 3) == "Aas" || kaartEnScoreBank[0].Substring(kaartEnScoreBank[0].Length - 4) == "Boer" ||
                kaartEnScoreBank[0].Substring(kaartEnScoreBank[0].Length - 4) == "Heer" ||
                kaartEnScoreBank[0].Substring(kaartEnScoreBank[0].Length - 5) == "Vrouw")
                {


                    TxtKaartBank.Text += kaartEnScoreBank[0] + "\n";

                }
                else
                {

                    TxtKaartBank.Text += kaartEnScoreBank[0] + " " + kaartEnScoreBank[1] + "\n";
                }

            }

        }

        private void BtnDeel_Click(object sender, RoutedEventArgs e)
        {
            scoreSpeler = 0;
            scoreBank = 0;
            TxtKaartBank.Text = "";
            TxtKaartSpeler.Text = "";
            for (int i = 0; i < 2; i++)
            {
                kaartEnScoreSpeler = GeefKaart(false);
                ScoreGeven(kaartEnScoreSpeler, kaartEnScoreBank, 0);
                PrintKaart(0);
                
            }
            kaartEnScoreBank = GeefKaart(true);
            ScoreGeven(kaartEnScoreSpeler, kaartEnScoreBank, 1);
            PrintKaart(1);
            BtnDeel.IsEnabled = false;
            BtnStand.IsEnabled = true;
            if (scoreSpeler < 21)
            {
                BtnHit.IsEnabled = true;
            }
            


        }

        private string[] GeefKaart(bool isSpeler)
        {
            string[] soortKaarten = { "Harten", "Schoppen", "Klaveren", "Schoppen",
                "Harten Aas", "Schoppen Aas", "Klaveren Aas", "Schoppen Aas",
                "Harten Boer", "Schoppen Boer", "Klaveren Boer", "Schoppen Boer",
            "Harten Vrouw", "Schoppen Vrouw", "Klaveren Vrouw", "Schoppen Vrouw",
                "Harten Heer", "Schoppen Heer", "Klaveren Heer", "Schoppen Heer"};
            int nummerKaart = randomNummer.Next(0, 19);
            soortKaart = soortKaarten[nummerKaart];
            if (soortKaarten[nummerKaart].Substring(soortKaarten[nummerKaart].Length - 3) == "Aas" && isSpeler == false)
            {
                    waardeKaart = 11;
            }
            else if (soortKaarten[nummerKaart].Substring(soortKaarten[nummerKaart].Length - 3) == "Aas" && isSpeler == true)
            {
                    waardeKaart = 11;
            }
            else if (soortKaarten[nummerKaart].Substring(soortKaarten[nummerKaart].Length - 4) != "Boer" &&
                soortKaarten[nummerKaart].Substring(soortKaarten[nummerKaart].Length - 4) != "Heer" &&
                soortKaarten[nummerKaart].Substring(soortKaarten[nummerKaart].Length - 5) != "Vrouw")
            {
                int[] nummers = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                int nummerVanKaart = randomNummer.Next(0, 7);
                waardeKaart = nummers[nummerVanKaart];
            }
            else
            {
                waardeKaart = 10;
            }
            string[] kaartEnScore = { soortKaart, Convert.ToString(waardeKaart) };
            return kaartEnScore;

        }

        private void ScoreGeven(string[] kaartEnScoreSpeler, string[] kaartEnScoreBank, int beurtNummer)
        {
            if (beurtNummer == 0)
            {
                if (kaartEnScoreSpeler[0].Substring(kaartEnScoreSpeler[0].Length - 3) != "Aas")
                {
                    scoreSpeler += Convert.ToInt32(kaartEnScoreSpeler[1]);
                    TxtScoreSpeler.Text = Convert.ToString(scoreSpeler);
                }
                else
                {
                    scoreSpeler += 1;
                    TxtScoreSpeler.Text = Convert.ToString(scoreSpeler);
                }
            }
            else
            {
                if (kaartEnScoreBank[0].Substring(kaartEnScoreBank[0].Length - 3) != "Aas")
                {
                    scoreBank += Convert.ToInt32(kaartEnScoreBank[1]);
                    TxtScoreBank.Text = Convert.ToString(scoreBank);
                }
                else
                {
                    scoreBank += 1;
                    TxtScoreBank.Text = Convert.ToString(scoreBank);

                }



            }

        }

        private void BtnHit_Click(object sender, RoutedEventArgs e)
        {
            kaartEnScoreSpeler = GeefKaart(false);
            PrintKaart(0);
            ScoreGeven(kaartEnScoreSpeler, kaartEnScoreBank, 0);
            if (scoreSpeler >= 21)
            {
                BtnHit.IsEnabled = false;
            }

        }

        private void BtnStand_Click(object sender, RoutedEventArgs e)
        {
            BtnHit.IsEnabled = false;
            while (scoreBank < 16)
            {
                kaartEnScoreBank = GeefKaart(true);
                PrintKaart(1);
                ScoreGeven(kaartEnScoreSpeler, kaartEnScoreBank, 1);
            }

            if (scoreSpeler == 21 && scoreBank != 21)
            {
                TxtStatus.Text = "Gewonnen";
                TxtStatus.Foreground = Brushes.Green;
            }
            else if (scoreSpeler > 21)
            {
                TxtStatus.Text = "Verloren";
                TxtStatus.Foreground = Brushes.Red;
            }
            else if (scoreSpeler > scoreBank && scoreSpeler <= 21)
            {
                TxtStatus.Text = "Gewonnen";
                TxtStatus.Foreground = Brushes.Green;
            }
            else if (scoreSpeler < 21 && scoreSpeler > scoreBank)
            {
                TxtStatus.Text = "Gewonnen";
                TxtStatus.Foreground = Brushes.Green;
            }

            else if (scoreSpeler == scoreBank)
            {
                TxtStatus.Text = "Push";
                TxtStatus.Foreground = Brushes.Orange;
            }
            else if (scoreBank == 21 && scoreSpeler < 21)
            {
                TxtStatus.Text = "Verloren";
                TxtStatus.Foreground = Brushes.Red;
            }
            else if (scoreBank > scoreSpeler && scoreBank <= 21)
            {
                TxtStatus.Text = "Verloren";
                TxtStatus.Foreground = Brushes.Red;
            }
            else if (scoreBank > scoreSpeler && (scoreSpeler <= 21 && scoreBank > 21))
            {
                TxtStatus.Text = "Gewonnen";
                TxtStatus.Foreground = Brushes.Green;
            }
            BtnStand.IsEnabled = false;
            BtnDeel.IsEnabled = true;

        }

    }
}
