using UnityEngine;
using UnityEngine.SceneManagement;
using static UI.PauseGame;

public class ButtonManager : MonoBehaviour
{   
    public void resumeScene(){
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        IsPaused = false;
    }

    public void changeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void changeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
