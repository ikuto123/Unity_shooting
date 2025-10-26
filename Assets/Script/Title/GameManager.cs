using UnityEngine;
using UnityEngine.SceneManagement;

namespace Taitle
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private GameObject _howToPlayUI;
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                
            }

            Cursor.lockState = CursorLockMode.None;
        }

        public void StartGame()
        {
            Debug.Log("ゲームスタート");
            SceneManager.LoadScene("Game");
        }

        public void HowToPlay()
        {
            _howToPlayUI.gameObject.SetActive(true);
            
        }

        public void QuitHowToPlay()
        {
            _howToPlayUI.gameObject.SetActive(false);
        }
        
        public void QuitGame()
        {
            Debug.Log("ゲーム終了処理が呼ばれました");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルドしたアプリの場合、アプリケーションを終了する
        Application.Quit();
#endif
        }
    }
}