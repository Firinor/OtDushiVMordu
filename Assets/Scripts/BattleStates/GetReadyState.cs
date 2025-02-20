using FirAnimations;
using FirTime;
using TMPro;
using Object = UnityEngine.Object;

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
        TextAnimation readyAnimation = _animationTarget.gameObject.AddComponent<TextAnimation>();
        readyAnimation.textComponent = _animationTarget;
        _animationTarget.text = _config.GetReadyText;
        readyAnimation.Curve = _config.GetReadyTextLiveTime;
        readyAnimation.EndPosition = _config.GetReadyTextFontSize;
        readyAnimation.Initialize();
        readyAnimation.OnComplete += DisplayFightText;
        readyAnimation.OnComplete += () => Object.Destroy(readyAnimation);
        readyAnimation.enabled = true;
        
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
        TextAnimation fightAnimation = _animationTarget.gameObject.AddComponent<TextAnimation>();
        fightAnimation.textComponent = _animationTarget;
        _animationTarget.text = _config.FightText;
        fightAnimation.Curve = _config.FightTextLiveTime;
        fightAnimation.EndPosition = _config.FightTextFontSize;
        fightAnimation.Initialize();
        fightAnimation.OnComplete += () =>
        {
            Object.Destroy(fightAnimation);
        };
        fightAnimation.enabled = true;
    }
}