using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Dresseur : MonoBehaviour
{
    [SerializeField]
    private DresseurData DresseurData = new DresseurData();
    private List<Pokemon> team = new List<Pokemon>();
    private List<Pokemon> currentTeam = new List<Pokemon>();

    [SerializeField]
    private float distance = 5;
    [SerializeField]
    private UIPokeBattle UIPokeBattle;
    [SerializeField]
    private UIDresseur UIDresseur;
    [SerializeField]
    private UIPokeMenu UIPokeMenu;
    [SerializeField]
    private GameObject cam;

    private bool isFighting = false;

    public Dresseur(string nameDresseur, List<Pokemon> team,int money,int badge)
    {
        DresseurData.nameDresseur = nameDresseur;
        this.team = team;
        DresseurData.money = money;
        DresseurData.badge = badge;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.setTeam(getSavedTeam());
        this.setCurrentTeam(getSavedCurrentTeam());
        this.healTeam();
    }

    private void FixedUpdate()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out raycastHit, distance))
        {
            Dresseur Opponant = raycastHit.collider.transform.parent.gameObject.GetComponent<Dresseur>();
            if (raycastHit.collider.gameObject != this.gameObject && !isFighting && !Opponant.getIsFighting())
            {
                Debug.Log(this.getNameDresseur() + " a vu " + Opponant.getNameDresseur());
                StartCoroutine(match(Opponant));
            }
        }
    }


    public string getNameDresseur() { return this.DresseurData.nameDresseur; }
    public int Money { get { return this.DresseurData.money; } set { this.DresseurData.money = value; } }
    public int Badge { get { return this.DresseurData.badge; } set { this.DresseurData.badge = value; } }
    public UIDresseur getUIDresseur() { return this.UIDresseur; }
    public UIPokeMenu getUIPokeMenu() { return this.UIPokeMenu; }
    public List<Pokemon> getTeam() { return this.team; }
    public List<Pokemon> getCurrentTeam() { return this.currentTeam; }
    public List<PokemonData> getSavedTeam() { return this.DresseurData.team; }
    public List<PokemonData> getSavedCurrentTeam() { return this.DresseurData.currentTeam; }
    public GameObject getCam() { return this.cam; }
    public bool getIsFighting() { return this.isFighting; }


    public void setNameDresseur(string nameDresseur) { this.DresseurData.nameDresseur = nameDresseur; }
    public void setTeam(List<PokemonData> SavedTeam)
    {
        this.team = new List<Pokemon>();
        foreach (PokemonData PokemonData in SavedTeam)
        {
            Pokemon pokemon = new Pokemon(PokemonData);
            this.team.Add(pokemon);
        } 
    }
    public void setCurrentTeam(List<PokemonData> SavedCurrentTeam)
    {
        this.currentTeam = new List<Pokemon>();
        foreach (PokemonData PokemonData in SavedCurrentTeam)
        {
            Pokemon pokemon = new Pokemon(PokemonData);
            this.currentTeam.Add(pokemon);
        }
    }
    public void setSavedTeam()
    {
        this.DresseurData.team = new List<PokemonData>();
        foreach (Pokemon Pokemon in this.team)
        {
            PokemonData pokemonData = Pokemon.PokemonData;
            this.DresseurData.team.Add(pokemonData);
        }
    }
    public void setSavedCurrentTeam()
    {
        this.DresseurData.currentTeam = new List<PokemonData>();
        foreach (Pokemon Pokemon in this.currentTeam)
        {
            PokemonData pokemonData = Pokemon.PokemonData;
            this.DresseurData.currentTeam.Add(pokemonData);
        }
    }
    public void healTeam() { this.currentTeam = this.team; }

    public void addPokemon(Pokemon pokemon)
    {
        team.Add(pokemon);
        DresseurData.team.Add(pokemon.PokemonData);
    }

    public bool calcul9G(Pokemon pokemon)
    {
        int PVMAX = pokemon.getHp();
        int PV = pokemon.getCurrentHp();
        int TAUX = pokemon.getSpecie().CaptureTaux;
        float STATUT = 1f;
        //STATUT = pokemon.getStatut(); Le satut du pokemon
        float TB = 1f;
        //BALL = 1; Le type de pokeball utiliser
        // TB = TB * BALL
        int NIVEAU = pokemon.getLevel();
        float FAIBLENIVEAU = 1f;
        if (NIVEAU < 13)
        {
            FAIBLENIVEAU = 3.6f - 2 * NIVEAU / 10f;
        }
        int NBBADGE = this.Badge;
        float BADGEREQUIS = 0; ;
        if (pokemon.getLevel() <= 25) { BADGEREQUIS = 0; }
        else if (pokemon.getLevel() <= 30) { BADGEREQUIS = 1; }
        else if (pokemon.getLevel() <= 35) { BADGEREQUIS = 2; }
        else if (pokemon.getLevel() <= 40) { BADGEREQUIS = 3; }
        else if (pokemon.getLevel() <= 45) { BADGEREQUIS = 4; }
        else if (pokemon.getLevel() <= 50) { BADGEREQUIS = 5; }
        else if (pokemon.getLevel() <= 55) { BADGEREQUIS = 6; }
        else if (pokemon.getLevel() <= 60) { BADGEREQUIS = 7; }
        else if (pokemon.getLevel() <= 100) { BADGEREQUIS = 8; }
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

    private IEnumerator match(Dresseur opponant)
    {
        opponant.isFighting = true;
        isFighting = true;
        opponant.transform.LookAt(this.transform);
        this.transform.LookAt(opponant.transform);
        int nb = this.UIPokeBattle.ChooseCombatBackground(); ;
        opponant.UIPokeBattle.ChangeCombatBackground(nb);
        this.UIPokeBattle.ChangeCombatBackground(nb);
        opponant.UIPokeBattle.ChangeUIPokemon(opponant.getCurrentTeam()[0], this.getCurrentTeam()[0]);
        this.UIPokeBattle.ChangeUIPokemon(this.getCurrentTeam()[0], opponant.getCurrentTeam()[0]);
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        opponant.UIDresseur.setOnlyScreen("Battle");
        this.UIDresseur.setOnlyScreen("Battle");
        opponant.UIPokeBattle.ChangeCombatText(opponant.getNameDresseur() + " va avoir un match avec " + this.getNameDresseur());
        this.UIPokeBattle.ChangeCombatText(this.getNameDresseur() + " va avoir un match avec " + opponant.getNameDresseur());
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        while (this.getCurrentTeam().Count > 0 && opponant.getCurrentTeam().Count > 0)
        {
            opponant.UIPokeBattle.ChangeUIPokemon(opponant.getCurrentTeam()[0], this.getCurrentTeam()[0]);
            this.UIPokeBattle.ChangeUIPokemon(this.getCurrentTeam()[0], opponant.getCurrentTeam()[0]);
            if (!this.getCurrentTeam()[0].isFighting)
            {
                yield return this.getCurrentTeam()[0].fight(opponant.getCurrentTeam()[0], UIPokeBattle, opponant.UIPokeBattle);
            }
            int winner = 0;
            yield return new WaitForSeconds(0.5f);
            if (opponant.getCurrentTeam()[0].getCurrentHp() <= 0)
            {
                opponant.UIPokeBattle.ChangeCombatText(this.getCurrentTeam()[0].getName() + " est vainqueur!");
                this.UIPokeBattle.ChangeCombatText(this.getCurrentTeam()[0].getName() + " est vainqueur!");
                winner = 1;
            }
            else if (this.getCurrentTeam()[0].getCurrentHp() <= 0)
            {
                opponant.UIPokeBattle.ChangeCombatText(opponant.getCurrentTeam()[0].getName() + " est vainqueur!");
                this.UIPokeBattle.ChangeCombatText(opponant.getCurrentTeam()[0].getName() + " est vainqueur!");
                winner = 2;
            }
            yield return new WaitForSeconds(UIPokeBattle.TextTime);
            if (winner == 1)
            {
                opponant.getCurrentTeam().RemoveAt(0);
            }
            else if (winner == 2)
            {
                this.getCurrentTeam().RemoveAt(0);
            }
            else if (winner == 0)
            {
                if(Random.Range(0,1) == 0)
                {
                    this.getCurrentTeam().RemoveAt(0);
                }
                else
                {
                    opponant.getCurrentTeam().RemoveAt(0);
                }
            }
        }
        yield return new WaitForSeconds(UIPokeBattle.TextTime);
        if (opponant.getCurrentTeam().Count == 0)
        {
            this.UIPokeBattle.ChangeCombatText(this.getNameDresseur() + " est vainqueur!");
            opponant.UIPokeBattle.ChangeCombatText(this.getNameDresseur() + " est vainqueur!");
            opponant.GameOver();
        }
        else if (this.getCurrentTeam().Count == 0)
        {
            this.UIPokeBattle.ChangeCombatText(opponant.getNameDresseur() + " est vainqueur!");
            opponant.UIPokeBattle.ChangeCombatText(opponant.getNameDresseur() + " est vainqueur!");
            this.GameOver();
        }
        else
        {
            this.UIPokeBattle.ChangeCombatText("Match nul");
            opponant.UIPokeBattle.ChangeCombatText("Match nul");
        }
        this.setSavedTeam();
        opponant.setSavedTeam();
        this.setSavedCurrentTeam();
        opponant.setSavedCurrentTeam();
        opponant.UIDresseur.setOnlyScreen("Menu");
        this.UIDresseur.setOnlyScreen("Menu");
        this.isFighting = false;
        opponant.isFighting = false;
    }

    public void GameOver()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * distance);
    }

    public void setupDresseurEditor(string nameDresseur, List<PokemonData> team, int money, int badge)
    {
        DresseurData.nameDresseur = nameDresseur;
        DresseurData.team = team;
        DresseurData.money = money;
        DresseurData.badge = badge;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Dresseur)), CanEditMultipleObjects]
