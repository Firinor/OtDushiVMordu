using FirGame.SceneManagement;

public class EndOfBattleState : BattleState
{
    public override void OnEnter()
    {
    }
    public override void Update()
    {
    }
    public override void OnExit()
    {  
        SceneManager.SwitchToScene(Scene.WorldMap);
    }
}