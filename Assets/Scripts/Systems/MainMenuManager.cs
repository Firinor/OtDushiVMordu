using FirGame.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] 
    private Button _continue;
    [SerializeField] 
    private Button _startEasy;
    [SerializeField] 
    private Button _startHard;

    private void Start()
    {
        if(ApplicationContext.Game.PlayerProgress.PlayerFighter == null)
            _continue.gameObject.SetActive(false);
        
        _continue.onClick.AddListener(OnContinueClick);
        _startEasy.onClick.AddListener(OnEasyGameClick);
        _startHard.onClick.AddListener(OnHardGameClick);
    }

    public void OnContinueClick()
    {
        SceneManager.SwitchToScene(Scene.WorldMap);
    }
    
    public void OnEasyGameClick()
    {
        ApplicationContext.Game.PlayerProgress = new PlayerProgress()
        {
            Difficulty = Difficulty.Easy,
        };
        SceneManager.SwitchToScene(Scene.ChooseFighter);
    }
    public void OnHardGameClick()
    {
        ApplicationContext.Game.PlayerProgress = new PlayerProgress()
        {
            Difficulty = Difficulty.Hard,
        };
        SceneManager.SwitchToScene(Scene.ChooseFighter);
    }
}
