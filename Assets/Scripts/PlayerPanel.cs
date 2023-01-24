using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] Image headImage;
    [SerializeField] TMP_Text nameText;

    public PlayerObject PlayerObj;

    private Sprite[] heads;
    private int currentHeadIndex = 0;

    private Color[] colors;
    private int currentColorIndex = 0;

    void Awake()
    {
        heads = Resources.LoadAll<Sprite>("Heads");
        colors = Resources.Load<ColorList>("ColorList").Colors;
    }

    private void Start()
    {
        nameText.text = PlayerObj.Name;
        PlayerObj.OnUINavigate += OnNavigate;

        currentHeadIndex = PlayerObj.HeadIndex;
        currentColorIndex = PlayerObj.ColorIndex;
        UpdateChange();
    }

    public void PreviousHead()
    {
        currentHeadIndex--;
        if (currentHeadIndex < 0)
        {
            currentHeadIndex = heads.Length - 1;
        }
        UpdateChange();
    }

    public void NextHead()
    {
        currentHeadIndex++;
        if (currentHeadIndex >= heads.Length)
        {
            currentHeadIndex = 0;
        }
        UpdateChange();
    }

    public void PreviousColor()
    {
        currentColorIndex--;
        if (currentColorIndex < 0)
        {
            currentColorIndex = colors.Length - 1;
        }
        UpdateChange();
    }

    public void NextColor()
    {
        currentColorIndex++;
        if (currentColorIndex >= colors.Length)
        {
            currentColorIndex = 0;
        }
        UpdateChange();
    }

    private void UpdateChange()
    {
        PlayerObj.HeadIndex = currentHeadIndex;
        PlayerObj.HeadSprite = heads[currentHeadIndex];
        PlayerObj.ColorIndex = currentColorIndex;
        PlayerObj.Color = colors[currentColorIndex];

        headImage.color = PlayerObj.Color;
        headImage.sprite = PlayerObj.HeadSprite;
    }

    private void OnNavigate(Vector2 value)
    {
        if (value == Vector2.up)
        {
            NextColor();
        }
        else if (value == Vector2.down)
        {
            PreviousColor();
        }
        else if (value == Vector2.right)
        {
            NextHead();
        }
        else if (value == Vector2.left)
        {
            PreviousHead();
        }
    }
}
