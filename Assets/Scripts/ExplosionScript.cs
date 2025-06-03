using UnityEngine;

public class ExplosionTechoScript : MonoBehaviour
{
    private const int Duracion = 1 ;
    private const float Radio = 0.2f ;
    private const int  Dano = 1;
    private CristianMovimiento _cristianScript;

    private void Start()
    {
        DestruirDisparo(Duracion);
    }
    private void DestruirDisparo(int duracion)
    {
                Destroy(gameObject, duracion);

    }

   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out _cristianScript))
        {
            _cristianScript.Golpe(Dano);
            DestruirDisparo(Duracion);
        }
    }

}
