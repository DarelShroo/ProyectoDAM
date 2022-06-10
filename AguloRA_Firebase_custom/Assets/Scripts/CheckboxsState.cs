using UnityEngine;
public class CheckboxsState : MonoBehaviour
{
    //Esta clase es un molde que contiene los valores booleanos
    //de los checkboxs contenidos en visitScene
    
    public static CheckboxsState instance;
    
    private static bool  lugares;

    private static bool personajes;

    private static bool arquitectura;

    private static bool tradiciones;

    private static  bool historiaAborigen;

    private static string nameInfo;

    private  GameObject iniciarVisita;

    private bool iniciado = false;
    public void Awake()
    {
        //Nos quedamos con el objeto original y destruimos todas las copias
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public bool Lugares
    {
        get => lugares;
        set => lugares = value;
    }

    public bool Personajes
    {
        get => personajes;
        set => personajes = value;
    }

    public bool Arquitectura
    {
        get => arquitectura;
        set => arquitectura = value;
    }

    public bool Tradiciones
    {
        get => tradiciones;
        set => tradiciones = value;
    }

    public bool HistoriaAborigen
    {
        get => historiaAborigen;
        set => historiaAborigen = value;
    }

    public bool Iniciado
    {
        get => iniciado;
        set => iniciado = value;
    }
}
