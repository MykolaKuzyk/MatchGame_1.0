using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Timer definition 
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed =0;
        int matchesFound =0;

        public MainWindow()
        {
            InitializeComponent();
            SetUpGame();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text += " - Play again ?";
            }
        }

        public void CountMatches()
        {
            matchesL.Content = matchesFound.ToString();
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
               "🐎", "🐎",
               "🐬", "🐬",
               "🐛", "🐛",
               "🐞", "🐞",
               "🐢", "🐢",
               "🕷", "🕷",
               "🐋", "🐋",
               "🐱‍", "🐱‍",
            };

            Random random = new Random();
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
           
            }
            timer.Start();
        }

        /* If it`s the first in the pair being clicked,
         * keep track of which TextBlock was clicked and
         * make the animal disappear. If it`s the second
         * one, either make it disappear(if its match) or
         * bring back the first one (if it`s not).
         */
        TextBlock lastTextBlockClicked;
        bool findingMatch= false;
        private void textBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                CountMatches();
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;

            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                matchesFound = 0;
                tenthsOfSecondsElapsed = 0;
                CountMatches();
                SetUpGame();
            }
        }
    }
}
