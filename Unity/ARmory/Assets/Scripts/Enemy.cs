using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Spawner spawner;
    public float moveSpeed = 5.0f;
    public float minDist = 2.0f;
    public float blockDist = 4.0f;

    private Animator animator;
    private bool blocked = false;

    void Start() {
        animator = GetComponent<Animator>();
        player = Player.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // follow player
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        if(!blocked)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > minDist) 
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            else 
                animator.SetTrigger("Attack");
        }
        else
        {
            if(Vector3.Distance(transform.position, player.transform.position) < blockDist)
                transform.position -= transform.forward * 3 * Time.deltaTime;
            else
                blocked = false;
        }
    }

    public void Die() {
        animator.SetTrigger("Die");
        spawner.enemyCount--;
        Destroy(gameObject, 1.3f);
    }

    public void Block()
    {
        blocked = true;
    }
}
