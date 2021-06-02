using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string sceneName;

    public void Load_Scene() => SceneManager.LoadScene(sceneName);
}
