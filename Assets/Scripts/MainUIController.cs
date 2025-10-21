using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Scenes/GameScene");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
