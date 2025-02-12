using System;
using System.Collections;
using UnityEngine;

public class OpponentAI
{
    private BattleSystem _battleSystem;
    private DangerLineSystem _dangerLineSystem;
    private Fighter _AIFighter;//bot
    private Fighter _opponent;//player

    public OpponentAI(BattleSystem battleSystem, DangerLineSystem dangerLineSystem, Fighter self, Fighter opponent)
    {
        _battleSystem = battleSystem;
        _dangerLineSystem = dangerLineSystem;
        _AIFighter = self;
        _opponent = opponent;
    }
    
    public IEnumerator Start()
    {
        while (true)
        {
            _dangerLineSystem.AddOpponentAction(FightCommand.Attack, LightAttack);    
            yield return new WaitForSeconds(2f);
            _AIFighter.ToCharge();
            yield return new WaitForSeconds(_AIFighter.ChargeTime);
            AttackData opponentAttack = _AIFighter.TryAttack();
            _AIFighter.ResetCharge();
            _dangerLineSystem.AddOpponentAction(FightCommand.Charge, () => HeavyAttack(opponentAttack));    
            yield return new WaitForSeconds(2f);
            _dangerLineSystem.AddOpponentAction(FightCommand.Evade, Evade);    
            yield return new WaitForSeconds(2f);
            _dangerLineSystem.AddOpponentAction(FightCommand.Defence, Defence);    
            yield return new WaitForSeconds(2f);
        }
    }

    private bool HeavyAttack(AttackData attack)
    {
        return HitPlayer(attack);
    }

    private bool LightAttack()
    {
        AttackData attack = _AIFighter.TryAttack();
        return HitPlayer(attack);
    }
    private bool HitPlayer(AttackData attack)
    {
        if (attack.Damage > 0)
        {
            _battleSystem.HitFighter(_opponent, attack);
            return true;
        }

        return false;
    }
    
    private bool Evade()
    {
        return _AIFighter.TryEvade();
    }
    
    private bool Defence()
    {
        _AIFighter.ToDefence();
        return true;
    }
}