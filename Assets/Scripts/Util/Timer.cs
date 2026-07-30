using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private float tiempo = 0f; //En segundos
    private int segundoAnterior = -1;
    public bool Activo { private set; get; } = false;
    public UnityEvent CambioTiempo { private set; get; } = new UnityEvent();
    [SerializeField] private TextMeshProUGUI scoreText;
    private void Start()
    {
        CambioTiempo.AddListener(FormatTime);
    }
    private void Update()
    {
        if (Activo)
        {
            tiempo += Time.deltaTime;
            int segundoActual = TiempoEnSegundos();
            if (segundoActual != segundoAnterior)
            {
                segundoAnterior = segundoActual;
                CambioTiempo?.Invoke();
            }
        }
    }

    public void InitTimer()
    {
        tiempo = 0f;
        segundoAnterior = -1;
        Activo = true;
    }

    public void StopTimer() => Activo = false;
    public int TiempoEnSegundos() => Mathf.FloorToInt(tiempo);
    public void FormatTime()
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        string score = "Depth: " + (string.Format("{0:00}:{1:00}", minutos, segundos));
        scoreText.text = $"{score}";
    }
}
