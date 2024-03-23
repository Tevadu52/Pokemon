using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokemonSpecieDataBase", menuName = "My Game/PokemonSpecieDataBase")]
public class PokemonSpecieDataBase : ScriptableObject
{
    [SerializeField]
    private List<PokemonSpecieData> dataBase;

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
        return dataBase[i];
    }
}