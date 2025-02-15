public struct PlayerProgress
{
    public FighterData PlayerFighter;
    public int WinCount;
    public FighterData[] OpponentsFighters;
    public Difficulty Difficulty;
}

public enum Difficulty
{
    Easy,
    Hard
}