using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Provides the Gacha panel functionality
 *  
 *  Something on the gameplay side will open this, set the RollParams if necessary,
 *  subscribe to RollComplete and then do what they want with the result.
 */

namespace Gacha
{
    public class GachaPanel : MonoBehaviour
    {
        private IGachaSystem m_GachaSystem;
        private IRunResources m_RunResources;
        private GachaSpinVisual m_SpinVisual;

        private GachaRollParams m_RollParams;
        private int m_RollsRemainingWithCustomParams;

        public event Action RollStarted;
        public event Action<GachaRollResult> RollComplete;
        public bool IsRolling { get; private set; } = false;

        private void Awake()
        {
            // This should be top level
            m_GachaSystem = GetComponentInParent<IGachaSystem>();

            // This should be per run
            m_RunResources = GetComponentInParent<IRunResources>();

            m_SpinVisual = GetComponentInChildren<GachaSpinVisual>();

            // Setup a default roll params. Something can set their own for special panel uses via SetRollParams
            ResetRollParams();
        }

        /// <summary>
        /// Set some custom roll params for a set number of rolls, to be used to limit the free rolls to lower rarities
        /// so people can't reset over and over to get the better slimes
        /// </summary>
        public void SetRollParams(GachaRollParams rollParams, int rolls)
        {
            m_RollParams = rollParams;
            m_RollsRemainingWithCustomParams = rolls;
        }

        public void ResetRollParams()
        {
            m_RollParams = new GachaRollParams()
            {
                MinimumRarity = Rarity.C,
                MaximumRarity = Rarity.S,
            };
        }

        public void Spin()
        {
            if (IsRolling)
                return;

            // If we're in a test scene without one of these then ignore and do it free
            if (m_RunResources != null)
            {
                if (m_RunResources.SlimeTokens < m_GachaSystem.SlimeTokenCost)
                    return;

                m_RunResources.SpendTokens(m_GachaSystem.SlimeTokenCost);
            }

            GachaRollResult result = m_GachaSystem.RollGacha(m_RollParams);

            m_RollsRemainingWithCustomParams--;
            if (m_RollsRemainingWithCustomParams == 0)
                ResetRollParams();

            StartCoroutine(SpinRoutine(result));
        }

        private IEnumerator SpinRoutine(GachaRollResult rollResult)
        {
            RollStarted?.Invoke();
            IsRolling = true;

            m_SpinVisual.VisualiseSpin(rollResult);

            // Wait for the visual to finish
            // This isn't very good but idk how else to do it without thinking about it for a day
            while (m_SpinVisual.IsRolling)
                yield return null;

            RollComplete?.Invoke(rollResult);
            IsRolling = false;

            m_RunResources?.AddSlime(rollResult.SelectedSlime);

            Debug.Log($"Completed a Spin and got:{Environment.NewLine}{rollResult}");
        }
    }
}
