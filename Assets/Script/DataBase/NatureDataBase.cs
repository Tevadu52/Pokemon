using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NatureDataBase", menuName = "My Game/NatureDataBase")]
public class NatureDataBase : ScriptableObject
{
    [SerializeField]
    private List<NatureData> dataBase;

    public NatureData GetPokemonSpecieData(int i)
    {
        return dataBase[i];
    }

    public NatureData GetRandomPokemonNatureData()
    {
        int i = Random.Range(0, dataBase.Count);
        return dataBase[i];
    }
}