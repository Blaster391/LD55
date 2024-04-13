using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRunResources
{
    public event System.Action<int, int> SlimeTokensChanged;
    public int SlimeTokens { get; }
    public void AddTokens(int tokensToAdd);
    public void SpendTokens(int tokensToSpend);
}

public class RunResources : MonoBehaviour, IRunResources
{
    public event System.Action<int, int> SlimeTokensChanged;

    [SerializeField] private int m_SlimeTokens = 0;
    public int SlimeTokens
    {
        get => m_SlimeTokens;
        set
        {
            int oldTokens = m_SlimeTokens;
            m_SlimeTokens += value;

            // Use this to update the UI etc.
            SlimeTokensChanged?.Invoke(oldTokens, m_SlimeTokens);
        }
    }

    public void AddTokens(int tokensToAdd)
    {
        SlimeTokens += tokensToAdd;
    }

    public void SpendTokens(int tokensToSpend)
    {
        if (SlimeTokens < tokensToSpend)
            throw new System.Exception("No no sir you can't afford that");

        SlimeTokens -= tokensToSpend;
    }
}
