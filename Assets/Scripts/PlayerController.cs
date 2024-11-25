using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 5.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup = false;
    float powerUpStrength = 15.0f;
    public GameObject powerUpIndicator;
    public AudioSource playerAudioSource;
    public AudioClip collisionSound;
    public AudioClip powerUpSound;
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(forwardInput * speed * focalPoint.transform.forward);
        powerUpIndicator.transform.position = transform.position + new Vector3(0,-0.5f,0);
        Death();

    }

    private void Death()
    {
        if (transform.position.y < -15)
        {
            playerAudioSource.PlayOneShot(powerUpSound);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        playerAudioSource.PlayOneShot(powerUpSound);
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountDownRoutine());
            powerUpIndicator.SetActive(true);
        }
    }

    IEnumerator PowerUpCountDownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerUpIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        playerAudioSource.PlayOneShot(collisionSound);
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
            Debug.Log("Collided with: " + collision.gameObject.name + "with powerup set to: " + hasPowerup);
        }
    }
}
