using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DangerLineSystem : MonoBehaviour
{
    [SerializeField]
    private HitWarningConfig hitConfig;
    public float warningTime => hitConfig.LifeTime;
    
    [SerializeField]
    private WarningSpritesConfig hitSpritesConfig;
    [SerializeField]
    private RectTransform hitPool;
    private List<EnemyActionWarning> enemyWarnings = new ();
    [SerializeField]
    private GameObject hitWarningViewPrefab;

    [SerializeField]
    private Fighter _player;
    [SerializeField]
    private Fighter _opponent;

    private Func<bool> OnPlayerHit;
    
    public void ClearAll()
    {
        for (int i = 0; i < hitPool.childCount; i++)
        {
            Destroy(hitPool.GetChild(i).gameObject); 
        }
        enemyWarnings = new ();
    }
    
    private void Update()
    {
        IEnumerable<EnemyActionWarning> activeWarnings = enemyWarnings.Where(a => a.Transform.gameObject.activeSelf);
        foreach (var hitWarning in activeWarnings)
        {
            hitWarning.Update();
        }
    }
    
    public void AddOpponentAction(FightCommand command, Func<bool> onActivateFunc)
    {
        var result = enemyWarnings.Find(a => a.Transform.gameObject.activeSelf == false);
        if (result == null)
        {
            RectTransform newObj = Instantiate(hitWarningViewPrefab, hitPool).GetComponent<RectTransform>();
            result = new EnemyActionWarning(newObj);
            enemyWarnings.Add(result);
        }

        Sprite icon = GetSprite(command);
        
        result.Initialize(hitConfig, icon, onActivateFunc);
        result.Transform.gameObject.SetActive(true);
    }

    private Sprite GetSprite(FightCommand command) 
        => command switch
    {
        FightCommand.Attack => hitSpritesConfig.LightAttack,
        FightCommand.Charge => hitSpritesConfig.HeavyAttack,
        FightCommand.Defence => hitSpritesConfig.Defence,
        FightCommand.Evade => hitSpritesConfig.Evade,
        _ => throw new Exception()
    };

    private void OnDestroy()
    {
        foreach (var hitWarning in enemyWarnings)
        {
            hitWarning.Dispose();
        }
    }
}