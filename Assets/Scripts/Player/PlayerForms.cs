using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerForms : MonoBehaviour
{
    public GameObject forma1;
    public GameObject forma2;
    public GameObject forma3;
    private int health = 3;
    private bool gameOver = false;
   
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
        EnemyBase[] enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        foreach (EnemyBase enemy in enemies) 
        {
            Destroy(enemy.gameObject);
        }
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
}