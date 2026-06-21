using TMPro;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;

public class UIMessage : MonoBehaviour
{
    public GameObject MessagePanel;
    public GameObject ScrewdriverRequiredMessage;
    public GameObject GasmaskMessage;
    public GameObject ScrewdriverMessage;

    public void ShowScrewdriverRequiredMessage()
    {
        if (MessagePanel != null && ScrewdriverRequiredMessage != null)
        {
            MessagePanel.SetActive(true);
            ScrewdriverRequiredMessage.SetActive(true);
        }
    }

    public void ShowGasmaskMessage()
    {
        if (MessagePanel != null && GasmaskMessage != null)
        {
            MessagePanel.SetActive(true);
            GasmaskMessage.SetActive(true);
        }
    }

    public void ShowScrewdriverMessage()
    {
        if (MessagePanel != null && ScrewdriverMessage != null)
        {
            MessagePanel.SetActive(true);
            ScrewdriverMessage.SetActive(true);
        }
    }

}
