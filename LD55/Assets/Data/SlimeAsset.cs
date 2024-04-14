using Gacha;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slime", menuName = "ScriptableObjects/Slime", order = 1)]
public class SlimeAsset : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Rarity Rarity { get; private set; }
    [field: SerializeField] public Summon Prefab { get; private set; }

    [field: SerializeField] public Sprite GachaSprite { get; private set; }

    public override string ToString()
    {
        // Add as we care about stuff
        return $"{Name}, Rarity: {Rarity}";
    }
}
