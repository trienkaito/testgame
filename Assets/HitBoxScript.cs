using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitBoxScript : MonoBehaviour
{
    public float damage = 1;
    public Vector3 faceRight = new Vector3(0.145f, -0.005f, 0);
    public Vector3 faceLeft = new Vector3(-0.145f, -0.005f, 0);

    private void Start()
    {
    }

    private void Update()
    {

    }
    
    void IsFacingRight(bool isRight){
        if(isRight){
            gameObject.transform.localPosition = faceRight;
        }
        else{
            gameObject.transform.localPosition = faceLeft;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemies")
        {
            EnemiesController enemy = other.GetComponent<EnemiesController>();

            if (enemy != null)
            {
                enemy.Health -= damage;
            }
        }
    }

}
