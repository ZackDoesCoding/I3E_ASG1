using UnityEngine;

public class Vent : MonoBehaviour
{
    public ScewInteractable Screw1;
    public ScewInteractable Screw2;
    public ScewInteractable Screw3;
    public ScewInteractable Screw4;

    private void Update()
    {
        RemoveScrews();
    }

    public void RemoveScrews()
    {
        if (Screw1 == null || Screw2 == null || Screw3 == null || Screw4 == null)
        {
            return;
        }

        if (Screw1.UnScrewed &&
            Screw2.UnScrewed &&
            Screw3.UnScrewed &&
            Screw4.UnScrewed)
        {
            gameObject.SetActive(false);
        }
    }

}
