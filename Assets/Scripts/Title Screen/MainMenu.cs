using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject credits;

    private bool showCredits;

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        showCredits = !showCredits;

        gameObject.SetActive(!showCredits);
        credits.SetActive(showCredits);
    }
}
