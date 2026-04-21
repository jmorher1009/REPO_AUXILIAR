using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class ControladorDatosJuego : MonoBehaviour
{
    private const int NUMERO_VIDAS = 6;
    public static ControladorDatosJuego Instancia;
    public GameObject jugador;
    public DatosJuego datosJuego = new DatosJuego();
    private Cronometro cronometro;
    private Puntuaje puntuaje;

    private void Awake()
    {
        if (Instancia != null)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame) CargarDatos();
        if (Keyboard.current.gKey.wasPressedThisFrame) GuardarDatos();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        jugador = null;
        puntuaje = FindAnyObjectByType<Puntuaje>();

        Debug.Log("Escena cargada — intentando cargar datos");
        CargarDatos();
    }

    int ObtenerSlotActual()
    {
        return PlayerPrefs.GetInt("SlotSeleccionado", 1);
    }

    string ObtenerRutaArchivo()
    {
        int slot = ObtenerSlotActual();
        return Path.Combine(Application.persistentDataPath, "partida_" + slot + ".json");
    }

    GameObject ObtenerJugador()
    {
        if (jugador == null)
        {
            RanaMovement rana = FindAnyObjectByType<RanaMovement>(FindObjectsInactive.Include);
            if (rana != null)
            {
                jugador = rana.gameObject;
            }
        }
        return jugador;
    }

    public void CargarDatos()
    {
        string ruta = ObtenerRutaArchivo();
        if (!File.Exists(ruta))
        {
            Debug.Log("No hay archivo de guardado → empezando desde cero");
            return;
        }

        string contenido = File.ReadAllText(ruta);
        datosJuego = JsonUtility.FromJson<DatosJuego>(contenido);

        GameObject j = ObtenerJugador();
        if (j != null)
        {
            j.transform.position = datosJuego.posicion;

            VidaJugador vida = j.GetComponentInChildren<VidaJugador>(true);
            if (vida != null)
            {
                vida.cantidadVida = datosJuego.vida;
                vida.ActualizarCorazonesUI();
            }
        }

        if (puntuaje == null) puntuaje = FindAnyObjectByType<Puntuaje>();
        if (puntuaje != null) puntuaje.EstablecerPuntos(datosJuego.puntuacion);

        if (cronometro == null) cronometro = FindAnyObjectByType<Cronometro>();
        if (cronometro != null)
        {
            cronometro.EstablecerTiempo(datosJuego.tiempoJugado);
        }

        Debug.Log("CARGADO slot " + ObtenerSlotActual() +
                  " | Puntos: " + datosJuego.puntuacion +
                  " | Tiempo: " + datosJuego.tiempoJugado);
    }

    public void GuardarDatos()
    {
        string ruta = ObtenerRutaArchivo();

        GameObject j = ObtenerJugador();
        if (j == null) return;

        datosJuego.posicion = j.transform.position;
        datosJuego.nombrePartida = "Partida " + ObtenerSlotActual();
        datosJuego.fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        VidaJugador vida = j.GetComponentInChildren<VidaJugador>(true);

        if (vida != null)
        {
            datosJuego.vida = vida.cantidadVida;
        }
        else
        {
            datosJuego.vida = NUMERO_VIDAS;
        }

        if (puntuaje == null) puntuaje = FindAnyObjectByType<Puntuaje>();
        if (puntuaje != null) datosJuego.puntuacion = puntuaje.ObtenerPuntos();
        else datosJuego.puntuacion = 0f;

        if (cronometro == null) cronometro = FindAnyObjectByType<Cronometro>();
        if (cronometro != null)
        {
            datosJuego.tiempoJugado = cronometro.tiempoActual;
        }
        else
        {
            datosJuego.tiempoJugado = 0f;
        }

        string json = JsonUtility.ToJson(datosJuego, true);
        File.WriteAllText(ruta, json);

        Debug.Log("PARTIDA GUARDADA - Vidas: " + datosJuego.vida);
    }

    public void BorrarPartidaActual()
    {
        string ruta = ObtenerRutaArchivo();

        if (File.Exists(ruta))
        {
            File.Delete(ruta);
            Debug.Log("Partida borrada correctamente");
        }
        else
        {
            Debug.Log("No había partida para borrar");
        }
    }
}