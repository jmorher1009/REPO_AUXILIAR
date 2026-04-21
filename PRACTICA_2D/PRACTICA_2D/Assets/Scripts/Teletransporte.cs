using UnityEngine;

public class Teletransporte : MonoBehaviour
{
    [Header("Destino")]
    [SerializeField] private Transform puntoDestino;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoTeleport;
    void Start()
    {
        if (puntoDestino == null && transform.childCount > 0)
        {
            puntoDestino = transform.GetChild(0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                audioSource.PlayOneShot(sonidoTeleport);
                rb.position = puntoDestino.position;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}