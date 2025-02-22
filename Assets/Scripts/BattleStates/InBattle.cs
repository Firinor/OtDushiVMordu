using System;
using System.Collections.Generic;
using FirAnimations;
using TMPro;
using UnityEngine;

public class InBattle : BattleState
{
    private readonly BattleManager _battleManager;
    private OpponentAI _opponentAI;
    
    private DangerLineSystem _dangerLineSystem;
    private Queue<FightCommand> _commands;
    
    private readonly Fighter _player;
    private readonly Fighter _opponent;
    
    private TextMeshProUGUI _screenCenterText;
    private TextConfig _textWarningConfig;
    
    private float timer;
    private bool isTimer;

    public InBattle(BattleManager manager)
    {
        _battleManager = manager;
        _dangerLineSystem = manager.DangerLineSystem;
        _player = manager.Player;
        _opponent = manager.Opponent;
        _commands = manager.PlayerInput.commands;
        _screenCenterText = manager.CenterText;
        _textWarningConfig = manager.TextConfig;
    }
    
    public override void OnEnter()
    {
        _commands.Clear();
        _opponentAI = new OpponentAI(_battleManager, _dangerLineSystem, _opponent, _player);
        _battleManager.StartCoroutine(_opponentAI.Start());
        _player.OnNoEnergy += ShowWarningText;
    }
    public override void Update()
    {
        CommandProcessing();
    }
    public override void OnExit()
    {  
        _player.OnNoEnergy -= ShowWarningText;
        _battleManager.StopAllCoroutines();
    }
    
    private void CommandProcessing()
    {
        if (_commands.Count <= 0)
            return;
        
        var command = _commands.Dequeue();
        switch (command)
        {
            case FightCommand.Charge:
                _player.ToCharge();
                break;
            case FightCommand.Attack:
                if(!_player.IsOnChargeState)
                    break;
                AttackData playerAttack = _player.TryAttack();
                if (playerAttack.Damage > 0)
                    _opponent.TakeHit(playerAttack);
                _player.ResetCharge();
                break;
            case FightCommand.Defence:
                _player.ToDefence();
                _player.ResetCharge();
                break;
            case FightCommand.Evade:
                _player.TryEvade();
                _player.ResetCharge();
                break;
            default:
                throw new Exception();
        }
    }
    private void ShowWarningText()
    {
        FirTextAnimationData data = new()
        {
            Text = _textWarningConfig.WarningText,
            LifeLine = _textWarningConfig.WarningTextLifeTime,
            MaxFontSize = _textWarningConfig.WarningTextFontSize
        };
        _screenCenterText.PlayFirTextAnimation(data);
    }
}