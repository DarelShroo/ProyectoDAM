using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class OpenInfo : MonoBehaviour
{ 
    //Esta clase se encarga de contener ciertos datos que deben ser compartidos con diferentes escenas
    //MapaScene InfoScene 
    public static string name = "";
    
    public static Color32 colorActivarAr;
    public static Color32 textActivarAr = Color.gray;
    public static bool flechaActiva;
    public static OpenInfo instance;
    public static string nombreAnterior = "";
    public static bool mostrado;
    public static bool cambioIdioma;

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

    public void getNameParada(GameObject getNameParada)
    {
        name = getNameParada.GetComponent<TextMesh>().text;
        Debug.Log(name);
        colorActivarAr = new Color32(189, 107, 153, 200); // color rosado
        textActivarAr = Color.white;
        flechaActiva = true;
    }
    
    public static string Name
    {
        get => name;
        set => name = value;
    }
}
