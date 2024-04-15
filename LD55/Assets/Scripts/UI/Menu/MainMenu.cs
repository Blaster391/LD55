using Scoreboard.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField]
    TMPro.TextMeshProUGUI m_personalBest = null;

    [SerializeField]
    GameObject m_highscorePanel = null;

    [SerializeField]
    List<TMPro.TextMeshProUGUI> m_highscores = null;

    [SerializeField]
    TMPro.TMP_InputField m_name = null;

    private void Start()
    {
        m_highscorePanel.SetActive(false);

        int highScore = PlayerPrefs.GetInt("score", 0);

        m_personalBest.text = $"Personal Best {highScore}";
        if(PlayerPrefs.GetInt("victory", 0) > 0)
        {
            m_personalBest.text += " (Victory)";
        }

        m_personalBest.gameObject.SetActive(highScore > 0);

        m_name.text = PlayerPrefs.GetString("name", m_name.text);

        Func<List<ScoreboardCore.Data.ScoreResult>, bool, bool> highscoresCallback = (results, success) =>
        {

            if (success)
            {
                m_highscorePanel.SetActive(true);
                for(int i = 0; i < m_highscores.Count; ++i)
                {
                    if(results.Count <= i)
                    {
                        m_highscores[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        m_highscores[i].text = $"{results[i].Ranking}. {results[i].Score.User} - {results[i].Score.ScoreValue}";
                    }

                }
               
            }

            return true;
        };

        ScoreboardComponent scoreboard = GetComponent<ScoreboardComponent>();
        scoreboard.GetHighscores(highscoresCallback, "");

        AudioListener.volume = 0.3f;
    }

    public void StartRun()
    {
        PlayerPrefs.SetString("name", m_name.text);
        PlayerPrefs.Save();

        SceneManager.LoadScene(m_SceneToLoad);
    }

    public int m_SceneToLoad = 1;

    public void ExitGame()
    {
        Application.Quit();
    }
}
