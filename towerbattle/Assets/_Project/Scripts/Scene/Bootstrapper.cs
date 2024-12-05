using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : PersistentSingleton<Bootstrapper>
{
    // NOTE: 첫 Scene에 포함되어 있어야 함
    static readonly int sceneIndex = 0;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static async void Init()
    {
        Debug.Log("Bootstrapper...");
#if UNITY_EDITOR
        // UnityEditor에서 Play mode에 진입할 때 Bootstrapper Scene을 최초로 Load되도록 함
        // (어떤 Scene이 활성화 되어 있던 상관X)
        //EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[sceneIndex].path);

        await SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
#endif
    }
}