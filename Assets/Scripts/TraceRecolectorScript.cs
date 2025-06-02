using UnityEngine;

public class TraceRecolector : MonoBehaviour
{
    public int tracesRecolectados = 0;
    public int totalTracesNivel = 3;

    public bool TieneTodosLosTraces()
    {
        return tracesRecolectados >= totalTracesNivel;
    }

    public void RecogerTrace()
    {
        tracesRecolectados++;
        Debug.Log($"Trace recogido. Total: {tracesRecolectados}");
    }

    public void ReiniciarTraces()
    {
        tracesRecolectados = 0;
    }
}
