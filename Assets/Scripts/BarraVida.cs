using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [SerializeField] private Image rellenoBarraVida; // Usamos SerializeField para mejor depuración
    private CristianMovimiento cristianMovimiento;
    private float vidaMaxima;

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
    }

    void ActualizarBarraVida()
    {
        // Verificar todas las referencias antes de usarlas
        if (rellenoBarraVida != null && cristianMovimiento != null)
        {
            float saludActual = cristianMovimiento.Salud;
            rellenoBarraVida.fillAmount = Mathf.Clamp01(saludActual / vidaMaxima);
        }
    }
}