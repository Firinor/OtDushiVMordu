using System;
using FirAnimations;
using TMPro;
using Object = UnityEngine.Object;


public class GetReadyState : BattleState
{
    private TextConfig _config;
    private TextMeshProUGUI _animationTarget;

    public Action OnEndState;
    
    public GetReadyState(TextConfig config, TextMeshProUGUI animationTarget)
    {
        _config = config;
        _animationTarget = animationTarget;
    }
    public override void OnEnter()
    {
        TextAnimation readyAnimation = _animationTarget.gameObject.AddComponent<TextAnimation>();
        readyAnimation.textComponent = _animationTarget;
        _animationTarget.text = _config.GetReadyText;
        readyAnimation.Curve = _config.GetReadyTextLiveTime;
        readyAnimation.EndPosition = _config.GetReadyTextFontSize;
        readyAnimation.Initialize();
        readyAnimation.OnComplete += DisplayFightText;
        readyAnimation.OnComplete += () => Object.Destroy(readyAnimation);
        readyAnimation.enabled = true;
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
            OnEndState?.Invoke();
        };
        fightAnimation.enabled = true;
    }
}