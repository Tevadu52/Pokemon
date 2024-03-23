using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NatureData
{
    public string name;
    public float attack = 1.0f;
    public float defense = 1.0f;
    public float speed = 1.0f;
    public float attackSpe = 1.0f;
    public float defenseSpe = 1.0f;
    public float health = 1.0f;

    public NatureData() { health = 1f; attack = 1f; defense = 1f; speed = 1f; }
}
