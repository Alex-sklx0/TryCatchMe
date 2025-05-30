using UnityEngine;

public class DisparoHieloScript : MonoBehaviour
{    private CristianMovimiento _cristian;
 private const float _factorRalentizacion=0.5f; 
    private const float _duracionRalentizacion = 3f; 
    public Vector3 Direccion { get; set; }
    public float velocidad = 5f;
    public float _dano = 0.75f;

    void Update()
    {
        transform.position += Direccion * velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        _cristian = collision.GetComponent<CristianMovimiento>();

        if (_cristian != null)
        {
            _cristian.AplicarRalentizacion(_factorRalentizacion, _duracionRalentizacion);
            _cristian.Golpe(_dano); 
        }

        Destroy(gameObject);
    }
    
        Destroy(gameObject);
    
}

}