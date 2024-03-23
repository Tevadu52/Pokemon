using UnityEngine;
using System.Linq;

public class PokemonDevoir : MonoBehaviour
{
    public string pokemonName;
    public enum Type
    {
        Electric,
        Ground,
        Ice,
        Steel,
        Flying,
    }
    public Type type;
    public int baseLife;
    private int currentLife;
    public int attack;
    public int defense;
    private int statsPoints;
    public float weight;
    public Type[] weaknessesTypes;
    public Type[] resistancesTypes;

    private float startTime;
    public float attackTimeReload = 2.5f;

    public Sprite sprite;

    private void Awake()
    {
        InitCurrentLife();
        InitStatsPoints();
    }

    void Start()
    {
        DisplayName();
        DisplayType();
        DisplayCurrentLife();
        DisplayAttack();
        DisplayDefense();
        DisplayStats();
        DisplayWeight();
        DisplayWeaknesses();
        DisplayResistances();
        takeDamage(10, Type.Ice);
        startTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        if (Time.realtimeSinceStartup >= startTime + attackTimeReload && CheckIfPokemonAlive())
        {
            startTime = Time.realtimeSinceStartup;
            int a = Random.Range(0, sizeof(Type));
            takeDamage(Random.Range(0, 10), (Type)a);
        }
    }

    private void DisplayName()
    {
        Debug.Log("Pokemon name is " + pokemonName);
    }
    private void DisplayType()
    {
        Debug.Log("Pokemon type is " + type);
    }
    private void DisplayCurrentLife()
    {
        Debug.Log("Pokemon current life is " + currentLife + " points");
    }
    private void DisplayAttack()
    {
        Debug.Log("Pokemon attack is " + attack + " points");
    }
    private void DisplayDefense()
    {
        Debug.Log("Pokemon defense is " + defense + " points");
    }
    private void DisplayStats()
    {
        Debug.Log("Pokemon stats is " + statsPoints + " points");
    }
    private void DisplayWeight()
    {
        Debug.Log("Pokemon weight is " + weight + " kg");
    }
    private void DisplayWeaknesses()
    {
        foreach (Type _type in weaknessesTypes)
        {
            Debug.Log("Pokemon is weak against type : " + _type);
        }
    }
    private void DisplayResistances()
    {
        foreach (Type _type in resistancesTypes)
        {
            Debug.Log("Pokemon is durable against type : " + _type);
        }
    }

    private void InitCurrentLife()
    {
        currentLife = baseLife;
    }
    private void InitStatsPoints()
    {
        statsPoints = attack + defense + baseLife;
    }
    private int GetAttackDamage()
    {
        return attack;
    }

    public void takeDamage(int _damage, Type _type)
    {
        int finalDamage = _damage;
        if (weaknessesTypes.Contains<Type>(_type))
        {
            finalDamage *= 2;
            Debug.Log("Weaknesse");
        }
        else if (resistancesTypes.Contains<Type>(_type))
        {
            finalDamage /= 2;
            Debug.Log("Resistance");
        }
        if (finalDamage > 0)
        {
            currentLife -= finalDamage;
        }
        Debug.Log("Pokemon take " + finalDamage + " damage point.");
        DisplayCurrentLife();
    }

    public bool CheckIfPokemonAlive()
    {
        if(currentLife > 0)
        {
            Debug.Log("Pokemon is still alive");
            return true;
        }
        else
        {
            Debug.Log("Pokemon is dead");
            return false;
        }
    }

    public int getCurrentLife() { return this.currentLife; }
}
