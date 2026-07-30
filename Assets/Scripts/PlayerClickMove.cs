using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClickMove : MonoBehaviour
{
    public float velocidad = 5f;

    private Rigidbody rb;
    private Vector3 destino;
    private bool tieneDestino = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        destino = transform.position;
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane pared = new Plane(Vector3.forward, transform.position);

            if (pared.Raycast(ray, out float distancia))
            {
                Vector3 punto = ray.GetPoint(distancia);
                // Solo X e Y, Z fijo
                destino = new Vector3(punto.x, punto.y, transform.position.z);
                tieneDestino = true;
            }
        }

        if (tieneDestino)
        {
            Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
            rb.MovePosition(nuevaPosicion);

            if (Vector3.Distance(transform.position, destino) < 0.05f)
                tieneDestino = false;
        }
    }
}