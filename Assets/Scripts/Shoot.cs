using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour, IInteractable
{
    public float shootPower = 1000.0f;
    public bool isOn;
   
    string IInteractable.GetInteractPrompt()
    {
        return $"E를 클릭해 발사";
    }

    void IInteractable.OnInteract()
    {
      
        if (isOn)
        {
            Debug.Log("클릭");
            Vector3 sho0t = (transform.forward + transform.up).normalized * shootPower;
            EventBus.Publish("OnShoot", sho0t);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        isOn = true;
    }
    private void OnCollisionExit(Collision other) { isOn = false; }


}
