using TMPro;
using UnityEngine;

namespace FirAnimations
{
    public static class FirAnimationsExtensions
    {
        public static void PlayFirTextAnimation(this TextMeshProUGUI textMesh, FirTextAnimationData data)
        {
            TextAnimation animation = textMesh.gameObject.GetComponent<TextAnimation>();
            if(animation is null) 
                animation = textMesh.gameObject.AddComponent<TextAnimation>();
            animation.OnComplete = null;
                
            animation.textComponent = textMesh;
            textMesh.text = data.Text;
            animation.Curve = data.LifeLine;
            animation.EndPosition = data.MaxFontSize;
            animation.Initialize();
            animation.OnComplete += () =>
            {
                textMesh.enabled = false;
            };
            if(data.OnEnd is not null)
                animation.OnComplete += data.OnEnd;
            textMesh.enabled = true;
            animation.Play();
        }
    }
}