using UnityEngine;

public class DeadCodeScript : MonoBehaviour
{
    public Transform cristian;
    public float velocidad = 3f;
    public float distanciaDetencion = 0.5f;
    public float distanciaPersecucion = 10f;
    
    private Rigidbody2D _rigidbody2D;
    private int _salud = 3;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (cristian == null) return;

        Vector3 direccion = cristian.position - transform.position;
        float distancia = direccion.magnitude;

        // Flip sprite
        transform.localScale = new Vector3(
            direccion.x >= 0 ? 1f : -1f, 
            1f, 
            1f);

        // Debug para diagnóstico
        Debug.Log($"Movimiento: {_rigidbody2D.linearVelocity} | Distancia: {distancia} | DentroRango: {distancia < distanciaPersecucion && distancia > distanciaDetencion}");
    }

    void FixedUpdate()
    {
        if (cristian == null) return;

        Vector3 direccion = cristian.position - transform.position;
        float distancia = direccion.magnitude;

        if (distancia < distanciaPersecucion && distancia > distanciaDetencion)
        {
            Vector2 movimiento = new Vector2(
                Mathf.Sign(direccion.x) * velocidad,
                _rigidbody2D.linearVelocity.y);
            
            _rigidbody2D.linearVelocity = movimiento;
        }
        else
        {
            _rigidbody2D.linearVelocity = new Vector2(0f, _rigidbody2D.linearVelocity.y);
        }
    }

    public void Golpe()
    {
        _salud -= 1;
        if (_salud == 0) Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaPersecucion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaDetencion);
    }
}