using UnityEngine;
using UnityEngine.UI;

public class textMapaScene : MonoBehaviour
{
    public Text textBanner;
    
    public Text textActivarAr;
    
    
    private string[] banner = new []
    {
        "Visitas",
        "Visit",
        "Besuchen Sie"
    };
    
    private string[] activarAr = new[]
    {
        "ACTIVAR LA REALIDAD AUMENTADA",
        "ACTIVATE AUGMENTED REALITY",
        "AUGMENTED REALITY AKTIVIEREN"
    };

    // Start is called before the first frame update
    void Start()
    {
        textBanner.text = banner[Lenguage.posIdioma].ToUpper();
        textActivarAr.text = activarAr[Lenguage.posIdioma];
    }
}
