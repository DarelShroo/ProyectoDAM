using UnityEngine;
using UnityEngine.UI;

public class textVisitSceneAr : MonoBehaviour
{
    public Text banner;
    public Text mapa;
    public Text info;
    
    private string[] textBanner = new []
    {
        "Visitas",
        "Visit",
        "Besuchen Sie"
    };

    private string[] textMapa =
    {
        "MAPA",
        "MAP",
        "KARTE"
    };

    private string[] textInfo =
    {
        "INFORMACIÓN",
        "INFORMATION",
        "INFORMATIONEN"
    };
    // Start is called before the first frame update
    void Start()
    {
        banner.text = textBanner[Lenguage.posIdioma].ToUpper();
        mapa.text = textMapa[Lenguage.posIdioma].ToUpper();
        info.text = textInfo[Lenguage.posIdioma].ToUpper();
    }
}
