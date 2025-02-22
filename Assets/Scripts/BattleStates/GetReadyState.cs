using FirAnimations;
using FirTime;
using TMPro;

public class GetReadyState : BattleState
{
    private TextConfig _config;
    private TextMeshProUGUI _animationTarget;
    private Fighter[] _fighters;
    private DangerLineSystem _dangerLineSystem;
    private float _timeLimit;
    
    public GetReadyState(BattleManager manager)
    {
        _fighters = new[]{manager.Player, manager.Opponent};
        _config = manager.TextConfig;
        _animationTarget = manager.CenterText;
        _dangerLineSystem = manager.DangerLineSystem;
        _timeLimit = manager.StatesTimeConfig.GerReadyLifeTime;
    }
    public override void OnEnter()
    {
        PrepareFighters();

        FirTextAnimationData data = new()
        {
            Text = _config.GetReadyText,
            LifeLine = _config.GetReadyTextLiveTime,
            MaxFontSize = _config.GetReadyTextFontSize,
            OnEnd = DisplayFightText
        };
        _animationTarget.PlayFirTextAnimation(data);
        
        new Timer().Start(_timeLimit, () => OnEndState?.Invoke());
    }

    private void PrepareFighters()
    {
        _dangerLineSystem.ClearAll();
        foreach (Fighter fighter in _fighters)
        {
            fighter.PrepareToBattle();
        }
    }

    private void DisplayFightText()
    {
        FirTextAnimationData data = new()
        {
            Text = _config.FightText,
            LifeLine = _config.FightTextLiveTime,
            MaxFontSize = _config.FightTextFontSize
        };
        _animationTarget.PlayFirTextAnimation(data);
    }
}