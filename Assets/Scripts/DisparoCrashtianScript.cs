using UnityEngine;

public class DisparoCrashtianScript : MonoBehaviour
{
    public float velocidad = 5f;
    public float duracionVida = 5f;
    public float dano = 2f;
    public AudioClip sonidoDisparo;

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {

        
        _rigidbody2D = GetComponent<Rigidbody2D>();

        // Buscar al jugador
        GameObject cristianObj = GameObject.FindGameObjectWithTag("Player");
        if (cristianObj != null)
        {
            Vector3 direccion = (cristianObj.transform.position - transform.position).normalized;
            if (direccion.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

            _rigidbody2D.linearVelocity = direccion * velocidad;
            
        }

        // Reproducir sonido
        if (sonidoDisparo != null && Camera.main != null)
        {
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(sonidoDisparo);
            }
        }

        // Autodestrucción después de cierto tiempo
        Destroy(gameObject, duracionVida);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignora al jefe (tag opcional)
        if (other.CompareTag("Crashtian") )
            return;

        // Daño a Cristian si lo golpea
        CristianMovimiento cristian = other.GetComponent<CristianMovimiento>();
        if (cristian != null)
        {
            cristian.Golpe(dano);
        }

        Destroy(gameObject);
    }
}
