using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool player1Turn = false;
        public bool computerOpponent = false;
        public Button[] buttons = new Button[9]; // Get all buttons
        int scorePlayer, scorePC; // Keep score
        /// <summary>
        /// More logical setup, although not working correctly =(
        ///private int[,] Solutions = new int[,]
        ///{
        ///    // Bottom
        ///    {1,3,3},
        ///
        ///    // Middle - Horizontal
        ///    {4,5,6},
        ///
        ///    // Top
        ///    {7,8,9},
        ///
        ///    // Left
        ///    {1,4,7},
        ///
        ///    // Right
        ///    {3,6,9},
        ///
        ///    // Diagonal - Right Up
        ///    {1,5,9},
        ///
        ///    // Diagonal - Right Down
        ///    {7,5,3},
        ///};

        // Required for the clicking function to work
        // It's looking for a '0' value in the Solutions and we don't have that
        // And we can't increase the '0' to '1' because then the problem will happen in the other end =(
        private int[,] Solutions = new int[,]
        {
            // Might look weird but just imagine +1 for every value so that it makes sense on a numpad =)
            {0,1,2},// Bottom
            {3,4,5},// Middle - Horizontal
            {6,7,8},// Top
            {0,3,6},// Left
            {1,4,7},// Middle - Vertical
            {2,5,8},// Right
            {0,4,8},// Diagonal - Up Right or Down Left
            {2,4,6}// Diagonal - Up Left or Down Right
        };

        // Main thread
        public MainWindow()
        {
            InitializeComponent();

            // Feeding array with buttons
            buttons = new Button[9] { _1, _2, _3, _4, _5, _6, _7, _8, _9 };

            // Setting events for all buttons & some other stuff
            for (int i = 0; i < 9; i++)
            {
                buttons[i].Click += new RoutedEventHandler(ClickHandler);
            }

            pcOption.Checked += new RoutedEventHandler(AIHandler);
            playersOption.Click += new RoutedEventHandler(MultiplayerHandler);

            Start(); // Start game! Reset stuff etc.
        }

        // Reset everything and start a new round
        private void Start()
        {
            Console.WriteLine("Reset occured");

            // Reset all buttons
            for (int i = 0; i < 9; i++)
            {
                buttons[i].Content = "";
                buttons[i].Focusable = true; // Not clickable
                buttons[i].FontWeight = FontWeights.Normal;
                BrushConverter bc = new BrushConverter();
                buttons[i].Background = (Brush)bc.ConvertFrom("#FFDDDDDD");
            }

            Random rnd = new Random(); // New random generator
            int startDraw = rnd.Next(0, 3); // Assign random value to variable
            player1Turn = startDraw > 1 ? true : false; // Make decision based of randomized value

            if (computerOpponent)
            {
                turnLbl.Content = player1Turn ? "You" : "PC"; // Whose turn is it?
                AI();
            }
            else
                turnLbl.Content = player1Turn ? "Player 1" : "Player 2"; // Whose turn is it?
        }

        // Handeling all the buttons in one function!
        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Click");
            Button newBtn = (Button)sender; // Setting new button as the 'sender' aka the button which called the function

            if (!newBtn.Content.Equals("")) // Is it empty?
            {
                newBtn.Focusable = false;
                return;
            }

            if (player1Turn) // Who's turn is it?
                newBtn.Content = "X";
            else if (!computerOpponent)
                newBtn.Content = "O";

            newBtn.Background = Brushes.LightGreen;
            player1Turn = !player1Turn; // Switch player

            CheckWinner();

            if (computerOpponent) // Are we playing with AI? if so - run AI function
                AI();
            else
                turnLbl.Content = player1Turn ? "Player 1" : "Player 2";
        }

        // Display message and set score etc.
        private void WinnerMessage(int player1Win)
        {
            string winner;
            if (player1Win == 3)
            {
                Console.WriteLine("No winner, draw");
                winner = "Nobody";
                goto Message; // We skip the other stuff, no need to match variables etc.
            }

            // Did somebody win this turn?
            scorePC += player1Win == 0 ? 0 : 1; // Should we add 1?
            scorePlayer += player1Win == 1 ? 1 : 0; // Should we add 1?
            score_p1.Text = scorePlayer.ToString(); // Change text
            score_p2.Text = scorePC.ToString(); // Change text

            if (computerOpponent)
                winner = player1Win == 1 ? "You" : "PC";
            else
                winner = player1Win == 1 ? "Player 1" : "Player 2";

            Message: // "Jump point"
            MessageBoxResult result = MessageBox.Show($"{winner} won this round!\nStart new game?", "Game over!", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Start();
                return;
            }
            else
                Environment.Exit(0);
        }

        // Who's the winner?
        private void CheckWinner()
        {
            int calc = 0;
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
                    bt1.Background = Brushes.OrangeRed;
                    bt1.FontWeight = FontWeights.Bold;
                    bt2.Background = Brushes.Red;
                    bt2.FontWeight = FontWeights.Bold;
                    bt3.Background = Brushes.DarkRed;
                    bt3.FontWeight = FontWeights.Bold;
                    WinnerMessage(!player1Turn ? 1 : 0);
                    break;
                }
                else // Is every item occupied? - No winner, new round
                    calc += !buttons[i].Content.Equals("") ? 1 : 0;
            }
            if (calc == 8)
                WinnerMessage(3);
        }

        // The AI brain
        public void AI()
        {
            turnLbl.Content = "PC";
            if (player1Turn) // Is it actually the players turn? If so, break the function
            {
                Console.WriteLine("Players turn, returning function");
                return;
            }

            Console.WriteLine("AI event occured");

            // Ugly but fast logic for buttons
            //if (buttons[5].Content.Equals(""))
            //    AISetButton(4);

            //if (buttons[0].Content.Equals("X") & buttons[2].Content.Equals("X"))
            //    AISetButton(1);

            // AI for defensive decision making
            if (buttons[2].Content.Equals("X") && buttons[4].Content.Equals("") && buttons[6].Content.Equals("X") || (buttons[0].Content.Equals("X") && buttons[4].Content.Equals("") && buttons[8].Content.Equals("X")))
            {
                AISetButton(4);
                return;
            }
            else if (buttons[0].Content.Equals("") && buttons[4].Content.Equals("X") && buttons[8].Content.Equals("X"))
            {
                AISetButton(0);
                return;
            }
            else if (buttons[0].Content.Equals("X") && buttons[4].Content.Equals("X") && buttons[8].Content.Equals(""))
            {
                AISetButton(8);
                return;
            }

            if (buttons[6].Content.Equals("") && buttons[4].Content.Equals("X") && buttons[2].Content.Equals("X"))
            {
                AISetButton(6);
                return;
            }
            else if (buttons[6].Content.Equals("X") && buttons[4].Content.Equals("X") && buttons[2].Content.Equals(""))
            {
                AISetButton(2);
                return;
            }

            // Basic AI for winning moves
            for (int i = 0; i < 8; i++)
            {
                int a = Solutions[i, 0],
                    b = Solutions[i, 1],
                    c = Solutions[i, 2];
                Button bt1 = buttons[a],
                    bt2 = buttons[b],
                    bt3 = buttons[c];

                if (bt1.Content.Equals("X") == bt3.Content.Equals("X") && bt2.Content.Equals(""))
                {
                    AISetButton(b);
                    return;
                }
                if (bt1.Content.Equals("X") == bt2.Content.Equals("X") && bt3.Content.Equals(""))
                {
                    AISetButton(c);
                    return;
                }
                if (bt2.Content.Equals("X") == bt3.Content.Equals("X") && bt1.Content.Equals(""))
                {
                    AISetButton(a);
                    return;
                }
                else if (buttons[i].Content.Equals(""))
                {
                    AISetButton(i);
                    return;
                }
            }

            // If AI has last move and cannot make decide, first empty
            // We will only get to this part of the code if all the other executions failed
            for (int i = 0; i < 8; i++)
            {
                if (buttons[i].Content.Equals(""))
                {
                    AISetButton(i);
                    return;
                }
            }
        }

        // The button control for the AI
        private void AISetButton(int index)
        {
            Console.WriteLine($"AISetButton({index})");
            if (index >= 0 && index <= 8)
            {
                buttons[index].Content = "O";
                buttons[index].Focusable = false;
                buttons[index].Background = Brushes.LightBlue;
            }
            player1Turn = true; // Players turn
            turnLbl.Content = "You";
            CheckWinner();
        }

        // The event of the radio button being 'Checked'
        // Fires up the AI and sets game settings accordingly
        private void AIHandler(object sender, RoutedEventArgs e)
        {
            computerOpponent = true; // Set bool
            Start(); // Restart so we have an AI
        }

        // Event for pressing the '2 Player' radiobutton
        private void MultiplayerHandler(object sender, RoutedEventArgs e)
        {
            computerOpponent = false;
            Start();
        }
    }
}