public class P_sefEditor : Editor
{
    List<string> Names = new List<string>()
    {
        "Sacha","Théo","Alexandre","Téva","Mano","Baptiste","Uma","Jérémy","Dorian","Charle"
    };
    int MaxMoney = 100;
    int MaxBadge = 8;
    int TeamMaxLenght = 1;
    float minValLevel = 1;
    float maxValLevel = 10;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("---------Setup---------");
        Object[] Objects = targets;

        MaxMoney = EditorGUILayout.IntSlider("Money Max",MaxMoney,0,1000);
        MaxBadge = EditorGUILayout.IntSlider("Badge Max",MaxBadge, 0, 8);
        TeamMaxLenght = EditorGUILayout.IntSlider("TeamRange", TeamMaxLenght, 1, 10);

        PokemonSpecieDataBase SpeciesDataBase = Resources.Load<PokemonSpecieDataBase>("DataBase/PokemonSpecieDataBase");

        EditorGUILayout.LabelField("Level minimum : " + Mathf.FloorToInt(minValLevel).ToString());
        EditorGUILayout.LabelField("Level maximum : " + Mathf.FloorToInt(maxValLevel).ToString());
        EditorGUILayout.MinMaxSlider("LevelRange", ref minValLevel, ref maxValLevel, 1, 100);

        if (GUILayout.Button("SetupDresseur"))
        {
            foreach (Object Object in Objects)
            {
                Dresseur Dresseur = (Dresseur)Object;
                string name = Names[Random.Range(0, Names.Count)];
                int money = Random.Range(0, MaxMoney + 1);
                int badge = Random.Range(0, MaxBadge + 1);
                List<PokemonData> team = new List<PokemonData>();
                int nb = Random.Range(1, TeamMaxLenght + 1);
                while (team.Count < nb)
                {
                    PokemonData pokemon = new PokemonData();
                    pokemon.specie = SpeciesDataBase.GetRandomPokemonSpecieData();
                    pokemon.namePokemon = pokemon.specie.NameSpecie;
                    pokemon.Level = Random.Range(Mathf.FloorToInt(minValLevel), Mathf.FloorToInt(maxValLevel));
                    if (Dresseur.calcul9G(new Pokemon(pokemon)))
                    {
                        team.Add(pokemon);
                    }
                }
                Dresseur.setupDresseurEditor(name, team, money, badge);
                EditorUtility.SetDirty(Dresseur);
            }
        }
    }
}
#endif