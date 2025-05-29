using UnityEngine;

public class ExplosionTechoScript : MonoBehaviour
{
    public float duracion ;
    public float radio ;
    public float dano ;

    void Start()
    {
      
        Destroy(gameObject, duracion);
    }
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CristianMovimiento cristian = other.GetComponent<CristianMovimiento>();
            if (cristian != null)
            {
                cristian.Golpe(dano); // Aplica daño directamente
            }
        }
    }

}
