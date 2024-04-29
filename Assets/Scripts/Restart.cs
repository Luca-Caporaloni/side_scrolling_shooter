using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    // Nombre de la escena de carga
    public string nombreEscenaCarga = "Cargando...";

    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CargarEscenaCarga();
        }
    }

    private void CargarEscenaCarga()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nombreEscenaCarga); // Carga la escena de carga
    }
}