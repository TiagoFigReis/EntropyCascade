using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    [SerializeField] private GameObject PauseCanvas;
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        PauseCanvas.SetActive(false);
    }
    
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        PauseCanvas.SetActive(false);
        SceneManager.LoadScene("Menu");
    }
}
