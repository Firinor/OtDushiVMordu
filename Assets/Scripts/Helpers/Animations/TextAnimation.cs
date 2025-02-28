using System;
using TMPro;
using UnityEngine;

namespace FirAnimations
{
    public class TextAnimation : MonoBehaviour, IFirAnimation
    {
        public float StartPosition;
        public float EndPosition;
        
        public TextMeshProUGUI textComponent;
        public AnimationCurve Curve = AnimationCurve.EaseInOut(0,0,1,1);
        
        public Action OnComplete;

        private float _time;
        private float _endTime;

        private float delta;
        
        public void Initialize()
        {
            Stop();
            OnComplete = null;
            delta = EndPosition - StartPosition;
            _endTime = Curve.keys[Curve.length-1].time;
            ToStartPoint();
        }
        
        public void Play() => enabled = true;
        public void Stop() => enabled = false;
        
        [ContextMenu("ToStartPoint")]
        public void ToStartPoint()
        {
            textComponent.fontSize = StartPosition;
            _time = 0;
        }
        [ContextMenu("ToEndPoint")]
        public void ToEndPoint()
        {
            textComponent.fontSize = EndPosition;
            _time = _endTime;
        }
        
        public void Update()
        {
            _time += Time.deltaTime;
            float curveValue = Curve.Evaluate(_time);
            textComponent.fontSize = StartPosition + (delta * curveValue);

            if (_time >= _endTime)
            {
                enabled = false;
                OnComplete?.Invoke();
            }
        }
    }
}