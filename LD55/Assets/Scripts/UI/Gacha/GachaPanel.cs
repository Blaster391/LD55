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

        private GachaRollParams m_RollParams;

        public event Action<GachaRollResult> RollStarted;
        public event Action<GachaRollResult> RollComplete;

        private void Awake()
        {
            // This should be top level
            m_GachaSystem = GetComponentInParent<IGachaSystem>();

            // This should be per run
            m_RunResources = GetComponentInParent<IRunResources>();

            // Setup a default roll params. Something can set their own for special panel uses via SetRollParams
            m_RollParams = new GachaRollParams()
            {
                MinimumRarity = Rarity.D,
                MaximumRarity = Rarity.S,
            };
        }

        public void SetRollParams(GachaRollParams rollParams)
        {
            m_RollParams = rollParams;
        }

        public void Spin()
        {
            if (m_RunResources.SlimeTokens < m_GachaSystem.SlimeTokenCost)
                return;

            m_RunResources.SpendTokens(m_GachaSystem.SlimeTokenCost);

            GachaRollResult result = m_GachaSystem.RollGacha(m_RollParams);

            // We want to fire something to start the roll animation maybe?
            RollStarted?.Invoke(result);
            Debug.Log("Started a Spin...");

            // Then maybe delay until we hear that the roll animation is done? Do we want to rely on that??
            RollComplete?.Invoke(result);
            Debug.Log($"Completed a Spin and got:{Environment.NewLine}{result}");
        }
    }
}
