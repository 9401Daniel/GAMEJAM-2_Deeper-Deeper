using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    private float repeatWidth;
    private void Start()
    {
        repeatWidth = GetComponent<BoxCollider>().size.y / 2;
    }

    private void Update()
    {
        if (transform.position.y > startPos.y + repeatWidth)
        {
            transform.position = startPos;
        }
    }
}