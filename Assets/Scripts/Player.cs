using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;

public class Player : MonoBehaviour
{
    public PlayerConfig Config;

    [SerializeField] int maxHealth = 100;

    [SerializeField] float speed = 0.7f;
    [SerializeField] float rotateSpeed = 0.3f;

    [SerializeField] TMP_Text damagePopup;

    int health;
    HealthBar healthBar;

    public PlayerInput playerInput;
    Vector2 moveInput;
    float rotateInput;

    [SerializeField] SpriteRenderer headRenderer;
    [SerializeField] Rigidbody2D headRigidbody;

    CinemachineImpulseSource impulseSource;
    [SerializeField] int cameraShakeMinThreshold = 7;
    [SerializeField] float cameraShakeMultiplier = 0.02f;

    public void Init(PlayerConfig config, HealthBar healthBar)
    {
        Config = config;
        this.healthBar = healthBar;
    }

    void Start()
    {
        health = maxHealth;
        
        impulseSource = GetComponent<CinemachineImpulseSource>();

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
        }

        UpdateColor(Config.Color);
        if (Config.HeadSprite != null)
        {
            headRenderer.sprite = Config.HeadSprite;
        }
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        headRigidbody.AddForce(moveInput * speed);
    }

    void Rotate()
    {
        headRigidbody.AddTorque((-rotateInput) * rotateSpeed);
    }

    public void Damage(int dmg, Vector2 collisionVelocity)
    {
        if (dmg >= cameraShakeMinThreshold)
        {
            impulseSource.GenerateImpulseWithVelocity(collisionVelocity * cameraShakeMultiplier);
        }
        
        var popup = Instantiate(damagePopup, headRigidbody.transform.position, Quaternion.identity);
        popup.text = dmg.ToString();

        health -= dmg;
        healthBar.SetFill(Mathf.Clamp((float)health/maxHealth, 0f, 1f));

        if (health <= 0)
        {
            gameObject.SetActive(false);
            GameManager.instance.OnPlayerDeath(this);
        }
    }

    void UpdateColor(Color newColor)
    {
        if (headRenderer != null)
        {
            headRenderer.color = newColor;
        }

        if (healthBar != null)
        {
            healthBar.SetColor(newColor);
        }

        foreach (var handRenderer in GetComponentsInChildren<Hand>())
        {
            handRenderer.GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    #region Input
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnRotate(InputValue value)
    {
        rotateInput = value.Get<float>();
    }
    #endregion
}
