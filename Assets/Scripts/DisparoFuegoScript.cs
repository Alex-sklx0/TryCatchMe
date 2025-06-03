using UnityEngine;

public class DisparoFuego : MonoBehaviour
{
    public const float Velocidad = 0.3f;
    private const float Dano = 1.25f;
    private CristianMovimiento _cristianScript;
    private Vector3 _direccion;
    public Vector3 Direccion
    {
        set
        {
            _direccion = value;
        }
        get
        {
            return _direccion;
        }
    }
    void Update()
    {
        transform.position += Direccion * Velocidad * Time.deltaTime;
    }
    public void DestruirDisparo()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out _cristianScript))
        {
            _cristianScript.Golpe(Dano);
            DestruirDisparo();
        }
        else if (!collision.isTrigger)
        {
            DestruirDisparo();
        }
    }
}

