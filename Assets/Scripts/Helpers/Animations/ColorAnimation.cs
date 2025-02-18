using System;
using UnityEngine;
using UnityEngine.UI;

namespace FirAnimations
{
    public class ColorAnimation : MonoBehaviour, IFirAnimation
    {
        public Color StartPosition;
        public Color EndPosition;
        
        public Image[] mainComponents;
        public AnimationCurve Curve = AnimationCurve.EaseInOut(0,0,1,1);
        
        public Action OnComplete;

        private float _time;
        private float _endTime;

        private Color delta;

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
            foreach (var component in mainComponents)
            {
                component.color = StartPosition;
            }
            _time = 0;
        }
        [ContextMenu("ToEndPoint")]
        public void ToEndPoint()
        {
            foreach (var component in mainComponents)
            {
                component.color = EndPosition;
            }
            _time = _endTime;
        }
        
        public void Update()
        {
            _time += Time.deltaTime;
            float curveValue = Curve.Evaluate(_time);
            Color toColor = StartPosition + (delta * curveValue);
            foreach (var component in mainComponents)
            {
                component.color = toColor;
            }

            if (_time >= _endTime)
            {
                enabled = false;
                OnComplete?.Invoke();
            }
        }
    }
}