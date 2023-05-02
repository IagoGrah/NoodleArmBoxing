using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image bufferImage;
    [SerializeField] Image fillImage;
    [SerializeField] TMP_Text nameText;

    [SerializeField] float bufferWait = 0.05f;
    [SerializeField] float bufferDuration = 0.5f;

    Coroutine bufferCoroutine;

    public void SetFill(float fill)
    {
        fillImage.fillAmount = fill;
        if (bufferCoroutine != null)
        {
            StopCoroutine(bufferCoroutine);
        }
        bufferCoroutine = StartCoroutine(BufferCoroutine());
    }

    public void SetColor(Color color)
    {
        fillImage.color = color;
        bufferImage.color = Color.Lerp(color, Color.black, 0.5f);
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    private IEnumerator BufferCoroutine()
    {
        var startingFill = bufferImage.fillAmount;
        yield return new WaitForSeconds(bufferWait);

        var counter = 0f;
        while (bufferImage.fillAmount > fillImage.fillAmount)
        {
            bufferImage.fillAmount = Mathf.SmoothStep(startingFill, fillImage.fillAmount, counter / bufferDuration);
            counter += Time.deltaTime;
            yield return null;
        }
    }
}
