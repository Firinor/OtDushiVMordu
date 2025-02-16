using System.Collections.Generic;
using FirMath;

public class OpponentsGenerator
{
    public List<FighterData> GenerateNewOpponents(OpponentsConfig _opponents)
    {
        List<int> opponentsIndexes = GameMath.AFewCardsFromTheDeck(_opponents.Count, _opponents.Fighters.Length);
        List<FighterData> result = new();
        for (int i = 0; i < opponentsIndexes.Count - 1; i++)
        {
            int index = opponentsIndexes[i];
            result.Add(_opponents.Fighters[index]);
        }
        result.Add(_opponents.LastBoss);

        return result;
    }
}