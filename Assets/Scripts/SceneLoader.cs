using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneReference sceneToLoad;

    private void Load() {
        SceneManager.LoadScene(sceneToLoad);
    }
}