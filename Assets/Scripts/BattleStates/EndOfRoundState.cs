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
        
        FirTextAnimationData data = new()
        {
            Text = winer.WinString,
            LifeLine = _config.WinerTextLiveTime,
            MaxFontSize = _config.WinerTextFontSize
        };
        
        _text.PlayFirTextAnimation(data);
        
        new Timer().Start(_manager.StatesTimeConfig.EndOfRoundLifeTime, _manager.NextRound);
    }
}