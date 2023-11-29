using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float maxForce = 100f;
    [SerializeField] private GameObject forceBar;
    private float horizontalInput;
    private Rigidbody rb;
    private float holdDown;
    private bool shoot = false;
    private Vector3 scaleChange;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        horizontalInput = Input.GetAxis("Horizontal");

        if (!shoot) {
            transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);

            if (Input.GetKeyDown(KeyCode.Space)) {
                holdDown = Time.time;
            }
            if (Input.GetKeyUp(KeyCode.Space)) {
                float holdDownTime = Time.time - holdDown;
                rb.AddForce(transform.forward * CalculateForce(holdDownTime));
                scaleChange = new Vector3(holdDownTime, 0, 0);
                forceBar.transform.localScale += scaleChange;
                if(forceBar.transform.localScale.x > 2) {
                    forceBar.transform.localScale = new Vector3(2, 0.2f, 1);
                }
                shoot = true;
            }
        }

        if(Input.GetKey(KeyCode.R)) { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  
        }
    }

    private float CalculateForce(float holdTime) {
        float maxForceHoldTime = 2f;
        float holdTimeNormalized = Mathf.Clamp01(holdTime / maxForceHoldTime);
        float force = holdTimeNormalized * maxForce;
        return force;
    }

    private void OnCollisionEnter(Collision collision) {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Restart")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
