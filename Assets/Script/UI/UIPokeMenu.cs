using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPokeMenu : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField]
    private Text pokemonNameMenuText;
    [SerializeField]
    private Text pokemonLevelMenuText;
    [SerializeField]
    private Text pokemonLifeMenuText;
    [SerializeField]
    private Text pokemonTypeMenuText;
    [SerializeField]
    private Text pokemonPoidsMenuText;
    [SerializeField]
    private Text pokemonTailleMenuText;
    [SerializeField]
    private Text pokemonDescriptionText;
    [SerializeField]
    private Image pokemonMenuSprite;

    private void ChangePokemonMenuSprite(Sprite sprite)
    {
        pokemonMenuSprite.sprite = sprite;
    }
    private void ChangeNameMenuText(string text)
    {
        pokemonNameMenuText.text = text;
    }
    private void ChangeLevelMenuText(int level)
    {
        pokemonLevelMenuText.text = "LV:" + level.ToString();
    }
    private void ChangeLifeMenuText(int currentLife, int totalLife)
    {
        if (currentLife < 0) { currentLife = 0; }
        pokemonLifeMenuText.text = currentLife + " / " + totalLife + " PV" ;
    }
    private void ChangeTypeMenuText(List<Element> elements)
    {
        pokemonTypeMenuText.text = "Type : " ;
        for (int i = 0; i < elements.Count; i++)
        {
            pokemonTypeMenuText.text += elements[i];
            if (i != elements.Count - 1)
            {
                pokemonTypeMenuText.text += " / ";
            }
        }
    }
    private void ChangePoidsMenuText(string poids)
    {
        pokemonPoidsMenuText.text = "Poids : " + poids;
    }
    private void ChangeTailleMenuText(string taille)
    {
        pokemonTailleMenuText.text = "Taille : " + taille;
    }
    private void ChangeDescriptionMenuText(string text)
    {
        pokemonDescriptionText.text = "Description : \n" + text;
    }
    public void ChangeUIPokemonMenu(Pokemon pokemon)
    {
        ChangePokemonMenuSprite(pokemon.getSprite());
        ChangeNameMenuText(pokemon.getName());
        ChangeLevelMenuText(pokemon.getLevel());
        ChangeLifeMenuText(pokemon.getCurrentHp(), pokemon.getHp());
        ChangeTypeMenuText(pokemon.getElements());
        ChangePoidsMenuText(pokemon.getSpecie().Info.poids);
        ChangeTailleMenuText(pokemon.getSpecie().Info.taille);
        ChangeDescriptionMenuText(pokemon.getSpecie().Info.description);
    }
}
