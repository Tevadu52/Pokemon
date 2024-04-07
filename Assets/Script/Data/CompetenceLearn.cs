using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CompetenceData;

[System.Serializable]
public class CompetenceLearn
{
    [SerializeField]
    private int id;
    public enum LearnMethod
    {
        Egg,
        LevelUp,
        Machine,
    }
    [SerializeField]
    private LearnMethod learnMethod;
    [SerializeField]
    private int levelRestriction;
    public int IdCompetence { get { return id; } set { id = value; } }
    public LearnMethod LearnBy { get { return learnMethod; } set { learnMethod = value; } }
    public int LevelRestriction { get { return levelRestriction; } set { levelRestriction = value; } }
}
