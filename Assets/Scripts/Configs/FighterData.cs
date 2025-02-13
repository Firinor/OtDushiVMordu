using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Balance/Fighter")]
public class FighterData : ScriptableObject
{
    public string Name;
    [Space]
    public int HitPoints;
    public int EnergyPoints;
    public float EnergyRegen;
    [Space] 
    public int LightAttackDamage;
    public int LightAttackEnergyCost;
    [Space] 
    public int HeavyAttackDamage;
    public int HeavyAttackEnergyCost;
    public float HeavyAttackChargeTime;
    public float ChargeEnergyCost;//per second
    [Space] 
    public int EvadeEnergyCost;
    [Space] 
    public List<FighterStateData> stateData;

    [Space] 
    public string WinString;
}