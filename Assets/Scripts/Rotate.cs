using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float speed = 30f;

    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * speed);
    }
}
