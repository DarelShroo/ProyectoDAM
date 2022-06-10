using System;
using Firebase;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class Estadisticas : MonoBehaviour
{
    [SerializeField] public Text textCabecera;
    
    [SerializeField] public Text textTitParadaMasVisitada;

    [SerializeField] public Text nombreParada;

    [SerializeField] public Text textTitIdiomaMasVisitado;
    
    [SerializeField] public RawImage banderaIdiomaMasVisitado;

    [SerializeField] public Texture es;

    [SerializeField] public Texture en;

    [SerializeField] public Texture de;

    [SerializeField] public GameObject imgCarga;
    [SerializeField] public GameObject estadisticasPrefab;
    [SerializeField] private FirebaseApp _app;
    [SerializeField] private DatabaseReference _reference;
    
    private string [] cabecera = {
        "ESTADÍSTICAS", 
        "STATISTICS",
        "STATISTIKEN"        
    };

    private string[] titParadaMasVisitada =
    {
        "PARADA MÁS VISITADA",
        "MOST VISITED STOP",
        "MEISTBESUCHTE HALTESTELLE"
    };

    private string [] titIdiomaMasVisitado =
    {
        "IDIOMA QUE MÁS VISITA LA APP",
        "LANGUAGE THAT MOST VISITS THE APP",
        "SPRACHE, DIE DIE APP AM MEISTEN BESUCHT"
    };

    private Estadisticas instance;
  
    
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            textCabecera.text = cabecera[Lenguage.posIdioma];
            textTitIdiomaMasVisitado.text = titIdiomaMasVisitado[Lenguage.posIdioma];
            textTitParadaMasVisitada.text = titParadaMasVisitada[Lenguage.posIdioma];
        
            _reference = FirebaseDatabase.DefaultInstance.RootReference;
            
            //_resetStateCheckBox.Reset();
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available) {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    _app = Firebase.FirebaseApp.DefaultInstance;
                    login();
                    FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri("https://aguloar-default-rtdb.firebaseio.com/");
                    loadData();
                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                } else {
                    UnityEngine.Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }
        catch (Exception e)
        {
        }
    }
    
    private void login(){
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    private void loadData()
    {
        try
        {
            FirebaseDatabase.DefaultInstance.GetReference("estadisticas").Child("idioma-mas-usado").Child("idioma").ValueChanged += getEstadisticasIdioma;
            estadisticasParada();
        }
        catch (Exception e)
        {
        }
    }

    private void estadisticasParada()
    {
        FirebaseDatabase.DefaultInstance.GetReference("estadisticas").Child("parada-mas-visitada").ValueChanged += getEstadisticasParadaMasVisitada;
    }

    private void getEstadisticasParadaMasVisitada(object sender, ValueChangedEventArgs e)
    {
        try
        {
            Parada parada;
            int visitasParadaAnterior = 0;
            int visitasParadaSiguiente = 0;
            int tempNum = 0;
            Parada parada0 = (Parada)Paradas.instance.listaParadas[0];
            string tituloParada = "";
            int mayor = Convert.ToInt32(e.Snapshot.Child(parada0.Nombre)
                .GetValue(true).ToString());
            int pos = 0;
            // Recorrer arreglo y ver si no es así
            // (comenzar desde el 1 porque el 0 ya lo tenemos contemplado arriba)
            for (int x = 1; x < Paradas.instance.listaParadas.Count; x++)
            {
                Parada paradaTemp = (Parada)Paradas.instance.listaParadas[x];
                int temp = Convert.ToInt32(e.Snapshot.Child(paradaTemp.Nombre)
                    .GetValue(true).ToString());
                if (temp > mayor) {
                    mayor = temp;
                    tituloParada = paradaTemp.Nombre;
                }
            }

            nombreParada.text = tituloParada;
        }
        catch (Exception exception)
        {
            
        }
    }

    private void getEstadisticasIdioma(object sender, ValueChangedEventArgs e)
    {
        try
        {

            int es = Convert.ToInt32(e.Snapshot.Child("es")
                .GetValue(true).ToString());
            int en =  Convert.ToInt32(e.Snapshot.Child("en")
                .GetValue(true).ToString());
            int de =  Convert.ToInt32(e.Snapshot.Child("de")
                .GetValue(true).ToString());

            int[] nVisitasIdiomas =
            {
                de,
                en,
                es
            };

            int mayor = nVisitasIdiomas[0];
            int pos = 0;
            // Recorrer arreglo y ver si no es así
            // (comenzar desde el 1 porque el 0 ya lo tenemos contemplado arriba)
            for (int x = 1; x < nVisitasIdiomas.Length; x++) {
                if (nVisitasIdiomas[x] > mayor) {
                    mayor = nVisitasIdiomas[x];
                    pos = x;
                }
            }

            if (pos == 0)
            {
                banderaIdiomaMasVisitado.texture = this.de;
            }else if (pos == 1)
            {
                banderaIdiomaMasVisitado.texture = this.en;
            }else
            {
                banderaIdiomaMasVisitado.texture = this.es;
            }

            imgCarga.SetActive(false);
            estadisticasPrefab.SetActive(true);
        }
        catch (Exception exception)
        {
        }
    }
}
