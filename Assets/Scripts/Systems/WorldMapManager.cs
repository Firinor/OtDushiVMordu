using System.Collections.Generic;
using UnityEngine;
using FirGame.SceneManagement;
using UnityEngine.UI;

public class WorldMapManager : MonoBehaviour
{
    [SerializeField]
    private FighterBinder[] onMapOpponents;
    
    [SerializeField] 
    private OpponentsConfig _opponents;

    [SerializeField] 
    private FighterData player;
    [SerializeField] 
    private Difficulty difficulty;
    [SerializeField] 
    private int winCount;
    
    void Start()
    {
        SetOpponents();
    }

    private void SetOpponents()
    {
        PlayerProgress progress = ApplicationContext.Game.PlayerProgress;
        FighterData[] opponents = progress.OpponentsFighters;
        if (!IsCorrect(opponents))
        {
            opponents = GenerateOpponents();
        }
        
        for (int i = 0; i < opponents.Length; i++)
        {
            onMapOpponents[i].ThisFighter = opponents[i];
            onMapOpponents[i].GetComponent<Image>().sprite = opponents[i].Portrait;
            Toggle opponentToggle = onMapOpponents[i].GetComponent<Toggle>();
            opponentToggle.interactable = i <= progress.WinCount;
            opponentToggle.isOn = i < progress.WinCount;
            int index = i;
            opponentToggle.onValueChanged.AddListener(b => ToBattle(opponents[index]));
            onMapOpponents[i].Initialize();
        }
    }

    private bool IsCorrect(FighterData[] opponents)
    {
        if(opponents is null || opponents.Length <= 0)
            return false;
        
        foreach (FighterData opponent in opponents)
        {
            if (opponent == null)
                return false;
        }

        return true;
    }

    private FighterData[] GenerateOpponents()
    {
        List<FighterData> result = new ChallengeFactory().GenerateNewOpponents(_opponents);
        
        ApplicationContext.Game.PlayerProgress = new PlayerProgress()
        {
            PlayerFighter = player,
            Difficulty = difficulty,
            OpponentsFighters = result.ToArray(),
            WinCount = winCount
        };

        return ApplicationContext.Game.PlayerProgress.OpponentsFighters;
    }
    
    private void ToBattle(FighterData opponent)
    {
        ApplicationContext.Game.NextBattleOpponent = opponent;
        SceneManager.SwitchToScene(Scene.Fight);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < onMapOpponents.Length; i++)
        {
            if (onMapOpponents[i] != null)
                onMapOpponents[i].GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        }
    }
}
