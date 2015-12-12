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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool gameOver;
        enum turn {X,O};
        turn Turn = turn.X;
        int turnCount;
        bool player1Turn = true;
        //private int[,] Solutions = new int[,]
        //{
        //    // Bottom
        //    {0,1,2},
        //    // Middle - Horizontal
        //    {3,4,5},
        //    // Top
        //    {6,7,8},
        //    // Left
        //    {0,3,6},
        //    // Right
        //    {2,5,8},
        //    // Diagonal - Right Up
        //    {0,4,8},
        //    // Diagonal - Right Down
        //    {6,4,2},
        //};
        private int[,] Solutions = new int[,]
        {
            {0,1,2},
            {3,4,5},
            {6,7,8},
            {0,3,6},
            {1,4,7},
            {2,5,8},
            {0,4,8},
            {2,4,6}
        };
        private Button[] buttons = new Button[9];
        int scorePlayer, scorePC;

        public MainWindow()
        {
            InitializeComponent();
            buttons = new Button[9] { _1, _2, _3, _4, _5, _6, _7, _8, _9 };
            for (int i = 0; i < 9; i++)
                buttons[i].Click += new RoutedEventHandler(Click);
            Start();
        }

        private void Start()
        {
            for (int i = 0; i < 9; i++)
            {
                buttons[i].IsEnabled = true;
                buttons[i].Content = "";
                BrushConverter bc = new BrushConverter();
                buttons[i].Background = (Brush)bc.ConvertFrom("#FFDDDDDD");
            }
            Turn = turn.X;
            gameOver = false;
        }

        // Handeling all the buttons in one function!
        private void Click(object sender, RoutedEventArgs e)
        {
            Button newBtn = (Button)sender;

            if (gameOver)
            {
                MessageBoxResult result = MessageBox.Show("Game over... Start new game?", "Game over", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    Start();
            }

            if (!newBtn.Content.Equals(""))
                newBtn.IsEnabled = false;

            if (player1Turn)
                newBtn.Content = "X";
            else
                newBtn.Content = "O";

            Turn = turn.O;
            player1Turn = !player1Turn;
            turnLbl.Content = Turn;

            gameOver = CheckWinner(buttons);
        }

        //private void AI()
        //{
        //    if (Turn == turn.O)
        //    {
        //        foreach (Button btn in buttons)
        //        {
        //            // do stuff
        //        }
        //        // Player's turn
        //        Turn = turn.X;
        //    }
        //}

        private bool CheckWinner(Button[] buttons)
        {
            bool gm = false;
            for (int i = 0; i < 8; i++)
            {
                int a = Solutions[i, 0],
                    b = Solutions[i, 1],
                    c = Solutions[i, 2];

                Button bt1 = buttons[a],
                    bt2 = buttons[b],
                    bt3 = buttons[c];

                // If selections are "blank", continue function
                if (bt1.Content.Equals("") || bt2.Content.Equals("") || bt3.Content.Equals(""))
                    continue;

                // If all the item is the same value, winner winner!
                if (bt1.Content == bt2.Content && bt2.Content == bt3.Content)
                {
                    // Set all the buttons to red and make the text bold for easier distinguishing
                    bt1.Background = Brushes.Red;
                    bt1.FontWeight = FontWeights.Bold;
                    bt2.Background = Brushes.Red;
                    bt2.FontWeight = FontWeights.Bold;
                    bt3.Background = Brushes.Red;
                    bt3.FontWeight = FontWeights.Bold;
                    gm = true;
                    break;
                }
            }
            return gm;
        }
    }
}
