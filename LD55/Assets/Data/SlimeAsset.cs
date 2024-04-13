using Gacha;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slime", menuName = "ScriptableObjects/Slime", order = 1)]
public class SlimeAsset : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField] public float DamageMin;
    [field: SerializeField] public float DamageMax;

    [field: SerializeField] public float Speed;
    [field: SerializeField] public float Mass;
    [field: SerializeField] public float Radius;

    [field: SerializeField] public Rarity Rarity;

    [field: SerializeField] public Sprite Sprite;

    public override string ToString()
    {
        // Add as we care about stuff
        return $"{Name}, Rarity: {Rarity}";
    }
}
