using UnityEngine;
using Unity.Netcode;
using TMPro;

public class QuestionFlowManager : NetworkBehaviour
{
    [Header("Question Boards (same order!)")]
    public GameObject[] hostBoardsBlue;   // Question1, Question2, ...
    public GameObject[] clientBoardsRed;  // Question1a, Question2a, ...

    [Header("Correct Answer Index per Question")]
    public int[] correctIndex;

    [Header("UI Text")]
    public TextMeshProUGUI heartsText;
    public TextMeshProUGUI scoreText;

    [Header("Respawn")]
    public Transform respawnPoint;

    private NetworkVariable<int> hearts = new NetworkVariable<int>(3);
    private NetworkVariable<int> score  = new NetworkVariable<int>(0);

    // IMPORTANT: -1 means “no question currently open”
    private NetworkVariable<int> currentQuestion = new NetworkVariable<int>(-1);

    void Start()
    {
        HideAllBoards();
        UpdateUI();
    }

    public override void OnNetworkSpawn()
    {
        hearts.OnValueChanged += (_, __) => UpdateUI();
        score.OnValueChanged += (_, __) => UpdateUI();
        currentQuestion.OnValueChanged += (_, __) => ShowCurrentBoardForLocalPlayer();

        if (IsServer)
        {
            // Start with nothing open
            currentQuestion.Value = -1;
        }

        HideAllBoards();
        UpdateUI();
    }

    public void OpenQuestion(int index)
    {
        if (!IsServer) return;

        if (index < 0 || index >= hostBoardsBlue.Length) return;
        currentQuestion.Value = index;
    }

    public void SubmitAnswer(int chosenIndex)
    {
        if (!IsServer)
        {
            SubmitAnswerServerRpc(chosenIndex);
            return;
        }

        ValidateAnswerOnServer(chosenIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitAnswerServerRpc(int chosenIndex)
    {
        ValidateAnswerOnServer(chosenIndex);
    }

    private void ValidateAnswerOnServer(int chosenIndex)
    {
        int q = currentQuestion.Value;

        if (q < 0) return;
        if (q >= correctIndex.Length) return;

        if (chosenIndex == correctIndex[q])
        {
            score.Value++;

            currentQuestion.Value = -1;
            CloseAllBoardsClientRpc();
        }
        else
        {
            hearts.Value--;

            if (hearts.Value <= 0)
            {
                hearts.Value = 3;
                score.Value = 0;

                currentQuestion.Value = -1;
                CloseAllBoardsClientRpc();
                ResetAndRespawnAllPlayers();
            }
        }
    }

    private void ResetAndRespawnAllPlayers()
    {
        if (!IsServer) return;
        if (respawnPoint == null) return;

        foreach (var kvp in NetworkManager.Singleton.ConnectedClients)
        {
            ulong clientId = kvp.Key;

            var rpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            RespawnLocalPlayerClientRpc(respawnPoint.position, respawnPoint.rotation, rpcParams);
        }
    }

    [ClientRpc]
    private void RespawnLocalPlayerClientRpc(Vector3 pos, Quaternion rot, ClientRpcParams rpcParams = default)
    {
        var localPlayer = NetworkManager.Singleton.LocalClient?.PlayerObject;
        if (localPlayer == null) return;

        localPlayer.transform.SetPositionAndRotation(pos, rot);

        var rb = localPlayer.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    [ClientRpc]
    private void CloseAllBoardsClientRpc()
    {
        HideAllBoards();
    }

    private void ShowCurrentBoardForLocalPlayer()
    {
        HideAllBoards();

        int q = currentQuestion.Value;
        if (q < 0) return; // IMPORTANT: show nothing unless triggered

        bool isHostSide = NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer;

        if (isHostSide)
        {
            if (q < hostBoardsBlue.Length && hostBoardsBlue[q] != null)
                hostBoardsBlue[q].SetActive(true);
        }
        else
        {
            if (q < clientBoardsRed.Length && clientBoardsRed[q] != null)
                clientBoardsRed[q].SetActive(true);
        }
    }

    private void HideAllBoards()
    {
        if (hostBoardsBlue != null)
            foreach (var go in hostBoardsBlue) if (go) go.SetActive(false);

        if (clientBoardsRed != null)
            foreach (var go in clientBoardsRed) if (go) go.SetActive(false);
    }

    private void UpdateUI()
    {
        if (heartsText != null) heartsText.text = "Hearts: " + hearts.Value;
        if (scoreText != null)  scoreText.text  = "Scoreboard: " + score.Value;
    }

    public void TriggerQuestion(int index)
    {
        OpenQuestion(index);
    }

    public void AddHeart() {
        if (!IsServer) return; // Only server should modify the NetworkVariable
        hearts.Value += 1;
    }
}
