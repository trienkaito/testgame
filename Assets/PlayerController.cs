using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro; // Thêm dòng này

public class PlayerController : MonoBehaviour
{
    public Image healthBar; // Tham chiếu đến thanh máu
    public TMP_Text healthText; // Thay đổi từ Text thành TMP_Text
    public float maxHealth = 100f; // Máu tối đa
    private float currentHealth;
    public float moveSpeed = 0.5f; // Tốc độ di chuyển của player
    public float collisionOffset = 1f; // Khoảng cách giữa player và vật thể
    public ContactFilter2D movementFilter;
    public LayerMask enemyLayer; // Lớp Layer cho quái vật
    List<RaycastHit2D> castCollision = new List<RaycastHit2D>();
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer render;
    public bool canMove = true;

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>(); // Nhận input từ bàn phím
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack"); // Kích hoạt animator chém
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateHealthBar();
        UpdateHealthText(); // Cập nhật số lượng máu ngay từ đầu
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = tryMove(movementInput);
                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            render.flipX = movementInput.x < 0; // Lật sprite theo hướng di chuyển
        }
    }

    private bool tryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollision,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
            );

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }

            return false;
        }
        return false;
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnLockMovement()
    {
        canMove = true;
    }

    // Hàm được gọi từ Animation Event khi chém kết thúc
    public void CheckHit()
    {
        Debug.Log("Animation Event Triggered");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1.5f, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemiesController enemyController = enemy.GetComponent<EnemiesController>();
            if (enemyController != null)
            {
                Debug.Log($"Enemy hit: {enemy.name}");
                enemyController.Health -= 1; // Gây sát thương lên quái
                Debug.Log($"Enemy health: {enemyController.Health}");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f); // Kích thước của vùng va chạm
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player health: {currentHealth}");
        UpdateHealthBar();
        UpdateHealthText(); // Cập nhật số lượng máu
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
        UpdateHealthText(); 
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth; // Cập nhật thanh máu
    }

    private void UpdateHealthText()
    {
        healthText.text = $"Health: {currentHealth}/{maxHealth}"; // Cập nhật số lượng máu
    }
}
