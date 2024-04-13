using Gacha;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject m_SpinAvailableButton;
    [SerializeField] private GachaPanel m_SpinPanel;

    private IGachaSystem m_GachaSystem;
    private IRunResources m_RunResources;

    private void Awake()
    {
        m_GachaSystem = GetComponentInParent<IGachaSystem>();
        m_RunResources = GetComponentInParent<IRunResources>();
        if (m_RunResources != null)
        {
            m_RunResources.SlimeTokensChanged += OnSlimeTokensChanged;
        }
    }

    private void Start()
    {
        if (m_RunResources != null)
        {
            OnSlimeTokensChanged(0, m_RunResources.SlimeTokens);
        }
        else
        {
            OpenPopupButton();
        }
    }

    private void OnSlimeTokensChanged(int oldTokens, int newTokens)
    {
        if(newTokens >= m_GachaSystem.SlimeTokenCost)
        {
            OpenPopupButton();
        }
        else
        {
            ClosePopupButton();
        }
    }

    private void OpenPopupButton()
    {
        m_SpinAvailableButton.SetActive(true);
    }

    private void ClosePopupButton()
    {
        m_SpinAvailableButton.SetActive(false);
    }

    public void OpenGachaPanel()
    {
        m_SpinPanel.gameObject.SetActive(true);
        GameManager.Instance.SetPaused(true);
    }

    public void CloseGachaPanel()
    {
        if (m_SpinPanel.IsRolling)
            // Can't exit while rolling or it wont finish and send back the result
            return;

        m_SpinPanel.gameObject.SetActive(false);
        GameManager.Instance.SetPaused(false);
    }
}
