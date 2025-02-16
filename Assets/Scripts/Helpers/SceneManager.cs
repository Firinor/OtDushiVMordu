namespace FirGame.SceneManagement
{
    public class SceneManager
    {
        public static void SwitchToScene(Scene sceneKey)
        {
           UnityEngine.SceneManagement.SceneManager.LoadScene((int)sceneKey);
        }
    }
}