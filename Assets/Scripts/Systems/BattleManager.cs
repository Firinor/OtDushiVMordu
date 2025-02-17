using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public BattleState state;
    
    [SerializeField]
    private Image[] _winPoints;

    private int _playerWinScore = 0;
    private int _opponentWinScore = 0;
    [SerializeField] 
    private WinLoseConfig _winLoseConfig;
    [SerializeField]
    private InputSystem _playerInput;
    [SerializeField] 
    private DangerLineSystem _dangerLineSystem;
    [SerializeField] 
    private AnimationController _animationController;
    
    [SerializeField]
    private Fighter _player;
    [SerializeField]
    private FighterData _playerData;
    [SerializeField]
    private Fighter _opponent;
    [SerializeField]
    private FighterData _opponentData;
    
    private FighterModel _playerModel;
    private FighterModel _opponentModel;
    
    [SerializeField] 
    private TextMeshProUGUI _screenCenterText;
    [SerializeField] 
    private TextWarningConfig _textWarningConfig;
    
    private void Start()
    {
        FighterData playerData = ApplicationContext.Game?.PlayerProgress.PlayerFighter;
        FighterData opponentData = ApplicationContext.Game?.NextBattleOpponent;

        if (playerData is null)
            playerData = _playerData;
        if (opponentData is null)
            opponentData = _opponentData;
        
        _playerModel = new FighterModel(playerData);
        _opponentModel = new FighterModel(opponentData);
        _player.Initialize(_playerModel);
        _opponent.Initialize(_opponentModel);

        _playerModel.OnDeath += PlayerLose;
        _opponentModel.OnDeath += PlayerWin;

        ResetUI();
        
        InBattleParams @params = GetInBattleParams();
        _animationController.OnEndAllAnimations += () =>
        {
            ChangeState(new InBattle(@params));
        };
        
        ChangeState(new AnimationsState(_animationController));
    }

    private void ResetUI()
    {
        foreach (Image point in _winPoints)
        {
            point.color = _winLoseConfig.NeutralColor;
        }
    }
    private InBattleParams GetInBattleParams()
    {
        return new InBattleParams { 
            BattleManager = this,
            Player =_player,
            PlayerInput = _playerInput,
            Opponent = _opponent,
            ScreenCenterText = _screenCenterText,
            TextWarningConfig = _textWarningConfig,
            DangerLineSystem = _dangerLineSystem,
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
            _winPoints[0].color = _winLoseConfig.PlayerWinColor;
            if (_playerWinScore > 1)
                _winPoints[1].color = _winLoseConfig.PlayerWinColor;
        }
        else
        {
            _opponentWinScore++;
            _winPoints[3].color = _winLoseConfig.EnemyWinColor;
            if (_opponentWinScore > 1)
                _winPoints[2].color = _winLoseConfig.EnemyWinColor;
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
        _playerModel.OnDeath -= PlayerLose;
        _opponentModel.OnDeath -= PlayerWin;
        StopAllCoroutines();
    }
}