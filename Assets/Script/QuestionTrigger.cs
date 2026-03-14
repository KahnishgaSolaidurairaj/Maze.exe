using UnityEngine;
using Unity.Netcode;

public class QuestionTrigger : MonoBehaviour
{
    public int questionIndex;
    public QuestionFlowManager manager;

    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        if (manager == null) return;
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer) return;

        if (other.CompareTag("Player"))
        {
            used = true;
            manager.TriggerQuestion(questionIndex);
            gameObject.SetActive(false);
        }
}

}
