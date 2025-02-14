public static class ApplicationContext
{
    private static GameSettings _gameSettings;
    public static GameSettings Settings
    {
        get => _gameSettings;
        set
        {
            _gameSettings = value;
            SaveLoadService.Save(SaveKey.Settings, _gameSettings);
        }
    }

    public static GameContext Game { get; set; }
}