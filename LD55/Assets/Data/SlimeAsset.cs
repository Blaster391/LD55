using Gacha;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slime", menuName = "ScriptableObjects/Slime", order = 1)]
public class SlimeAsset : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField] public float DamageMin { get; private set; }
    [field: SerializeField] public float DamageMax { get; private set; }

    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float Mass { get; private set; }
    [field: SerializeField] public float Radius { get; private set; }

    [field: SerializeField] public Rarity Rarity { get; private set; }

    [field: SerializeField] public List<Sprite> SpriteList { get; private set; }
    public override string ToString()
    {
        // Add as we care about stuff
        return $"{Name}, Rarity: {Rarity}";
    }
}
