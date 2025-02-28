using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ToExit);
    }

    private void ToExit()
    {
        var currentScene = SceneManager.GetActiveScene();

        switch (currentScene.buildIndex)
        {
            case (int)Scene.Fight:
                FirGame.SceneManagement.SceneManager.SwitchToScene(Scene.WorldMap);
                break;
            case (int)Scene.WorldMap:
                FirGame.SceneManagement.SceneManager.SwitchToScene(Scene.MainMenu);
                break;
            case (int)Scene.ChooseFighter:
                FirGame.SceneManagement.SceneManager.SwitchToScene(Scene.MainMenu);
                break;
            default:
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(ToExit);
    }
}
