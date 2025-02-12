using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    public Slider Slider;
    public Image Bar;
    
    public Color LightAttack;
    public Color HeavyAttack;
    
    public void ChengeCharge(float chargeTime)
    {
        Slider.value = chargeTime;
        
        if (chargeTime >= Slider.maxValue)
            Bar.color = HeavyAttack;
        else
            Bar.color = LightAttack;
    }
}