using UnityEngine;

[CreateAssetMenu(menuName = "Balance/HitWarningConfig")]
public class HitWarningConfig : ScriptableObject
{
    public float LifeTime;
    public float StartXPosition = 1800;
    public float ActionXPosition = -45;
    public float EndXPosition = -400;
}