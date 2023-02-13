using UnityEngine;

public class ShadowEffect : MonoBehaviour
{
    [SerializeField] Vector3 Offset = new(0.05f, -0.05f);
    [SerializeField] SpriteRenderer ShadowPrefab;

    SpriteRenderer shadow;

    void Start()
    {
        shadow = Instantiate(ShadowPrefab);
        shadow.transform.parent = transform;

        shadow.transform.position = transform.position + Offset;
        shadow.transform.localRotation = Quaternion.identity;
        shadow.transform.localScale = Vector3.one;

        var rend = GetComponent<SpriteRenderer>();
        shadow.sprite = rend.sprite;
    }

    void LateUpdate()
    {
        shadow.transform.SetPositionAndRotation(transform.position + Offset, Quaternion.identity);
    }
}
