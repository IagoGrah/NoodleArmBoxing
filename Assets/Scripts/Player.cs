using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public PlayerObject PlayerObject;

    [SerializeField] int maxHealth = 100;

    [SerializeField] float speed = 0.7f;
    [SerializeField] float rotateSpeed = 0.3f;

    [SerializeField] TMP_Text damagePopup;
    [SerializeField] ParticleSystem punchParticle;

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

    public void Init(PlayerObject playerObj, HealthBar healthBar)
    {
        PlayerObject = playerObj;
        PlayerObject.OnPlayerMove += OnMove;
        PlayerObject.OnPlayerRotate += OnRotate;

        healthBar.SetName(playerObj.Name);
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

        UpdateColor(PlayerObject.Color);
        if (PlayerObject.HeadSprite != null)
        {
            headRenderer.sprite = PlayerObject.HeadSprite;
        }
    }

    void FixedUpdate()
    {
        if (NetworkManager.IsHost)
        {
            Move();
            Rotate();
        }
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

        AudioManager.Instance.PlayPunchSound(dmg);
        var particles = Instantiate(punchParticle, headRigidbody.transform.position, Quaternion.LookRotation(collisionVelocity, Vector3.forward));

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

    public override void OnDestroy()
    {
        PlayerObject.OnPlayerMove -= OnMove;
        PlayerObject.OnPlayerRotate -= OnRotate;
        base.OnDestroy();
    }

    #region Input
    void OnMove(Vector2 value)
    {
        moveInput = value;
    }

    void OnRotate(float value)
    {
        rotateInput = value;
    }
    #endregion
}
