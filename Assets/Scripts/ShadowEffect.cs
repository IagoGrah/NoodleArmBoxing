using UnityEngine;

public class ShadowEffect : MonoBehaviour
{
    [SerializeField] Vector3 Offset = new(0.05f, -0.05f);
    [SerializeField] Material Material;

    GameObject shadow;

    void Start()
    {
        shadow = new GameObject("Shadow");
        shadow.transform.parent = transform;

        shadow.transform.position = transform.position + Offset;
        shadow.transform.localRotation = Quaternion.identity;
        shadow.transform.localScale = Vector3.one;

        var rend = GetComponent<SpriteRenderer>();
        var shadowRend = shadow.AddComponent<SpriteRenderer>();
        shadowRend.sprite = rend.sprite;
        shadowRend.material = Material;

        shadowRend.sortingLayerName = rend.sortingLayerName;
        shadowRend.sortingOrder = rend.sortingOrder - 1;
    }

    void LateUpdate()
    {
        shadow.transform.SetPositionAndRotation(transform.position + Offset, Quaternion.identity);
        //shadow.transform.eulerAngles = transform.eulerAngles;
    }
}
