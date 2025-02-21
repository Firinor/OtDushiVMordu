using FirGame.SceneManagement;
using UnityEngine;

public class EndOfBattleState : BattleState
{
    public override void OnEnter()
    {
    }
    public override void Update()
    {
        if(Input.anyKey)
            OnExit();
    }
    public override void OnExit()
    {  
        SceneManager.SwitchToScene(Scene.WorldMap);
    }
}