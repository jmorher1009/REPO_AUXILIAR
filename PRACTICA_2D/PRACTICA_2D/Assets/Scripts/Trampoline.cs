using UnityEngine;

public class Trampolin : MonoBehaviour
{
    public float fuerzaSalto = 18f;
    public Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoTrampolin;

    private float tiempoCooldown = 0.2f;
    private float tiempoUltimoSalto = 0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Comprobamos si ha pasado suficiente tiempo desde el último salto
            if (Time.time >= tiempoUltimoSalto + tiempoCooldown)
            {
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

                // Solo si cae hacia abajo (o está en el punto exacto de 0)
                if (rb != null && rb.linearVelocity.y <= 0.05f)
                {
                    tiempoUltimoSalto = Time.time;
                    audioSource.PlayOneShot(sonidoTrampolin);

                    animator.SetTrigger("Bounce");

                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                    rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
                }
            }
        }
    }
}