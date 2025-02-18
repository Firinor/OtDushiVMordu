using System;
using UnityEngine;

namespace FirAnimations
{
    [Serializable]
    public struct RectTransformPosition
    {
        public Vector2 AncoredPosition;
        public Vector3 Scale;
            
        public static RectTransformPosition operator -(RectTransformPosition a, RectTransformPosition b)
            => new RectTransformPosition
            {
                AncoredPosition = a.AncoredPosition - b.AncoredPosition,
                Scale = a.Scale - b.Scale
            };
        public static RectTransformPosition operator +(RectTransformPosition a, RectTransformPosition b)
            => new RectTransformPosition
            {
                AncoredPosition = a.AncoredPosition + b.AncoredPosition,
                Scale = a.Scale + b.Scale
            };
    }
}