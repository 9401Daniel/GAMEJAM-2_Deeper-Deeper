using UnityEngine;

public class MoveUp : MonoBehaviour

{
    [SerializeField] private float speed = 1f;
    private bool isPlaying = false;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isPlaying)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        }
    }

    public void SetIsPlaying(bool isPlaying)
    {
        this.isPlaying = isPlaying;
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
    }
}
