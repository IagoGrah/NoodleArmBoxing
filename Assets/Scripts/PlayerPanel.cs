using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] Image headImage;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_InputField nameInput;

    [SerializeField] GameObject arrows;
    [SerializeField] GameObject readyText;
    [SerializeField] GameObject removeButton;

    [HideInInspector] public PlayerObject PlayerObj;

    public float readyHeadScaleFactor = 1.3f;

    private Sprite[] heads;
    private int currentHeadIndex = 0;

    private Color[] colors;
    private int currentColorIndex = 0;

    private bool isReady;
    private bool isOnlyPlayer => PlayersManager.Instance.Players.Count < 2;

    void Awake()
    {
        heads = Resources.LoadAll<Sprite>("Heads");
        colors = Resources.Load<ColorList>("ColorList").Colors;
    }

    private void Start()
    {
        nameInput.text = PlayerObj.Name;
        nameText.text = PlayerObj.Name;

        PlayerObj.OnUINavigate += OnNavigate;
        PlayerObj.OnUISubmit += OnSubmit;
        PlayerObj.OnUICancel += OnCancel;

        PlayersManager.Instance.InputManager.onPlayerJoined += OnPlayerJoined;
        PlayersManager.Instance.InputManager.onPlayerLeft += OnPlayerLeft;

        UpdateRemoveButton();

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

    public void RemovePlayer()
    {
        Destroy(PlayerObj.gameObject);
        Destroy(gameObject);
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

    public void UpdateName()
    {
        PlayerObj.Name = nameInput.text;
        nameText.text = nameInput.text;
    }

    public void OnSelectNameInput()
    {
        var keyboardPlayer = PlayersManager.Instance.Players.Find(x => x.PlayerInput.currentControlScheme == "Keyboard&Mouse");
        if (keyboardPlayer != null)
        {
            keyboardPlayer.PlayerInput.DeactivateInput();
        }
    }

    public void OnDeselectNameInput()
    {
        var keyboardPlayer = PlayersManager.Instance.Players.Find(x => x.PlayerInput.currentControlScheme == "Keyboard&Mouse");
        if (keyboardPlayer != null)
        {
            keyboardPlayer.PlayerInput.ActivateInput();
        }
    }

    private void OnNavigate(Vector2 value)
    {
        if (!isReady)
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

    private void SetReady(bool ready)
    {
        isReady = ready;
        readyText.SetActive(ready);
        arrows.SetActive(!ready);
        headImage.rectTransform.localScale *= ready ? readyHeadScaleFactor : (1 / readyHeadScaleFactor);
        nameInput.gameObject.SetActive(!ready);
        nameText.gameObject.SetActive(ready);
        UpdateRemoveButton();
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        UpdateRemoveButton();
    }

    private void OnPlayerLeft(PlayerInput player)
    {
        UpdateRemoveButton();
    }

    private void UpdateRemoveButton()
    {
        removeButton.SetActive(!isReady && !isOnlyPlayer);
    }

    private void OnSubmit()
    {
        if (!isReady)
        {
            SetReady(true);
        }
    }
    private void OnCancel()
    {
        if (isReady)
        {
            SetReady(false);
        }
        else if (!isOnlyPlayer)
        {
            RemovePlayer();
        }
    }

    private void OnDestroy()
    {
        PlayerObj.OnUINavigate -= OnNavigate;
        PlayerObj.OnUISubmit -= OnSubmit;
        PlayerObj.OnUICancel -= OnCancel;

        PlayersManager.Instance.InputManager.onPlayerJoined -= OnPlayerJoined;
        PlayersManager.Instance.InputManager.onPlayerLeft -= OnPlayerLeft;
    }
}
