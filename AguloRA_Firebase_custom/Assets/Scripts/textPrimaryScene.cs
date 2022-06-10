using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class textPrimaryScene : MonoBehaviour
{
    public Text textVisitas;

    public Text textCambioCli;

    public Text textAppFunc;

    public Text textEstadisticas;

    public Text textIdioma;

    private ResetStateCheckBox _resetStateCheckBox;

    private string[] visita = new[]
    {
        "VISITAS",
        "VISITS",
        "BESUCHE"
    };
    
    private string[] cambioCli = new []
    {
        "CAMBIO CLIMATICO",
        "CLIMATE CHANGE",
        "KLIMAWANDEL"
    };
    
    private string[] appFunc = new[]
    {
        "APP Y FUNCIONES",
        "APP AND FUNCTIONS",
        "APP UND FUNKTIONEN"
    };

    private string[] estadisticas = new[]
    {
        "ESTADÍSTICAS",
        "STATISTICS",
        "STATISTIKEN"
    };
    
    private string[] idiomas = new[]
    {
        "IDIOMAS",
        "LANGUAGES",
        "SPRACHEN"
    };

    // Start is called before the first frame update
    void Start()
    {
        textVisitas.text = visita[Lenguage.posIdioma];
        textCambioCli.text = cambioCli[Lenguage.posIdioma];
        textAppFunc.text = appFunc[Lenguage.posIdioma];
        textEstadisticas.text = estadisticas[Lenguage.posIdioma];
        textIdioma.text = idiomas[Lenguage.posIdioma];

       // _resetStateCheckBox.Reset();

        if (Lenguage.idioma == "de")
        {
            textVisitas.fontSize =14;
            textCambioCli.fontSize = 14;
            textAppFunc.fontSize = 14;
            textEstadisticas.fontSize = 14;
            textIdioma.fontSize = 14;
        }
        
        //Pre cargamos el mapa
        AsyncOperation async = SceneManager.LoadSceneAsync(7);
        async.allowSceneActivation = false;
    }
}
