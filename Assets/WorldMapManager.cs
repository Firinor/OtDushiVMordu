using UnityEngine;
using FirGame.SceneManagement;
using UnityEngine.UI;

public class WorldMapManager : MonoBehaviour
{
    [SerializeField]
    private FighterBinder[] onMapOpponents;
    
    void Start()
    {
        SetOpponents();
    }

    private void SetOpponents()
    {
        PlayerProgress progress = ApplicationContext.Game.PlayerProgress;
        FighterData[] opponents = progress.OpponentsFighters;
        for (int i = 0; i < opponents.Length; i++)
        {
            onMapOpponents[i].ThisFighter = opponents[i];
            onMapOpponents[i].GetComponent<Image>().sprite = opponents[i].Portrait;
            Toggle opponentToggle = onMapOpponents[i].GetComponent<Toggle>();
            opponentToggle.interactable = i <= progress.WinCount;
            opponentToggle.isOn = i < progress.WinCount;
            int index = i;
            opponentToggle.onValueChanged.AddListener(b => ToBattle(opponents[index]));
        }
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
            onMapOpponents[i].GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        }
    }
}
