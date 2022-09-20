using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameManager gameManager;
    private Rigidbody rgb;
    private Renderer colorr;

    private void Start()
    {
        rgb = GetComponent<Rigidbody>();
        colorr = GetComponent<Renderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bucket"))
        {
            TechnicalOperation();
            gameManager.BallScored();
        }
        else if (other.CompareTag("Ground"))
        {
            TechnicalOperation();
            gameManager.BallMissed();
        }
    }
    void TechnicalOperation()
    {
        gameManager.ParcEffect(gameObject.transform.position,colorr.material.color);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero); 
        rgb.velocity = Vector3.zero;
        rgb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
