using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace System.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] Slider loadingBar;
        [SerializeField] TMP_Text loadingText;
        [SerializeField] float fillSpeed = 0.5f;
        [SerializeField] Canvas loadingCanvas;
        [SerializeField] Camera loadingCamera;
        [SerializeField] SceneGroup[] sceneGroups;

        float targetProgress;
        bool isLoading;

        public readonly SceneGroupManager manager = new SceneGroupManager(); 

        private void Awake() 
        {
            loadingText.text = $"LOADING...{0.00}%";
            manager.OnSceneGroupLoaded += () => 
            {
                loadingText.text = $"LOADING...{100.00}%";
                loadingBar.value = 1;
            };
        }

        async void Start()
        {
            

            await LoadSceneGroup(0);
        }

        void Update()
        {
            if (!isLoading)
                return;
            
            float currentFillAmount = loadingBar.value;
            float progressDiffrence = Mathf.Abs(currentFillAmount - targetProgress);
            
            float dynamicFillSpeed = progressDiffrence * fillSpeed;

            loadingBar.value = Mathf.Lerp(currentFillAmount, targetProgress, Time.deltaTime * dynamicFillSpeed);
            loadingText.text = $"LOADING...{loadingBar.value * 100f:0.00}%";
        }

        public async Task LoadSceneGroup(int index)
        {
            loadingBar.value = 0f;
            targetProgress = 1f;

            if (index < 0 || index >= sceneGroups.Length)
            {
                Debug.LogError("Invalid scene group index: " + index);
                return;
            }

            LoadingProgress progress = new LoadingProgress();
            progress.Progressed += target => targetProgress = Mathf.Max(target, targetProgress);

            EnableLoadingCanvas();
            await manager.LoadScenes(sceneGroups[index], progress);
            EnableLoadingCanvas(false);
        }

        void EnableLoadingCanvas(bool enable = true)
        {
            isLoading = enable;
            loadingCanvas.gameObject.SetActive(enable);
            loadingCamera.gameObject.SetActive(enable);
        }
    }

    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> Progressed;

        const float ratio = 1f;

        public void Report(float value)
        {
            Progressed?.Invoke(value / ratio);
        }
    }
}