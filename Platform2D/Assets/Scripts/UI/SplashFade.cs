using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashFade : MonoBehaviour
{
    [SerializeField] private Image logoImage;

    private IEnumerator Start()
    {
        logoImage.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSeconds(2.5f);
        FadeOut();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MainMenu");
    }

    private void FadeIn()
    {
        logoImage.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    private void FadeOut()
    {
        logoImage.CrossFadeAlpha(0.0f, 2.5f, false);
    }
}
