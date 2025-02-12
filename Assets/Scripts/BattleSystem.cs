using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public bool IsBattleRunning;
    
    [SerializeField]
    private Image[] WinPoints;

    [SerializeField] 
    private DangerLineSystem _dangerLineSystem;
    
    private OpponentAI _opponentAI;
    private Coroutine _AIRoutine;
    
    private Queue<FightCommand> _commands;
    [SerializeField]
    private Fighter _player;
    [SerializeField]
    private Fighter _opponent;

    [SerializeField] 
    private TextMeshProUGUI _screenCenterText;
    [SerializeField] 
    private TextWarningConfig _textWarningConfig;
    
    private float timer;
    private bool isTimer;

    //bootstrap
    private void Awake()
    {
        var player = Resources.Load<FighterData>("Fighters/Bogatyr");
        var opponent = Resources.Load<FighterData>("Fighters/Koshey");
        InputSystem playerInput = GameObject.Find("InputSystem").GetComponent<InputSystem>();
        _dangerLineSystem.Initialize();
        _opponentAI = new OpponentAI(this, _dangerLineSystem, _opponent, _player);
        StartNewBattle(player, playerInput, opponent);
    }

    public void StartNewBattle(FighterData player, InputSystem playerInput, FighterData opponent)
    {
        _commands = playerInput.commands;
        FighterModel playerModel = new FighterModel(player);
        FighterModel opponentModel = new FighterModel(opponent);
        _player.Initialize(playerModel);
        _opponent.Initialize(opponentModel);

        _AIRoutine = StartCoroutine(_opponentAI.Start());
        IsBattleRunning = true;
    }

    private void Update()
    {
        CommandProcessing();
        TimerTick();
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
                    HitFighter(_opponent, playerAttack);
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

    public void HitFighter(Fighter target, AttackData attack)
    {
        target.TakeHit(attack);
    }
 
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}