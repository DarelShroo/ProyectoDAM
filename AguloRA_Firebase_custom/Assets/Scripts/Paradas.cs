using System.Collections;
using UnityEngine;

public class Paradas : MonoBehaviour
{
    public static Paradas instance;

    private ArrayList paradas = new ArrayList();

    public static ArrayList active = new ArrayList();

    private Paradas()
    {
        paradas.Add(new Parada("El Chorro (La Vica)", "28.185979342838800,-17.194736627318500", "lugares", false));
        paradas.Add(new Parada("Campo de fútbol (Antiguas plataneras)", "28.188905058174800,-17.196098556066700", "lugares", false));
        paradas.Add(new Parada("Cesarina Bento","28.187580190227000, -17.194099099175500","personajes",false));
        paradas.Add(new Parada("Leoncio Bento","28.188920,-17.194191","personajes",false));
        paradas.Add(new Parada("Casa de Pedro Sánchez Padilla","28.189146490734600,-17.194580294917400","personajes",false));
        paradas.Add(new Parada("Casa de los Hermanos Bethencourt","28.189653971774900,-17.195273012510600","personajes",false));
        paradas.Add(new Parada("Félix Herrera Cabello","28.1899529,-17.1958180","personajes",false));
        paradas.Add(new Parada("Filiichristi","28.192568518571200,-17.195423401784300","personajes",false));
        paradas.Add(new Parada("El Pescante (El Transportador)","28.189501,-17.194816","arquitectura",false));
        paradas.Add(new Parada("Acequia Pérez","28.188201922186600,-17.193782964517700","arquitectura",false));
        paradas.Add(new Parada("Casa del pintor José Aguiar","28.188867800785800,-17.193281903155100","arquitectura",false));
        paradas.Add(new Parada("Las Hogueras de San Marcos","28.188787,-17.194006","tradiciones",false));
        paradas.Add(new Parada("Los Piques","28.188905164858900,-17.194781497651700","tradiciones",false));
        paradas.Add(new Parada("Casa natal Antonio Jesús Trujillo Armas", "28.189010222952400,-17.194472658744500", "personajes", false));
        paradas.Add(new Parada("Cueva aborigen", "28.186642430801000,-17.195661536125900", "historiaaborigen", false));
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                //Destroy(gameObject);
            }
        }
    }

    public Paradas Instance
    {
        get => instance;
        set => instance = value;
    }

    public ArrayList listaParadas
    {
        get => paradas;
        set => paradas = value;
    }
}