using System;
using System.Collections;
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
        _dangerLineSystem.Initialize(HitPlayer);
        StartNewBattle(player, playerInput, opponent);
    }

    public void StartNewBattle(FighterData player, InputSystem playerInput, FighterData opponent)
    {
        _commands = playerInput.commands;
        FighterModel playerModel = new FighterModel(player);
        FighterModel opponentModel = new FighterModel(opponent);
        _player.Initialize(playerModel);
        _opponent.Initialize(opponentModel);
        
        StartCoroutine(OpponentAI());
        IsBattleRunning = true;
    }

    private void Update()
    {
        if (_commands.Count > 0)
        {
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
                    _player.ToEvade();
                    _player.ResetCharge();
                    break;
                default:
                    throw new Exception();
            }
            //Debug.Log("Fight command : " + command);
        }
        
        TimerTick();
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

    private void HitFighter(Fighter target, AttackData attack)
    {
        target.TakeHit(attack);
    }
    private bool HitPlayer()
    {
        AttackData opponentAttack = _opponent.TryAttack();
        if (opponentAttack.Damage > 0)
        {
            HitFighter(_player, opponentAttack);
            return true;
        }

        return false;
    }
    private IEnumerator OpponentAI()
    {
        while (true)
        {
            _dangerLineSystem.AddOpponentAttack();    
            yield return new WaitForSeconds(5f);
        }
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}