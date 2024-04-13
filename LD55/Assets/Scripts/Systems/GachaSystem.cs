using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gacha
{
    /*
     *  Contains all of the core behaviours of the Gacha mechanic and classes that help provide those.
     *  This should sit at the top level of the scene's hierarchy
     */

    public enum Rarity
    {
        D, C, B, A, S // etc.
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
        // The slime - this could contain the rarity itself
        // Any stat shifts 0 -> 1 
        public float DamageModifer;

        public override string ToString()
        {
            return $"Slime: {SelectedSlime}" + System.Environment.NewLine
                + $"Damage Modifier: {DamageModifer}";
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

        public int SlimeTokenCost => m_Config.SlimeTokenCost;

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

            return Rarity.D;
        }


        #region System
        private void Awake()
        {
            m_Config = Resources.FindObjectsOfTypeAll<GachaConfig>().FirstOrDefault();

            // Populate our database
            var slimeAssets = Resources.FindObjectsOfTypeAll<SlimeAsset>();
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

        private void OnDestroy()
        {
            m_SlimeDatabase.Clear();
        }
        #endregion

        public GachaRollResult RollGacha(GachaRollParams rollParams)
        {
            GachaRollResult result = new();

            // Select a rarity
            Rarity selectedRarity = (Rarity)Random.Range((int)rollParams.MinimumRarity, (int)rollParams.MaximumRarity);

            // Select a random slime of this rarity
            var possibleSlimes = m_SlimeDatabase[selectedRarity];
            result.SelectedSlime = possibleSlimes.ElementAt(Random.Range(0, possibleSlimes.Count));

            // Other stuff
            result.DamageModifer = m_Config.StatRollCurve.Evaluate(Random.Range(0f, 1f));

            return result;
        }
    }
}
