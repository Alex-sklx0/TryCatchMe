using UnityEngine;

public class DisparoFuegoScript : MonoBehaviour
{
    public Vector3 Direccion { get; set; }
    public float velocidad = 5f;
    public float danoBase = 1f;

    void Update()
    {
        transform.position += Direccion * velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CristianMovimiento cristian = collision.GetComponent<CristianMovimiento>();
            if (cristian != null)
            {
                float danoFinal = danoBase * 1.25f;
                cristian.Golpe(danoFinal); // Asegúrate de que este método acepte `float`
            }

            Destroy(gameObject);
        }
        else if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}

