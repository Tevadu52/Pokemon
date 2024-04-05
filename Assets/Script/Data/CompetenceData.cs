using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompetenceData
{
    [SerializeField]
    private string nameCompetenceFR;
    [SerializeField]
    private string nameCompetence;
    [SerializeField]
    private int id;
    [SerializeField]
    private int power;
    [SerializeField]
    private int pp;
    [SerializeField]
    private int accuracy;
    [SerializeField]
    private ElementData element;
    [SerializeField]
    private string description;

    public int Id {  get { return id; } set { id = value; } }
    public int Power { get { return power; } set { power = value; } }
    public int Pp { get { return pp; } set { pp = value; } }
    public int Accuracy { get { return accuracy; } set { accuracy = value; } }
    public ElementData Element { get { return element; } set { element = value; } }
    public string Description { get { return description; } set { description = value; } }
    public string NameCompetence { get { return nameCompetence; } set { nameCompetence = value; } }
    public string NameCompetenceFR { get { return nameCompetenceFR; } set { nameCompetenceFR = value; } }
}
