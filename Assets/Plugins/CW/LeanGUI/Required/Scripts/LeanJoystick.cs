using UnityEngine;

public class LeanJoystickControl : MonoBehaviour
{
    [SerializeField] private KeyCode key = KeyCode.None;
    public KeyCode Key { get => key; set => key = value; }

    [SerializeField] private Vector2 delta = new Vector2(0.0f, 10.0f);
    public Vector2 Delta { get => delta; set => delta = value; }

    [SerializeField] private bool scaleByTime = false;
    public bool ScaleByTime { get => scaleByTime; set => scaleByTime = value; }
    public RectTransform Handle { get; private set; }

    private LeanJoystickControl cachedJoystick;

    protected virtual void OnEnable()
    {
        cachedJoystick = GetComponent<LeanJoystickControl>();

        if (cachedJoystick == null)
        {
            Debug.LogError("LeanJoystickControl requires a LeanJoystick component on the same GameObject.", this);
            this.enabled = false; // Disable the script if no joystick is found  
        }
    }

    protected virtual void Update()
    {
        if (cachedJoystick != null && Input.GetKey(key)) // Check if the specified key is held down  
        {
            Vector2 finalDelta = delta;

            // Scale by Time.deltaTime if required  
            if (scaleByTime)
            {
                finalDelta *= Time.deltaTime;
            }

            // Update the joystick's handle position directly  
            Vector2 newPosition = cachedJoystick.Handle.anchoredPosition + finalDelta;

            // Optionally clamp the position if needed (if your joystick supports limits)  
            // newPosition = Vector2.ClampMagnitude(newPosition, maxMagnitude); // Uncomment and set maxMagnitude if needed  

            // Set the new position  
            cachedJoystick.Handle.anchoredPosition = newPosition;
        }
        else
        {
            // If the key is released, you might want to reset the joystick position  
            // Uncomment the line below if you want to reset the position when the key is released  
            // cachedJoystick.Handle.anchoredPosition = Vector2.zero;   
        }
    }
}

internal class LeanJoystick
{
    public RectTransform Handle { get; set; }
    public Vector2 Position { get; internal set; }
}