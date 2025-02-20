using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseFighterManager : MonoBehaviour
{
    private Action _onFighterPick;
    [SerializeField] 
    private Image _fighterImage;

    [SerializeField] 
    private Button _toWorldMap;
    
    [SerializeField] 
    private FighterBinder[] _binders;

    [SerializeField] 
    private OpponentsConfig _opponents;
        
    private void Start()
    {
        for (int i = 0; i < _binders.Length; i++)
        {
            int index = i;
            _binders[i].GetComponent<Toggle>().onValueChanged.AddListener(
                b =>
                {
                    if (b)
                        PlayerPick(_binders[index].ThisFighter);
                }
            );
            _binders[i].Initialize();
        }

        _binders[0].GetComponent<Toggle>().isOn = true;
        
        _toWorldMap.onClick.AddListener(GenerateOpponents);
    }

    private void GenerateOpponents()
    {
        List<FighterData> result = new ChallengeFactory().GenerateNewOpponents(_opponents);
        
        ApplicationContext.Game.PlayerProgress = new PlayerProgress()
        {
            PlayerFighter = ApplicationContext.Game.PlayerProgress.PlayerFighter,
            Difficulty = ApplicationContext.Game.PlayerProgress.Difficulty,
            OpponentsFighters = result.ToArray(),
        };
    }


    private void PlayerPick(FighterData pick)
    {
        _fighterImage.sprite = pick.stateData[0].Sprites[0];
        
        ApplicationContext.Game.PlayerProgress = new PlayerProgress()
        {
            PlayerFighter = pick,
            Difficulty = ApplicationContext.Game.PlayerProgress.Difficulty,
        };
    }

    private void OnDestroy()
    {
        _toWorldMap.onClick.RemoveListener(GenerateOpponents);
    }
}