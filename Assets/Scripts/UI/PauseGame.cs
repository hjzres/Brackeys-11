using UnityEngine;

namespace UI {
    
    public class PauseGame : MonoBehaviour 
    {
        public static bool IsPaused;
        [SerializeField] private GameObject pauseMenu;
        
        private void Start() {
            pauseMenu.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pauseMenu.activeSelf)
                {
                    ResumeGame();
                }
                else
                {
                    Pause();
                }
            }
        }

        private void Pause()
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenu.SetActive(true);
            IsPaused = true;
        }

        private void ResumeGame()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseMenu.SetActive(false);
            IsPaused = false;
        }
    }
}
