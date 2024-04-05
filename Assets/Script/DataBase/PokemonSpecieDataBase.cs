using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Linq;

[CreateAssetMenu(fileName = "PokemonSpecieDataBase", menuName = "My Game/PokemonSpecieDataBase")]
public class PokemonSpecieDataBase : BaseData
{
    [SerializeField]
    private List<PokemonSpecieData> dataBase;

    public List<PokemonSpecieData> DataBase 
    {
        get { return dataBase; }
        set { dataBase = value; }
    }

    public PokemonSpecieData GetPokemonSpecieData(int id)
    {
        foreach (PokemonSpecieData data in dataBase)
        {
            if (data.Id == id) return data;
        }
        return null;
    }

    public PokemonSpecieData GetRandomPokemonSpecieData()
    {
        int i = Random.Range(0, dataBase.Count);
        return DataBase[i];
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PokemonSpecieDataBase))]
public class PokemonSpecieDataBase_selfEditor : Editor
{
    private bool isRunning;
    float minValLevel = 1;
    float maxValLevel = 1025;

    public string URL = "https://pokeapi.co/api/v2/pokemon/";
    public string URL_Species = "https://pokeapi.co/api/v2/pokemon-species/";

    public IEnumerator FetchData(List<PokemonSpecieData> dataBase)
    {
        isRunning = true;
        dataBase.Clear();
        for (int i = Mathf.FloorToInt(minValLevel); i < Mathf.FloorToInt(maxValLevel) + 1; i++)
        {
            if (isRunning)
            {
                using (UnityWebRequest request = UnityWebRequest.Get(URL + i))
                using (UnityWebRequest request_Species = UnityWebRequest.Get(URL_Species + i))
                {
                    yield return request.SendWebRequest();
                    yield return request_Species.SendWebRequest();
                    if (request.result == UnityWebRequest.Result.ConnectionError || request_Species.result == UnityWebRequest.Result.ConnectionError)
                    {
                        Debug.Log(request.error);
                        Debug.Log(request_Species.error);
                    }
                    else
                    {
                        JObject rss = JObject.Parse(request.downloadHandler.text);
                        JObject rss_Species = JObject.Parse(request_Species.downloadHandler.text);
                        yield return PasteData(rss, rss_Species, dataBase);
                    }
                }
            }
        }
        isRunning = false;
    }

    public IEnumerator PasteData(JObject rss, JObject rss_Species, List<PokemonSpecieData> dataBase)
    {
        
        PokemonSpecieData data = new PokemonSpecieData();
        data.Id = (int)rss["id"];
        if (Resources.Load<Sprite>("Sprites/Front" + data.Id) == null)
        {
            yield return GetThePng((string)rss["sprites"]["front_default"], "Front", data.Id);
        }
        else
        {
            data.FrontSprite = Resources.Load<Sprite>("Sprites/Front" + data.Id);
        }
        if (Resources.Load<Sprite>("Sprites/Back" + data.Id) == null)
        {
            if ((string)rss["sprites"]["back_default"] != null)
            {
                yield return GetThePng((string)rss["sprites"]["back_default"], "Back", data.Id);
            }
            else
            {
                data.BackSprite = Resources.Load<Sprite>("Sprites/Front" + data.Id);
            }
        }
        else
        {
            data.BackSprite = Resources.Load<Sprite>("Sprites/Back" + data.Id);
        }
        data.NameSpecie = (string)rss_Species["names"][findFrench(rss_Species, "names")]["name"];
        data.CaptureTaux = (int)rss_Species["capture_rate"];
        data.BaseXP = (int)rss["base_experience"];
        List<ElementData> newElements = new List<ElementData>();
        int i = 0;
        while (i < rss["types"].Count<object>())
        {
            newElements.Add(Resources.Load<ElementData>("Types/" + (string)rss["types"][i]["type"]["name"]));
            i++;
        }
        data.Elements = newElements;
        Stats newStats = new Stats();
        newStats.Hp = (int)rss["stats"][0]["base_stat"];
        newStats.Attack = (int)rss["stats"][1]["base_stat"];
        newStats.Defense = (int)rss["stats"][2]["base_stat"];
        newStats.AttackSpe = (int)rss["stats"][3]["base_stat"];
        newStats.DefenseSpe = (int)rss["stats"][4]["base_stat"];
        newStats.Speed = (int)rss["stats"][5]["base_stat"];
        data.Stats = newStats;
        if(rss_Species["flavor_text_entries"].Count<object>() != 0)
        {
            data.Description = (string)rss_Species["flavor_text_entries"][findFrench(rss_Species, "flavor_text_entries")]["flavor_text"];
        }
        data.Taille = (float)rss["height"] /10 + " m";
        data.Poids = (float)rss["weight"] /10 + " kg";
        List<CompetenceLearn> newCompetencesLearn = new List<CompetenceLearn>();
        i = 0;
        while (i < rss["moves"].Count<object>())
        {
            CompetenceLearn competenceLearn = new CompetenceLearn();
            competenceLearn.CompetenceData = Resources.Load<CompetenceDataBase>("DataBase/CompetenceDataBase").GetCompetenceData((string)rss["moves"][i]["move"]["name"]);
            competenceLearn.LevelRestriction = (int)rss["moves"][i]["version_group_details"][(rss["moves"][i]["version_group_details"].Count<object>() - 1)]["level_learned_at"];
            string learnby = (string)rss["moves"][i]["version_group_details"][rss["moves"][i]["version_group_details"].Count<object>() - 1]["move_learn_method"]["name"];
            if(learnby == "egg")
            {
                competenceLearn.LearnBy = CompetenceLearn.LearnMethod.Egg;
            }
            else if (learnby == "level-up")
            {
                competenceLearn.LearnBy = CompetenceLearn.LearnMethod.LevelUp;
            }
            else if (learnby == "machine")
            {
                competenceLearn.LearnBy = CompetenceLearn.LearnMethod.Machine;
            }
            newCompetencesLearn.Add(competenceLearn);
            i++;
        }
        data.CompetenceLearnable = newCompetencesLearn;
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

    public  IEnumerator GetThePng(string url, string path, int id)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest(); 
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
                yield return new WaitForSeconds(.1f);
            }
            else
            {
                string savePath = Application.dataPath + "/Resources/Sprites/" + path + id.ToString() + ".png";
                System.IO.File.WriteAllBytes(savePath, www.downloadHandler.data);
            }
        }
    }
    public override void OnInspectorGUI()
    {
        PokemonSpecieDataBase pokemonSpecieDataBase = (PokemonSpecieDataBase)target;

        DrawDefaultInspector();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("---------Setup---------");

        EditorGUILayout.LabelField("First Pokemon : " + Mathf.FloorToInt(minValLevel).ToString());
        EditorGUILayout.LabelField("Last Pokemon : " + Mathf.FloorToInt(maxValLevel).ToString());
        EditorGUILayout.MinMaxSlider("Range", ref minValLevel, ref maxValLevel, 1, 1025);

        if (GUILayout.Button("AddPokemons") && !isRunning)
        {
            EditorCoroutineUtility.StartCoroutine(FetchData(pokemonSpecieDataBase.DataBase), pokemonSpecieDataBase);
        }
        if (GUILayout.Button("Stop") && isRunning)
        {
            isRunning = false;
        }
    }
}
#endif
