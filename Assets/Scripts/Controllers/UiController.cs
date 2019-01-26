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
            
            SplashImage.canvasRenderer.SetAlpha(0);
            InfoImage.canvasRenderer.SetAlpha(0);
            ScoreImage.canvasRenderer.SetAlpha(0);
        }

        public void ShowSplash()
        {
            Debug.Log("Show splash screen");
            SplashImage.CrossFadeAlpha(1, 2f, false);
        }
        
        public void HideSplash()
        {
            Debug.Log("Hide splash screen");
            SplashImage.CrossFadeAlpha(0, 2f, false);
        }

        public void ShowInfo()
        {
            Debug.Log("Show info screen");
            InfoImage.CrossFadeAlpha(1, 2f, false);
        }
        
        public void HideInfo()
        {
            Debug.Log("Hide info screen");
            InfoImage.CrossFadeAlpha(0, 2f, false);
        }

        public void ShowScore()
        {
            InfoImage.CrossFadeAlpha(0, 2f, false);
        }
        
        public void HideScore()
        {
            InfoImage.CrossFadeAlpha(0, 2f, false);
        }

        public void FadeToBlack()
        {
            BlackoutImage.CrossFadeAlpha(1, 2f, false);
        }

        public void FadeToGame()
        {
            BlackoutImage.CrossFadeAlpha(0, 2f, false);
        }
    }
}