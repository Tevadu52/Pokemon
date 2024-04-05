using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int Hp;
    public int Attack;
    public int Defense;
    public int AttackSpe;
    public int DefenseSpe;
    public int Speed;
}

[System.Serializable]
public class PokemonSpecieData
{
    [Header("Base")]
    [SerializeField]
    private string nameSpecie;
    [SerializeField]
    private int id;
    [SerializeField]
    private List<ElementData> elements;
    [SerializeField]
    private Sprite frontSprite;
    [SerializeField]
    private Sprite backSprite;


    [Header("Stats")]
    [SerializeField]
    private Stats baseStats;
    [SerializeField]
    private int baseXP;
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

    [Header("Competence")]
    [SerializeField]
    private List<CompetenceLearn> competenceLearnable;

    public string NameSpecie { get { return nameSpecie; } set { nameSpecie = value; } }
    public int Id { get { return id; } set { id = value; } }
    public Stats Stats { get { return baseStats; } set { baseStats = value; } }
    public List<ElementData> Elements { get { return elements; } set { elements = value; } }
    public Sprite FrontSprite { get { return frontSprite; } set { frontSprite = value; } }
    public Sprite BackSprite { get { return backSprite; } set { backSprite = value; } }
    public string Poids { get { return info.poids; } set { info.poids = value; } }
    public string Taille { get { return info.taille; } set { info.taille = value; } }
    public string Description { get { return info.description; } set { info.description = value; } }
    public int CaptureTaux { get { return captureTaux; } set { captureTaux = value; } }
    public int BaseXP { get { return baseXP; } set { baseXP = value; } }
    public List<CompetenceLearn> CompetenceLearnable { get { return competenceLearnable; } set { competenceLearnable = value; } }
}

