using UnityEngine;

internal class Bootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void StartOfGame()
    {
        LoadPlayerSettings();
        LoadPlayerProgress();
    }
    
    private static void LoadPlayerSettings()
    {
        ApplicationContext.Settings = SaveLoadService.Load<GameSettings>(SaveKey.Settings);
    }
    private static void LoadPlayerProgress()
    {
        ApplicationContext.Game = 
        new GameContext
        {
            PlayerProgress = SaveLoadService.Load<PlayerProgress>(SaveKey.PlayerProgress)
        };
    }
}