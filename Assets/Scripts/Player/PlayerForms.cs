using UnityEngine;

public class PlayerForms : MonoBehaviour
{
    public GameObject forma1;
    public GameObject forma2;
    public GameObject forma3;
    private int health = 3;
    private bool gameOver = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public GameObject FormaActual
    {
        get
        {
            if (health == 3) return forma3;
            else if (health == 2) return forma2;
            else return forma1;
        }
    }

    public bool IsGameOver => gameOver;
    private PlayerClickMove playerClickMove;

    void Start()
    {
        playerClickMove = GetComponent<PlayerClickMove>();
        ActivarForma(3);
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    public void SiguienteForma()
    {
        if (!gameOver)
        {
            health--;
            if (health <= 0)
            {
                GameOver();
                return;
            }

            AudioManager.Instance.PlaySFXPlayerDamage();
            ActivarForma(health);

        }
    }

    public void GameOver()
    {
        gameOver = true;
        UIManager.Instance.ShowGameOver();
        AudioManager.Instance.PlaySFXPlayerDeath();
        Debug.Log("GAME OVER");
        health = 0;
    }

    private void ActivarForma(int index)
    {
        forma1.SetActive(index == 1);
        forma2.SetActive(index == 2);
        forma3.SetActive(index == 3);
        playerClickMove.SetForma(FormaActual);
    }

    public void ResetPlayer()
    {
        health = 3;
        gameOver = false;
        ActivarForma(health);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        playerClickMove.SetPlayerStart(true);
    }
}