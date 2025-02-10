using System;

public class FighterModel
{
    public FighterModel(FighterData data)
    {
        this.data = data;
        CurrentHitPoints = data.HitPoints;
        CurrentEnergyPoints = data.EnergyPoints;
    }

    public FighterState state;
    
    public FighterData data;
    public float CurrentHitPoints;
    public float CurrentEnergyPoints;

    public Action<float> OnHPChange;
    public Action<float> OnEPChange;
    public Action OnDeath;
    public Action OnAttack;
}