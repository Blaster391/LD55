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
        public bool IsRolling { get; private set; } = false;

        private void Awake()
        {
            m_GachaSystem = GetComponentInParent<GachaSystem>();
        }

        public void VisualiseSpin(GachaRollResult rollResult)
        {
            StartCoroutine(VisualiseSpinRoutine(rollResult));
        }

        private IEnumerator VisualiseSpinRoutine(GachaRollResult rollResult)
        {
            IsRolling = true;

            float spinProgressTime = 0;

            while (spinProgressTime < m_SpinDuration)
            {
                // Figure out what to show
                SlimeAsset selectedSlimeAsset = m_GachaSystem.PickRandomSlime(m_GachaSystem.PickRandomRarity());
                m_ImageDisplay.texture = selectedSlimeAsset.SpriteList[0].texture;

                // Figure out how long to show it
                float spinProgress = spinProgressTime / m_SpinDuration;
                float timeToShowSlime = (1 - m_SpinSpeedCurve.Evaluate(spinProgress)) * m_SpinSpeed;

                yield return new WaitForSeconds(timeToShowSlime);

                m_ImageDisplay.texture = null;
                yield return new WaitForSeconds(0.1f);

                spinProgressTime += timeToShowSlime + 0.1f;
            }

            m_ImageDisplay.texture = rollResult.SelectedSlime.SpriteList[0].texture;

            SpinVisualComplete?.Invoke();

            IsRolling = false;
        }
    }
}
