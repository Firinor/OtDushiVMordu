using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle), typeof(Image))]
public class FighterBinder : MonoBehaviour
{
    public FighterData ThisFighter;

    public void Start()
    {
        GetComponent<Image>().sprite = ThisFighter.Portrait;
    }
}
