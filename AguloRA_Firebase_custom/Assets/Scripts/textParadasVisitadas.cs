using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class textParadasVisitadas : MonoBehaviour
{
    public Text text;
    
    private string[] es = new []
    {
        "¡Ánimo, estás a punto de dar el primer paso!",
        "¡Estupendo, lo más difícil ya está hecho!",
        "¡Ánimo, ya queda poco para completar las visitas!",
        "¡Enhorabuena!, has visitado todas las paradas"
    };
    
    private string[] en = new []
    {
        "Come on, you are about to take the first step!",
        "Great, the hardest thing is already done!",
        "Come on, there is little left to complete the visits!", 
        "Congratulations! You have visited all the stops"
    };

    private string[] de = new []
    {
        "Komm schon, du machst den ersten Schritt!",
        "Super, das Schwierigste ist schon getan!",
        "Mut, es bleibt wenig übrig, um die Besuche abzuschließen!",
        "Herzlichen Glückwunsch, Sie haben alle Stationen besucht"
    };
    

    // Update is called once per frame
    void Update()
    {
        try
        {
            string path = Application.persistentDataPath + "/paradasVisitadas.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            text.text = textParada(Lenguage.idioma, nParadas(path));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private string textParada(string leng, int n)
    {
        int posArraytext = n <= 6 ? 0 : (n <= 10) ? 1 : (n < 15 ) ? 2 : n == 15 ? 3 : 0;
        string text = "";

        if (Lenguage.idioma == null)
        {
            leng = "es";
        }
        
        switch (leng)
        {
            case "es":
                text = "Has visitado " + n + " lugares de 15.\n"+ es[posArraytext];
                break;
            case "en": 
                text = "You have visited " + n + " stops out of 15.\n"+ en[posArraytext];
                break;
            case "de":
                text = "Sie haben " + n + " von 15 Haltestellen besucht.\n" +de[posArraytext];
                break;
        }
        
        return text;
    }


    private static int nParadas(string path)
    {
        int nParadasVisitadas;
        
        var lineas = File.ReadLines(path);
        var enumerable = lineas.ToList();
        nParadasVisitadas= enumerable.Count();
        
        return nParadasVisitadas;
    }
}
