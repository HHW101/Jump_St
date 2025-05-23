using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    // Start is called before the first frame update
    public ItemData itemData;
    public Action addItem;
    public Transform dropPosition;
    private void Awake()
    {
        
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        CharaterManager.Instance.Player=this;
    }
}
