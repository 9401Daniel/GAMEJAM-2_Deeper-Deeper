using UnityEngine;

public class MoveUp : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
    }
}
