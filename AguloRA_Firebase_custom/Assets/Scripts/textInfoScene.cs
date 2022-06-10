using UnityEngine;
using UnityEngine.UI;

public class textInfoScene : MonoBehaviour
{
    public Text banner;

    public Text cargando;

    private string[] textCargando =
    {
        "Cargando ...",
        "Loading ...",
        "Laden ..."
    };

    
    private string[] textBanner = new[]
    {
        "Visitas",
        "Visit",
        "Besuchen Sie"
    };

    void Start()
    {
        banner.text = textBanner[Lenguage.posIdioma].ToUpper();
        cargando.text = textCargando[Lenguage.posIdioma];
    }
}
