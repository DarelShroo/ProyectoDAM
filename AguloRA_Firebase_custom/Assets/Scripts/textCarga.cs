using UnityEngine;
using UnityEngine.UI;

public class textCarga : MonoBehaviour
{
    public Text cargando;

    private string[] textCargando =
    {
        "Cargando ...",
        "Loading ...",
        "Laden ..."
    };
    
    void Start()
    {
        cargando.text = textCargando[Lenguage.posIdioma];
    }
}
