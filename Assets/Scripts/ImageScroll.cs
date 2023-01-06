using UnityEngine;
using UnityEngine.UI;

public class ImageScroll : MonoBehaviour
{
    [SerializeField] Vector2 speed;

    Vector2 offset;
    Material material;

    void Awake()
    {
        material = GetComponent<Image>().material;
    }

    void Update()
    {
        offset = speed * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
