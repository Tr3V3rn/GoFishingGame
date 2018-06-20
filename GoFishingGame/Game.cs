using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoFishingGame
{
    class Game
    {
        private List<Player> players;
        private Dictionary<Values, Player> book;
        private Deck stock;
        private TextBox textBoxOnForm;

        public Game(string playerName,IEnumerable<string> opponentNames,TextBox textBoxOnForm)
        {
            Random random = new Random();
            this.textBoxOnForm = textBoxOnForm;
            players = new List<Player>();
            players.Add(new Player(playerName, random, textBoxOnForm));
            foreach(string player in opponentNames)
            {
                players.Add(new Player(player, random, textBoxOnForm));
            }
            book = new Dictionary<Values, Player>();
            stock = new Deck();
            Deal();
            players[0].SortHand();
        }

        private void Deal()
        {
            //shuffle the stock
            stock.Shuffle();
            //deal five cards to each player
            foreach (Player player in players)
            {
                for(int i = 0; i<=4; i++)
                {
                    player.TakeCard(stock.Deal());
                }
            }
            //for each player's PullOutBooks method
            foreach (Player player in players)
            {
                player.PullOutBooks();
            }
        }

        public bool PlayOneRound(int selectedPlayerCard)
        {
            //i am the first one to play the game and i have the value of my selected card
            Card selectPlayerCardValue = players[0].Peek(selectedPlayerCard);
            Values value = selectPlayerCardValue.Value;
            players[0].AskForACard(players, 0, stock, value);

            //the other players need to use the other AskForACard function because i dont know the card value. it's random

            for (int i = 1; i < players.Count; i++)
            {
                players[i].AskForACard(players, i, stock);
            }
            //i want to call PullOutBooks() for each player with a deck to remove their books

            for (int y = 0; y < players.Count; y++)
            {
                if (PullOutBooks(players[y]))
                {
                    for (int card = 0; card < 5; card++)
                    {
                        players[y].TakeCard(stock.Deal());
                    }
                }
            }
            players[0].SortHand();
            if (stock.Count == 0)
            {
                textBoxOnForm.Text = "The stock is out of card. Game over!" + Environment.NewLine;
                return true;
            }
            else
            {
                return false;
            }
                
        }
              
        public bool PullOutBooks(Player player)
        {
            IEnumerable<Values> temp = new List<Values>();
            temp = player.PullOutBooks();
            foreach(Values value in temp)
            {
                book.Add(value, player);
            }
            if (player.CardCount == 0)
                return true;
            else
                return false;
        }

        public string DescribeBooks()
        {
            string bookDescription = "";
            foreach(Values value in book.Keys)
            {
                Player lookupPlayer = book[value];
                bookDescription += lookupPlayer.Name + " has a book of " + Card.Plural(value) + Environment.NewLine;
            }
            return bookDescription;    
        }

        public string GetWinnerName()
        {
            int mostBooks=0;
            //keep track of how many books each player ended up with.

            //as i add the players name to the new dictionary. I create a counter with that addition to see how 
            //many times that names has been added.
            Dictionary<string, int> winner = new Dictionary<string, int>();
            int playerOccurence = 0;
            foreach (Values value in book.Keys)
            {
                Player lookupPlayer = book[value];
                string name = lookupPlayer.Name;
               winner.Add(lookupPlayer.Name, playerOccurence);
                book.Remove(value);
                //look for another occurence of player name
                    if(winner.ContainsKey(lookupPlayer.Name))
                    {
                        playerOccurence++;
                    }
            }
            foreach(string name in winner.Keys)
            {               
                if(winner[name] > mostBooks)
                {
                    mostBooks = winner[name]; //get the highest number of books
                }
            }
            string winnerList = "";
            bool tie = false;
            foreach (string name  in winner.Keys)
            {
                if(winner[name]==mostBooks) //check the list against that highest number of books
                {
                    //if we find a match we want to add to the winner List
                    if(!String.IsNullOrEmpty(winnerList))
                    {
                        winnerList += " and ";
                        tie = true;
                    }
                    winnerList += name;
                }
            }
            winnerList += " with " + mostBooks + " books";
            if(tie)
                return "A tie between " + winnerList;
            else
                return winnerList;
   
  

        }
    }
}
