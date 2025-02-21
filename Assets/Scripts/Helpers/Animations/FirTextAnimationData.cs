using System;
using UnityEngine;

namespace FirAnimations
{
    public struct FirTextAnimationData
    {
        public string Text;
        public AnimationCurve LifeLine;
        public float MaxFontSize;
        public Action OnEnd;
    }
}