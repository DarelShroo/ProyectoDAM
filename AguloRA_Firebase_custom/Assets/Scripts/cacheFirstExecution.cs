using System;
using System.IO;
using System.Linq;
using Firebase;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cacheFirstExecution : MonoBehaviour
{
    public GameObject contenedorIdiomas;
    private bool activa = false;
    [SerializeField] private FirebaseApp _app;
    [SerializeField] private DatabaseReference _reference;
    private int nVeces = 0;

    void Start()
    {
   _reference = FirebaseDatabase.DefaultInstance.RootReference;
   
        try
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
                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                } else {
                    UnityEngine.Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
            
            string path = Application.persistentDataPath + "/preference.txt";

            if (!File.Exists(path))
            {
                //Si la ruta no existe pasamos activa a true
                //para que se visualize el contenedor de idiomas
                activa = true;
            }
            if (!OpenInfo.cambioIdioma)
            {
                //Comprueba si se accede desde otra ventana a la escena idiomas.
                //Si no es a si y es la primera ejecución pasamos activa a true
                //Seteamos el idioma obtenido del fichero
                //Cambiamos la escena a primaryScene
                
                var lineas = File.ReadLines(path);
                var enumerable = lineas.ToList();

                activa = true;
                Lenguage.idioma = enumerable[0];
                SceneManager.LoadScene(1);
            }
            else
            {
                activa = true;
            }
        }catch(Exception e){}
    }

    void Update()
    {
        try
        {
            //Actualizamos la visibilidad del contendor de idiomas
            contenedorIdiomas.SetActive(activa);
        }
        catch (Exception e) { }
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

    public void saveData(string idioma)
    {
        try
        {
            //Guardamos las preferencias de idioma en un fichero
            
            string path = Application.persistentDataPath + "/preference.txt";
            File.WriteAllText(path, idioma);
            
            //Seteamos los valores a false para que no se produzcan saltos de escenas
            //inesperados desde las otras ventanas hacia la de idiomas
            FirebaseDatabase.DefaultInstance.GetReference("estadisticas").Child("idioma-mas-usado").Child("idioma").ValueChanged += setNuevoValor;
            OpenInfo.cambioIdioma = false;
            activa = false;
        }
        catch (Exception e)
        {
        }
    }
    
    private void setNuevoValor(object sender, ValueChangedEventArgs e)
    {
        try
        {
            string numeroString = e.Snapshot.Child(Lenguage.idioma)
                .GetValue(true).ToString();

            int numero = Convert.ToInt32(numeroString);

            if (nVeces == 0)
            {
                FirebaseDatabase.DefaultInstance.RootReference.Child("estadisticas")
                    .Child("idioma-mas-usado")
                    .Child("idioma")
                    .Child(Lenguage.idioma).SetValueAsync(numero + 1);
            }

            nVeces++;
        }
        catch (Exception exception) { }
       
    }

}
