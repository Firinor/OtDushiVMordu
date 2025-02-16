using TMPro;

public struct InBattleParams
{
    public BattleManager BattleManager;
    public Fighter Player;
    public InputSystem PlayerInput;
    public Fighter Opponent;
    public TextMeshProUGUI ScreenCenterText;
    public TextWarningConfig TextWarningConfig;
    public DangerLineSystem DangerLineSystem;
}