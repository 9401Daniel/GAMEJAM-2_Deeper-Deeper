using System.Collections;
using UnityEngine;
public class Shark : EnemyBase
{
    [SerializeField] protected float speedHorizontal; // Velocidad horizontal específica para Tentacle
    [SerializeField] protected float timeToChangeDirection = 2f; // Tiempo para cambiar de dirección horizontal
    protected Coroutine changeDirectionCoroutine;

    private Vector2 rotationAngles = new Vector2(0f, 180f); // Ángulos de rotación para cambiar la dirección
    private bool isFacingRight = true; // Estado de la dirección actual

    protected override void Move()
    {
        base.Move(); // Llamamos al movimiento vertical de la clase base
        transform.Translate(Vector3.right * speedHorizontal * Time.deltaTime, Space.World);
        changeDirectionCoroutine ??= StartCoroutine(ChangeDirection());
    }

    protected virtual IEnumerator ChangeDirection()
    {
        while (true)
        {
            isFacingRight = !isFacingRight;
            speedHorizontal *= -1;
            transform.localRotation = Quaternion.Euler(0f, isFacingRight ? rotationAngles.x : rotationAngles.y, 0f); // Cambia la rotación según la dirección
            yield return new WaitForSeconds(timeToChangeDirection);
        }
    }

    void OnDestroy()
    {
        if (changeDirectionCoroutine != null)
        {
            StopCoroutine(changeDirectionCoroutine);
        }
    }
}