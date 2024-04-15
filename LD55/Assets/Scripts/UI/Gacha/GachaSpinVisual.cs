using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gacha
{
    public class GachaSpinVisual : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private float m_SpinDuration;
        [SerializeField] private float m_SpinSpeed;
        [SerializeField] private AnimationCurve m_SpinSpeedCurve;

        [Header("References")]
        [SerializeField] private RawImage m_ImageDisplay;

        [SerializeField]
        private TMPro.TextMeshProUGUI m_SlimeRank = null;

        [SerializeField]
        private TMPro.TextMeshProUGUI m_SlimeType = null;

        [SerializeField]
        private TMPro.TextMeshProUGUI m_SlimeDamageModifier = null;

        public event Action<GachaRollResult> SpinVisualComplete;

        [SerializeField]
        private ParticleSystem m_FanfareParticleSystem;

        private GachaSystem m_GachaSystem;
        private GachaPanel m_GachaPanel;
        float m_spinProgressTime = 0;
        public bool IsRolling { get; private set; } = false;

        [SerializeField]
        private float m_imageSpinTime = 2.0f;
        [SerializeField]
        private float m_imageSpinHeight = 3.0f;

        private float m_currentImageSpinTime = 0.0f;
        private bool m_shouldImageSpin = false;
        private Vector2 m_imagePosition = Vector2.zero;

        private void Awake()
        {
            m_GachaSystem = GetComponentInParent<GachaSystem>();
            m_imagePosition = m_ImageDisplay.transform.position;
        }

        public void VisualiseSpin(GachaRollResult rollResult)
        {
            StartCoroutine(VisualiseSpinRoutine(rollResult));
        }

        public void OnEnable()
        {
            m_ImageDisplay.texture = null;
            m_ImageDisplay.color = Color.clear;
            m_ImageDisplay.transform.position = m_imagePosition;
            m_ImageDisplay.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

        public void SkipSpin()
        {
            if (!IsRolling)
            {
                return;
            }

            m_spinProgressTime = m_SpinDuration;
        }

        private IEnumerator VisualiseSpinRoutine(GachaRollResult rollResult)
        {
            m_currentImageSpinTime = 0.0f;
            m_shouldImageSpin = false;
            IsRolling = true;

            int RandomDamage = 0;
            m_spinProgressTime = 0.0f;

            while (m_spinProgressTime < m_SpinDuration)
            {
                // Figure out what to show
                SlimeAsset selectedSlimeAsset = m_GachaSystem.PickRandomSlime(m_GachaSystem.PickRandomRarity());
                m_ImageDisplay.texture = selectedSlimeAsset.GachaSprite.texture;
                m_ImageDisplay.color = Color.white;
                m_SlimeRank.text = $"{selectedSlimeAsset.Rarity}";
                m_SlimeType.text = $"{selectedSlimeAsset.Name}";
                //m_SlimeDamageModifier.text = $"+{RandomDamage}";

                // Figure out how long to show it
                float spinProgress = m_spinProgressTime / m_SpinDuration;
                float timeToShowSlime = (1 - m_SpinSpeedCurve.Evaluate(spinProgress)) * m_SpinSpeed;

                yield return new WaitForSecondsRealtime(timeToShowSlime);

                m_ImageDisplay.texture = null;
                m_ImageDisplay.color = Color.clear;
                yield return new WaitForSecondsRealtime(0.05f);

                m_spinProgressTime += timeToShowSlime + 0.05f;
            }

            m_ImageDisplay.texture = rollResult.SelectedSlime.GachaSprite.texture;
            m_ImageDisplay.color = Color.white;
            m_SlimeRank.text = $"{rollResult.SelectedSlime.Rarity}";
            m_SlimeType.text = $"{rollResult.SelectedSlime.Name}";
            //m_SlimeDamageModifier.text = $"+{RandomDamage}";

            SpinVisualComplete?.Invoke(rollResult);

            int particles = (int)rollResult.SelectedSlime.Rarity * 10;

            m_FanfareParticleSystem.Emit(particles);

            IsRolling = false;

            StartCoroutine(SpinImage());
        }

        private IEnumerator SpinImage()
        {
            m_shouldImageSpin = true;

            while(m_currentImageSpinTime < m_imageSpinTime && m_shouldImageSpin)
            {
                m_currentImageSpinTime += Time.deltaTime;

                float alpha = m_currentImageSpinTime / m_imageSpinTime;

                float movementAlpha = alpha * 2.0f;
                if (movementAlpha > 1.0f)
                {
                    movementAlpha = 1.0f - (movementAlpha - 1.0f);
                }

                m_ImageDisplay.transform.position = m_imagePosition + Vector2.up * movementAlpha * m_imageSpinHeight;
                m_ImageDisplay.transform.rotation = Quaternion.Euler(0.0f, 0.0f, alpha * 360.0f);

                yield return 0;
            }

            m_ImageDisplay.transform.position = m_imagePosition;
            m_ImageDisplay.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }
}
