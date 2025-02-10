using System;
using System.Collections;
using System.Collections.Generic;
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
                case FightCommand.Attack:
                    if(_player.TrySpendEnergy())
                        HitOpponent();
                    break;
                case FightCommand.Defence:
                    _player.ToDefence();
                    break;
                default:
                    throw new Exception();
            }
            Debug.Log("Fight command : " + command);
        }
    }

    private void HitOpponent()
    {
        _opponent.TakeHit(_player.LightHitDamage);
    }
    
    private void HitPlayer()
    {
        _player.TakeHit(_opponent.LightHitDamage);
    }

    private IEnumerator OpponentAI()
    {
        while (true)
        {
            _dangerLineSystem.AddOpponentAttack();    
            yield return new WaitForSeconds(3f);
        }
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}