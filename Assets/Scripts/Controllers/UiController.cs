using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private Image BlackoutImage;
        [SerializeField] private Image SplashImage;
        [SerializeField] private Image InfoImage;
        [SerializeField] private Image[] ScoreImages;

        public static UiController Instance;

        public void ShowSplash()
        {
            SplashImage.CrossFadeAlpha(1, 2f, false);
        }
        
        public void HideSplash()
        {
            SplashImage.CrossFadeAlpha(0, 2f, false);
        }

        public void ShowInfo()
        {
            InfoImage.CrossFadeAlpha(1, 2f, false);
        }
        
        public void HideInfo()
        {
            InfoImage.CrossFadeAlpha(0, 2f, false);
        }

        public void ShowScore()
        {
            foreach (var img in ScoreImages)
            {
                img.CrossFadeAlpha(1, 2f, false);
            }
        }
        
        public void HideScore()
        {
            foreach (var img in ScoreImages)
            {
                img.CrossFadeAlpha(0, 2f, false);
            }
        }

        public void FadeToBlack()
        {
            BlackoutImage.CrossFadeAlpha(1, 2f, false);
        }

        public void FadeToGame()
        {
            BlackoutImage.CrossFadeAlpha(0, 2f, false);
        }
        
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
            foreach (var img in ScoreImages)
            {
                img.canvasRenderer.SetAlpha(0);
            }
        }
    }
}