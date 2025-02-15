public class GameContext
{
    private PlayerProgress _progress;
    public PlayerProgress PlayerProgress
    {
        get => _progress;
        set
        {
            _progress = value;
            SaveLoadService.Save(SaveKey.PlayerProgress, _progress);
        }
    }

    public void PlayerWin()
    {
        _progress.WinCount++;
        SaveLoadService.Save(SaveKey.PlayerProgress, _progress);
    }

    public FighterData NextBattleOpponent;
}