using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoFishingGame
{
    class Player
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }
        private Random random;
        private Deck cards;
        private TextBox textBoxOnForm;


        public Player(String name, Random random, TextBox textBoxOnForm)
        {
            this.name = name;
            this.random = random;
            this.textBoxOnForm = textBoxOnForm;
            cards = new Deck(new Card[] { });
            textBoxOnForm.Text += this.name + " has joined the game" + Environment.NewLine;
        }

        public IEnumerable<Values> PullOutBooks() //complete set of all four cards that have the same value
        {
            List<Values> books = new List<Values>();
            for (int i = 1; i <=13; i++)
            {
                Values value = (Values)i;
                int howMany = 0;

                for(int card = 0; card < cards.Count; card++)
                {
                    if (cards.Peek(card).Value == value)
                        howMany++;
                }
                if(howMany == 4)
                {
                    books.Add(value);
                    cards.PullOutValues(value);
                }                
            }
            return books;
        }

        public Values GetRandomValue() //get a random value but it has to be in the deck.
        {
            int randomCard = random.Next(cards.Count);
            Card card = cards.Peek(randomCard);
            Values value = card.Value;
            return value;
        }

        public Deck DoYouHaveAny(Values value) //where an opponent asks if I have cards on a certain value
        {
            Deck cardsIHave = cards.PullOutValues(value);
            textBoxOnForm.Text += Name + " has " + cardsIHave.Count + " " + Card.Plural(value) + Environment.NewLine;
            return cardsIHave;
        }

        public void AskForACard(List<Player> players, int myIndex, Deck stock)  //Ask for a card for myself assuming I am player[0]
        {
            //choose a random value from the deck I have
            //need to check the players' deck as well as the count of cards in the stock
            if (stock.Count > 0)
            {
                if (cards.Count == 0)
                {
                    //take card off the top of the stock and add to my deck
                    cards.Add(stock.Deal());
                }
                Values randValue = GetRandomValue();
                AskForACard(players, myIndex, stock, randValue);
            }                       

        }
        public void AskForACard(List<Player> players, int myIndex, Deck stock, Values value)
        {
            textBoxOnForm.Text += Name + "asks is anyone has a " + value.ToString() + Environment.NewLine;
            for(int i = 0; i < players.Count; i++)
            {
                int totalCollectedCards = 0;
                if(i != myIndex)
                {
                    Deck collectedCards = players[i].DoYouHaveAny(value);
                    totalCollectedCards += collectedCards.Count;
                    while(collectedCards.Count > 0)
                    {
                        cards.Add(collectedCards.Deal());
                    }
                    
                    if(totalCollectedCards == 0 && stock.Count > 0)
                    {
                        cards.Add(stock.Deal());
                        textBoxOnForm.Text += Name + " had to draw from the stock." + Environment.NewLine;
                    }                   
                }
            }
        }

        public int CardCount
        {
            get
            {
                return cards.Count;
            }
        }

        public void TakeCard(Card card)
        {
            cards.Add(card);
        }

        public IEnumerable<string> GetCardNames()
        {
            return cards.GetCardNames();
        }
        public Card Peek(int cardNumber)
        {
            return cards.Peek(cardNumber);
        }
        public void SortHand()
        {
            cards.SortByValue();
        }
    }
}
