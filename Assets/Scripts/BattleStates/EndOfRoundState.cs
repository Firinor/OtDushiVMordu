using TMPro;

public class EndOfRoundState : BattleState
{
    private Fighter _winer;
    private TextMeshProUGUI _text;
    
    public EndOfRoundState(Fighter winer)
    {
        _winer = winer;
    }

    public override void OnEnter()
    {
    }
    public override void Update()
    {
        
    }
    public override void OnExit()
    {  
        //SceneManager.SwitchToScene(Scene.WorldMap);
    }
}