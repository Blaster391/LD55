using Gacha;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaConfig", menuName = "ScriptableObjects/GachaConfig", order = 1)]
public class GachaConfig : ScriptableObject
{
    [field: SerializeField] public List<RarityChance> RarityChances { get; private set; }

    [field: SerializeField] public int SlimeTokenCost { get; set; }

    [field: SerializeField] public int SlimeTokenIncreasePerRoll { get; set; }
}
