using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RanaMovement : MonoBehaviour
{
    public float velocidad;
    public float JumpForce;

    private Rigidbody2D rb;
    private float horizontal;
    private Animator animator;
    private bool grounded;
    public bool muerto = false; // publico para que se llame desde la instancia para arregar el bug de morir y revivir
    public static RanaMovement Instancia;

    [Header("Límites")]
    [SerializeField] private float alturaMuerte;
    [SerializeField] private float alturaMaxima;

    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoSaltar;

    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioClip sonidoGanar;

    [SerializeField] private AudioSource audioSource3;
    [SerializeField] private AudioClip sonidoPerder;

    [Header("Música de Fondo")]
    [SerializeField] private AudioSource musicaFondo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        alturaMaxima = 21f;
        alturaMuerte = -12f;
    }

    void Awake()
    {
        Instancia = this;
        Time.timeScale = 1f;
    }

    void Update()
    {
        horizontal = Keyboard.current.aKey.isPressed ? -1 :
                     Keyboard.current.dKey.isPressed ? 1 : 0;

        Debug.DrawRay(transform.position, Vector3.down * 0.6f, Color.red);
        grounded = Physics2D.Raycast(transform.position, Vector3.down, 0.6f);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && grounded)
        {
            Jump();
        }

        animator.SetBool("running", horizontal != 0.0f);

        if (horizontal < 0.0f)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (horizontal > 0.0f)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (!muerto && transform.position.y < alturaMuerte)
        {
            muerto = true;
            Morir();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * velocidad, rb.linearVelocity.y);

        if (transform.position.y >= alturaMaxima)
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            }

            transform.position = new Vector2(transform.position.x, alturaMaxima);
        }
    }

    private void Jump()
    {
        audioSource.PlayOneShot(sonidoSaltar);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
    }

    public void Revivir()
    {
        muerto = false;
        rb.linearVelocity = Vector2.zero;
    }

    public void Ganar()
    {
        MenuGanar menu = FindAnyObjectByType<MenuGanar>();

        if (menu != null && musicaFondo.isPlaying)
        {
            musicaFondo.Stop();
            audioSource2.PlayOneShot(sonidoGanar);
            menu.MostrarGanar();
        }
    }

    public void Morir()
    {
        MenuMorir menu = FindAnyObjectByType<MenuMorir>();

        if (menu != null)
        {
            audioSource3.PlayOneShot(sonidoPerder);
            menu.MostrarGameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bandera"))
        {
            Ganar();
        }
    }
}