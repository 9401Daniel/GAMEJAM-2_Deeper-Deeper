using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerForms : MonoBehaviour
{
    public GameObject forma1;
    public GameObject forma2;
    public GameObject forma3;

    private int formaActual = 0;

    void Start()
    {
        ActivarForma(0);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SiguienteForma();
        }
    }

    public void SiguienteForma()
    {
        formaActual++;
        if (formaActual > 2)
        {
            Debug.Log("GAME OVER");
            formaActual = 0;
            return;
        }
        ActivarForma(formaActual);
    }

    private void ActivarForma(int index)
    {
        forma1.SetActive(index == 0);
        forma2.SetActive(index == 1);
        forma3.SetActive(index == 2);
    }
}