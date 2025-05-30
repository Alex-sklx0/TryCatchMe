using UnityEngine;
using UnityEngine.UI;

public class BarraVidaScript : MonoBehaviour
{
    [SerializeField] private Image rellenoBarraVida; // Usamos SerializeField para mejor depuración
    private CristianMovimiento cristianMovimiento;
    private float vidaMaxima;
    private bool corazonesOcultos = false;


    void Start()
    {
        // 1. Verificar y asignar la referencia al Image
        if (rellenoBarraVida == null)
        {
            Debug.LogError("No se asignó la imagen de relleno en el Inspector", this);
            return;
        }

        // 2. Buscar al jugador de forma más robusta
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("No se encontró objeto con tag 'Player'", this);
            return;
        }

        // 3. Obtener el componente CristianMovimiento
        cristianMovimiento = playerObj.GetComponent<CristianMovimiento>();
        if (cristianMovimiento == null)
        {
            Debug.LogError("El jugador no tiene componente CristianMovimiento", this);
            return;
        }

        // 4. Inicializar vida máxima
        vidaMaxima = cristianMovimiento.Salud;
        
        // Opcional: Actualizar inmediatamente
        ActualizarBarraVida();
    }

    void Update()
    {
        // Actualizar cada frame (o podrías usar eventos)
        ActualizarBarraVida();
        if (cristianMovimiento == null && !corazonesOcultos)
{
    rellenoBarraVida.gameObject.SetActive(false);
    corazonesOcultos = true;
}

    }

    void ActualizarBarraVida()
    {
        if (rellenoBarraVida != null && cristianMovimiento != null)
        {
            float saludActual = cristianMovimiento.Salud;

            if (saludActual <= 0 && !corazonesOcultos)
            {
                rellenoBarraVida.gameObject.SetActive(false); // Ocultar corazón
                corazonesOcultos = true;
            }
            else if (saludActual > 0 && corazonesOcultos)
            {
                rellenoBarraVida.gameObject.SetActive(true); // Por si revive
                corazonesOcultos = false;
            }

            if (saludActual > 0)
            {
                rellenoBarraVida.fillAmount = Mathf.Clamp01(saludActual / vidaMaxima);
            }

        }
    
}


}