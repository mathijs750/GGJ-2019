using UnityEngine;

namespace Controllers
{
    public class UiController : MonoBehaviour
    {
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
    }
}