using UnityEngine;
public class SeaUrchin : EnemyBase
{
    [Header("Rotación de Caída (Free Fall)")]
    [Tooltip("Velocidad mínima a la que puede girar.")]
    [SerializeField] private float minTumbleSpeed = 60f;
    [Tooltip("Velocidad máxima a la que puede girar.")]
    [SerializeField] private float maxTumbleSpeed = 180f;

    // Variables internas para guardar cómo va a girar este erizo específico
    private Vector3 randomRotationAxis;
    private float tumbleSpeed;
    private void Start()
    {
        // Al instanciarse, calculamos un eje de rotación totalmente aleatorio.
        // Random.onUnitSphere genera una dirección 3D al azar perfecta para esto.
        randomRotationAxis = Random.onUnitSphere;

        // Le asignamos una velocidad aleatoria entre tus dos valores.
        tumbleSpeed = Random.Range(minTumbleSpeed, maxTumbleSpeed);
    }
    protected override void Update()
    {
        // Mantener la caída y la autodestrucción
        base.Update();

        // Girar el objeto usando su eje y velocidad únicos.
        // Usamos Space.World para que la rotación se sienta externa (caótica) y no robótica.
        transform.Rotate(randomRotationAxis * tumbleSpeed * Time.deltaTime, Space.World);
    }
}