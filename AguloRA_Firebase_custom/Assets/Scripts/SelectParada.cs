using UnityEngine;
using UnityEngine.UI;


public class SelectParada : MonoBehaviour
{
    //Variables públicas
    [SerializeField]
    public Toggle lugares;
    
    [SerializeField]
    public Toggle personajes;
    
    [SerializeField]
    public Toggle arquitectura;
    
    [SerializeField]
    public Toggle tradiciones;
    
    [SerializeField]
    public Toggle historiaAborigen;
    
    [SerializeField]
    public GameObject iniciarVisita;
    
    //Variables privadas
    private bool imgActive;
    private int nSelect = 0;
    
    private void Awake()
    {
        iniciarVisita.SetActive(false);
    }
    
    public void addOrRemoveParada()
    {
        //TODO COMPROBAR
        Lenguage.idioma = (Lenguage.idioma == null) ? "es" : Lenguage.idioma;
    }

    public void paradasAmostrar()
    {
        //Recorremos la lista de paradas
        foreach (var parada in Paradas.instance.listaParadas)
        {
            Parada p = (Parada) parada;
            //Buscamos las que pertenezcan al tipo seleccionado
            if (Paradas.active.Contains(p.Tipo.ToLower()))
            {
                //Si se encuentra se pone a true para que se muestre en el mapa
                p.Visible = true;
            }
            else
            {
                //Si no está se setea a false su visibilidad
                p.Visible = false;
            }
        }
    }

    public void setToggle(Toggle toggle)
    {
        //Seteamos el valor del checkbox
        if (toggle.isOn)
        {
            //Si está a on el numero de checkbox aumenta (nSelectOff)
            nSelect++;
            
            //Comprobamos a que tipo pertenece el checkbox
            list(toggle.name.ToLower(), true);
            
            //Añadimos a la lista de tipo de parada activa
            Paradas.active.Add(toggle.name.ToLower());
        }
        else
        {
            nSelect--;
            list(toggle.name.ToLower(), false);
            Paradas.active.Remove(toggle.name.ToLower());
        }
    }

    private void list(string nombre, bool active)
    {
        //Seteamos los valores de la clase CheckBoxState según el valor de los checkbox de la escena VisitScene
        switch (nombre)
        {
            case "lugares": 
                CheckboxsState.instance.Lugares = active;
                break;
            case "personajes": 
                CheckboxsState.instance.Personajes = active;
                break;
            case "arquitectura": 
                CheckboxsState.instance.Arquitectura = active;
                break;
            case "tradiciones": 
                CheckboxsState.instance.Tradiciones = active;
                break;
            case "historiaaborigen": 
                CheckboxsState.instance.HistoriaAborigen = active;
                break;
        }
    }

    private void listener(bool state, Toggle toggle)
    {
        //Se encarga de estar atento a los cambios que se produzcan en los valores de los checkboxs
        if (state)
        {
            toggle.isOn = true;
        }
    }

    private void Start()
    {
        //TODO COMPROBAR
        iniciarVisita.SetActive(false);
    }

    private void Update()
    {
        //Comprobamos si hay algún checkbox con valor true,
        //si es a si se visualiza la imágen que permite cambiar a la escena mapa
        if (nSelect == 0)
        {
            imgActive = false;
        }
        else
        {
            imgActive = true;
        }
        
        //Comprobamos cada checkbox y el valor de cada unos de los valores booleanos de la clase CheckBoxState
        listener(CheckboxsState.instance.Lugares, lugares);
        listener(CheckboxsState.instance.Personajes, personajes);
        listener(CheckboxsState.instance.Arquitectura, arquitectura);
        listener(CheckboxsState.instance.Tradiciones, tradiciones);
        listener(CheckboxsState.instance.HistoriaAborigen, historiaAborigen);

        iniciarVisita.SetActive(imgActive);
    }
}