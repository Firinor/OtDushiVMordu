using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Core/BattleStatesTimeConfig")]
public class BattleStatesTimeConfig : ScriptableObject
{
    public float VSLifeTime = 6;
    [FormerlySerializedAs("PrepareLifeTime")] public float GerReadyLifeTime = 2;
    public float EndOfRoundLifeTime = 2;
}