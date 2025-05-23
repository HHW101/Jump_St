using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerCondition : MonoBehaviour,IDamagable
{
    // Start is called before the first frame update
    
    public UICondition uicondition;
    Condition health { get { return uicondition.health; } }
    Condition stamina { get { return uicondition.stamina; } }
    public float Stamina { get { return uicondition.stamina.nowValue; } }
    void Update()
    {
        if (Stamina <= 0)
            CharaterManager.Instance.Player.controller.RunEnd();
        if (!CharaterManager.Instance.Player.controller.isRun)
            stamina.Add(5 * Time.deltaTime);
        if (CharaterManager.Instance.Player.controller.isRun)
            stamina.Add(-10 * Time.deltaTime);
        if (health.nowValue < 0f)
        {
            Die();
        }
    }
   
    public bool UseStamina(float amount)
    {
        if(stamina.nowValue < amount)
              return false;
        stamina.Add(-amount);
        return true;
    }
    public void Heal(float amount)
    {
        health.Add(amount);
    }
    public void HealS(float amount)
    {
        stamina.Add(amount);
    }
    public void Die()
    {
        Debug.Log("»ç¸Á");
    }

    public void TakeDamage(int damageAmount)
    {
        health.Add(-damageAmount);
       
    }
}
