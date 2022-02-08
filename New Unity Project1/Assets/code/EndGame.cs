using UnityEngine;
using UnityEngine.SceneManagement;

namespace code
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;

        public void MenuButton()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

        public void Lose()
        {
            _losePanel.SetActive(true);
            Time.timeScale = 0;
        }

        public void Win()
        {
            _winPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}