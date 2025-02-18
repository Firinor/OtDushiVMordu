using UnityEngine;

[CreateAssetMenu(menuName = "Messages/TextWarningConfig")]
public class TextConfig : ScriptableObject
{
    public string WarningText = "НЕТ СИЛ!";
    public AnimationCurve WarningTextLifeTime;
    public float WarningTextFontSize;
    
    public string GetReadyText = "ПРИГОТОВИТЬСЯ";
    public AnimationCurve GetReadyTextLiveTime;
    public float GetReadyTextFontSize;
    public string FightText = "МАХАЧ!";
    public AnimationCurve FightTextLiveTime;
    public float FightTextFontSize;
}