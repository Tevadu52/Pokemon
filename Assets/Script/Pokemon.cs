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

    private bool is_lucky()
    {
        return true; /// A modifié pour les battles stats acuracy et esquive
    }
    public float howDeal(Pokemon enemy)
    {
        float result = 1f;
        foreach(ElementData element in this.Elements)
        {
            foreach (ElementData elementAdverse in enemy.Elements)
            {
                if (element.Strong.Contains(elementAdverse))
                {
                    result *= 2;
                }
                else if (element.Weak.Contains(elementAdverse))
                {
                    result /= 2;
                }
            }
        }
        return result;
    }

    private float calculDamage(Pokemon enemy, bool attackSpe = false) // Change attackSpe par l'obtention de la compétence utiliser
    {
        float _attack;
        float _defense;
        if (attackSpe)
        {
            _attack = this.AttackSpe;
            _defense = enemy.DefenseSpe;
        }
        else
        {
            _attack = this.Attack;
            _defense = enemy.Defense;
        }
        float damage = (enemy.Level * 0.4f + 2) * _attack * 100f;// 100 est la puissance de la compétence utiliserr
        damage /= _defense;
        damage /= 50f;
        damage += 2;
        damage *= this.howDeal(enemy);
        if (damage < 0) { damage = 0f; }
        return damage;
    }

    public IEnumerator attackPokemon(Pokemon enemy, UIPokeBattle instance, UIPokeBattle instanceEnnemy)
    {
        if (this.howDeal(enemy) > 1)
        {
            instance.ChangeCombatText("C'est super efficace!");
            instanceEnnemy.ChangeCombatText("C'est super efficace!");
        }
        if (this.howDeal(enemy) < 1)
        {
            instance.ChangeCombatText("C'est très peu efficace!");
            instanceEnnemy.ChangeCombatText("C'est très peu efficace!");
        }
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        float finalDamage = this.calculDamage(enemy);
        if (Random.Range(0, 100) < 5) // C'est un coup critique
        {
            finalDamage *= 1.5f;
            instance.ChangeCombatText("C'est un coup critique!");
            instanceEnnemy.ChangeCombatText("C'est un coup critique!");
        }
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        instance.ChangeCombatText(enemy.Name + " a reçu " + Mathf.FloorToInt(finalDamage) + " point de dégats.");
        instanceEnnemy.ChangeCombatText(enemy.Name + " a reçu " + Mathf.FloorToInt(finalDamage) + " point de dégats.");
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        enemy.takeDamage(Mathf.FloorToInt(finalDamage));
    }

    public IEnumerator fight(Pokemon enemy, UIPokeBattle instance, UIPokeBattle instanceEnnemy)
    {
        //Debug.Log($"Pokemon1 lv:{this.Level} stats {this.attack},{this.defense} et Pokemon2 lv:{enemy.Level} stats {enemy.attack},{enemy.defense}");
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        instance.ChangeCombatText(this.Name + " VS " + enemy.Name);
        instanceEnnemy.ChangeCombatText(enemy.Name + " VS " + this.Name);
        Pokemon first = this;
        Pokemon second = enemy;
        if (this.Speed < enemy.Speed)
        {
            first = enemy;
            second = this;
        }
        int nb = 0;
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        instance.ChangeCombatText(first.Name + " est le premier à attaquer!");
        instanceEnnemy.ChangeCombatText(first.Name + " est le premier à attaquer!");
        instance.ChangeUIPokemon(this, enemy);
        instanceEnnemy.ChangeUIPokemon(enemy, this);
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        while (first.CurrentHp > 0)
        {
            if (first.is_lucky())
            {
                instance.ChangeCombatText(first.Name + " attaque " + second.Name);
                instanceEnnemy.ChangeCombatText(first.Name + " attaque " + second.Name);
                yield return new WaitForSeconds(UIPokeBattle.TextTime);
                yield return first.attackPokemon(second, instance, instanceEnnemy);
                if (second.CurrentHp <= 0)
                {
                    instance.ChangeUIPokemon(this, enemy);
                    instanceEnnemy.ChangeUIPokemon(enemy, this);
                    break; 
                };
            }
            else
            {
                instance.ChangeCombatText(first.Name + " râte son attaque!");
                instanceEnnemy.ChangeCombatText(first.Name + " râte son attaque!");
                yield return new WaitForSeconds(UIPokeBattle.TextTime);
            }
            instance.ChangeUIPokemon(this, enemy);
            instanceEnnemy.ChangeUIPokemon(enemy, this);
            if (second.is_lucky())
            {
                instance.ChangeCombatText(second.Name + " attaque " + first.Name);
                instanceEnnemy.ChangeCombatText(second.Name + " attaque " + first.Name);
                yield return new WaitForSeconds(UIPokeBattle.TextTime);
                yield return second.attackPokemon(first, instance, instanceEnnemy);
            }
            else
            {
                instance.ChangeCombatText(second.Name + " râte son attaque!");
                instanceEnnemy.ChangeCombatText(second.Name + " râte son attaque!");
                yield return new WaitForSeconds(UIPokeBattle.TextTime);
            }
            instance.ChangeUIPokemon(this, enemy);
            instanceEnnemy.ChangeUIPokemon(enemy, this);
            nb++;
            if (nb > 100) { break; }
        }
        if (enemy.CurrentHp <= 0)
        {
            instance.ChangeCombatText(first.Name + " est vainqueur !");
            instanceEnnemy.ChangeCombatText(first.Name + " est vainqueur !");
            yield return new WaitForSeconds(UIPokeBattle.TextTime);
            yield return this.addXP(enemy, instance);
            while (this.PokemonData.XP > (this.PokemonData.Level * this.PokemonData.Level * this.PokemonData.Level))
            {
                yield return this.LevelUp(instance, instanceEnnemy);
            }
        }
        else if (this.CurrentHp <= 0)
        {
            instance.ChangeCombatText(enemy.Name + " est vainqueur !");
            instanceEnnemy.ChangeCombatText(enemy.Name + " est vainqueur !");
            yield return new WaitForSeconds(UIPokeBattle.TextTime);
            yield return enemy.addXP(this, instanceEnnemy);
            while (enemy.PokemonData.XP > (enemy.PokemonData.Level * enemy.PokemonData.Level * enemy.PokemonData.Level))
            {
                yield return enemy.LevelUp(instanceEnnemy, instance);
            }
        }
    }
}
