using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageTextMesh;



    public void SetText(string text)
    {
        messageTextMesh.text = text;
    }
}
