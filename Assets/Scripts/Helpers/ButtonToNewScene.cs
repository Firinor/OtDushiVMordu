using FirGame.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonToNewScene : MonoBehaviour
{
    [SerializeField] 
    private Scene _scene;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.SwitchToScene(_scene));
    }
}
