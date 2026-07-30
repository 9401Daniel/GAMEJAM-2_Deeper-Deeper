using UnityEngine;

public class RotatePropeller : MonoBehaviour
{
    [SerializeField, Range(200, 500)] private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
    }
}