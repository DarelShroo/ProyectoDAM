using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class Lenguage : MonoBehaviour
{
    public static Lenguage instance;
    public static string idioma = "es";
    public static int posIdioma ;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            //Se ejecuta por primera vez e intenta leer el fichero de preferencias
            string path = Application.persistentDataPath + "/preference.txt";
            
            try
            {
                var lineas = File.ReadLines(path);
                var enumerable = lineas.ToList();
                if (File.Exists(path))
                {
                    idioma = enumerable[0];
                }
            }
            catch (Exception e)
            {
                Debug.Log("Se desborda el array contenido en el fichero de preferencias en idioma");
            }
            
            //Evitamos el objeto original se destruya
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

    private void Update()
    {
        //Actualizamos el índice del idioma
        posIdioma = idioma == "es" ? 0 : idioma == "en" ? 1 : idioma == "de" ? 2 : 0;
    }

    public void setIdioma(string leng)
    {
        //Seteamos los valores de idioma "es" "en" "de"
        idioma = leng;
    }
}
