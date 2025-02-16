using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class OpponentAI
{
    private readonly BattleManager _battleManager;
    private readonly DangerLineSystem _dangerLineSystem;
    private readonly Fighter _AIFighter;//bot
    private readonly Fighter _opponent;//player

    private readonly float _evadeTime;
    private readonly float _warningTime;
    
    public OpponentAI(BattleManager battleManager, DangerLineSystem dangerLineSystem, Fighter self, Fighter opponent)
    {
        _battleManager = battleManager;
        _dangerLineSystem = dangerLineSystem;
        _AIFighter = self;
        _opponent = opponent;
        
        _evadeTime = _AIFighter.EvadeTime;
        _warningTime = dangerLineSystem.warningTime;
    }
    
    public IEnumerator Start()
    {
        while (true)
        {
            if (_opponent.ChargeValue > _opponent.HeavyAttackChargeTime / 2)
            {
                _dangerLineSystem.AddOpponentAction(FightCommand.Evade, Evade);
                yield return new WaitForSeconds(_warningTime + _evadeTime);
                continue;
            }

            int pattern = Random.Range(0, 3);

            switch (pattern)
            {
                case 0:
                    LightAttack();
                    break;
                case 1:
                    yield return HeavyAttack();
                    break;
                case 2:
                    _dangerLineSystem.AddOpponentAction(FightCommand.Defence, Defence);  
                    break;
                default:
                    throw new Exception();
            }
            
            //float idleTime = Random.Range(0.1f, 3f);
            float idleTime = 1f;
            yield return new WaitForSeconds(idleTime);
        }
    }

    private void LightAttack()
    {
        AttackData opponentAttack = _AIFighter.TryAttack();
        _AIFighter.ResetCharge();
        _dangerLineSystem.AddOpponentAction(FightCommand.Attack, () => HitPlayer(opponentAttack));
    }
    private IEnumerator HeavyAttack()
    {
        _AIFighter.ToCharge();
        while (_AIFighter.IsOnChargeState
               && _AIFighter.ChargeValue < _AIFighter.HeavyAttackChargeTime + 0.1f)
        {
            yield return null;
        }

        if (_AIFighter.IsOnChargeState)
        {
            AttackData opponentAttack = _AIFighter.TryAttack();
            _dangerLineSystem.AddOpponentAction(FightCommand.Charge, () => HitPlayer(opponentAttack));    
        }
        _AIFighter.ResetCharge();
    }
    private bool HitPlayer(AttackData attack)
    {
        if (attack.Damage > 0)
        {
            return _opponent.TakeHit(attack);
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