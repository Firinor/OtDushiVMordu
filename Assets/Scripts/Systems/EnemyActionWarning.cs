using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyActionWarning: IDisposable
{
    private bool isActive;
    public RectTransform Transform;
    private float currentLifeTime;
    
    private float activationXPoint;
    private float disableXPoint;

    private Func<bool> TryUseAction;

    private AttackData _attackData;
    
    private float X => Transform.anchoredPosition.x;

    public EnemyActionWarning(RectTransform transform)
    {
        Transform = transform;
    }
    
    public void Initialize(HitWarningConfig hitConfig, Sprite sprite, Func<bool> action)
    {
        Transform.anchoredPosition = new Vector2(hitConfig.StartXPosition, 0f);
        currentLifeTime = hitConfig.LifeTime;
        activationXPoint = hitConfig.ActionXPosition;
        disableXPoint = hitConfig.EndXPosition;

        Transform.GetComponent<Image>().sprite = sprite;

        TryUseAction = action;
        isActive = true;
    }

    public void Update()
    {
        float deltaX = X - activationXPoint;
        float deltaLifeTimePersentage =  (currentLifeTime - Time.deltaTime)/currentLifeTime;
        float delta = (deltaX + activationXPoint) * deltaLifeTimePersentage;
        
        Transform.anchoredPosition = new Vector2(delta, 0f);

        if (isActive && X < activationXPoint)
        {
            isActive = false;
            if(!TryUseAction.Invoke())
                ToDisable();
        }

        if (X < disableXPoint)
        {
            ToDisable();
        }
        currentLifeTime -= Time.deltaTime;
    }

    private void ToDisable()
    {
        TryUseAction = null;
        if(Transform != null)
            Transform.gameObject.SetActive(false);
    }

    public void Dispose()
    {
        ToDisable();
    }
}