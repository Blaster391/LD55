using Gacha;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaSpinCompleteEffects : MonoBehaviour
{
    private GachaSpinVisual m_SpinVisual;
    [SerializeField]
    private ParticleSystem m_FanfareParticleSystem;

    private void Awake()
    {
        m_SpinVisual = GetComponentInParent<GachaSpinVisual>();
        m_SpinVisual.SpinVisualComplete += OnSpinVisualComplete;

        m_FanfareParticleSystem = GetComponent<ParticleSystem>();
    }

    private void OnSpinVisualComplete(GachaRollResult rollResult)
    {
        int particles = (int)rollResult.SelectedSlime.Rarity * 10;

        m_FanfareParticleSystem.Emit(particles);
    }
}
