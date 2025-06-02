using UnityEngine;

public class LogicTrapScript : MonoBehaviour, IDanable
{
    [Header("Configuración Básica")]
    //Constantes
    public const float TiempoEntreAtaques;
    public const float RadioDeteccion;
    private const string TagJugador = "Player";

    //Privadas
    private int _salud = 3;
    private float _tiempoUltimoAtaque;

    //Publicas
    public GameObject disparoPrefab;
    public Transform cristian;


    private void Start()
    {
        try
        {
            cristian = GameObject.FindGameObjectWithTag(TagJugador)?.transform;
           
        }
        catch (Exception e)
        {

            Debug.LogError("Error en Start: " + e.Message);
        }

    }

    private void Update()
    {

        if (cristian == null) return;

        Vector2 direccion = (cristian.position - transform.position).normalized;
        if (direccion.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

        try
        {
            // Verificar distancia y tiempo para atacar
            if (Vector2.Distance(transform.position, cristian.position) <= RadioDeteccion &&
                Time.time >= _tiempoUltimoAtaque + TiempoEntreAtaques)
            {
                Disparo();
                _tiempoUltimoAtaque = Time.time;
            }

        }
        catch (Exception e)
        {

            Debug.LogError("Error al momento de atacar:" + e.Message);
        }


    }

    private void Disparo()
    {
        Vector3 direccion = new Vector3(transform.localScale.x, 0.0f, 0.0f);

        if (disparoPrefab == null) return;
        try
        {
            GameObject disparo = Instantiate(disparoPrefab, transform.position + direccion * 0.12f, Quaternion.identity);

            disparo.GetComponent<DisparoCondicionalScript>().Direccion = direccion;
        }
        catch (Exception e)
        {
            
            Debug.LogError("Error al disparar:" + e.Message);  
        }
        // Configurar dirección al jugador
    }

    public void Golpe()
    {
        try
        {
            RecibirDano();
        }
        catch (Exception e)
        {
            Debug.LogError("Error al recibir daño: " + e.Message);
        }
        
    }

    private void RecibirDano()
    {
        _salud --;

        if (_salud <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnDrawGizmosSelected()
    {
        // Color del radio de ataque (rojo transparente)
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawWireSphere(transform.position, RadioDeteccion);
    }
}