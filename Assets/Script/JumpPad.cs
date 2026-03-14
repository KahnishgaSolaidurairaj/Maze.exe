using UnityEngine;

public class JumpPad : MonoBehaviour {
    public float jumpForce = 10f; // Upward force
    public bool onlyApplyOnce = false; // optional for one-time jump

    private void OnCollisionEnter(Collision collision) {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player")) {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null) {
                // Reset vertical velocity for consistent jump
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                
                // Apply upward force
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                // Optional: destroy or disable pad if only once
                if (onlyApplyOnce)
                    gameObject.SetActive(false);
            }
        }
    }
}