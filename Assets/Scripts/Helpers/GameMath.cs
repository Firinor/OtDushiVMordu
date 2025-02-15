using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace FirMath
{
    public static class GameMath
    {
        public static List<int> AFewCardsFromTheDeck(int NumberOfCardsDrawn, int DeckLength, bool WithoutDuplicates = true)
        {
            if (NumberOfCardsDrawn <= 0 || DeckLength <= 0)
            {
                return null;
            }

            if (NumberOfCardsDrawn >= DeckLength)
            {
                return Full(DeckLength);
            }

            return RandomCards(NumberOfCardsDrawn, DeckLength, WithoutDuplicates);
        }
        private static List<int> RandomCards(int NumberOfCardsDrawn, int DeckLength, bool WithoutDuplicates = true)
        {
            List<int> result = new List<int>(new int[NumberOfCardsDrawn]);
            while (NumberOfCardsDrawn > 0)
            {
                result[NumberOfCardsDrawn - 1] = Random.Range(0, DeckLength);
                NumberOfCardsDrawn--;
            }
            if (WithoutDuplicates)
            {
                RemoveDuplicate(result);
            }
            return result;
        }

        private static void RemoveDuplicate(List<int> result)
        {
            List<int> check = new List<int>();
            int resultLength = result.Count;
            for (int i = 0; i < resultLength; i++)
            {
                while (check.Contains(result[i]))
                {
                    result[i]++;
                    if(result[i] >= resultLength)
                    {
                        result[i] = 0;
                    }
                }
                check.Add(result[i]);
            }
        }
        private static List<int> Full(int i)
        {
            List<int> result = new List<int>(new int[i]);
            while (i > 0)
            {
                i--;
                result[i] = i;
            }
            return result;
        }
    }
}