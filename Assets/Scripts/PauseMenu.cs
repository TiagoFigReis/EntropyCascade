using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    [SerializeField] private GameObject PauseCanvas, firstSelectedButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Player.GamePaused = false;
        PauseCanvas.SetActive(false);
    }
    
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        Player.GamePaused = false;
        PauseCanvas.SetActive(false);
        SceneManager.LoadScene("Menu");
    }
}
