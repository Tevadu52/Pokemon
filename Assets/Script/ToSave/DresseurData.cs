using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DresseurData
{ 
    public string nameDresseur;
    public int money;
    public int badge;
    public List<PokemonData> team = new List<PokemonData>();
    public List<PokemonData> currentTeam = new List<PokemonData>();
}
