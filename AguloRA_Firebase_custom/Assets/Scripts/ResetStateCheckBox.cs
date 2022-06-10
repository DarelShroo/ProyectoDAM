using UnityEngine;
using UnityEngine.UI;

public class ResetStateCheckBox : MonoBehaviour
{
    
    private string[] clickParada = new []
    {
        //Texto nombre parada seleccionada por defecto
        "Clica sobre una Parada ...",
        "Click on a stop ...",
        "Klicken Sie auf eine Haltestelle ..."
    };

    public void Reset()
    {
        //Reseteamos los checkboxs y las paradas activas en el mapa.
        CheckboxsState.instance.Lugares = false;
        CheckboxsState.instance.Personajes = false;
        CheckboxsState.instance.Arquitectura = false;
        CheckboxsState.instance.Tradiciones = false;
        CheckboxsState.instance.HistoriaAborigen = false;
        
        Paradas.active.Clear();
    }

    public void ResetParadas(Text text)
    {
        //Reseteamos las paradas activas en el mapa
        Paradas.active.Clear();
        text.text = clickParada[Lenguage.posIdioma];
    }
}
