using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ButtonManager : MonoBehaviour
    {   
        public void ResumeScene(){
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            PauseGame.IsPaused = false;
        }

        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void ChangeScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
