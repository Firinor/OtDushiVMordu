using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public BattleState state;
    
    [SerializeField]
    private Image[] WinPoints;

    private int _playerWinScore = 0;
    private int _opponentWinScore = 0;
    [SerializeField] 
    private WinLoseConfig _winLoseConfig;
    [SerializeField]
    private InputSystem playerInput;
    [SerializeField] 
    private DangerLineSystem dangerLineSystem;
    
    [SerializeField]
    private Fighter _player;
    [SerializeField]
    private FighterData _playerData;
    [SerializeField]
    private Fighter _opponent;
    [SerializeField]
    private FighterData _opponentData;
    
    private FighterModel playerModel;
    private FighterModel opponentModel;
    
    [SerializeField] 
    private TextMeshProUGUI _screenCenterText;
    [SerializeField] 
    private TextWarningConfig _textWarningConfig;
    
    private void Start()
    {
        FighterData playerData = ApplicationContext.Game?.PlayerProgress.PlayerFighter;
        FighterData opponentData = ApplicationContext.Game?.NextBattleOpponent;

        if (playerData == null)
            playerData = _playerData;
        if (opponentData == null)
            opponentData = _opponentData;
        
        playerModel = new FighterModel(playerData);
        opponentModel = new FighterModel(opponentData);
        _player.Initialize(playerModel);
        _opponent.Initialize(opponentModel);

        playerModel.OnDeath += PlayerLose;
        opponentModel.OnDeath += PlayerWin;

        ResetUI();
        
        InBattleParams @params = GetInBattleParams();
        ChangeState(new InBattle(@params));
    }

    private void ResetUI()
    {
        foreach (Image point in WinPoints)
        {
            point.color = _winLoseConfig.NeutralColor;
        }
    }
    private InBattleParams GetInBattleParams()
    {
        return new InBattleParams { 
            BattleManager = this,
            Player =_player,
            PlayerInput = playerInput,
            Opponent = _opponent,
            ScreenCenterText = _screenCenterText,
            TextWarningConfig = _textWarningConfig,
            DangerLineSystem = dangerLineSystem,
        };
    }
    private void PlayerWin() => EndOfRound(isPlayerWin: true);
    private void PlayerLose() => EndOfRound(isPlayerWin: false);
    private void EndOfRound(bool isPlayerWin)
    {
        _player.ChangeStateOnEndBattle(isPlayerWin);
        _opponent.ChangeStateOnEndBattle(!isPlayerWin);
        
        Fighter winer = isPlayerWin ? _player : _opponent;
        _screenCenterText.text = winer.WinString;
        _screenCenterText.enabled = true;

        if (isPlayerWin)
        {
            _playerWinScore++;
            WinPoints[0].color = _winLoseConfig.PlayerWinColor;
            if (_playerWinScore > 1)
                WinPoints[1].color = _winLoseConfig.PlayerWinColor;
        }
        else
        {
            _opponentWinScore++;
            WinPoints[3].color = _winLoseConfig.EnemyWinColor;
            if (_opponentWinScore > 1)
                WinPoints[2].color = _winLoseConfig.EnemyWinColor;
        }
        
        ChangeState(new WinLoseState());
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
        playerModel.OnDeath -= PlayerLose;
        opponentModel.OnDeath -= PlayerWin;
        StopAllCoroutines();
    }
}