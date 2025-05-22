// Add this to your text GameObject
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CyberPunkFont : MonoBehaviour {
    void Start() {
        GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
        GetComponent<TextMeshProUGUI>().fontMaterial.EnableKeyword("GLOW_ON");
    }
}
