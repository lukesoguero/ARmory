using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 700f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(projectile, transform.position, transform.rotation);
        arrow.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, launchVelocity, 0));
        Destroy(arrow, 3.0f);
    }
}
