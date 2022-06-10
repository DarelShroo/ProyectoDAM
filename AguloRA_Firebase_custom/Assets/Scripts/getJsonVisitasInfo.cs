using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using Firebase;
using Firebase.Database;

namespace DefaultNamespace
{
    public class getJsonVisitasInfo : MonoBehaviour
    {
        //Variables públicas serializadas   
        [SerializeField]
        public RawImage imagen;
        
        [SerializeField]
        public GameObject imgGameObject;
        
        [SerializeField]
        public TextMeshProUGUI textDescripcion;
        
        [SerializeField]
        public Text titulo;
        
        [SerializeField]
        public GameObject imgCarga;
        
        [SerializeField]
        public GameObject scrollArea;
        
        //Variables privadas
        private string descripcion;
        private Texture texture;
        private string img_url;
        private Uri url = new  Uri("https://app.agulopuntoinfo.es/wp-json/agulo/v1/get-paradas?lang=");
        private string extra;
        private string slug;
        private String id_parada;
        [SerializeField] private FirebaseApp _app;
        [SerializeField] private DatabaseReference _reference;
        private void Start()
        {
            getTextVisitasInfo();
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

        [RuntimeInitializeOnLoadMethod]
        public void getTextVisitasInfo()
        {
            //Realizamos una petición al gestor de contenidos para traernos el json
            // StartCoroutine(makeRequest());
            
            loadData();
            
            //Realizamos una petición al servidor para traernos la imágen
            StartCoroutine(makeRequestImage());
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
            Debug.Log(e.Snapshot.GetRawJsonValue());
             //La petición ha sido satisfactoria
                var json = JsonConvert.DeserializeObject<List<ObjectInfoParada>>(e.Snapshot.Child(Lenguage.idioma).GetRawJsonValue());

                //Recorremos todos los objetos deserializados
                foreach (var data in json)
                {
                    //Tratamos de buscar si coincide con el nombre del objeto seteado en OpenInfo.name
                    if (data.titulo.Replace(" ", "").Equals(OpenInfo.name.Replace("\n", "").Replace(" ", "")))
                    {
                        descripcion = data.descripcion
                            .Replace("<p>", "")
                            .Replace("</p>", "")
                            .Replace("&#8211;", "- ")
                            .Replace("&nbsp;", "\n")
                            .Replace("<li>", "*")
                            .Replace("</li>", "</line-height>")
                            .Replace("<ul>", "")
                            .Replace("</ul>", "")
                            .Replace("<strong>", "<b>")
                            .Replace("</strong>", "</b>")
                            .Replace(
                                "<p dir=" + '"' + "ltr" + '"' + " data-placeholder=" + '"' + "Traducción" + '"' + ">",
                                "")
                            .Replace(
                                "<p id=" + '"' + "tw-target-text" + '"' + " class=" + '"' +
                                "tw-data-text tw-text-large XcVN5d tw-ta" + '"' + " dir=" + '"' + "ltr" + '"' +
                                " data-placeholder=" + '"' + "Traducción" + '"' + ">", "")
                            .Replace(
                                "<p class=" + '"' + "tw-data-text tw-text-large XcVN5d tw-ta" + '"' + " dir=" + '"' +
                                "ltr" + '"' + " data-placeholder=" + '"' + "Traducción" + '"' + ">", "\n")
                            .Replace("<span class=" + '"' + "Y2IQFc" + '"' + " lang=" + '"' + "en" + '"' + ">",
                                "<font-weight=400>")
                            .Replace("<span style=" + '"' + "font-weight: 400;" + '"' + ">", "<font-weight=400>")
                            .Replace(
                                "<li style=" + '"' + "font-weight: 400;" + '"' + " aria-level=" + '"' + "1" + '"' + ">",
                                "* <line-height=120%><font-weight=400>")
                            .Replace("</span>", "</font-weight>")
                            .Replace("<p style=" + '"' + "text-align: center;" + '"' + ">", "<align=center>");
                        textDescripcion.text = descripcion;
                        titulo.text = data.titulo;
                        img_url = data.imagen;
                    }
                }
        }
        
        [RuntimeInitializeOnLoadMethod]
        IEnumerator makeRequestImage()
        {
            //Realizamos una petición al servidor
            yield return new WaitForSeconds(2);
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(img_url);
            yield return request.SendWebRequest();
            
            if (request.result != UnityWebRequest.Result.Success )
            {
                //La petición falla
                Debug.Log(request.error);
            }
            else
            {
                //Construimos la imágen
                texture =  ((DownloadHandlerTexture)request.downloadHandler).texture;
                imagen.texture = texture;
                imgGameObject.SetActive(true);
                imagen.SetAllDirty();
                scrollArea.SetActive(true);
                imgCarga.SetActive(false);
            }
        }
    }
}