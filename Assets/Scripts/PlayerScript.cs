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
            Healing healing = other.GetComponent<Healing>();
            if (healing == null)
            {
                Debug.LogWarning("Healing object is missing Healing on: " + other.gameObject.name);
                return;
            }

            healing.HealPlayer(this);
            Debug.Log("Player healed. Current health: " + Health);

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("damage"))
        {
            Damage damage = other.GetComponent<Damage>();
            if (damage == null)
            {
                Debug.LogWarning("Damage object is missing Damage on: " + other.gameObject.name);
                return;
            }

            if (damage.DamagePlayerPeriodic(this))
            {
                Debug.Log("Player damaged. Current health: " + Health);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("damage")) return;

        Damage damage = other.GetComponent<Damage>();
        if (damage == null) return;

        if (damage.DamagePlayerPeriodic(this))
        {
            Debug.Log("Player damaged. Current health: " + Health);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("damage")) return;

        Damage damage = other.GetComponent<Damage>();
        if (damage == null) return;

        damage.ResetDamageTimer(this);
    }

    
}
