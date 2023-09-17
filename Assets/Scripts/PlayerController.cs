using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private TMP_Text txtTime;
    [SerializeField] private float timeValue;


    public float speed = 0;
    public TextMeshProUGUI countText;
    private int count;
    public GameObject winTextObject;
    public GameObject loserTextObject;
    public GameOverScreen GameOverScreen;
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    public void GameOver()
    {
        GameOverScreen.Setup(count);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
        loserTextObject.SetActive(false);

        InvokeRepeating("DecreaseTime", 1f, 1f);
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Quadrados = " + count.ToString();
        if(count >= 10)
        {
            winTextObject.SetActive(true);
            GameOver();
        }
    }


    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }
    }

    private void DecreaseTime()
    {
        if(timeValue > 0f)
        {
            timeValue--;
        }
        else
        {
            timeValue = 0f;
            loserTextObject.SetActive(true);
            GameOver();


        }

        DisplayTime(timeValue);
    }

    private void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0f;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        txtTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
