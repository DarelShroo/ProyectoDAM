using UnityEngine;
using UnityEngine.UI;

public class textInfoCanvas : MonoBehaviour
{
    public Text textAtencion;
    
    public Text textAviso;

    public Text textAvisoGps;

    private string[] atencion =
    {
        "¡Atención!",
        "Attention!",
        "Achtung!"
    };
    
    private string[] aviso =
    {
        "Haz click en el punto que quieres ver y una vez " +
        "llegues ahí, apunta hacia el disparador para" +
        " lanzar la realidad aumentada.",
        "Click on the point you want to see and once you get there, point the trigger to launch the augmented reality.",
        "Klicken Sie auf den Punkt, den Sie sehen möchten, " +
        "und sobald Sie dort sind, drücken Sie den Auslöser, " +
        "um die erweiterte Realität zu starten. " +
        "die erweiterte Realität einführen."
    };

    private string[] avisoGps =
    {
     "Compruebe su conexión a internet y su ubicación e inténtelo de nuevo.",
     "Check your internet connection and location and try again.",
     "Überprüfen Sie Ihre Internetverbindung und Ihren Standort und versuchen Sie es erneut."
    };
    void Start()
    {
        textAtencion.text = atencion[Lenguage.posIdioma];
        textAviso.text = aviso[Lenguage.posIdioma];
        textAvisoGps.text = avisoGps[Lenguage.posIdioma];
    }
}
