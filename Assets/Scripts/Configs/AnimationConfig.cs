using UnityEngine;

[CreateAssetMenu(menuName = "UI/AnimationConfig")]
public class AnimationConfig : ScriptableObject
{
    public float TimeLimit;
    
    [Header("Start time")]
    public float Background;
    public float VS;
    public float PlayerFighter;
    public float OpponentFighter;
    public float PlayerHP;
    public float OpponentHP;
    public float PlayerCharge;
    public float OpponentCharge;
    public float DangerLine;
    public float WinPoints;
}