using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected float speed;
    protected Vector3 moveDirection;

    // Nuevo: Guardará el límite en el eje Y donde el objeto debe morir
    protected float destroyYLimit;

    // Actualizamos el inicializador para recibir el límite
    public virtual void Initialize(float moveSpeed, Vector3 direction, float destroyLimit)
    {
        speed = moveSpeed;
        moveDirection = direction.normalized;
        destroyYLimit = destroyLimit;
    }

    protected virtual void Update()
    {
        Move();
        CheckBounds(); // Comprobamos en cada frame si ya salió de la pantalla
    }

    protected virtual void Move()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    // Nuevo método para verificar la posición
    protected virtual void CheckBounds()
    {
        // Si el enemigo va hacia ARRIBA y su posición actual supera el límite superior...
        if (moveDirection.y > 0 && transform.position.y > destroyYLimit)
        {
            Destroy(gameObject);
        }
        // Si el enemigo va hacia ABAJO y su posición es menor que el límite inferior...
        else if (moveDirection.y < 0 && transform.position.y < destroyYLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aquí puedes agregar la lógica de daño al jugador
            Debug.Log($"{gameObject.name} hit the player!");
            //Destroy(gameObject); // Destruye el enemigo al colisionar con el jugador
        }
    }
}