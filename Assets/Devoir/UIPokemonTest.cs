using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPokemonTest : MonoBehaviour
{
    [SerializeField]
    private Text pokemonNameText;
    [SerializeField]
    private Text pokemonLevelText;
    [SerializeField]
    private Text pokemonLifeText;
    [SerializeField]
    private Text pokemonTypeText;
    [SerializeField]
    private Image pokemonSprite;

    public static UIPokemonTest instance;

    public PokemonDevoir pokemon2;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameManager dans la scène");
            return;
        }
        instance = this;
    }

    public void ChangePokemonSprite(Sprite sprite)
    {
        pokemonSprite.sprite = sprite;
    }

    public void ChangeNameText(string text)
    {
        pokemonNameText.text = text;
    }
    public void ChangeLevelText(int level)
    {
        pokemonLevelText.text = level.ToString();
    }

    public void ChangeLifeText(int currentLife, int totalLife)
    {
        if (currentLife < 0) { currentLife = 0; }
        pokemonLifeText.text = currentLife + " / " + totalLife;
    }

    public void ChangeTypeText(string text) 
    {  
        pokemonTypeText.text = text;
    }

    public void ChangeUIPokemon( PokemonDevoir pokemon2)
    {
        ChangePokemonSprite(pokemon2.sprite);
        ChangeNameText(pokemon2.pokemonName);
        ChangeLifeText(pokemon2.getCurrentLife(), pokemon2.baseLife);
        ChangeTypeText(pokemon2.type.ToString());
    }
}
