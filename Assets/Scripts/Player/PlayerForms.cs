using System.Collections;
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
    public Animator animator;

    public ParticleSystem damageEffect;
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

    private void Update()
    {
        if (gameOver)
        {
            animator.SetBool("Death", true);
            animator.gameObject.transform.rotation = Quaternion.Euler(0, 258, 0);
        }
    }
    public void SiguienteForma()
    {
        if (!gameOver)
        {
            health--;
            StartCoroutine(PlayDamageEffect());
            if (health <= 0)
            {
                GameOver();
                return;
            }
            ActivarForma(health);
        }
    }

    private IEnumerator PlayDamageEffect()
    {
        damageEffect.Play();
        AudioManager.Instance.PlaySFXPlayerDamage();
        yield return new WaitForSeconds(0.3f);
    }

    public void GameOver()
    {
        gameOver = true;
        UIManager.Instance.ShowGameOver();
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
        animator.SetBool("Death", false);
        health = 3;
        gameOver = false;
        ActivarForma(health);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        playerClickMove.SetPlayerStart(true);
        animator.gameObject.transform.rotation = Quaternion.Euler(-73, 175, 0);
    }
}