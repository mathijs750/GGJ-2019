using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private Image BlackoutImage; 
        [SerializeField] private Image SplashImage; 
        [SerializeField] private Image InfoImage; 
        [SerializeField] private Image ScoreImage; 
        
        public static UiController Instance;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public void ShowSplash()
        {
            Debug.Log("Show splash screen");
        }
        
        public void ShowInfo(){}
        
        public void ShowScore(){}
        
        public void FadeToBlack(){}
        
        public void FadeToGame(){}
    }
}