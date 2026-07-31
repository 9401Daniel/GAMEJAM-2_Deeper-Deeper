using UnityEngine;

public class SaveScore : MonoBehaviour
{
    public static SaveScore Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveHighScore(string score)
    {
        string currentHighScore = GetHighScore();
        if (string.IsNullOrEmpty(currentHighScore))
        {
            PlayerPrefs.SetString("HighScore", score);
            PlayerPrefs.Save();
            Debug.Log("New high score saved: " + score);
            return;
        }
        int segundosGuardados = TiempoASegundos(currentHighScore);
        int segundosNuevos = TiempoASegundos(score);

        if (segundosNuevos > segundosGuardados)
        {
            // El nuevo puntaje es mejor (menor tiempo)
            PlayerPrefs.SetString("HighScore", score);
            PlayerPrefs.Save();
            Debug.Log("¡Nuevo récord!");
        }
        else
        {
            Debug.Log("No superó el mejor tiempo.");
        }
    }

    public string GetHighScore()
    {
        return PlayerPrefs.GetString("HighScore");
    }

    public static int TiempoASegundos(string tiempo)
    {
        string[] partes = tiempo.Split(':');

        int minutos = int.Parse(partes[0]);
        int segundos = int.Parse(partes[1]);

        return minutos * 60 + segundos;
    }
}
