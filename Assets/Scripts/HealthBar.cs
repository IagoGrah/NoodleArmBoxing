using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fillImage;

    public void SetFill(float fill)
    {
        fillImage.fillAmount = fill;
    }

    public void SetColor(Color color)
    {
        fillImage.color = color;
    }
}
