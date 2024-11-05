using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemiesController
{
    public float attack2Range = 2.5f; // Bán kính tấn công của Attack2
    public Vector2 attack2Offset = new Vector2(0.4f, -0.2f); // Vị trí tấn công của Attack2

    private void Awake()
    {
        Health = 50; // Giá trị máu dành riêng cho Boss
        attackInterval = 5f; // Thời gian giữa các lần tấn công của Boss
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(BossAttackSequenceCoroutine());
    }

    private IEnumerator BossAttackSequenceCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval); 
            PerformAttack1();
                        yield return new WaitForSeconds(0.5f); 

            AllowPlayerHitAgain(); 

            yield return new WaitForSeconds(attackInterval); 
            PerformAttack2();
                        yield return new WaitForSeconds(0.5f); 

            AllowPlayerHitAgain(); 
        }
    }

    private void PerformAttack1()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(CheckForPlayerHitCoroutine(attackRange, Vector2.zero)); // Sử dụng phạm vi và vị trí mặc định của Attack1
        }
        else
        {
            Debug.LogError("Animator không được tìm thấy trên Boss");
        }
    }

    private void PerformAttack2()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack2Trigger");
            StartCoroutine(CheckForPlayerHitCoroutine(attack2Range, attack2Offset)); // Sử dụng phạm vi và vị trí của Attack2
        }
        else
        {
            Debug.LogError("Animator không được tìm thấy trên Boss");
        }
    }

    private IEnumerator CheckForPlayerHitCoroutine(float range, Vector2 offset)
    {
        yield return new WaitForSeconds(0.5f); 
        DetectPlayerHit(); 
    }

    // Hàm kiểm tra va chạm với Player
    public void DetectPlayerHit()
    {
        Vector2 attackPosition = (Vector2)transform.position + attack2Offset; // Tính vị trí tấn công với offset cho Attack2
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPosition, attack2Range, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            if (!isPlayerHit) // Kiểm tra xem player có đang bị tấn công hay không
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damageAmount); // Gây sát thương cho player
                    isPlayerHit = true; // Đánh dấu rằng player đã bị tấn công
                    Debug.Log($"Player hit: {player.name}"); // Kiểm tra player bị trúng
                }
            }
        }
    }
}
 