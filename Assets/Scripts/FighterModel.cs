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
    public float CurrentChargeTime;

    public Action<float> OnHPChange;
    public Action<float> OnEPChange;
    public Action<float> OnChargeChange;
    public Action OnDeath;
}