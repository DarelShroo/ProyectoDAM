using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class textVisitas : MonoBehaviour
{
    public Text textBanner;
    public Text textLugares;
    public Text textPersonajes;
    public Text textArquitectura;
    public Text textHistoriaAb;
    public Text textTradiciones;
    public Text textIniciarVisitas;
    
    private string[] banner = new []
    {
        "Visitas",
        "Visit",
        "Besuchen Sie"
    };

    private string[] lugares = new []
    {
        "Lugares",
        "Places",
        "Orte"
    };
    
    private string[] personajes = new []
    {
        "Personajes",
        "Characters",
        "Zeichen"
    };

    private string[] arquitectura = new[]
    {
        "Arquitectura",
        "Architecture",
        "Architektur"
    };

    private string[] historiaAb = new[]
    {
        "Historia Aborigen",
        "Aboriginal History",
        "Geschichte der Aborigines"
    };

    private string[] tradiciones = new[]
    {
        "Tradiciones",
        "Traditions",
        "Traditionen"
    };
    
    private string[] iniciarVisitas = new[]
    {
        "Iniciar Visita",
        "Start Visit",
        "Start Besuch"
    };
    
    void Start()
    {
        textBanner.text = banner[Lenguage.posIdioma].ToUpper();
        textLugares.text = lugares[Lenguage.posIdioma].ToUpper();
        textPersonajes.text = personajes[Lenguage.posIdioma].ToUpper();
        textArquitectura.text = arquitectura[Lenguage.posIdioma].ToUpper();
        textHistoriaAb.text = historiaAb[Lenguage.posIdioma].ToUpper();
        textTradiciones.text  = tradiciones[Lenguage.posIdioma].ToUpper();
        textIniciarVisitas.text = iniciarVisitas[Lenguage.posIdioma];


        if (Lenguage.idioma == "de")
        {
            textLugares.fontSize = 14;
            textPersonajes.fontSize = 14;
            textArquitectura.fontSize = 14;
            textHistoriaAb.fontSize = 14;
            textTradiciones.fontSize  = 14;
        }
    }
}
