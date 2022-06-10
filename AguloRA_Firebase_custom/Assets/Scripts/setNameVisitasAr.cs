using UnityEngine;
using UnityEngine.UI;

public class setNameVisitasAr : MonoBehaviour
{
    public Text text;
    public void Update()
    {
        //TODO COMPROBAR
        text.text = OpenInfo.name.Replace("\n","");
    }
}
