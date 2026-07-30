using System.Collections;
using UnityEngine;
public class Tentacle : EnemyBase
{
    [SerializeField] protected float speedHorizontal; // Velocidad horizontal específica para Tentacle
    [SerializeField] protected float timeToChangeDirection = 2f; // Tiempo para cambiar de dirección horizontal
    protected Coroutine changeDirectionCoroutine;
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
            speedHorizontal *= -1;
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