using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InBattle : BattleState
{
    private readonly BattleSystem _battleSystem;
    private OpponentAI _opponentAI;
    
    private DangerLineSystem _dangerLineSystem;
    private readonly Queue<FightCommand> _commands;
    
    private readonly Fighter _player;
    private readonly Fighter _opponent;
    
    private TextMeshProUGUI _screenCenterText;
    private TextWarningConfig _textWarningConfig;
    
    private float timer;
    private bool isTimer;

    public InBattle(InBattleParams @params)
    {
        _battleSystem = @params.BattleSystem;
        _dangerLineSystem = @params.DangerLineSystem;
        _player = @params.Player;
        _opponent = @params.Opponent;
        _commands = @params.PlayerInput.commands;
        _screenCenterText = @params.ScreenCenterText;
        _textWarningConfig = @params.TextWarningConfig;
    }
    
    public override void OnEnter()
    {
        _dangerLineSystem.Initialize();
        _opponentAI = new OpponentAI(_battleSystem, _dangerLineSystem, _opponent, _player);
        _battleSystem.StartCoroutine(_opponentAI.Start());
    }
    public override void Update()
    {
        CommandProcessing();
        TimerTick();
    }
    public override void OnExit()
    {  
        _battleSystem.StopAllCoroutines();
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
                else
                    ShowWarningText();
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
        _screenCenterText.text = _textWarningConfig.WarningText;
        _screenCenterText.enabled = true;
        isTimer = true;
        timer = _textWarningConfig.LifeTime;
    }
    private void TimerTick()
    {
        if (!isTimer)
            return;
        
        timer -= Time.deltaTime;
        if (timer > 0)
            return;
        
        _screenCenterText.enabled = false;
        isTimer = false;
    }
}

public class WinLoseState : BattleState
{
    public override void OnEnter()
    {
    }
    public override void Update()
    {
    }
    public override void OnExit()
    {  
    }
}
