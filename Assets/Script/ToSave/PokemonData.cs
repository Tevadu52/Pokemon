using UnityEngine;

[System.Serializable]
public class PokemonData
{
    public string namePokemon;
    public PokemonSpecieData specie;
    public int Level;
    public NatureData Nature;
    public float XP;
    public int CurrentHp;

    public Stats IVs;
    public Stats EVs;
}
