using Gacha;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject m_SpinAvailableButton;
    [SerializeField] private GachaPanel m_SpinPanel;
    [SerializeField] private List<GameObject> m_bonusFX = new List<GameObject>();

    [SerializeField]
    private float m_popupSlideDistance = 350.0f;

    [SerializeField]
    private float m_popupShakeIntensity = 10.0f;
    [SerializeField]
    private float m_popupShakeRate = 1.0f;
    [SerializeField]
    private float m_popupSlideSpeed = 100.0f;

    private float m_popupShakeAlpha = 0.0f;
    private float m_intensity = 1.0f;

    private IGachaSystem m_GachaSystem;
    private IRunResources m_RunResources;
    private Vector2 m_popupBaseLocation = Vector2.zero;

    private void Awake()
    {
        m_GachaSystem = GetComponentInParent<IGachaSystem>();
        m_RunResources = GetComponentInParent<IRunResources>();
        if (m_RunResources != null)
        {
            m_RunResources.SlimeTokensChanged += OnSlimeTokensChanged;
        }

        m_popupBaseLocation = m_SpinAvailableButton.transform.position;
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

    private void Update()
    {
        if(GameManager.Instance.IsGameOver())
        {
            ClosePopupButton();
            CloseGachaPanel();
        }

        m_popupShakeAlpha += Time.deltaTime * m_popupShakeRate;
        int availableSpins = m_RunResources.SlimeTokens / m_GachaSystem.SlimeTokenCost;
        float currentIntensity = m_popupShakeIntensity * availableSpins;
        m_intensity = Mathf.Lerp(m_intensity, currentIntensity, 0.1f * Time.deltaTime);
        float sinAlpha = Mathf.Sin(m_popupShakeAlpha);

        m_SpinAvailableButton.transform.rotation = Quaternion.Euler(0f, 0f, sinAlpha * m_intensity);

        foreach (var effect in m_bonusFX)
        {
            effect.SetActive((availableSpins > 2));
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

    private IEnumerator OpenPopupButtonSlide()
    {
        float alpha = 0.0f;

        while(alpha < 1.0f)
        {
            yield return 0;
            alpha  += Time.deltaTime * m_popupSlideSpeed;

            m_SpinAvailableButton.transform.position = Vector2.Lerp(m_popupBaseLocation + Vector2.right * m_popupSlideDistance, m_popupBaseLocation, alpha);

        }
        m_SpinAvailableButton.transform.position = m_popupBaseLocation;
    }

    private IEnumerator ClosePopupButtonSlide()
    {
        float alpha = 0.0f;

        while (alpha < 1.0f)
        {
            yield return 0;
            alpha += Time.deltaTime * m_popupSlideSpeed;

            m_SpinAvailableButton.transform.position = Vector2.Lerp(m_popupBaseLocation, m_popupBaseLocation + Vector2.right * m_popupSlideDistance, alpha);

        }
        m_SpinAvailableButton.transform.position = m_popupBaseLocation + Vector2.right * m_popupSlideDistance;
        m_SpinAvailableButton.SetActive(false);
    }


    private void OpenPopupButton()
    {
        if(!m_SpinAvailableButton.gameObject.activeInHierarchy)
        {
            m_SpinAvailableButton.transform.position = m_popupBaseLocation + Vector2.right * m_popupSlideDistance;
            m_SpinAvailableButton.SetActive(true);
            StartCoroutine(OpenPopupButtonSlide());
        }
    }

    private void ClosePopupButton()
    {
        if (m_SpinAvailableButton.gameObject.activeInHierarchy)
        {
            StartCoroutine(ClosePopupButtonSlide());
        }
    }

    public void OpenGachaPanel()
    {
        if (m_RunResources.SlimeTokens < m_GachaSystem.SlimeTokenCost)
        {
            return;
        }

        m_SpinPanel.gameObject.SetActive(true);
        GameManager.Instance.SetPaused(true);
        GameManager.Instance.FlockManager.RecallFlock();

        GameManager.Instance.AudioManager.OpenGacha();
    }

    public void CloseGachaPanel()
    {
        if (m_SpinPanel.IsRolling)
            // Can't exit while rolling or it wont finish and send back the result
            return;

        m_SpinPanel.gameObject.SetActive(false);
        GameManager.Instance.SetPaused(false);
        GameManager.Instance.FlockManager.RecallFlock();

        GameManager.Instance.AudioManager.CloseGacha();
    }
}
