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
    private int speed;
    public bool isFighting = false;

    public Pokemon(PokemonData PokemonData)
    { 
        this.pokemonData = PokemonData;
        this.setupStats();
        this.fullHeal();
    }

    public PokemonData PokemonData { get { return this.pokemonData; } }
    public string getName() { return this.PokemonData.namePokemon; }
    public PokemonSpecieData getSpecie() { return this.PokemonData.specie; }
    public float getXP() { return this.PokemonData.XP; }
    public int getLevel() { return this.PokemonData.Level; }
    public int getHp() { return this.hp; }
    public int getAttack() { return this.attack; }
    public int getDefense() { return this.defense; }
    public int getSpeed() { return this.speed; }
    public List<Element> getElements() { return this.getSpecie().Elements; }
    public Sprite getSprite() { return this.getSpecie().Sprite; }
    public int getCurrentHp() { return this.PokemonData.CurrentHp; }

    public void setName(string namePokemon) { this.PokemonData.namePokemon = namePokemon; }
    public void addXP(Pokemon ennemi) 
    {
        float part1 = (float)((ennemi.getSpecie().Stat.baseXP * ennemi.getLevel() * 1.5) / 5);
        float part2 = ((2 * ennemi.getLevel() + 10) / ((ennemi.getLevel() + this.getLevel() + 10)) * (ennemi.getLevel() + this.getLevel() + 10));
        this.PokemonData.XP += part1 * part2; 
    }
    public IEnumerator LevelUp(UIPokeBattle instance, UIPokeBattle instanceEnnemy)
    {
        instance.ChangeCombatText(this.getName() + " a gagné de l'expérience après ce combat.");
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        if (this.PokemonData.XP > (this.PokemonData.Level * this.PokemonData.Level * this.PokemonData.Level))
        {
            this.PokemonData.Level += 1;
            this.PokemonData.XP -= (this.PokemonData.Level * this.PokemonData.Level * this.PokemonData.Level);
            instance.ChangeUIPokemonHimself(this);
            instanceEnnemy.ChangeUIPokemonAdverse(this);
            instance.ChangeCombatText(this.getName() + " est monté d'un niveau !!");
            yield return new WaitForSeconds(UIPokeBattle.TextTime);
        }
    }
    public void fullHeal() { this.PokemonData.CurrentHp = this.hp; }
    public void takeDamage(int damage) 
    { 
        this.PokemonData.CurrentHp -= damage;
        if (this.getCurrentHp() <= 0)
        {
            this.PokemonData.CurrentHp = 0;
        }
    }
    public void setupStats()
    {
        this.hp = Mathf.FloorToInt((2 * PokemonData.specie.Stat.baseHp + Random.Range(0, 31) * PokemonData.Level) / 100 + PokemonData.Level + 10);
        this.attack = Mathf.FloorToInt((2 * PokemonData.specie.Stat.baseAttack + Random.Range(0, 31) / 100 * PokemonData.Level) + 5);
        this.defense = Mathf.FloorToInt((2 * PokemonData.specie.Stat.baseDefense + Random.Range(0, 31) / 100 * PokemonData.Level) + 5);
        this.speed = Mathf.FloorToInt((2 * PokemonData.specie.Stat.baseDefense + Random.Range(0, 31) / 100 * PokemonData.Level) + 5);
    }

    private bool is_lucky()
    {
        return true; /// A modifié pour les battles stats acuracy et esquive
    }
    public int howDeal(Pokemon enemy)
    {
        int result = 1;
        foreach(Element element in this.getElements())
        {
            foreach (Element elementAdverse in enemy.getElements())
            {
                if (element.Strong.Contains(elementAdverse.Type))
                {
                    result *= 2;
                }
                else if (element.Weak.Contains(elementAdverse.Type))
                {
                    result /= 2;
                }
            }
        }
        
        return result;
    }

    private int calculDamage(Pokemon enemy)
    {
        double damage = 0.5;
        damage *= 10;
        damage *= this.getAttack() / enemy.getDefense();
        damage *= this.howDeal(enemy);
        if (damage < 0) { damage = 0; }
        damage++;
        return ((int)damage);
    }

    public void attackPokemon(Pokemon enemy, UIPokeBattle instance, UIPokeBattle instanceEnnemy)
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
        enemy.takeDamage(this.calculDamage(enemy));
    }

    public IEnumerator fight(Pokemon enemy, UIPokeBattle instance, UIPokeBattle instanceEnnemy)
    {
        this.isFighting = true;
        enemy.isFighting = true;
        this.setupStats();
        enemy.setupStats();
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        instance.ChangeCombatText(this.getName() + " VS " + enemy.getName());
        instanceEnnemy.ChangeCombatText(enemy.getName() + " VS " + this.getName());
        Pokemon first = this;
        Pokemon second = enemy;
        if (this.getSpeed() < enemy.getSpeed())
        {
            first = enemy;
            second = this;
        }
        int nb = 0;
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        instance.ChangeCombatText(first.getName() + " est le premier à attaquer!");
        instanceEnnemy.ChangeCombatText(first.getName() + " est le premier à attaquer!");
        while (first.getCurrentHp() > 0)
        {
            yield return new WaitForSeconds(UIPokeBattle.TextTime);
            instance.ChangeUIPokemon(this, enemy);
            instanceEnnemy.ChangeUIPokemon(enemy, this);
            if (first.is_lucky())
            {
                instance.ChangeCombatText(first.getName() + " attaque " + second.getName());
                instanceEnnemy.ChangeCombatText(first.getName() + " attaque " + second.getName());
                first.attackPokemon(second, instance, instanceEnnemy);
                yield return new WaitForSeconds(UIPokeBattle.TextTime);
                instance.ChangeCombatText(second.getName() + " a reçu " + (first.calculDamage(second)) + " point de dégats.");
                instanceEnnemy.ChangeCombatText(second.getName() + " a reçu " + (first.calculDamage(second)) + " point de dégats.");
                if (second.getCurrentHp() <= 0)
                {
                    instance.ChangeUIPokemon(this, enemy);
                    instanceEnnemy.ChangeUIPokemon(enemy, this);
                    break; 
                };
            }
            else
            {
                instance.ChangeCombatText(first.getName() + " râte son attaque!");
                instanceEnnemy.ChangeCombatText(first.getName() + " râte son attaque!");
            }
            instance.ChangeUIPokemon(this, enemy);
            instanceEnnemy.ChangeUIPokemon(enemy, this);
            yield return new WaitForSeconds(UIPokeBattle.TextTime);
            if (second.is_lucky())
            {
                instance.ChangeCombatText(second.getName() + " attaque " + first.getName());
                instanceEnnemy.ChangeCombatText(second.getName() + " attaque " + first.getName());
                second.attackPokemon(first, instance, instanceEnnemy);
                yield return new WaitForSeconds(UIPokeBattle.TextTime);
                instance.ChangeCombatText(first.getName() + " a reçu " + (second.calculDamage(first)) + " point de dégats.");
                instanceEnnemy.ChangeCombatText(first.getName() + " a reçu " + (second.calculDamage(first)) + " point de dégats.");
            }
            else
            {
                instance.ChangeCombatText(second.getName() + " râte son attaque!");
                instanceEnnemy.ChangeCombatText(second.getName() + " râte son attaque!");
            }
            instance.ChangeUIPokemon(this, enemy);
            instanceEnnemy.ChangeUIPokemon(enemy, this);
            nb++;
            if (nb > 100) { break; }
        }
        if (enemy.getCurrentHp() <= 0)
        {
            this.addXP(enemy);
            while (this.PokemonData.XP > (this.PokemonData.Level * this.PokemonData.Level * this.PokemonData.Level))
            {
                yield return this.LevelUp(instance, instanceEnnemy);
            }
        }
        else if (this.getCurrentHp() <= 0)
        {
            enemy.addXP(this);
            while (enemy.PokemonData.XP > (enemy.PokemonData.Level * enemy.PokemonData.Level * enemy.PokemonData.Level))
            {
                yield return enemy.LevelUp(instanceEnnemy, instance);
            }
        }
        this.isFighting = false;
        enemy.isFighting = false;
    }
}
