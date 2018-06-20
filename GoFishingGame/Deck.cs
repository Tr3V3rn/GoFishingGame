using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFishingGame
{
    class Deck
    {
        private List<Card> cards; //A list to hold the cards for a Deck
        private Random random = new Random();

        public Deck() //contructor, lets create the list object
        {
            cards = new List<Card>();
            //lets add cards to the list object, specifically 52 cards
            for (int suit = 0; suit <=3; suit++)
            {
                for(int value = 1; value <=13;value++)
                {
                    cards.Add(new Card((Suits)suit,(Values)value));
                }
            }
        }

        public Deck(IEnumerable<Card> initialCards) // can pass any collection into this function
        {
            cards = new List<Card>(initialCards);
        }

        public int Count
        {
            get
            {
                return cards.Count;
            }
        }

        public void Add(Card cardToAdd)
        {
            cards.Add(cardToAdd);
        }

        public Card Deal(int index)
        {
            Card cardToDeal = cards[index];
            cards.RemoveAt(index);
            return cardToDeal;
        }

        public void Shuffle()
        {
            //create a temporary random list to hold shuffled cards
            List<Card> tempCards = new List<Card>();

            while(cards.Count > 0)
            {
                int randomCardIndex = random.Next(0, cards.Count); //got the index of random card selected
                tempCards.Add(cards[randomCardIndex]); //add that card to the tempcards list
                cards.RemoveAt(randomCardIndex); //remove that card from the original list         
            }
            //then we can copy the tempcard list to the card list
            cards = tempCards;
        }

        public IEnumerable<string> GetCardNames()
        {
            //returns a string array that contains each card's name
            string [] cardNames = new string[cards.Count];
            //add cards objects into that array next

            for(int card = 0; card <= cards.Count; card++)
            {
                cardNames[card] = cards[card].Name;
            }

            /*foreach (Card card in cards)
            {
                int arrayIndex = 0;
                cardNames[arrayIndex] = card.Name;
                cards.Remove(card);
                arrayIndex++;            
            }
            */
            return cardNames; //want to be able to iterate over the name of each card
        }

        public void Sort()
        {
            cards.Sort(new CardComparer_byValue());
        }

        public Card Peek(int cardNumber) //look at a particular card in the Deck and returning a reference to that card
        {
            return cards[cardNumber];
        }

        public Card Deal() // Deal the top card off the deck
        {
            return Deal(0);
        }

        public bool ContainsValue(Values value)
        {
            foreach (Card card in cards)
            {
                if (card.Value == value)
                    return true;                   
            }
            return false;
        }

        public Deck PullOutValues(Values value)
        {
            Deck deckToReturn = new Deck(new Card[] { }); //create a new empty deck with no cards in the array
            for (int i = cards.Count - 1; i >= 0;i--)
            {
                if(cards[i].Value == value)
                {
                    deckToReturn.Add(Deal(i));
                }
            }
            return deckToReturn;
        }

        public bool HasBook(Values value)
        {
            int NumberOfCards = 0;
            foreach (Card card in cards)
            {
                if(card.Value == value)
                {
                    NumberOfCards++;
                }
            }
            if(NumberOfCards == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SortByValue()
        {
            cards.Sort(new CardComparer_byValue());
        }

    }
}
