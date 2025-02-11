using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DangerLineSystem : MonoBehaviour
{
    [SerializeField]
    private HitWarningConfig hitConfig;
    [SerializeField]
    private RectTransform hitPool;
    private List<HitWarning> hitWarnings = new ();
    [SerializeField]
    private GameObject hitWarningViewPrefab;

    [SerializeField]
    private Fighter _player;
    [SerializeField]
    private Fighter _opponent;

    private Func<bool> OnPlayerHit;
    
    public void Initialize(Func<bool> OnPlayerHit)
    {
        this.OnPlayerHit += OnPlayerHit;
        for (int i = 0; i < hitPool.childCount; i++)
        {
            Destroy(hitPool.GetChild(i).gameObject); 
        }
    }
    
    private void Update()
    {
        IEnumerable<HitWarning> activeHits = hitWarnings.Where(a => a.Transform.gameObject.activeSelf);
        foreach (var hitWarning in activeHits)
        {
            hitWarning.Update();
        }
    }
    
    public void AddOpponentAttack()
    {
        var result = hitWarnings.Find(a => a.Transform.gameObject.activeSelf == false);
        if (result == null)
        {
            RectTransform newObj = Instantiate(hitWarningViewPrefab, hitPool).GetComponent<RectTransform>();
            result = new HitWarning(newObj);
            hitWarnings.Add(result);
            result.TryHitOpponent += HitPlayer;
        }
        result.Initialize(hitConfig);
        result.Transform.gameObject.SetActive(true);
    }

    private bool HitPlayer()
    {
        return OnPlayerHit.Invoke();
    }

    private void OnDestroy()
    {
        foreach (var hitWarning in hitWarnings)
        {
            hitWarning.TryHitOpponent -= HitPlayer;
        }
    }
}