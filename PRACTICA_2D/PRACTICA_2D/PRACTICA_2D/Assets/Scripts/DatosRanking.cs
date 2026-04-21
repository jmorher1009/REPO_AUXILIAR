using System;
using System.Collections.Generic;

// Estructura para cada fila del ranking
[Serializable]
public class EntradaRanking
{
    public float tiempo;
    public float puntos;
    public int vidas;
    public string fecha;
}

// Clase contenedora para JSON
[Serializable]
public class DatosRankings
{
    public List<EntradaRanking> listaRankings = new List<EntradaRanking>();
}