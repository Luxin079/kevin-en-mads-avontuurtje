using UnityEngine;
using UnityEngine.UI;

public class ScopeOverlayToggle : MonoBehaviour
{
    //[SerializeField]private Texture scopeTexture; // Assign your PNG texture in Inspector
    private RawImage rawImage;
    [SerializeField] private Camera playerCamera;
    private void Start()
    {
       

        if (playerCamera == null)
        {
            Debug.LogError("ScopeOverlayToggle: No Camera found in parent!");
        }


        rawImage = GetComponent<RawImage>();

      
    }

    private void OnGUI()
    {
       
        
        if (playerCamera.fieldOfView <= 31f && playerCamera.fieldOfView >= 29f)
        {
        
            rawImage.enabled = true;
        
        }
        else if (playerCamera.fieldOfView > 30f) {

      
            rawImage.enabled = false;
        }
    }
}
