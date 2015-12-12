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
        int turnCount;
        bool player1Turn = true;
        bool computerOpponent = false;

        // More logical setup, although not working correctly =(
        //private int[,] Solutions = new int[,]
        //{
        //    // Bottom
        //    {1,3,3},

        //    // Middle - Horizontal
        //    {4,5,6},

        //    // Top
        //    {7,8,9},

        //    // Left
        //    {1,4,7},

        //    // Right
        //    {3,6,9},

        //    // Diagonal - Right Up
        //    {1,5,9},

        //    // Diagonal - Right Down
        //    {7,5,3},
        //};

        // Required for the clicking function to work
        // It's looking for a '0' value in the Solutions and we don't have that
        // And we can't increase the '0' to '1' because then the problem will happen in the other end =(
        private int[,] Solutions = new int[,]
        {
            {0,1,2},// Bottom
            {3,4,5},// Middle - Horizontal
            {6,7,8},// Top
            {0,3,6},// Left
            {1,4,7},// Middle - Vertical
            {2,5,8},// Right
            {0,4,8},// Diagonal - Up Right or Down Left
            {2,4,6}// Diagonal - Up Left or Down Right
        };
        private Button[] buttons = new Button[9]; // Get all buttons
        int scorePlayer, scorePC; // Keep score

        public MainWindow()
        {
            InitializeComponent();

            // Feeding array with buttons
            buttons = new Button[9] { _1, _2, _3, _4, _5, _6, _7, _8, _9 };

            // Setting events for all buttons & some other stuff
            for (int i = 0; i < 9; i++)
                buttons[i].Click += new RoutedEventHandler(Click);
            pcOption.Checked += new RoutedEventHandler(AIHandler);

            Start(); // Start game! Reset stuff etc.
        }

        private void Start()
        {
            for (int i = 0; i < 9; i++)
            {
                buttons[i].Focusable = true; // Not clickable
                buttons[i].Content = "";
                BrushConverter bc = new BrushConverter();
                buttons[i].Background = (Brush)bc.ConvertFrom("#FFDDDDDD");
            }
            player1Turn = !player1Turn; // The same player won't start after the new round
        }

        // Handeling all the buttons in one function!
        private void Click(object sender, RoutedEventArgs e)
        {
            Button newBtn = (Button)sender; // Setting new button as the 'sender' aka the button which called the function

            //if (gameOver) // Are we finished yet?
            //{
            //    MessageBoxResult result = MessageBox.Show("Game over... Start new game?", "Game over", MessageBoxButton.YesNo);
            //    if (result == MessageBoxResult.Yes)
            //        Start();
            //}

            if (!newBtn.Content.Equals("")) // Is it empty?
                newBtn.Focusable = false;

            if (player1Turn) // Who's turn is it?
                newBtn.Content = "X";
            if (!player1Turn)
                newBtn.Content = "O";

            player1Turn = !player1Turn;

            switch (computerOpponent)
            {
                case true:
                    if (player1Turn)
                        turnLbl.Content = "You";
                    else
                        turnLbl.Content = "PC";
                    break;
                case false:
                    if (player1Turn)
                        turnLbl.Content = "Player 1";
                    else
                        turnLbl.Content = "Player 2";
                    break;
            }

            CheckWinner(buttons);
        }

        // Bad name, I know but it's logical
        // Checks settings and changes game settings accordingly
        private void AIHandler(object sender, RoutedEventHandler e)
        {
            // Are we playing with a PC or PvP
            if (pcOption.IsChecked == true)
            {
                AI(); // Start AI
                computerOpponent = true;
                Start();
            }
        }

        private void AI()
        {
            if (!player1Turn)
            {
                if (buttons[5].Content.Equals(""))
                    AISetButton(5);
                if (buttons[0].Content.Equals("X") & buttons[2].Content.Equals("X"))
                    AISetButton(1);
            }
        }

        private void AISetButton(int index)
        {
            if (index > 0 && index < 8)
            {
                buttons[index].Content = "O";
                buttons[index].Focusable = false;
            }
        }

        private void WinnerMessage(bool player1Win)
        {
            string winner = player1Win ? "Player 1" : "Player 2";
            MessageBoxResult result = MessageBox.Show($"{winner} won this round!\nStart new game?", "Game over!", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                Start();
            else
                Application.Current.Shutdown();
        }

        private void CheckWinner(Button[] newButtons)
        {
            for (int i = 0; i < 8; i++)
            {
                int a = Solutions[i, 0],
                    b = Solutions[i, 1],
                    c = Solutions[i, 2];

                Button bt1 = newButtons[a],
                    bt2 = newButtons[b],
                    bt3 = newButtons[c];

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
                    WinnerMessage(false);
                    break;
                }
            }
        }
    }
}
