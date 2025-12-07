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

    public void Pause()
    {
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        Time.timeScale = 0f;
        Player.GamePaused = true;
        PauseCanvas.SetActive(true);
    }
    
    public void ResumeGame()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
        Player.GamePaused = false;
        PauseCanvas.SetActive(false);
    }
    
    public void RestartGame()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
        Player.GamePaused = false;
        PauseCanvas.SetActive(false);
        SceneManager.LoadScene("Scenes/GameScene");
    }
    
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        Player.GamePaused = false;
        PauseCanvas.SetActive(false);
        SceneManager.LoadScene("Menu");
    }
}