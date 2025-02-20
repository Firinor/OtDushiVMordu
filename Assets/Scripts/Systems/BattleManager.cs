using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private BattleState _state;
    
    [SerializeField]
    private Image[] _playerWinPoints;
    public Image[] PlayerWinPoints => _playerWinPoints;
    [SerializeField]
    private Image[] _opponentWinPoints;
    public Image[] OpponentWinPoints => _opponentWinPoints;
    
    [SerializeField] 
    private WinLoseConfig _winLoseConfig;
    public WinLoseConfig WinLoseConfig => _winLoseConfig;
    [SerializeField] 
    private BattleStatesTimeConfig _statesTimeConfig;
    public BattleStatesTimeConfig StatesTimeConfig => _statesTimeConfig;
    [SerializeField]
    private InputSystem _playerInput;
    public InputSystem PlayerInput => _playerInput;
    [SerializeField] 
    private DangerLineSystem _dangerLineSystem;
    public DangerLineSystem DangerLineSystem => _dangerLineSystem;
    [SerializeField] 
    private AnimationController _animationController;
    public AnimationController AnimationController => _animationController;
    
    [SerializeField]
    private Fighter _player;
    public Fighter Player => _player;
    [SerializeField]
    private FighterData _playerData;
    [SerializeField]
    private Fighter _opponent;
    public Fighter Opponent => _opponent;
    [SerializeField]
    private FighterData _opponentData;
    
    private FighterModel _playerModel;
    private FighterModel _opponentModel;
    
    [SerializeField] 
    private TextMeshProUGUI _screenCenterText;
    public TextMeshProUGUI CenterText => _screenCenterText;
    [SerializeField] 
    private TextConfig _textConfig;
    public TextConfig TextConfig => _textConfig;
    
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
        
        _playerModel.OnWinCountChange += OnPlayerWin;
        _opponentModel.OnWinCountChange += OnEnemyWin;
        
        _animationController.OnEndAllAnimations += ToGetReadyState;
        ChangeState(new AnimationsState(this));
    }

    private void OnPlayerWin(int winCount)
    {
        if (winCount == 1)
            PlayerWinPoints[0].color = _winLoseConfig.PlayerWinColor;
        else if (winCount == 2)
            PlayerWinPoints[1].color = _winLoseConfig.PlayerWinColor;
        else
            throw new Exception();
    }
    private void OnEnemyWin(int winCount)
    {
        if (winCount == 1)
            OpponentWinPoints[0].color = _winLoseConfig.EnemyWinColor;
        else if (winCount == 2)
            OpponentWinPoints[1].color = _winLoseConfig.EnemyWinColor;
        else
            throw new Exception();
    }
    
    private void ToGetReadyState()
    {
        GetReadyState getReadyState 
            = new GetReadyState(this);
        
        getReadyState.OnEndState += ToInBattleState;
            
        ChangeState(getReadyState);
    }

    private void ToInBattleState()
    {
        ChangeState(new InBattle(this));
    }

    private void ResetUI()
    {
        foreach (Image point in PlayerWinPoints)
        {
            point.color = _winLoseConfig.NeutralColor;
        }
        foreach (Image point in OpponentWinPoints)
        {
            point.color = _winLoseConfig.NeutralColor;
        }
    }
    private void PlayerWin() => EndOfRound(isPlayerWin: true);
    private void PlayerLose() => EndOfRound(isPlayerWin: false);
    private void EndOfRound(bool isPlayerWin)
    { 
        ChangeState(new EndOfRoundState(this, isPlayerWin));
    }
    public void NextRound()
    {
        if (Player.WinCount == 2)
        {
            ApplicationContext.Game.PlayerWin();
            ChangeState(new EndOfBattleState());
        }
        else if(Opponent.WinCount == 2)
        {
            ChangeState(new EndOfBattleState());
        }
        else
        {
            ToGetReadyState();
        }
    }

    private void Update()
    {
        _state?.Update();
    }
    
    private void ChangeState(BattleState newState)
    {
        if (_state == newState)
            return;
        
        _state?.OnExit();
        _state = newState;
        _state.OnEnter();
    }
    
    private void OnDestroy()
    {
        _playerModel.OnDeath -= PlayerLose;
        _opponentModel.OnDeath -= PlayerWin;
        StopAllCoroutines();
    }
}