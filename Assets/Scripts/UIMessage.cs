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
    public float messageDuration = 3f;

    // Coroutine to handle message display timing
    private Coroutine hideCoroutine;

    private void HideAllMessages()
    {
        if (ScrewdriverRequiredMessage != null) ScrewdriverRequiredMessage.SetActive(false);
        if (GasmaskMessage != null) GasmaskMessage.SetActive(false);
        if (ScrewdriverMessage != null) ScrewdriverMessage.SetActive(false);
    }

    private void ShowMessage(GameObject messageToShow)
    {
        if (MessagePanel == null || messageToShow == null) return;

        HideAllMessages();
        MessagePanel.SetActive(true);
        messageToShow.SetActive(true);

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideMessageAfterDelay());
    }

    private System.Collections.IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);

        HideAllMessages();
        if (MessagePanel != null)
        {
            MessagePanel.SetActive(false);
        }

        hideCoroutine = null;
    }

    public void ShowScrewdriverRequiredMessage()
    {
        ShowMessage(ScrewdriverRequiredMessage);
    }

    public void ShowGasmaskMessage()
    {
        ShowMessage(GasmaskMessage);
    }

    public void ShowScrewdriverMessage()
    {
        ShowMessage(ScrewdriverMessage);
    }

}
