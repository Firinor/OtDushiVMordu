using UnityEngine;

[CreateAssetMenu(menuName = "Messages/TextWarningConfig")]
public class TextWarningConfig : ScriptableObject
{
    public float LifeTime = .8f;
    public string WarningText = "НЕТ СИЛ!";
}