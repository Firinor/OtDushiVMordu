using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    public Slider HPSlider;
    public Slider EPSlider;
    public ChargeBar ChargeBar;
    public Image FighterImage;

    private FighterModel _model;
    private Dictionary<FighterState, FighterStateData> _states;

    public Action OnNoEnergy;
    
    private float timer;
    private bool isTimer;

    public float HeavyAttackChargeTime => _model.data.HeavyAttackChargeTime;

    public bool IsOnChargeState => _model.state == FighterState.Charge;
    public float ChargeValue => _model.CurrentChargeTime;
    public float EvadeTime => _model.data.stateData.Find(x => x.State == FighterState.Evade).Time;
    public string WinString => _model.data.WinString;
    public int WinCount => _model.WinCount;
    
    public void Initialize(FighterModel model)
    {
        _model = model;

        gameObject.name = model.data.name;
        
        GenerateStates();
        
        HPSlider.maxValue = model.data.HitPoints;
        model.OnHPChange += ChangeHP;
        ChangeHP(model.data.HitPoints);
        EPSlider.maxValue = model.data.EnergyPoints;
        model.OnEPChange += ChangeEP;
        ChangeEP(model.data.EnergyPoints);

        if (ChargeBar != null)
        {
            ChargeBar.Slider.maxValue = model.data.HeavyAttackChargeTime;
            model.OnChargeChange += ChargeBar.ChengeCharge;
        }
        
        ToIdleState();
    }

    public void AddWinPoint()
    {
        _model.WinCount++;
        _model.OnWinCountChange?.Invoke(_model.WinCount);
    }

    private void GenerateStates()
    {
        _states = new();
        foreach (var state in _model.data.stateData)
        {
            _states.Add(state.State, state);
        }
    }

    private void Update()
    {
        EnergyRegenTick();
        Charge();
        StateTimerTick();
    }

    private void Charge()
    {
        if (_model.state != FighterState.Charge)
        {
            ResetCharge();
            _model.OnChargeChange?.Invoke(_model.CurrentChargeTime);
            return;
        }
        
        _model.CurrentChargeTime += Time.deltaTime;
        bool isChargeFailed = !TrySpendEnergy(_model.data.ChargeEnergyCost * Time.deltaTime);

        if (isChargeFailed)
        {
            ToIdleState();
            ResetCharge();   
        }
        
        _model.OnChargeChange?.Invoke(_model.CurrentChargeTime);
    }

    public void ResetCharge()
    {
        _model.CurrentChargeTime = 0;
    }

    private void EnergyRegenTick()
    {
        bool isInRegenState = 
            _model.state == FighterState.Idle 
            ||  _model.state == FighterState.NoEnergy;
        
        if(!isInRegenState)
            return;
        
        if (_model.CurrentEnergyPoints >= _model.data.EnergyPoints)
            return;
        
        _model.CurrentEnergyPoints += _model.data.EnergyRegen * Time.deltaTime;
        if(_model.state == FighterState.NoEnergy)
            ToIdleState();
        
        _model.CurrentEnergyPoints = Math.Min(_model.CurrentEnergyPoints, _model.data.EnergyPoints);
        _model.OnEPChange?.Invoke(_model.CurrentEnergyPoints);
    }

    private void StateTimerTick()
    {
        if (!isTimer)
            return;
        
        timer -= Time.deltaTime;
        if (timer > 0)
            return;
        
        ToIdleState();
        
        isTimer = false;
    }
    public void PrepareToBattle()
    {
        _model.CurrentHitPoints = _model.data.HitPoints;
        _model.CurrentEnergyPoints = _model.data.EnergyPoints;
        _model.CurrentChargeTime = 0;
        _model.OnHPChange?.Invoke(_model.CurrentHitPoints);
        _model.OnEPChange?.Invoke(_model.CurrentEnergyPoints);
        _model.OnChargeChange?.Invoke(_model.CurrentChargeTime);
        ChangeState(FighterState.Idle);
    }

    private void ToIdleState()
    {
        if (_model.CurrentEnergyPoints >= _model.data.LightAttackEnergyCost)
            ChangeState(FighterState.Idle);
        else
            ChangeState(FighterState.NoEnergy);
    }
    
    public bool TrySpendEnergy(float energyCount)
    {
        _model.CurrentEnergyPoints -= energyCount;
        
        bool result = _model.CurrentEnergyPoints >= 0;

        _model.CurrentEnergyPoints = Math.Max(_model.CurrentEnergyPoints, 0);
        _model.OnEPChange?.Invoke(_model.CurrentEnergyPoints);
        
        return result;
    }
    public AttackData TryAttack()
    {
        if (_model.CurrentChargeTime >= _model.data.HeavyAttackChargeTime
            && _model.CurrentEnergyPoints >= _model.data.HeavyAttackEnergyCost)
        {
            TrySpendEnergy(_model.data.HeavyAttackEnergyCost);
            ChangeState(FighterState.Attack);
            return new AttackData{Damage = _model.data.HeavyAttackDamage, IsHeavy = true};
        }

        if (TrySpendEnergy(_model.data.LightAttackEnergyCost))
        {
            ChangeState(FighterState.Attack);
            return new AttackData{Damage = _model.data.LightAttackDamage, IsHeavy = false};
        }

        ChangeState(FighterState.NoEnergy);
        return default;
    }
    public void ChangeStateOnEndBattle(bool isWinPose)
    {
        ChangeState(isWinPose ? FighterState.Win : FighterState.Lose);
    }
    private void ChangeState(FighterState newState)
    {
        _model.state = newState;
        FighterImage.sprite = _states[newState].Sprites[0];

        if (_states[newState].Time > 0)
        {
            isTimer = true;
            timer = _states[newState].Time;
        }
        else
        {
            isTimer = false;
        }
        
        if(newState == FighterState.NoEnergy)
            OnNoEnergy?.Invoke();
    }

    public bool TakeHit(AttackData attack)
    {
        ResetCharge();
        
        bool isInSaveState = _model.state == FighterState.Evade;
        if(isInSaveState)
            return false;
        
        isInSaveState = _model.state == FighterState.Defense && !attack.IsHeavy;
        if(isInSaveState)
            return false;
        
        _model.CurrentHitPoints -= attack.Damage;
        _model.CurrentHitPoints = Mathf.Max(_model.CurrentHitPoints, 0);
        _model.OnHPChange.Invoke(_model.CurrentHitPoints);
        if (_model.CurrentHitPoints <= 0)
        {
            _model.OnDeath.Invoke();
        }
        else
        {
            ChangeState(FighterState.Attacked);
        }
        return true;
    }
    public void ToDefence()
    {
        ChangeState(FighterState.Defense);
    }
    public bool TryEvade()
    {
        if (TrySpendEnergy(_model.data.EvadeEnergyCost))
        {
            ChangeState(FighterState.Evade);
            return true;
        }

        ChangeState(FighterState.NoEnergy);
        return false;
    }
    public void ToCharge()
    {
        if(_model.CurrentEnergyPoints >= _model.data.LightAttackEnergyCost)
            ChangeState(FighterState.Charge);
        else
            ChangeState(FighterState.NoEnergy);
    }


    private void ChangeEP(float points)
    {
        EPSlider.value = points;
    }
    private void ChangeHP(float points)
    {
        HPSlider.value = points;
    }

    private void OnDestroy()
    {
        _model.OnHPChange -= ChangeHP;
        _model.OnEPChange -= ChangeEP;
        if (ChargeBar != null)
        {
            _model.OnChargeChange -= ChargeBar.ChengeCharge;
        }
    }
}