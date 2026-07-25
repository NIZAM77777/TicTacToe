using UnityEngine;

public class RateUs : MonoBehaviour
{
    public void OpenPlayStore()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.mn7studios.tictactoe");
#endif
    }
}