using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
    
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Scenes/GameScene");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
