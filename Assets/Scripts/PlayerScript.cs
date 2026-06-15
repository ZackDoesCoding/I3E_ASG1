using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public UIManager uiManager;
    public int Health = 5;
    public int MaxHealth = 10;

    private void OnMenu(InputValue value)
    {
        if (!value.isPressed) return;
        if (uiManager == null) return;

        uiManager.toggleMenu();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered with: " + other.gameObject.name + " | tag=" + other.tag);

        if (other.CompareTag("healing")) 
        {

        ModifyHealth modifyHealth = other.GetComponent<ModifyHealth>();
        if (modifyHealth == null)
        {
            Debug.LogWarning("Healing object is missing ModifyHealth on: " + other.gameObject.name);
            return;
        }

        modifyHealth.Heal(this);
        Debug.Log("Player healed. Current health: " + Health);

        Destroy(other.gameObject);
        }
        else if (other.CompareTag("damage"))
        {
            ModifyHealth modifyHealth = other.GetComponent<ModifyHealth>();
            if (modifyHealth == null)
            {
                Debug.LogWarning("Damage object is missing ModifyHealth on: " + other.gameObject.name);
                return;
            }

            modifyHealth.DamagePlayerPeriodic(this);
            Debug.Log("Player damaged. Current health: " + Health);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("damage")) return;

        ModifyHealth modifyHealth = other.GetComponent<ModifyHealth>();
        if (modifyHealth == null) return;

        int previousHealth = Health;
        modifyHealth.DamagePlayerPeriodic(this);

        if (Health != previousHealth)
        {
            Debug.Log("Player damaged. Current health: " + Health);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("damage")) return;

        ModifyHealth modifyHealth = other.GetComponent<ModifyHealth>();
        if (modifyHealth == null) return;

        modifyHealth.ResetDamageTimer(this);
    }

    
}
