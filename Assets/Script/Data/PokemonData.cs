using UnityEngine;

[System.Serializable]
public class PokemonData
{
    public string namePokemon;
    public PokemonSpecieData specie;
    public int Level;
    public float XP;
    private int currentHp;
    public int CurrentHp {  get { return currentHp; } set {  currentHp = value; } }
}
