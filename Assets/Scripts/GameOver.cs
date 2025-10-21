using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private TextMeshProUGUI ScoreText, EnemiesText;

    public void GameOverMenu(float score, float enemies)
    {
        GameOverCanvas.SetActive(true);
        
        Time.timeScale = 0f;
        
        ScoreText.text = "Score: " + score;
        EnemiesText.text = "Enemies Killed: " + enemies;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameOverCanvas.SetActive(false);
        SceneManager.LoadScene("Scenes/GameScene");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
