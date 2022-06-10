using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Firebase;
using Firebase.Database;
using Mapbox.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class textActivarAr : MonoBehaviour
{
    //Variables públicas serializadas
    [SerializeField]
    public GameObject _avisoAr;
    
    [SerializeField]
    public GameObject _aviso;
    
    [SerializeField]
    public Text txtSaludo;

    [SerializeField]
    public Text avisoGps;
    
    [SerializeField]
    public Text textName;
    
    [SerializeField]
    public GameObject imgCarga;
    
    //Variables privadas
    private bool _textActivarArBool;
    private Uri url = new  Uri("https://app.agulopuntoinfo.es/wp-json/agulo/v1/get-paradas?lang=");
 
    private string[] txtClickParada = new []
    {
        "Clica sobre una Parada ...",
        "Click on a stop ...",
        "Klicken Sie auf eine Haltestelle ..."
    };

    private string[] txtAvisoGps =
    {
        "Compruebe su conexión a internet y su ubicación e inténtelo de nuevo.",
        "Check your internet connection and location and try again.",
        "Überprüfen Sie Ihre Internetverbindung und Ihren Standort und versuchen Sie es erneut."
    };
    
    [SerializeField] private FirebaseApp _app;
    [SerializeField] private DatabaseReference _reference;
    
    void Update()
    {
        if (!OpenInfo.mostrado)
        {
            _aviso.SetActive(true);
            OpenInfo.mostrado = true;
        }


        if (!OpenInfo.Name.Replace("\n", "")
                .Replace(" ", "")
                .Equals(OpenInfo.nombreAnterior.Replace(" ", "")) &&
            !textName.text.Replace(" ", "").Equals(txtClickParada[Lenguage.posIdioma]
                .Replace(" ", "")))
        {
            try
            {
                imgCarga.SetActive(true);
                OpenInfo.nombreAnterior = OpenInfo.Name.Replace("\n", "");

                //activarAr.text = txtActivarAr[Lenguage.posIdioma];
                avisoGps.text = txtAvisoGps[Lenguage.posIdioma];

                loadData();
            }
            catch (Exception e)
            {
            }
        }
    }



    private void Start()
    {
        //_resetStateCheckBox.Reset();
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                _app = Firebase.FirebaseApp.DefaultInstance;
                login();
                FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri("https://aguloar-default-rtdb.firebaseio.com/");
                _reference = FirebaseDatabase.DefaultInstance.RootReference;
               
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
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
        
        private void  loadData()
        {
            FirebaseDatabase.DefaultInstance.GetReference("info-parada").ValueChanged += valueChange;
        }

        private void valueChange(object sender, ValueChangedEventArgs e)
        {
        try {
            Debug.Log(e.Snapshot.GetRawJsonValue());
             //La petición ha sido satisfactoria
                var json = JsonConvert.DeserializeObject<List<ObjectInfoParada>>(e.Snapshot.Child(Lenguage.idioma).GetRawJsonValue());

                //Recorremos todos los objetos deserializados
                foreach (var data in json)
                {
                    if (data.titulo.Equals(OpenInfo.name.Replace("\n", "")))
                    {
                        txtSaludo.text = data.saludo;
                    }
                }
                
                imgCarga.SetActive(false);
                _avisoAr.SetActive(true);
                }catch(Exception ex){}
        }
}
