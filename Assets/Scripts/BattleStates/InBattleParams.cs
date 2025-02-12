using TMPro;

public struct InBattleParams
{
    public BattleSystem BattleSystem;
    public Fighter Player;
    public FighterData PlayerData;
    public InputSystem PlayerInput;
    public Fighter Opponent;
    public FighterData OpponentData;
    public TextMeshProUGUI ScreenCenterText;
    public TextWarningConfig TextWarningConfig;
    public DangerLineSystem DangerLineSystem;
}