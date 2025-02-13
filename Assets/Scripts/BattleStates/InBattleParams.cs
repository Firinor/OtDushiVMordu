using TMPro;

public struct InBattleParams
{
    public BattleSystem BattleSystem;
    public Fighter Player;
    public InputSystem PlayerInput;
    public Fighter Opponent;
    public TextMeshProUGUI ScreenCenterText;
    public TextWarningConfig TextWarningConfig;
    public DangerLineSystem DangerLineSystem;
}