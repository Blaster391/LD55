using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Gacha
{
    /*
     *  Contains all of the core behaviours of the Gacha mechanic and classes that help provide those.
     *  This should sit at the top level of the scene's hierarchy
     */

    public enum Rarity
    {
        C, B, A, S // etc.
    }

    [System.Serializable]
    public struct RarityChance
    {
        public Rarity Rarity;
        public float Chance;
    }

    public struct GachaRollParams
    {
        public Rarity MinimumRarity;
        public Rarity MaximumRarity;
        // anything else we want
    }

    public struct GachaRollResult
    {
        public SlimeAsset SelectedSlime;

        public override string ToString()
        {
            return $"Slime: {SelectedSlime}";
        }
    }

    public interface IGachaSystem
    {
        public int SlimeTokenCost { get; }
        public GachaRollResult RollGacha(GachaRollParams rollParams);
    }

    public class GachaSystem : MonoBehaviour, IGachaSystem
    {
        private GachaConfig m_Config;

        public int SlimeTokenCost {get; private set;}

        private Dictionary<Rarity, List<SlimeAsset>> m_SlimeDatabase;

        public IReadOnlyDictionary<Rarity, IReadOnlyList<SlimeAsset>> SlimeDatabase
        {
            get
            {
                Dictionary<Rarity, IReadOnlyList<SlimeAsset>> dictWithReadOnlyList = new();
                foreach(Rarity rarity in m_SlimeDatabase.Keys)
                {
                    dictWithReadOnlyList.Add(rarity, m_SlimeDatabase[rarity]);
                }
                return dictWithReadOnlyList;
            }
        }

        public SlimeAsset PickRandomSlime(Rarity rarity)
        {
            List<SlimeAsset> slimesOfRarity = m_SlimeDatabase[rarity];
            return slimesOfRarity[Random.Range(0, slimesOfRarity.Count)];
        }

        public IReadOnlyDictionary<Rarity, float> RarityRollChances
        {
            get
            {
                Dictionary<Rarity, float> rarityRollChances = new();

                float totalChance = 0;
                foreach (RarityChance rarityChance in m_Config.RarityChances)
                {
                    totalChance += rarityChance.Chance;
                }

                foreach (RarityChance rarityChance in m_Config.RarityChances)
                {
                    rarityRollChances.Add(rarityChance.Rarity, rarityChance.Chance / totalChance);
                }

                return rarityRollChances;
            }
        }

        public Rarity PickRandomRarity()
        {
            IReadOnlyDictionary<Rarity, float> rarityRollChances = RarityRollChances;
            float randomPickValue = Random.Range(0f, 1f);

            foreach(Rarity rarity in System.Enum.GetValues(typeof(Rarity)))
            {
                if (randomPickValue < rarityRollChances[rarity])
                    return rarity;

                randomPickValue -= rarityRollChances[rarity];
            }

            return Rarity.C;
        }


        #region System
        private void Awake()
        {
            m_Config = Resources.LoadAll<GachaConfig>("Config")[0]; // Load on its own doesn't find it, idk why
            SlimeTokenCost = m_Config.SlimeTokenCost;

            // Populate our database
            var slimeAssets = Resources.LoadAll<SlimeAsset>("Slimes");
            m_SlimeDatabase = new();
            foreach (SlimeAsset slime in slimeAssets)
            {
                if (!m_SlimeDatabase.ContainsKey(slime.Rarity))
                {
                    m_SlimeDatabase.Add(slime.Rarity, new List<SlimeAsset>());
                }
                m_SlimeDatabase[slime.Rarity].Add(slime);
            }
        }
        #endregion

        public GachaRollResult RollGacha(GachaRollParams rollParams)
        {
            GachaRollResult result = new();

            // Select a rarity
            Rarity selectedRarity = PickRandomRarity();

            // Select a random slime of this rarity
            var possibleSlimes = m_SlimeDatabase[selectedRarity];
            result.SelectedSlime = possibleSlimes.ElementAt(Random.Range(0, possibleSlimes.Count));

            // Increase cost for next roll by some amount
            SlimeTokenCost += m_Config.SlimeTokenIncreasePerRoll;

            return result;
        }
    }
}
