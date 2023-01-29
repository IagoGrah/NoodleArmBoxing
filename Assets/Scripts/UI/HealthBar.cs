using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] TMP_Text nameText;

    public void SetFill(float fill)
    {
        fillImage.fillAmount = fill;
    }

    public void SetColor(Color color)
    {
        fillImage.color = color;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }
}
