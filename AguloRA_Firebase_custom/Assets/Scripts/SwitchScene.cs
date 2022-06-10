using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
   //Texto nombre paradas por defecto
   private string[] clickParada = new []
   {
      "Clica sobre una Parada ...",
      "Click on a stop ...",
      "Klicken Sie auf eine Haltestelle ..."
   };

   public void switchScene(int scene)
   {
      //Cambiamos al número de escena que
      //se pasa por parámetro
      SceneManager.LoadScene(scene);
   }
   public void switchSceneactivarAr(Text text)
   {
      //Activa el botón para cambiar a la escena de AR
      if (!text.text.Equals(clickParada[Lenguage.posIdioma]))
      {
         SceneManager.LoadScene(3);
      }
   }

   public void switchSceneIdiomas()
   {
      //Se encarga de hacer el cambio a la escena idiomas desde otra escena
      OpenInfo.cambioIdioma = true;
      SceneManager.LoadScene(0);
   }
}
