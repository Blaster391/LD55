using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartRun()
    {
        SceneManager.LoadScene(m_SceneToLoad);
    }

    public int m_SceneToLoad = 1;

    public void ExitGame()
    {
        Application.Quit();
    }
}
