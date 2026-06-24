using TMPro;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;

public class UIMessage : MonoBehaviour
{
    // MESSAGE PANEL REFERENCES
    public GameObject MessagePanel;
    public GameObject ScrewdriverRequiredMessage;
    public GameObject GasmaskMessage;
    public GameObject ScrewdriverMessage;
    public GameObject DoorLeverMessage1;
    public GameObject DoorLeverMessage2;
    public GameObject BatteriesCollectedMessage;
    public GameObject GeneratorDoorNeedsBatteriesMessage;
    public GameObject BothLeversTurnedOnMessage;
    public float messageDuration = 3f;

    // Keeps track of the active message coroutine so it can be replaced cleanly
    private Coroutine MessageTimer;

    private void HideAllMessages()
    {
        // Hide every message object before showing a new message set
        if (ScrewdriverRequiredMessage != null) ScrewdriverRequiredMessage.SetActive(false);
        if (GasmaskMessage != null) GasmaskMessage.SetActive(false);
        if (ScrewdriverMessage != null) ScrewdriverMessage.SetActive(false);
        if (DoorLeverMessage1 != null) DoorLeverMessage1.SetActive(false);
        if (DoorLeverMessage2 != null) DoorLeverMessage2.SetActive(false);
        if (BatteriesCollectedMessage != null) BatteriesCollectedMessage.SetActive(false);
        if (GeneratorDoorNeedsBatteriesMessage != null) GeneratorDoorNeedsBatteriesMessage.SetActive(false);
        if (BothLeversTurnedOnMessage != null) BothLeversTurnedOnMessage.SetActive(false);
    }

    private void ShowMessages(params GameObject[] messagesToShow)
    {
        // Guard against missing UI references or an empty request
        if (MessagePanel == null || messagesToShow == null || messagesToShow.Length == 0) return;

        // Stop any message that is currently running before showing the next one
        if (MessageTimer != null)
        {
            StopCoroutine(MessageTimer);
            MessageTimer = null;
        }

        HideAllMessages();
        MessagePanel.SetActive(true);

        // Activate the requested message objects
        bool hasAnyMessage = false;
        foreach (GameObject message in messagesToShow)
        {
            if (message == null) continue;

            message.SetActive(true);
            hasAnyMessage = true;
        }

        if (!hasAnyMessage)
        {
            MessagePanel.SetActive(false);
            return;
        }

        MessageTimer = StartCoroutine(HideMessageAfterDelay());
    }

    private System.Collections.IEnumerator HideMessageAfterDelay()
    {
        // Keep the message visible for the configured duration
        yield return new WaitForSeconds(messageDuration);

        HideAllMessages();
        if (MessagePanel != null)
        {
            MessagePanel.SetActive(false);
        }

        MessageTimer = null;
    }

    public void ShowScrewdriverRequiredMessage()
    {
        // Show the screwdriver required message
        ShowMessages(ScrewdriverRequiredMessage);
    }

    public void ShowGasmaskMessage()
    {
        // Show the gasmask required message
        ShowMessages(GasmaskMessage);
    }

    public void ShowScrewdriverMessage()
    {
        // Show the screwdriver pickup message
        ShowMessages(ScrewdriverMessage);
    }

    public void ShowDoorLeverMessage()
    {
        // Show the two lever-door hints one after the other
        if (MessageTimer != null)
        {
            StopCoroutine(MessageTimer);
            MessageTimer = null;
        }

        MessageTimer = StartCoroutine(Sequence());

        // Local coroutine keeps the timing logic close to the caller
        System.Collections.IEnumerator Sequence()
        {
            if (MessagePanel == null)
            {
                MessageTimer = null;
                yield break;
            }

            MessagePanel.SetActive(true);

            if (DoorLeverMessage1 != null)
            {
                // Show the first lever-door message, then wait before the second
                HideAllMessages();
                DoorLeverMessage1.SetActive(true);
                yield return new WaitForSeconds(messageDuration);
            }

            if (DoorLeverMessage2 != null)
            {
                // Show the second lever-door message after the first finishes
                HideAllMessages();
                DoorLeverMessage2.SetActive(true);
                yield return new WaitForSeconds(messageDuration);
            }

            HideAllMessages();
            MessagePanel.SetActive(false);
            MessageTimer = null;
        }
    }

    public void ShowBatteriesCollectedMessage()
    {
        // Show the batteries collected message
        ShowMessages(BatteriesCollectedMessage);
    }

    public void ShowGeneratorDoorNeedsBatteriesMessage()
    {
        // Show the generator door warning message
        ShowMessages(GeneratorDoorNeedsBatteriesMessage);
    }

    public void ShowBothLeversTurnedOnMessage()
    {
        // Show the lever door success message
        ShowMessages(BothLeversTurnedOnMessage);
    }
}
