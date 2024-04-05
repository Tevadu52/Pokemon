using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CompetenceData;

[System.Serializable]
public class CompetenceLearn
{
    [SerializeField]
    private CompetenceData competenceData;
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
    public CompetenceData CompetenceData { get { return competenceData; } set { competenceData = value; } }
    public LearnMethod LearnBy { get { return learnMethod; } set { learnMethod = value; } }
    public int LevelRestriction { get { return levelRestriction; } set { levelRestriction = value; } }
}
