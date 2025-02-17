using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] 
    private Animation _background;
    [SerializeField] 
    private Animation _vs;
    [SerializeField] 
    private Animation _playerFighter;
    [SerializeField] 
    private Animation _opponentFighter;
    [SerializeField] 
    private Animation _playerHP;
    [SerializeField] 
    private Animation _opponentHP;
    [SerializeField] 
    private Animation _playerCharge;
    [SerializeField] 
    private Animation _opponentCharge;
    [SerializeField] 
    private Animation _dangerLine;
    [SerializeField] 
    private Animation _winPoints;

    [SerializeField] 
    private AnimationConfig _animationConfig;

    private float time;
    private float timeLimit;

    public Action OnEndAllAnimations;
    public void StartAnimations()
    {
        time = 0;
        timeLimit = _animationConfig.TimeLimit;
        enabled = true;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= _animationConfig.Background && !_background.isPlaying)
            _background.Play();
        if (time >= _animationConfig.VS && !_vs.isPlaying)
            _vs.Play();
        if (time >= _animationConfig.PlayerFighter && !_playerFighter.isPlaying)
            _playerFighter.Play();
        if (time >= _animationConfig.PlayerCharge && !_playerCharge.isPlaying)
            _playerCharge.Play();
        if (time >= _animationConfig.PlayerHP && !_playerHP.isPlaying)
            _playerHP.Play();
        if (time >= _animationConfig.OpponentFighter && !_opponentFighter.isPlaying)
            _opponentFighter.Play();
        if (time >= _animationConfig.OpponentCharge && !_opponentCharge.isPlaying)
            _opponentCharge.Play();
        if (time >= _animationConfig.OpponentHP && !_opponentHP.isPlaying)
            _opponentHP.Play();
        if (time >= _animationConfig.DangerLine && !_dangerLine.isPlaying)
            _dangerLine.Play();
        if (time >= _animationConfig.WinPoints && !_winPoints.isPlaying)
            _winPoints.Play();

        if (time > timeLimit)
        {
            enabled = false;
            OnEndAllAnimations?.Invoke();
            Debug.Log("OnEndAllAnimations");
        }
    }
}