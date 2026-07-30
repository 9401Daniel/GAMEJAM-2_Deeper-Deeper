using System.Collections;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    private float repeatHeight;
    private void Start()
    {
        repeatHeight = GetComponent<BoxCollider>().size.y / 2;
    }

    private void Update()
    {
        if (transform.position.y > startPos.y + repeatHeight - 1)
        {
            transform.position = startPos;
        }
    }
}