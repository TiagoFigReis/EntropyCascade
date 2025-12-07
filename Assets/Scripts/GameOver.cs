using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject GameOverCanvas, firstSelectedButton;
    [SerializeField] private TextMeshProUGUI ScoreText, EnemiesText;

    public void GameOverMenu(float score, float enemies)
    {
        GameOverCanvas.SetActive(true);
        
        Cursor.visible = true;
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        
        Player.GamePaused = true;
        
        Time.timeScale = 0f;
        
        ScoreText.text = "Score: " + score;
        EnemiesText.text = "Enemies Killed: " + enemies;
    }

    public void RestartGame()
    {
        GameOverCanvas.SetActive(false);
        SceneManager.LoadScene("Scenes/GameScene");
        Time.timeScale = 1f;
        Player.GamePaused = false;
        Cursor.visible = false;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}