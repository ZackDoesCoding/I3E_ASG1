using UnityEngine;

public class CursorStateWatchdog : MonoBehaviour
{
    private bool lastVisible;
    private CursorLockMode lastLockMode;

    private void Awake()
    {
        lastVisible = Cursor.visible;
        lastLockMode = Cursor.lockState;
    }

    private void LateUpdate()
    {
        bool currentVisible = Cursor.visible;
        CursorLockMode currentLockMode = Cursor.lockState;

        lastVisible = currentVisible;
        lastLockMode = currentLockMode;
    }

    public static void ReportExpectedState(bool visible, CursorLockMode lockMode, string source)
    {
        // Intentionally left blank.
    }
}
