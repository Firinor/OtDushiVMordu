using UnityEngine;

[CreateAssetMenu(menuName = "Balance/WinLoseConfig")]
public class WinLoseConfig : ScriptableObject
{
    public int RoundsToWin = 2;
    public Color NeutralColor;
    public Color PlayerWinColor;
    public Color EnemyWinColor;
}