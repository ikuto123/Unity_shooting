using UnityEngine;
using UnityEngine.SceneManagement;

namespace Taitle
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private GameObject _HowToPlayUI;
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                
                DontDestroyOnLoad(gameObject); 
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartGame()
        {
            Debug.Log("ゲームスタート");
            SceneManager.LoadScene("Game");
        }

        public void HowToPlay()
        {
            _HowToPlayUI.gameObject.SetActive(true);
            
        }

        public void QuitHowToPlay()
        {
            _HowToPlayUI.gameObject.SetActive(false);
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