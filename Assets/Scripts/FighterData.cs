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
    public List<FighterStateData> stateData;
}