using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerForms : MonoBehaviour
{
    public GameObject forma1;
    public GameObject forma2;
    public GameObject forma3;
    private int health = 3;
    public GameObject FormaActual
    {
        get
        {
            if (health == 3) return forma3;
            else if (health == 2) return forma2;
            else return forma1;
        }
    }
    private PlayerClickMove playerClickMove;

    void Start()
    {
        playerClickMove = GetComponent<PlayerClickMove>();
        ActivarForma(3);
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
        health--;
        if (health <= 0)
        {
            Debug.Log("GAME OVER");
            health = 0;
            return;
        }
        ActivarForma(health);
    }

    private void ActivarForma(int index)
    {
        forma1.SetActive(index == 1);
        forma2.SetActive(index == 2);
        forma3.SetActive(index == 3);
        playerClickMove.SetForma(FormaActual);
    }
}