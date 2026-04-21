using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
public class GestorRankings : MonoBehaviour
{
    public static GestorRankings Instancia;
    private string rutaArchivo;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        rutaArchivo = Path.Combine(Application.persistentDataPath, "rankings.json");
    }

    public void GuardarNuevaPuntuacion(float tiempo, float puntos, int vidas)
    {
        DatosRankings datos = CargarRankings();
        EntradaRanking nuevaEntrada = new EntradaRanking
        {
            tiempo = tiempo,
            puntos = puntos,
            vidas = vidas,
            fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
        };
        datos.listaRankings.Add(nuevaEntrada);
        datos.listaRankings = datos.listaRankings.OrderBy(t => Mathf.FloorToInt(t.tiempo)).ThenByDescending(t => t.puntos).ThenByDescending(t => t.vidas).Take(10).ToList();

        File.WriteAllText(rutaArchivo, JsonUtility.ToJson(datos, true));
    }

    public DatosRankings CargarRankings()
    {
        if (!File.Exists(rutaArchivo)) return new DatosRankings();

        var datos = JsonUtility.FromJson<DatosRankings>(File.ReadAllText(rutaArchivo));

        datos.listaRankings = datos.listaRankings
            .OrderBy(t => Mathf.FloorToInt(t.tiempo))
            .ThenByDescending(t => t.puntos)
            .ThenByDescending(t => t.vidas)
            .ToList();

        return datos;
    }

    public string ObtenerTextoFormateado()
    {
        DatosRankings datos = CargarRankings();
        if (datos.listaRankings == null || datos.listaRankings.Count == 0)
            return "TOP 10 MEJORES TIEMPOS\n\nNo hay récords registrados.";

        string textoFinal = "TOP 10 MEJORES TIEMPOS\n\n";
        int puesto = 1;
        foreach (var entrada in datos.listaRankings)
        {
            int min = Mathf.FloorToInt(entrada.tiempo / 60);
            int seg = Mathf.FloorToInt(entrada.tiempo % 60);
            textoFinal += $"{puesto}. {min:00}:{seg:00} | Pts: {entrada.puntos} | Vidas: {entrada.vidas}\n";
            puesto++;
        }
        return textoFinal;
    }
}