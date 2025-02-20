using System;
using System.Collections;
using UnityEngine;
using FirAnimations;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private RectTransformAnimation _background;
    [SerializeField] private float _backgroundStartTime;
    [SerializeField] private ColorAnimation _backgroundColor;
    [SerializeField] private float _backgroundColorStartTime;
    [SerializeField] private RectTransformAnimation _vs;
    [SerializeField] private float _vsStartTime;
    [SerializeField] private RectTransformAnimation _playerFighter;
    [SerializeField] private float _playerFighterStartTime;
    [SerializeField] private RectTransformAnimation _opponentFighter;
    [SerializeField] private float _opponentFighterStartTime;
    [SerializeField] private RectTransformAnimation _playerHP;
    [SerializeField] private float _playerHPStartTime;
    [SerializeField] private RectTransformAnimation _opponentHP;
    [SerializeField] private float _opponentHPStartTime;
    [SerializeField] private RectTransformAnimation _playerCharge;
    [SerializeField] private float _playerChargeStartTime;
    [SerializeField] private RectTransformAnimation _opponentCharge;
    [SerializeField] private float _opponentChargeStartTime;
    [SerializeField] private ColorAnimation _dangerLine;
    [SerializeField] private float _dangerLineStartTime;
    [SerializeField] private RectTransformAnimation _winPoints;
    [SerializeField] private float _winPointsStartTime;

    private float _time;
    private float _timeLimit;

    public Action OnEndAllAnimations;
    
    public void StartAnimations(float timeLimit)
    {
        _time = 0;
        _timeLimit = timeLimit;
        
        StartCoroutine(PlayAnimation(_background, _backgroundStartTime));
        StartCoroutine(PlayAnimation(_backgroundColor, _backgroundColorStartTime));
        StartCoroutine(PlayAnimation(_vs, _vsStartTime));
        StartCoroutine(PlayAnimation(_playerFighter, _playerFighterStartTime));
        StartCoroutine(PlayAnimation(_playerHP, _playerHPStartTime));
        StartCoroutine(PlayAnimation(_playerCharge, _playerChargeStartTime));
        StartCoroutine(PlayAnimation(_opponentFighter, _opponentFighterStartTime));
        StartCoroutine(PlayAnimation(_opponentHP, _opponentHPStartTime));
        StartCoroutine(PlayAnimation(_opponentCharge, _opponentChargeStartTime));
        StartCoroutine(PlayAnimation(_dangerLine, _dangerLineStartTime));
        StartCoroutine(PlayAnimation(_winPoints, _winPointsStartTime));
        
        enabled = true;
    }

    private IEnumerator PlayAnimation(IFirAnimation animation, float startDelay = 0)
    {
        animation.Initialize();
        float time = 0;
        while (time < startDelay)
        {
            time += Time.deltaTime;
            yield return null;
        }
        animation.Play();
    }
    
    private void Update()
    {
        _time += Time.deltaTime;
        if (_time > _timeLimit)
        {
            enabled = false;
            OnEndAllAnimations?.Invoke();
        }
    }
}