using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager Instance;

    [SerializeField] private ChatMessage chatMessagePrefab;
    [SerializeField] private CanvasGroup chatContent;
    [SerializeField] private TMP_InputField chatInput;

    public string playerName;



    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one ChatManager instance!");
        }
        Instance = this;
    }



    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
    }



    private void GameManager_OnGameStarted(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.GetLocalPlayerType() == GameManager.PlayerType.Cross)
        {
            playerName = "CrossPlayer";
        }
        else
        {
            playerName = "CirclePlayer";
        }
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage(chatInput.text, playerName);
            chatInput.text = "";
        }
    }



    public void SendChatMessage(string message, string fromWho = null)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        string s = fromWho + ": " + message;
        SendChatMessageServerRpc(s);
    }



    private void AddMessage(string message)
    {
        ChatMessage chatMessage = Instantiate(chatMessagePrefab, chatContent.transform);
        chatMessage.SetText(message);
    }



    [ServerRpc(RequireOwnership = false)]
    private void SendChatMessageServerRpc(string message)
    {
        ReceiveChatMessageClientRpc(message);
    }



    [ClientRpc]
    private void ReceiveChatMessageClientRpc(string message)
    {
        AddMessage(message);
    }
}
