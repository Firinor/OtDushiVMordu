using System;
using UnityEngine;

public class HitWarning
{
    private bool isActive;
    public RectTransform Transform;
    private int damage;
    private float currentLifeTime;
    
    private float activationXPoint;
    private float disableXPoint;

    public Action OnActivation;
    private float X => Transform.anchoredPosition.x;

    public HitWarning(RectTransform transform)
    {
        Transform = transform;
    }
    
    public void Initialize(HitWarningConfig hitConfig)
    {
        Transform.anchoredPosition = new Vector2(hitConfig.StartXPosition, 0f);
        currentLifeTime = hitConfig.LifeTime;
        activationXPoint = hitConfig.ActionXPosition;
        disableXPoint = hitConfig.EndXPosition;
        
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
            OnActivation?.Invoke();
            isActive = false;
        }

        if (X < disableXPoint)
        {
            Transform.gameObject.SetActive(false);
        }
        currentLifeTime -= Time.deltaTime;
    }
}