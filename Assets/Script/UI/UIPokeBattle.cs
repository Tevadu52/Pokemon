using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPokeBattle : MonoBehaviour
{

    [Header("Combat")]
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

    [Header("CombatAdverse")]
    [SerializeField]
    private Text pokemonNameAdverseText;
    [SerializeField]
    private Text pokemonLevelAdverseText;
    [SerializeField]
    private Text pokemonLifeAdverseText;
    [SerializeField]
    private Text pokemonTypeAdverseText;
    [SerializeField]
    private Image pokemonAdverseSprite;

    [SerializeField]
    private Text combatText;
    [SerializeField]
    private Image combatBackground;
    public static float TextTime = 1f;


    private void ChangePokemonSprite(Sprite sprite, bool adverse = false)
    {
        if (adverse) {pokemonAdverseSprite.sprite = sprite;}
        else {pokemonSprite.sprite = sprite;}
    }
    private void ChangeNameText(string text, bool adverse = false)
    {
        if (adverse) { pokemonNameAdverseText.text = text; }
        else { pokemonNameText.text = text;}
    }
    private void ChangeLevelText(int level, bool adverse = false)
    {
        if (adverse) { pokemonLevelAdverseText.text = "LV:" + level.ToString(); }
        else { pokemonLevelText.text = "LV:" + level.ToString();}
    }
    private void ChangeLifeText(int currentLife, int totalLife, bool adverse = false)
    {
        if (currentLife < 0) { currentLife = 0; }
        if (adverse) { pokemonLifeAdverseText.text = currentLife + " / " + totalLife + " PV"; }
        else { pokemonLifeText.text = currentLife + " / " + totalLife + " PV"; }
    }
    private void ChangeTypeText(List<Element> elements, bool adverse = false)
    {
        if (adverse) 
        { 
            pokemonTypeAdverseText.text = "";
            for (int i = 0; i < elements.Count; i++)
            {
                pokemonTypeAdverseText.text += elements[i];
                if (i != elements.Count - 1)
                {
                    pokemonTypeAdverseText.text +=  " / ";
                }
            }
        }
        else 
        { 
            pokemonTypeText.text = "";
            for (int i = 0; i < elements.Count; i++)
            {
                pokemonTypeText.text += elements[i];
                if (i != elements.Count - 1)
                {
                    pokemonTypeText.text += " / ";
                }
            }
        }
    }

    public void ChangeCombatText(string text)
    {
        combatText.text = text;
    }
    public int ChooseCombatBackground()
    {
        return Random.Range(1, (Resources.LoadAll<Sprite>("Backgrounds/").Length + 1));
    }
    public void ChangeCombatBackground(int nb)
    {
        combatBackground.sprite = Resources.Load<Sprite>("Backgrounds/Battle Backgrounds " + nb);
    }

    public void ChangeUIPokemon(Pokemon pokemon1, Pokemon pokemon2)
    {
        ChangeUIPokemonHimself(pokemon1);
        ChangeUIPokemonAdverse(pokemon2);
    }
    public void ChangeUIPokemonHimself(Pokemon pokemon1)
    {
        ChangePokemonSprite(pokemon1.getSprite());
        ChangeNameText(pokemon1.getName());
        ChangeLevelText(pokemon1.getLevel());
        ChangeLifeText(pokemon1.getCurrentHp(), pokemon1.getHp());
        ChangeTypeText(pokemon1.getElements());
    }
    public void ChangeUIPokemonAdverse(Pokemon pokemon2)
    {
        ChangePokemonSprite(pokemon2.getSprite(), true);
        ChangeNameText(pokemon2.getName(), true);
        ChangeLevelText(pokemon2.getLevel(), true);
        ChangeLifeText(pokemon2.getCurrentHp(), pokemon2.getHp(), true);
        ChangeTypeText(pokemon2.getElements(), true);
    }
}
