using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5.0f;
    public float minDist = 2.0f;

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        player = Player.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // follow player
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        if (Vector3.Distance(transform.position, player.transform.position) > minDist) {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        } else {
            animator.SetTrigger("Attack");
        }
    }

    public void Die() {
        animator.SetTrigger("Die");
        Destroy(gameObject, 5.0f);
    }
}
