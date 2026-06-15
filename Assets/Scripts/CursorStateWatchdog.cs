using UnityEngine;

public class CursorStateWatchdog : MonoBehaviour
{
    [Header("Debug Options")]
    [SerializeField] private bool logEveryChange = true;
    [SerializeField] private bool includeStackTrace = false;

    private static bool hasExpectedState;
    private static bool expectedVisible;
    private static CursorLockMode expectedLockMode;
    private static string expectedSource = "(none)";

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

        bool changedSinceLastFrame = currentVisible != lastVisible || currentLockMode != lastLockMode;
        if (changedSinceLastFrame && logEveryChange)
        {
            Debug.Log($"[CursorWatchdog] Frame {Time.frameCount}: Cursor changed to visible={currentVisible}, lock={currentLockMode}");
        }

        if (hasExpectedState)
        {
            bool differsFromExpected = currentVisible != expectedVisible || currentLockMode != expectedLockMode;
            if (differsFromExpected)
            {
                string message =
                    $"[CursorWatchdog] Frame {Time.frameCount}: External override detected. " +
                    $"Expected visible={expectedVisible}, lock={expectedLockMode} (set by {expectedSource}) but got visible={currentVisible}, lock={currentLockMode}.";

                if (includeStackTrace)
                {
                    Debug.LogWarning(message + "\nStackTrace:\n" + System.Environment.StackTrace);
                }
                else
                {
                    Debug.LogWarning(message);
                }
            }
        }

        lastVisible = currentVisible;
        lastLockMode = currentLockMode;
    }

    public static void ReportExpectedState(bool visible, CursorLockMode lockMode, string source)
    {
        hasExpectedState = true;
        expectedVisible = visible;
        expectedLockMode = lockMode;
        expectedSource = source;

        Debug.Log($"[CursorWatchdog] Frame {Time.frameCount}: Expected state set by {source}: visible={visible}, lock={lockMode}");
    }
}
