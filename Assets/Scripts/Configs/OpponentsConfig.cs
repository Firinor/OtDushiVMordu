using UnityEngine;

[CreateAssetMenu(menuName = "Core/OpponentsConfig")]
public class OpponentsConfig : ScriptableObject
{
    public FighterData[] Fighters;
    public int Count;
    public FighterData LastBoss;
}