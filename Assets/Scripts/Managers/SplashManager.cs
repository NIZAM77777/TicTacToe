using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private float splashDuration = 3f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(splashDuration);

        SceneManager.LoadScene("MainMenu");
    }
}