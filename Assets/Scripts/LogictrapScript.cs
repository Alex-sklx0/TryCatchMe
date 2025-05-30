using UnityEngine;

public class LogicTrapScript : MonoBehaviour
{
    [Header("Configuración Básica")]
    public float tiempoEntreAtaques;
    public float _rangoDeteccion;
    private int _salud = 3;
    public GameObject disparoPrefab;

    public Transform cristian;
    private float _tiempoUltimoAtaque;

    private void Start()
    {
        cristian = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
            if (cristian == null) return;

Vector2 direccion = (cristian.position - transform.position).normalized;
        if (direccion.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        // Verificar distancia y tiempo para atacar
        if (Vector2.Distance(transform.position, cristian.position) <= _rangoDeteccion &&
            Time.time >= _tiempoUltimoAtaque + tiempoEntreAtaques)
        {
            Disparo();
            _tiempoUltimoAtaque = Time.time;
        }
    }

    private void Disparo()
    {
        Vector3 direccion = new Vector3(transform.localScale.x, 0.0f, 0.0f);

        if (disparoPrefab == null) return;

               GameObject disparo = Instantiate(disparoPrefab, transform.position + direccion * 0.12f, Quaternion.identity);

                disparo.GetComponent<DisparoCondicionalScript>().Direccion = direccion;


        // Configurar dirección al jugador

    }

    public void Golpe()
    {
        _salud -= 1;
        if (_salud <= 0)
        {
            Destroy(gameObject);
        }
    }
      void OnDrawGizmosSelected()
{
    // Color del radio de ataque (rojo transparente)
    Gizmos.color = new Color(1, 0, 0, 0.4f);
    Gizmos.DrawWireSphere(transform.position, _rangoDeteccion);
}
}