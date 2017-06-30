using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Set
{
    static public class Deck
    {
        static public List<Card> AllCards { get; set; }
        static public Card[] InPlay { get; set; }
        static public List<Card> Discard { get; set; }

        static public void CreateDeck()
        {
            AllCards = new List<Card>();
            InPlay = new Card[12];
            Discard = new List<Card>();

            string[] Colors = { "red", "yellow", "purple" };
            string[] Shapes = { "cross", "oval", "diamond" };
            string[] Patterns = { "empty", "striped", "filled" };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 1; l < 4; l++)
                        {
                            AllCards.Add(new Card
                            {
                                Color = Colors[i],
                                Shape = Shapes[j],
                                Pattern = Patterns[k],
                                Num = l
                            });
                        }
                    }
                }
            }
        }

        static public void Shuffle(int numTimes)    //Fisher-Yates shuffle
        {
            int CardCount = AllCards.Count();
            Random rand = new Random();

            for (int times = 0; times < numTimes; times++)
            {
                for (var i = 0; i < CardCount; i++)
                {
                    int swap = rand.Next(CardCount);
                    Card temp = AllCards[swap];
                    AllCards[swap] = AllCards[i];
                    AllCards[i] = temp;
                }
            }
        }

        static public Card DrawCard()
        {
            Card c = AllCards[0];
            AllCards.RemoveAt(0);
            return c;
        }
    }
}
