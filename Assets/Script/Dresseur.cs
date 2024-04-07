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
    private UIDresseur uiDresseur;
    [SerializeField]
    private UIPokeMenu uiPokeMenu;
    [SerializeField]
    private UIPokeBattle uiPokeBattle;
    [SerializeField]
    private GameObject cam;

    private bool isFighting = false;

    public bool IsFighting {  get { return isFighting; } set { isFighting = value; } }
    public UIPokeBattle UIPokeBattle { get { return uiPokeBattle; } }
    public UIDresseur UIDresseur { get { return uiDresseur; } }
    public UIPokeMenu UIPokeMenu { get { return uiPokeMenu; } }

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
                StartCoroutine(FightManager.Instance.match(this, Opponant));
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
public class Dresseur_selfEditor : Editor
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
        NatureDataBase NatureDataBase = Resources.Load<NatureDataBase>("DataBase/NatureDataBase");

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
                    pokemon.Nature = NatureDataBase.GetRandomPokemonNatureData();
                    Pokemon pokemonFinal = new Pokemon(pokemon);
                    if (GameObject.FindAnyObjectByType<FightManager>().calcul9G(Dresseur, pokemonFinal))
                    {
                        team.Add(pokemon);
                        Debug.Log("Capture réussie !");
                    }
                }
                Dresseur.setupDresseurEditor(name, team, money, badge);
                EditorUtility.SetDirty(Dresseur);
            }
        }
    }
}
#endif