using UnityEngine;
using UnityEngine.SceneManagement;

public class NivelesSystem : MonoBehaviour
{
    public void BtnN1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
