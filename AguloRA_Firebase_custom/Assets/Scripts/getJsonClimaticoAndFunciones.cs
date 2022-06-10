using System;
using System.Collections;
using Assets.Mapbox.Unity.MeshGeneration.Modifiers.MeshModifiers;
using Firebase;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class getJsonClimaticoAndFunciones : MonoBehaviour
    {
        //Variables públicas serializadas   
        [SerializeField]
        public RawImage imagen;

        [SerializeField]
        public GameObject imgGameObject;
        
        [SerializeField]
        public TextMeshProUGUI textMeshProObject;
        
        [SerializeField]
        public GameObject imgCarga;
        
        [SerializeField]
        public Text textBanner;
        
        [SerializeField]
        public GameObject scrollArea;
        
        //Variables privadas
        private string descripcion;
        private Texture texture;
        private string img_url;
        private Uri url;
        private ResetStateCheckBox _resetStateCheckBox;
        
        private string[] textBannerClimatico = new[]
        {
            //Texto Banner Cambio Climatico
            "CAMBIO CLIMATICO",
            "CLIMATE CHANGE",
            "KLIMAWANDEL"
        }; 
        
        private string[] textBannerAppFunciones = new[]
        {
            //Texto banner APP Y FUNCIONES
            "APP Y FUNCIONES",
            "APP AND FUNCTION",
            "APP UND FUNKTIONEN"
        };

        [SerializeField] private FirebaseApp _app;
        [SerializeField] private DatabaseReference _reference;

        private void Start()
        {
            getTextCambioClimaticoAndFunc();
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


        [RuntimeInitializeOnLoadMethod]
        public void getTextCambioClimaticoAndFunc()
        {
       
            //Obtenemos el número de escena en la que nos encontramos
            int nEscena = SceneManager.GetActiveScene().buildIndex;
            
            //Seteamos el texto  del banner de la escena según su idioma y la escena en la que nos encontramos
            textBanner.text = (nEscena == 4) ? textBannerClimatico[Lenguage.posIdioma] : textBannerAppFunciones[Lenguage.posIdioma];
            //Inicializamos la petición al gestor de contenidos
            //StartCoroutine(makeRequest());

            loadData();
            //Inicializamos la obtención de la textura desde el gestor de contenidos para posteriormente convertirla a imagen
            StartCoroutine(makeRequestImage());
        }

  
       

        private void  loadData()
        {
            //Obtenemos el número de escena en la que nos encontramos
            int nEscena = SceneManager.GetActiveScene().buildIndex;

            FirebaseDatabase.DefaultInstance.GetReference(nEscena == 4 ? "cambio-cli" : "app-func").ValueChanged += valueChange;
        }

        //Descarga de Datos FirebaseDatabase
        private void valueChange(object sender, ValueChangedEventArgs e)
        {
            var objeto = e.Snapshot.Child(Lenguage.idioma);
            //Filtramos los datos traidos
                    descripcion = objeto.Child("descripcion").GetValue(true).ToString()
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
                        .Replace("<p dir=" + '"' + "ltr" + '"' + " data-placeholder=" + '"' + "Traducción" + '"' + ">",
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
                        .Replace("</span>", "</font-weight>");
                    Debug.Log("<p class=" + '"' + "tw-data-text tw-text-large XcVN5d tw-ta" + '"' + "dir=" + '"' +
                              "ltr" + '"' + " data-placeholder=" + '"' + "Traducción" + '"' + ">");
                    textMeshProObject.text = descripcion;
                    //Seteamos la url de la imágen
                    img_url = objeto.Child("imagen").GetValue(true).ToString();
        }

        [RuntimeInitializeOnLoadMethod]

        
        //Descarga JSON con JsonDotNet
        IEnumerator makeRequest()
        {
            //Realizamos una petición al servidor
                UnityWebRequest request = UnityWebRequest.Get(url + Lenguage.idioma);
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    //La petición falla
                    Debug.Log(request.error);
                }
                else
                {
                    //Si la petición es satisfactoria intentamos serializar el objeto y traernos el json del gestor de contenidos.
                    var objeto =
                        JsonConvert.DeserializeObject<ObjectImgAndText>(request.downloadHandler.text);
                    //Filtramos los datos traidos
                    descripcion = objeto.descripcion
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
                        .Replace("<p dir=" + '"' + "ltr" + '"' + " data-placeholder=" + '"' + "Traducción" + '"' + ">",
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
                        .Replace("</span>", "</font-weight>");
                    Debug.Log("<p class=" + '"' + "tw-data-text tw-text-large XcVN5d tw-ta" + '"' + "dir=" + '"' +
                              "ltr" + '"' + " data-placeholder=" + '"' + "Traducción" + '"' + ">");
                    textMeshProObject.text = descripcion;
                    //Seteamos la url de la imágen
                    img_url = objeto.imagen;
                }
           
        }
        [RuntimeInitializeOnLoadMethod]
        IEnumerator makeRequestImage()
        {
            //Realiza una petición al servidor
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