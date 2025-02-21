using TMPro;
using UnityEngine;

namespace FirAnimations
{
    public static class FirAnimationsExtensions
    {
        public static void InitializeFirTextAnimation(this TextMeshProUGUI textMesh, FirTextAnimationData data)
        {
            TextAnimation animation = textMesh.gameObject.AddComponent<TextAnimation>();
            animation.textComponent = textMesh;
            textMesh.text = data.Text;
            animation.Curve = data.LifeLine;
            animation.EndPosition = data.MaxFontSize;
            animation.Initialize();
            if(data.OnEnd is not null)
                animation.OnComplete += data.OnEnd;
            animation.OnComplete += () =>
            {
                textMesh.enabled = false;
                Object.Destroy(animation);
            };
            textMesh.enabled = true;
        }
    }
}