using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    public Slider HPSlider;
    public Slider EPSlider;
    public Image FighterImage;

    private FighterModel _model;
    private Dictionary<FighterState, FighterStateData> _states;
    public int LightHitDamage => _model.data.LightAttackDamage;

    private float timer;
    private bool isTimer;
    
    public void Initialize(FighterModel model)
    {
        _model = model;

        GenerateStates();
        
        HPSlider.maxValue = model.data.HitPoints;
        model.OnHPChange += ChangeHP;
        ChangeHP(model.data.HitPoints);
        EPSlider.maxValue = model.data.EnergyPoints;
        model.OnEPChange += ChangeEP;
        ChangeEP(model.data.EnergyPoints);
        //model.OnAttack += OnLightAttack;
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
        _model.CurrentEnergyPoints += _model.data.EnergyRegen * Time.deltaTime;
        _model.OnEPChange?.Invoke(_model.CurrentEnergyPoints);

        if(!isTimer)
            return;
        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChangeState(FighterState.Idle);
            isTimer = false;
        }
    }

    public bool TrySpendEnergy()
    {
        bool result;
        
        _model.CurrentEnergyPoints -= _model.data.LightAttackEnergyCost;
        
        result = _model.CurrentEnergyPoints >= 0;

        ChangeState(result ? FighterState.Attack : FighterState.NoEnergy);

        _model.CurrentEnergyPoints = Math.Max(_model.CurrentEnergyPoints, 0);
        _model.OnEPChange?.Invoke(_model.CurrentEnergyPoints);
        
        return result;
    }

    private void ChangeState(FighterState newState)
    {
        _model.state = newState;
        FighterImage.sprite = _states[newState].Sprites[0];
        isTimer = true;
        timer = _states[newState].Time;
    }

    public void TakeHit(int damage)
    {
        _model.CurrentHitPoints -= damage;
        _model.CurrentHitPoints = Mathf.Max(_model.CurrentHitPoints, 0);
        _model.OnHPChange.Invoke(_model.CurrentHitPoints);
        if (_model.CurrentHitPoints <= 0)
        {
            ChangeState(FighterState.Lose);
            _model.OnDeath.Invoke();
        }
        else
        {
            ChangeState(FighterState.Attacked);
        }
    }
    public void ToDefence()
    {
        ChangeState(FighterState.Defense);
    }
    private void OnEvade()
    {
        ChangeState(FighterState.Evade);
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
        //_model.OnAttack -= OnLightAttack;
    }
}