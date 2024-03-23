using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PokemonSpecieData
{
    [Header("Base")]
    [SerializeField]
    private string nameSpecie;
    [SerializeField]
    private int id;
    [SerializeField]
    private List<Element> elements;
    [SerializeField]
    private Sprite sprite;

    [System.Serializable]
    public struct Stats
    {
        public int baseHp;
        public int baseAttack;
        public int baseDefense;
        public int baseAttackSpe;
        public int baseDefenseSpe;
        public int baseSpeed;
        public int baseXP;
    }

    [Header("Stats")]
    [SerializeField]
    private Stats stat;
    [SerializeField]
    private int captureTaux;

    [System.Serializable]
    public struct Infos
    {
        public string poids;
        public string taille;
        public string description;
    }

    [Header("Info")]
    [SerializeField]
    private Infos info;

    public string NameSpecie { get { return nameSpecie; } }
    public int Id { get { return id; } }
    public Stats Stat { get { return stat; } }
    public List<Element> Elements { get { return elements; } }
    public Sprite Sprite { get { return sprite; } }
    public Infos Info { get { return info; } }
    public int CaptureTaux { get { return captureTaux; } }
}

