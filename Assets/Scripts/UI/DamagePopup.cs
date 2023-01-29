using System.Collections;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] float fadeDuration;
    [SerializeField] Vector2 moveDirection;

    [SerializeField] TMP_Text text;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        var fadeColor = text.color;
        
        Vector2 startingPosition = transform.position;
        Vector2 targetPosition = startingPosition + moveDirection;

        var timePassed = 0f;
        while (timePassed < fadeDuration)
        {
            var lerpFactor = timePassed / fadeDuration;
            
            fadeColor.a = Mathf.Lerp(1f, 0f, lerpFactor);
            text.color = fadeColor;

            transform.position = Vector2.Lerp(startingPosition, targetPosition, lerpFactor);

            timePassed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
