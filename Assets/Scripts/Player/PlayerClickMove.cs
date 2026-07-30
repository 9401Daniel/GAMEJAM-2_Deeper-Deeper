using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClickMove : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Rotación")]
    public float velocidadRotacion = 12f; // Ajusta qué tan rápido da la vuelta el submarino
    private Rigidbody rb;
    private Vector3 destino;
    private bool tieneDestino = false;
    private GameObject forma;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        destino = transform.position;
    }

    void Update()
    {
        // 1. Detección de Clic (Igual que tu código)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane pared = new Plane(Vector3.forward, transform.position);

            if (pared.Raycast(ray, out float distancia))
            {
                Vector3 punto = ray.GetPoint(distancia);
                destino = new Vector3(punto.x, punto.y, transform.position.z);
                tieneDestino = true;
            }
        }

        // 2. Mover y Rotar
        if (tieneDestino)
        {
            // --- MOVIMIENTO ---
            Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
            rb.MovePosition(nuevaPosicion);
            // --- ROTACIÓN ---
            Vector3 direccion = (destino - transform.position).normalized;
            if (direccion != Vector3.zero)
            {
                // Al usar Vector3.back (hacia la cámara), los vectores nunca chocan,
                // eliminando el "mirroring" y los giros raros por completo.
                Quaternion rotacionObjetivo;
                if (forma.name != "Diver Assets")
                {
                    rotacionObjetivo = Quaternion.LookRotation(direccion, Vector3.up);
                }
                else
                {
                    rotacionObjetivo = Quaternion.LookRotation(direccion, Vector3.forward);
                }

                // Si el modelo se acuesta o se ve raro, cambia Vector3.back por Vector3.forward
                forma.transform.rotation = Quaternion.Slerp(forma.transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
            }

            // --- DETENCIÓN ---
            // Si estamos lo suficientemente cerca, nos detenemos
            if (Vector3.Distance(transform.position, destino) < 0.05f)
            {
                tieneDestino = false;
            }
        }
    }

    public void SetForma(GameObject nuevaForma)
    {
        forma = nuevaForma;
    }
}