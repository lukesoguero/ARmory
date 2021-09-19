using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EQUIPPED
{
    CROSSBOW,
    SWORD,
    SHIELD
};

public class Player : MonoBehaviour
{
    #region Singleton Setup and Awake
    public static Player Instance
    {
        get
        {
            return instance;
        }
    }

    private static Player instance = null;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public EQUIPPED currentEquipped = EQUIPPED.CROSSBOW;

    public GameObject crossbow;
    public GameObject sword;
    public GameObject shield;

    // Start is called before the first frame update
    void Start()
    {
        EquipCrossbow();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            crossbow.GetComponent<Crossbow>().Shoot();
        }
    }

    public void EquipCrossbow()
    {
        //Debug.Log("Crossbow equipped");
        currentEquipped = EQUIPPED.CROSSBOW;

        crossbow.SetActive(true);
        sword.SetActive(false);
        shield.SetActive(false);
    }

    public void EquipSword()
    {
        //Debug.Log("Sword equipped");
        currentEquipped = EQUIPPED.SWORD;

        sword.SetActive(true);
        crossbow.SetActive(false);
        shield.SetActive(false);
    }

    public void EquipShield()
    {
        //Debug.Log("Shield equipped");
        currentEquipped = EQUIPPED.SHIELD;

        shield.SetActive(true);
        sword.SetActive(false);
        crossbow.SetActive(false);
    }
}
