using TMPro;
using UnityEngine;

public class Puntuaje : MonoBehaviour
{
    private float puntos;
    private TextMeshProUGUI textMesh;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        ActualizarTexto();
    }

    public void SumarPuntos(float puntosM)
    {
        puntos += puntosM;
        ActualizarTexto();
    }

    public float ObtenerPuntos()
    {
        return puntos;
    }

    public void EstablecerPuntos(float nuevosPuntos)
    {
        puntos = nuevosPuntos;
        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        if (textMesh != null)
        {
            textMesh.text = "Puntos: " + puntos;
        }
    }
}