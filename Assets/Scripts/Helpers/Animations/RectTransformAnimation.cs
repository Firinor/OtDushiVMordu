using System;
using UnityEngine;

namespace FirAnimations
{
    public class RectTransformAnimation : MonoBehaviour, IFirAnimation
    {
        public RectTransformPosition StartPosition;
        public RectTransformPosition EndPosition;
        
        public RectTransform mainComponent;
        public AnimationCurve Curve = AnimationCurve.EaseInOut(0,0,1,1);
        
        public Action OnComplete;

        private float _time;
        private float _endTime;

        private RectTransformPosition delta;

        public void Initialize()
        {
            Stop();
            delta = EndPosition - StartPosition;
            _endTime = Curve.keys[Curve.length-1].time;
            ToStartPoint();
        }
        
        public void Play() => enabled = true;
        public void Stop() => enabled = false;
        
        [ContextMenu("ToStartPoint")]
        public void ToStartPoint()
        {
            mainComponent.anchoredPosition = StartPosition.AncoredPosition;
            mainComponent.localScale = StartPosition.Scale;
            _time = 0;
        }
        [ContextMenu("ToEndPoint")]
        public void ToEndPoint()
        {
            mainComponent.anchoredPosition = EndPosition.AncoredPosition;
            mainComponent.localScale = EndPosition.Scale;
            _time = _endTime;
        }
        
        public void Update()
        {
            _time += Time.deltaTime;
            float curveValue = Curve.Evaluate(_time);
            mainComponent.anchoredPosition = StartPosition.AncoredPosition + (delta.AncoredPosition * curveValue);
            mainComponent.localScale = StartPosition.Scale + (delta.Scale * curveValue);

            if (_time >= _endTime)
            {
                enabled = false;
                OnComplete?.Invoke();
            }
        }
    }
}