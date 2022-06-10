using UnityEngine;
using UnityEngine.UI;

public class ComponentInfo : MonoBehaviour
{
    public static ComponentInfo instance;
    
    [SerializeField]
    public Image bannerAr;
    
    [SerializeField]
    public Text textActivarAr;
    
    [SerializeField]
    public Text textName;
    
    [SerializeField]
    public GameObject flecha;
    private void Awake()
    {
        //Evitamos que desaparezca el objeto y nos quedamos con el original
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

    private string[] textParada = new []
    {
        //Texto titulo parada
        "Clica sobre una Parada ...",
        "Click on a stop ...",
        "Klicken Sie auf eine Haltestelle ..."
    };
    
    private void Update()
    {
        bannerAr.color = OpenInfo.colorActivarAr; // Color banner
        textName.text = OpenInfo.Name.Replace("\n","") != "" ?
            OpenInfo.Name.Replace("\n",""):
            textParada[Lenguage.posIdioma];
        textActivarAr.color = OpenInfo.textActivarAr;
        flecha.SetActive(OpenInfo.flechaActiva);
    }
}
