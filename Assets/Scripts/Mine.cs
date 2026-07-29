using UnityEngine;
public class Mine : EnemyBase
{
    [Header("Configuración de Rotación")]
    [Tooltip("Velocidad a la que girará la mina.")]
    [SerializeField] private float rotationSpeed = 45f;

    [Tooltip("Eje sobre el cual rotará. (0,1,0) para girar como un trompo, (0,0,1) como un volante.")]
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 1, 0);
    protected override void Update()
    {
        // IMPORTANTE: Llamamos a base.Update() para que siga moviéndose hacia arriba 
        // y comprobando si debe destruirse (la lógica que ya hicimos).
        base.Update();

        // Rotamos la mina de manera constante sobre su propio eje local.
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
    }
}