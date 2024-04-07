using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    private static FightManager instance = null;
    public static FightManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public IEnumerator match(Dresseur opponant1, Dresseur opponant2)
    {
        opponant2.IsFighting = true;
        opponant1.IsFighting = true;
        opponant2.transform.LookAt(opponant1.transform);
        opponant1.transform.LookAt(opponant2.transform);
        int nb = opponant1.UIPokeBattle.ChooseCombatBackground(); ;
        opponant2.UIPokeBattle.ChangeCombatBackground(nb);
        opponant1.UIPokeBattle.ChangeCombatBackground(nb);
        opponant2.UIPokeBattle.ChangeUIPokemon(opponant2.getCurrentTeam()[0], opponant1.getCurrentTeam()[0]);
        opponant1.UIPokeBattle.ChangeUIPokemon(opponant1.getCurrentTeam()[0], opponant2.getCurrentTeam()[0]);
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        opponant2.UIDresseur.setOnlyScreen("Battle");
        opponant1.UIDresseur.setOnlyScreen("Battle");
        opponant2.UIPokeBattle.ChangeCombatText(opponant2.getNameDresseur() + " va avoir un match avec " + opponant1.getNameDresseur());
        opponant1.UIPokeBattle.ChangeCombatText(opponant1.getNameDresseur() + " va avoir un match avec " + opponant2.getNameDresseur());
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        while (opponant1.getCurrentTeam().Count > 0 && opponant2.getCurrentTeam().Count > 0)
        {
            opponant2.UIPokeBattle.ChangeUIPokemon(opponant2.getCurrentTeam()[0], opponant1.getCurrentTeam()[0]);
            opponant1.UIPokeBattle.ChangeUIPokemon(opponant1.getCurrentTeam()[0], opponant2.getCurrentTeam()[0]);
            yield return this.fight(opponant1.getCurrentTeam()[0], opponant2.getCurrentTeam()[0], opponant1.UIPokeBattle, opponant2.UIPokeBattle);
            yield return new WaitForSeconds(0.5f);
            if (opponant2.getCurrentTeam()[0].CurrentHp <= 0)
            {
                opponant2.getCurrentTeam().RemoveAt(0);
            }
            else if (opponant1.getCurrentTeam()[0].CurrentHp <= 0)
            {
                opponant1.getCurrentTeam().RemoveAt(0);
            }
        }
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        if (opponant2.getCurrentTeam().Count == 0)
        {
            opponant1.UIPokeBattle.ChangeCombatText(opponant1.getNameDresseur() + " est vainqueur!");
            opponant2.UIPokeBattle.ChangeCombatText(opponant1.getNameDresseur() + " est vainqueur!");
            opponant2.GameOver();
        }
        else if (opponant1.getCurrentTeam().Count == 0)
        {
            opponant1.UIPokeBattle.ChangeCombatText(opponant2.getNameDresseur() + " est vainqueur!");
            opponant2.UIPokeBattle.ChangeCombatText(opponant2.getNameDresseur() + " est vainqueur!");
            opponant1.GameOver();
        }
        else
        {
            opponant1.UIPokeBattle.ChangeCombatText("Match nul");
            opponant2.UIPokeBattle.ChangeCombatText("Match nul");
        }
        opponant1.setSavedTeam();
        opponant2.setSavedTeam();
        opponant1.setSavedCurrentTeam();
        opponant2.setSavedCurrentTeam();
        opponant2.UIDresseur.setOnlyScreen("Menu");
        opponant1.UIDresseur.setOnlyScreen("Menu");
        opponant1.IsFighting = false;
        opponant2.IsFighting = false;
    }

    private bool is_lucky()
    {
        return true; /// A modifié pour les battles stats acuracy et esquive
    }
    public float howDeal(Pokemon fighter1, Pokemon fighter2)
    {
        float result = 1f;
        foreach (ElementData element in fighter1.Elements)
        {
            foreach (ElementData elementAdverse in fighter2.Elements)
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
    private float calculDamage(Pokemon fighter1, Pokemon fighter2, bool attackSpe = false) // Change attackSpe par l'obtention de la compétence utiliser
    {
        float _attack;
        float _defense;
        if (attackSpe)
        {
            _attack = fighter1.AttackSpe;
            _defense = fighter2.DefenseSpe;
        }
        else
        {
            _attack = fighter1.Attack;
            _defense = fighter2.Defense;
        }
        float damage = (fighter2.Level * 0.4f + 2) * _attack * 100f;// 100 est la puissance de la compétence utiliserr
        damage /= _defense;
        damage /= 50f;
        damage += 2;
        damage *= this.howDeal(fighter1, fighter2);
        if (damage < 0) { damage = 0f; }
        return damage;
    }
    public IEnumerator attackPokemon(Pokemon fighter1, Pokemon fighter2, UIPokeBattle instance, UIPokeBattle instanceEnnemy)
    {
        if (this.howDeal(fighter1, fighter2) > 1)
        {
            instance.ChangeCombatText("C'est super efficace!");
            instanceEnnemy.ChangeCombatText("C'est super efficace!");
        }
        if (this.howDeal(fighter1, fighter2) < 1)
        {
            instance.ChangeCombatText("C'est très peu efficace!");
            instanceEnnemy.ChangeCombatText("C'est très peu efficace!");
        }
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        float finalDamage = this.calculDamage(fighter1, fighter2);
        if (Random.Range(0, 100) < 5) // C'est un coup critique
        {
            finalDamage *= 1.5f;
            instance.ChangeCombatText("C'est un coup critique!");
            instanceEnnemy.ChangeCombatText("C'est un coup critique!");
        }
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        instance.ChangeCombatText(fighter2.Name + " a reçu " + Mathf.FloorToInt(finalDamage) + " point de dégats.");
        instanceEnnemy.ChangeCombatText(fighter2.Name + " a reçu " + Mathf.FloorToInt(finalDamage) + " point de dégats.");
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        fighter2.takeDamage(Mathf.FloorToInt(finalDamage));
    }
    public IEnumerator fight(Pokemon fighter1, Pokemon fighter2, UIPokeBattle instance, UIPokeBattle instanceEnnemy)
    {
        //Debug.Log($"Pokemon1 lv:{fighter1.Level} stats {fighter1.attack},{fighter1.defense} et Pokemon2 lv:{fighter2.Level} stats {fighter2.attack},{fighter2.defense}");
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        instance.ChangeCombatText(fighter1.Name + " VS " + fighter2.Name);
        instanceEnnemy.ChangeCombatText(fighter2.Name + " VS " + fighter1.Name);
        Pokemon first = fighter1;
        Pokemon second = fighter2;
        if (fighter1.Speed < fighter2.Speed)
        {
            first = fighter2;
            second = fighter1;
        }
        int nb = 0;
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        instance.ChangeCombatText(first.Name + " est le premier à attaquer!");
        instanceEnnemy.ChangeCombatText(first.Name + " est le premier à attaquer!");
        instance.ChangeUIPokemon(fighter1, fighter2);
        instanceEnnemy.ChangeUIPokemon(fighter2, fighter1);
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        while (first.CurrentHp > 0)
        {
            if (this.is_lucky())
            {
                instance.ChangeCombatText(first.Name + " attaque " + second.Name);
                instanceEnnemy.ChangeCombatText(first.Name + " attaque " + second.Name);
                yield return new WaitForSeconds(UIPokeBattle.TextTime);
                yield return this.attackPokemon(first, second, instance, instanceEnnemy);
                if (second.CurrentHp <= 0)
                {
                    instance.ChangeUIPokemon(fighter1, fighter2);
                    instanceEnnemy.ChangeUIPokemon(fighter2, fighter1);
                    break;
                };
            }
            else
            {
                instance.ChangeCombatText(first.Name + " râte son attaque!");
                instanceEnnemy.ChangeCombatText(first.Name + " râte son attaque!");
                yield return new WaitForSeconds(UIPokeBattle.TextTime);
            }
            instance.ChangeUIPokemon(fighter1, fighter2);
            instanceEnnemy.ChangeUIPokemon(fighter2, fighter1);
            if (this.is_lucky())
            {
                instance.ChangeCombatText(second.Name + " attaque " + first.Name);
                instanceEnnemy.ChangeCombatText(second.Name + " attaque " + first.Name);
                yield return new WaitForSeconds(UIPokeBattle.TextTime);
                yield return this.attackPokemon(second, first, instance, instanceEnnemy);
            }
            else
            {
                instance.ChangeCombatText(second.Name + " râte son attaque!");
                instanceEnnemy.ChangeCombatText(second.Name + " râte son attaque!");
                yield return new WaitForSeconds(UIPokeBattle.TextTime);
            }
            instance.ChangeUIPokemon(fighter1, fighter2);
            instanceEnnemy.ChangeUIPokemon(fighter2, fighter1);
            nb++;
            if (nb > 100) { break; }
        }
        if (fighter2.CurrentHp <= 0)
        {
            instance.ChangeCombatText(first.Name + " est vainqueur !");
            instanceEnnemy.ChangeCombatText(first.Name + " est vainqueur !");
            yield return new WaitForSeconds(UIPokeBattle.TextTime);
            yield return fighter1.addXP(fighter2, instance);
            while (fighter1.PokemonData.XP > (fighter1.PokemonData.Level * fighter1.PokemonData.Level * fighter1.PokemonData.Level))
            {
                yield return fighter1.LevelUp(instance, instanceEnnemy);
            }
        }
        else if (fighter1.CurrentHp <= 0)
        {
            instance.ChangeCombatText(fighter2.Name + " est vainqueur !");
            instanceEnnemy.ChangeCombatText(fighter2.Name + " est vainqueur !");
            yield return new WaitForSeconds(UIPokeBattle.TextTime);
            yield return fighter2.addXP(fighter1, instanceEnnemy);
            while (fighter2.PokemonData.XP > (fighter2.PokemonData.Level * fighter2.PokemonData.Level * fighter2.PokemonData.Level))
            {
                yield return fighter2.LevelUp(instanceEnnemy, instance);
            }
        }
    }

    public bool calcul9G(Dresseur dresseur,Pokemon pokemon)
    {
        int PVMAX = pokemon.Hp;
        int PV = pokemon.CurrentHp;
        int TAUX = pokemon.Specie.CaptureTaux;
        float STATUT = 1f;
        //STATUT = pokemon.getStatut(); Le satut du pokemon
        float TB = 1f;
        //BALL = 1; Le type de pokeball utiliser
        // TB = TB * BALL
        int NIVEAU = pokemon.Level;
        float FAIBLENIVEAU = 1f;
        if (NIVEAU < 13)
        {
            FAIBLENIVEAU = 3.6f - 2 * NIVEAU / 10f;
        }
        int NBBADGE = dresseur.Badge;
        float BADGEREQUIS = 0; ;
        if (pokemon.Level <= 25) { BADGEREQUIS = 0; }
        else if (pokemon.Level <= 30) { BADGEREQUIS = 1; }
        else if (pokemon.Level <= 35) { BADGEREQUIS = 2; }
        else if (pokemon.Level <= 40) { BADGEREQUIS = 3; }
        else if (pokemon.Level <= 45) { BADGEREQUIS = 4; }
        else if (pokemon.Level <= 50) { BADGEREQUIS = 5; }
        else if (pokemon.Level <= 55) { BADGEREQUIS = 6; }
        else if (pokemon.Level <= 60) { BADGEREQUIS = 7; }
        else if (pokemon.Level <= 100) { BADGEREQUIS = 8; }
        float BADGEPENALITE = Mathf.Pow(0.8f, Mathf.Max(BADGEREQUIS - NBBADGE, 0f));
        Debug.Log($"Le dresseur possede {NBBADGE} sur les {BADGEREQUIS} Badges nécéssaire.");
        float a = (((((((3f * PVMAX) - (2f * PV)) * TB) * TAUX) * BADGEPENALITE) / (3f * PVMAX) * FAIBLENIVEAU) * STATUT);
        float b = Mathf.Floor(65536f / Mathf.Pow((255f / a), (3f / 16f)));
        float capture;
        // Calcul des chances de vaciller 4 fois dans le cas "capture non critique" et des chances de vaciller 1 fois dans le cas "capture critique" 
        if (a < 255)
        {
            capture = Mathf.Pow((b / 65536f), 4f);
        }
        else
        {
            capture = 1;
        }
        Debug.Log($"La probabilité de capturer {pokemon.PokemonData.namePokemon} de niveau {NIVEAU} est de {Mathf.Round(capture * 10000f) / 100f} %");
        return Random.Range(0, 101) <= Mathf.Round(capture * 10000f) / 100f;
    }
}
