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

        public event Action SpinVisualComplete;

        private GachaSystem m_GachaSystem;
        private GachaPanel m_GachaPanel;

        private void Awake()
        {
            m_GachaSystem = GetComponentInParent<GachaSystem>();

            m_GachaPanel = GetComponentInParent<GachaPanel>();
            m_GachaPanel.RollStarted += OnSpinStarted;
        }

        private void OnSpinStarted(GachaRollResult rollResult)
        {
            // Who knows what this will do yet
            //m_GachaSystem.SlimeDatabase

            StartCoroutine(VisualiseSpin(rollResult));
        }

        private IEnumerator VisualiseSpin(GachaRollResult rollResult)
        {
            float spinProgressTime = 0;

            while (spinProgressTime < m_SpinDuration)
            {

                // Figure out what to show
                SlimeAsset selectedSlimeAsset = m_GachaSystem.PickRandomSlime(m_GachaSystem.PickRandomRarity());
                m_ImageDisplay.texture = selectedSlimeAsset.Sprite.texture;

                // Figure out how long to show it
                float spinProgress = spinProgressTime / m_SpinDuration;
                float timeToShowSlime = (1 - m_SpinSpeedCurve.Evaluate(spinProgress)) * m_SpinSpeed;

                yield return new WaitForSeconds(timeToShowSlime);

                m_ImageDisplay.texture = null;
                yield return new WaitForSeconds(0.1f);

                spinProgressTime += timeToShowSlime + 0.1f;
            }

            m_ImageDisplay.texture = rollResult.SelectedSlime.Sprite.texture;

            SpinVisualComplete?.Invoke();
        }
    }
}
