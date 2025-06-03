using UnityEngine;

public class DisparoHielo : MonoBehaviour
{

    //constantes
    private const float FactorRalentizacion=0.5f; 
    private const float DuracionRalentizacion = 3f; 
    private const float Velocidad = 0.3f;
    private const float Dano = 0.75f;
    //variables privadas
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
            _cristianScript.AplicarRalentizacion(FactorRalentizacion, DuracionRalentizacion);
            _cristianScript.Golpe(Dano);
            DestruirDisparo();
        }

        else if (!collision.isTrigger || collision.CompareTag("Disparo"))
        {
            DestruirDisparo();
        }
            }

}

