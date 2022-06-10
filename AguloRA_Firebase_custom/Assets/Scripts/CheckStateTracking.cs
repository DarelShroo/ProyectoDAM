using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Firebase;
using Firebase.Database;
using Mapbox.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using VideoPlayer = UnityEngine.Video.VideoPlayer;

public class CheckStateTracking : MonoBehaviour
{
    //Esta clase se encarga de comprobar continuamente el estado del tracking de la imágen para AR
    //Evita que muestre en AR el contenido de la parada equivocada.
    //Se encarga de traer los videos del gestor de contenidos.
    
    [SerializeField]
    public GameObject[] objetos;
    private  VideoPlayer vid ;
    private string urlVideo = "";
    private int nAccedido = 0;
    
    [SerializeField] private FirebaseApp _app;
    [SerializeField] private DatabaseReference _reference;
    private int nVeces = 0;

    public void Awake()
    {
        StartCoroutine(makeRequest());
    }

    private void Start()
    {
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
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
    
    public void saveData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("estadisticas").Child("parada-mas-visitada")
            .ValueChanged += setNuevoValor;
    }

    private void setNuevoValor(object sender, ValueChangedEventArgs e)
    {
        string numeroString = e.Snapshot.Child(OpenInfo.name.Replace("\n", ""))
            .GetValue(true).ToString();

        int numero = Convert.ToInt32(numeroString);

        if (nVeces == 0)
        {
            FirebaseDatabase.DefaultInstance.RootReference.Child("estadisticas")
                .Child("parada-mas-visitada")
                .Child(OpenInfo.name.Replace("\n", "")).SetValueAsync(numero + 1);
        }

        nVeces++;
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

    public void Update()
    {
        try
        {
            foreach (var objeto in objetos)
            {
                //Busca el objeto que está tratando de hacer visible en la ventana de visitSceneAr
                if (objeto.activeSelf && objeto.name == OpenInfo.name.Replace("\n", ""))
                {
                    //Si el objeto está activo y su nombre es el mismo que el de OpenInfo.name entonces entrará aquí haciendol
                    objeto.transform.Find("Quad").gameObject.SetActive(true);
                    saveData();

                    nAccedido++;
                    if (nAccedido == 1)
                    {
                        
                    }
                    if (objeto.transform.Find("Quad").GetChild(0).name == "anim")
                    {
                        string path = Application.persistentDataPath + "/paradasVisitadas.txt";
            
                        if (!File.Exists(path))
                        {
                            File.Create(path);
                        }
                        
                        var lineas = File.ReadLines(path);
                        var enumerable = lineas as string[] ?? lineas.ToArray();

                        bool existe = false;
                        foreach (var parada in enumerable)
                        {
                            if (parada == objeto.name.Replace("\n",""))
                            {
                                existe = true;
                            }
                        }
                        
                        if (!existe)
                        {
                            File.WriteAllText(path, getText(enumerable) + objeto.name.Replace("\n","") + "\n");
                        } 
                    }
                }
                else
                {
                    objeto.transform.Find("Quad").gameObject.SetActive(false);
                    nAccedido = 0;
                }
            }
        }
        catch (Exception e)
        {
        }
    }
    
        IEnumerator makeRequest()
        {
            string url = "https://app.agulopuntoinfo.es/wp-json/agulo/v1/get-modelos"; 
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            
            if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    var json =
                        JsonConvert.DeserializeObject<List<ObjectVid>>(request.downloadHandler.text);
                    foreach (var objectVid in json)
                    {
                        if (objectVid.titulo.Equals(OpenInfo.name.Replace("\n", "")))
                        {

                            if (!objectVid.video.Equals(""))
                            {

                                urlVideo = objectVid.video;
                            }
                        }
                    }
                }
            try
            {
                string path = Application.persistentDataPath + "/paradasVisitadas.txt";
            
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                foreach (var objeto in objetos)
                {
                    if (objeto.name.Replace("\n","").Equals(OpenInfo.name.Replace("\n", "")))
                    {
                        var lineas = File.ReadLines(path);
                        var enumerable = lineas as string[] ?? lineas.ToArray();

                        bool existe = false;
                        foreach (var parada in enumerable)
                        {
                            if (parada == objeto.name.Replace("\n",""))
                            {
                                existe = true;
                            }
                        }
                        
                        vid = objeto.transform.Find("Quad").gameObject.GetComponent<VideoPlayer>();
                        vid.url = urlVideo;

                        vid.transform.localScale = new Vector3(1.815f,1.936f,0.00001f);
                        vid.aspectRatio = VideoAspectRatio.FitVertically;
                        vid.audioOutputMode = VideoAudioOutputMode.AudioSource;
                        vid.EnableAudioTrack (0, true);
                        vid.SetDirectAudioVolume(0, 1);
                        vid.SetDirectAudioMute(0,false);
                        vid.controlledAudioTrackCount = 1;
                        vid.audioOutputMode = VideoAudioOutputMode.Direct;
                        vid.Prepare();
                        vid.Play();
                        if (!existe)
                        {
                            File.WriteAllText(path, getText(enumerable) + objeto.name.Replace("\n","") + "\n");
                        } 
                    }

                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

        }

    private string getText(IEnumerable<string> lineas)
    {
        string text = "";
        foreach (var linea in lineas)
        {
            text += linea+"\n";
        }

        return text;
    }
}
