using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    
    [SerializeField]
    private Image[] WinPoints;
    
    [SerializeField]
    private Fighter _player;
    [SerializeField]
    private Fighter _opponent;
    
    [SerializeField] 
    private TextMeshProUGUI _screenCenterText;
    [SerializeField] 
    private TextWarningConfig _textWarningConfig;

    //bootstrap
    private void Awake()
    {
        var playerData = Resources.Load<FighterData>("Fighters/Bogatyr");
        var opponentData = Resources.Load<FighterData>("Fighters/Koshey");
        InputSystem playerInput = GameObject.Find("InputSystem").GetComponent<InputSystem>();
        DangerLineSystem dangerLineSystem = GameObject.Find("DangerLineSystem").GetComponent<DangerLineSystem>();
        
        InBattleParams @params = new InBattleParams { 
            BattleSystem = this,
            Player =_player,
            PlayerData = playerData,
            PlayerInput = playerInput,
            Opponent = _opponent,
            OpponentData = opponentData,
            ScreenCenterText = _screenCenterText,
            TextWarningConfig = _textWarningConfig,
            DangerLineSystem = dangerLineSystem,
        };
        ChangeState(new InBattle(@params));
    }

    private void Update()
    {
        state?.Update();
    }
    
    private void ChangeState(BattleState newState)
    {
        if (state == newState)
            return;
        
        state?.OnExit();
        state = newState;
        state.OnEnter();
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}