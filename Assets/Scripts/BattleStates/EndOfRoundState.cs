using FirAnimations;
using FirTime;
using TMPro;
using UnityEngine;

public class EndOfRoundState : BattleState
{
    private BattleManager _manager;
    
    private Fighter _player;
    private Fighter _opponent;
    private bool _isPlayerWin;
    private TextConfig _config;
    
    private TextMeshProUGUI _text;
    
    public EndOfRoundState(BattleManager manager, bool isPlayerWin)
    {
        _manager = manager;
        _player = manager.Player;
        _opponent = manager.Opponent;
        _isPlayerWin = isPlayerWin;
        _text = manager.CenterText;
        _config = manager.TextConfig;
    }

    public override void OnEnter()
    {
        _player.ChangeStateOnEndBattle(_isPlayerWin);
        _opponent.ChangeStateOnEndBattle(!_isPlayerWin);
        
        Fighter winer = _isPlayerWin ? _player : _opponent;
        _text.text = winer.WinString;

        winer.AddWinPoint();
        
        TextAnimation endOfRoundAnimation = _text.gameObject.AddComponent<TextAnimation>();
        endOfRoundAnimation.textComponent = _text;
        _text.text = winer.WinString;
        endOfRoundAnimation.Curve = _config.WinerTextLiveTime;
        endOfRoundAnimation.EndPosition = _config.WinerTextFontSize;
        endOfRoundAnimation.Initialize();
        
        endOfRoundAnimation.OnComplete += () => Object.Destroy(endOfRoundAnimation);
        endOfRoundAnimation.enabled = true;
        
        new Timer().Start(_manager.StatesTimeConfig.EndOfRoundLifeTime, _manager.NextRound);
    }
}