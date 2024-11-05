using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    protected Animator animator;
    public float health = 3; // Máu của enemy
    public LayerMask playerLayer; // Lớp Layer cho player
    public float damageAmount = 5f; // Số lượng sát thương
    public float attackRange = 3f; // Bán kính tấn công
    public float attackInterval = 5f; // Thời gian giữa các lần tấn công
    protected bool isPlayerHit = false; // Biến kiểm tra xem player có bị tấn công hay không

    public float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    public void Defeated()
    {
        animator.SetTrigger("Defeated");
        Invoke("removeObj", 1.0f); // Hủy đối tượng sau 1 giây để đảm bảo animation hoàn thành
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        while (true) // Vòng lặp vô tận
        {
            yield return new WaitForSeconds(attackInterval); // Đợi thời gian giữa các lần tấn công
            Attack(); // Gọi hàm tấn công

            yield return new WaitForSeconds(0.5f); // Đợi 0.5 giây để kiểm tra xem player có bị tấn công hay không
            AllowPlayerHitAgain(); // Gọi hàm cho phép player bị tấn công lại   
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack"); // Gọi animation tấn công
    }

    // Hàm này sẽ được gọi từ Animation Event
    public void CheckForPlayerHit()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            if (!isPlayerHit) // Kiểm tra xem player có đang bị tấn công hay không
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damageAmount); // Gọi hàm trừ máu
                    isPlayerHit = true; // Đánh dấu rằng player đã bị tấn công
                    Debug.Log($"Player hit: {player.name}"); // Kiểm tra player bị trúng
                }
            }
        }
    }

    // Hàm này có thể được gọi sau một khoảng thời gian để cho phép player bị tấn công lại
    public void AllowPlayerHitAgain()
    {
        isPlayerHit = false; // Cho phép player bị tấn công lại
    }

    void removeObj()
    {
        Destroy(gameObject);
    }
}
