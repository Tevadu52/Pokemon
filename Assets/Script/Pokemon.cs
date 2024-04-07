using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Pokemon
{
    [SerializeField]
    private PokemonData pokemonData = new PokemonData();

    private int hp;
    private int attack;
    private int defense;
    private int attackSpe;
    private int defenseSpe;
    private int speed;

    public Pokemon(PokemonData PokemonData)
    { 
        this.pokemonData = PokemonData;
        this.SetupIV();
        this.setupStats();
        this.fullHeal();
    }

    public PokemonData PokemonData 
    { 
        get { return this.pokemonData; } 
    }
    public string Name 
    { 
        get { return this.PokemonData.namePokemon; } 
        set { this.PokemonData.namePokemon = value; } 
    }
    public PokemonSpecieData Specie
    {
        get { return this.PokemonData.specie; }
    }
    public float XP
    {
        get { return this.PokemonData.XP; }
    }
    public int Level
    {
        get { return this.PokemonData.Level; }
    }
    public int CurrentHp
    {
        get { return this.PokemonData.CurrentHp; }
    }
    public int Hp
    {
        get { return this.hp; }
    }
    public int Attack
    {
        get { return this.attack; }
    }
    public int Defense
    {
        get { return this.defense; }
    }
    public int AttackSpe
    {
        get { return this.attackSpe; }
    }
    public int DefenseSpe
    {
        get { return this.defenseSpe; }
    }
    public int Speed
    {
        get { return this.speed; }
    }
    public List<ElementData> Elements
    {
        get { return this.Specie.Elements; }
    }
    public Sprite FrontSprite
    {
        get { return this.Specie.FrontSprite; }
    }
    public Sprite BackSprite
    {
        get { return this.Specie.BackSprite; }
    }
    public IEnumerator addXP(Pokemon ennemi, UIPokeBattle instance) 
    {
        float part1 = (((float)ennemi.Specie.BaseXP * ennemi.Level * 1.5f) / 5f);
        float part2 = Mathf.Pow((2f * ennemi.Level + 10) / ((ennemi.Level + this.Level + 10)), 2.5f);
        this.PokemonData.XP += part1 * part2 + 1;
        instance.ChangeCombatText(this.Name + " a gagné " + Mathf.FloorToInt(part1 * part2 + 1) + " point d'expérience après ce combat.");
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
    }
    public IEnumerator LevelUp(UIPokeBattle instance, UIPokeBattle instanceEnnemy)
    {
        if (this.PokemonData.XP > Mathf.Pow(this.PokemonData.Level, 3))
        {
            this.PokemonData.Level += 1;
            this.PokemonData.XP -= Mathf.Pow(this.PokemonData.Level,3);
            instance.ChangeUIPokemonHimself(this);
            instanceEnnemy.ChangeUIPokemonAdverse(this);
            instance.ChangeCombatText(this.Name + " est monté d'un niveau !!");
            this.setupStats();
            yield return new WaitForSeconds(UIPokeBattle.TextTime);
        }
    }
    public void fullHeal() { this.PokemonData.CurrentHp = this.hp; }
    public void takeDamage(int damage) 
    { 
        this.PokemonData.CurrentHp -= damage;
        if (this.CurrentHp <= 0)
        {
            this.PokemonData.CurrentHp = 0;
        }
    }

    public void SetupIV()
    {
        pokemonData.IVs.Hp = Random.Range(0, 31);
        pokemonData.IVs.Attack = Random.Range(0, 31);
        pokemonData.IVs.Defense = Random.Range(0, 31);
        pokemonData.IVs.AttackSpe = Random.Range(0, 31);
        pokemonData.IVs.DefenseSpe = Random.Range(0, 31);
        pokemonData.IVs.Speed = Random.Range(0, 31);
    }

    public void setupStats()
    {
        float level = PokemonData.Level;
        float baseHP = PokemonData.specie.Stats.Hp;
        this.hp = Mathf.FloorToInt(((2f * baseHP + pokemonData.IVs.Hp) * level) / 100f + level + 10f);
        float baseAttack = PokemonData.specie.Stats.Attack;
        this.attack = Mathf.FloorToInt(((((2f * baseAttack + pokemonData.IVs.Attack) / 100f) * level) + 5f) * PokemonData.Nature.attack);
        float baseDefense = PokemonData.specie.Stats.Defense;
        this.defense = Mathf.FloorToInt(((((2f * baseDefense + pokemonData.IVs.Defense) / 100f) * level) + 5f) * PokemonData.Nature.defense);
        float baseAttackSpe = PokemonData.specie.Stats.AttackSpe;
        this.attackSpe = Mathf.FloorToInt(((((2f * baseAttackSpe + pokemonData.IVs.AttackSpe) / 100f) * level) + 5f) * PokemonData.Nature.attackSpe);
        float baseDefenseSpe = PokemonData.specie.Stats.DefenseSpe;
        this.defenseSpe = Mathf.FloorToInt(((((2f * baseDefenseSpe + pokemonData.IVs.DefenseSpe) / 100f) * level) + 5f) * PokemonData.Nature.defenseSpe);
        float baseSpeed = PokemonData.specie.Stats.Speed;
        this.speed = Mathf.FloorToInt(((((2f * baseSpeed + pokemonData.IVs.Speed) / 100f) * level) + 5f) * PokemonData.Nature.speed);
    }
}
