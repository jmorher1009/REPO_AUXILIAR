using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    private Puntuaje puntos;
    public static List<Moneda> todasLasMonedas = new List<Moneda>();

    [SerializeField] private int cantidadPuntos;
    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioClip sonidoMoneda;
    [SerializeField] private AudioClip sonidoMonedaGrande;

    void Awake()
    {
        todasLasMonedas.Add(this);
    }

    void OnDestroy()
    {
        todasLasMonedas.Remove(this);
    }

    void OnEnable()
    {
        if (puntos == null)
            puntos = FindAnyObjectByType<Puntuaje>();
    }
private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            puntos?.SumarPuntos(cantidadPuntos);

            if (cantidadPuntos > 50)
            {
                audioSource2.PlayOneShot(sonidoMonedaGrande);
            }
            else
            {
                audioSource1.PlayOneShot(sonidoMoneda);
            }

            gameObject.SetActive(false);
        }
    }
}