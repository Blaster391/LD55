using System.Collections.Generic;
using UnityEngine;

public interface IRunResources
{
    // Tokens
    public event System.Action<int, int> SlimeTokensChanged;
    public int SlimeTokens { get; }
    public void AddTokens(int tokensToAdd);
    public void SpendTokens(int tokensToSpend);

    // Slimes
    public event System.Action<SlimeAsset> SlimeAdded;
    public void AddSlime(SlimeAsset slimeAsset);
    public List<SlimeAsset> SlimeInventory { get; }
}

public class RunResources : MonoBehaviour, IRunResources
{
    #region Tokens
    public event System.Action<int, int> SlimeTokensChanged;

    [SerializeField] private int m_SlimeTokens = 0;
    public int SlimeTokens
    {
        get => m_SlimeTokens;
        set
        {
            int oldTokens = m_SlimeTokens;
            m_SlimeTokens = value;

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
    #endregion

    #region Slimes
    public event System.Action<SlimeAsset> SlimeAdded;

    public void AddSlime(SlimeAsset slimeAsset)
    {
        SlimeInventory.Add(slimeAsset);
        SlimeAdded?.Invoke(slimeAsset);
    }

    public List<SlimeAsset> SlimeInventory { get; private set; }
    #endregion


    private void Awake()
    {
        SlimeInventory = new List<SlimeAsset>();
    }
}
