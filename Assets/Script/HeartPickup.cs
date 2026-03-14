using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public float rotateSpeed = 60f;
    public QuestionFlowManager qfm;

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && qfm != null)
        {
            Debug.Log("Heart collected!");

            // Call the public method instead of accessing private variable
            qfm.AddHeart();

            Destroy(gameObject);
        }
    }
}