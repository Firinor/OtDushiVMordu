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
}