using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemStats
{
    public float damage;
    public float defense;
    public float weight;

    // Constructor (optioneel maar handig)
    public ItemStats(float damage, float defense, float weight)
    {
        this.damage = damage;
        this.defense = defense;
        this.weight = weight;
    }
}
