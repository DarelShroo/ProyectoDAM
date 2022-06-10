public class Parada
{
    //Constriccion del objeto Parada
    private string nombre;
    private string coordenadas;
    private string tipo;
    private bool visible;

    public Parada(string nombre, string coordenadas, string tipo, bool visible)
    {
        this.nombre = nombre;
        this.coordenadas = coordenadas;
        this.tipo = tipo;
        this.visible = visible;
    }

    public Parada()
    {
    }

    public string Nombre
    {
        get => nombre;
        set => nombre = value;
    }

    public string Coordenadas
    {
        get => coordenadas;
        set => coordenadas = value;
    }

    public string Tipo
    {
        get => tipo;
        set => tipo = value;
    }

    public bool Visible
    {
        get => visible;
        set => visible = value;
    }
}
