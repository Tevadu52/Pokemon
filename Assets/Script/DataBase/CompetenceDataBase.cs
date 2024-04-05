using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Linq;

[CreateAssetMenu(fileName = "CompetenceDataBase", menuName = "My Game/CompetenceDataBase")]
public class CompetenceDataBase : BaseData
{
    [SerializeField]
    private List<CompetenceData> dataBase;

    public List<CompetenceData> DataBase
    {
        get { return dataBase; }
        set { dataBase = value; }
    }

    public CompetenceData GetCompetenceData(string name)
    {
        foreach (CompetenceData data in dataBase)
        {
            if (data.NameCompetence == name) return data;
        }
        return null;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CompetenceDataBase))]
public class CompetenceDataBase_selfEditor : Editor
{
    float minValLevel = 1;
    float maxValLevel = 919;

    public string URL = "https://pokeapi.co/api/v2/move/";

    public IEnumerator FetchData(List<CompetenceData> dataBase)
    {
        dataBase.Clear();
        for (int i = Mathf.FloorToInt(minValLevel); i < Mathf.FloorToInt(maxValLevel) + 1; i++)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(URL + i))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    JObject rss = JObject.Parse(request.downloadHandler.text);
                    PasteData(rss, dataBase);
                }
            }
        }
    }

    public void PasteData(JObject rss, List<CompetenceData> dataBase)
    {
        CompetenceData data = new CompetenceData();
        data.NameCompetence = (string)rss["name"];
        data.NameCompetenceFR = (string)rss["names"][findFrench(rss, "names")]["name"];
        data.Id = (int)rss["id"];
        if ((string)rss["accuracy"] != null)
        {
            data.Accuracy = (int)rss["accuracy"];
        }
        data.Pp = (int)rss["pp"];
        if((string)rss["power"] != null)
        {
            data.Power = (int)rss["power"];
        }
        data.Element = Resources.Load<ElementData>("Types/" + (string)rss["type"]["name"]);
        if (rss["flavor_text_entries"].Count<object>() != 0)
        {
            data.Description = (string)rss["flavor_text_entries"][findFrench(rss, "flavor_text_entries")]["flavor_text"];
        }
        dataBase.Add(data);
    }

    public int findFrench(JObject rss, string rssClass)
    {
        int i = 0;
        while (i < rss[rssClass].Count<object>())
        {
            if ((string)rss[rssClass][i]["language"]["name"] == "fr")
            {
                return i;
            }
            i++;
        }
        return 0;
    }

    public override void OnInspectorGUI()
    {
        CompetenceDataBase competenceDataBase = (CompetenceDataBase)target;

        DrawDefaultInspector();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("---------Setup---------");

        EditorGUILayout.LabelField("First competence : " + Mathf.FloorToInt(minValLevel).ToString());
        EditorGUILayout.LabelField("Last competence : " + Mathf.FloorToInt(maxValLevel).ToString());
        EditorGUILayout.MinMaxSlider("Range", ref minValLevel, ref maxValLevel, 1, 919);

        if (GUILayout.Button("AddCompetence"))
        {
            EditorCoroutineUtility.StartCoroutine(FetchData(competenceDataBase.DataBase), competenceDataBase);
        }
    }
}
#endif
