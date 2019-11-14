using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScore;

    //pola do zmiany z poziomu unity
    public float tapForce = 10f;
    public float TiltSmooth = 5f;
    public Vector3 StartPos;
    public AudioSource tapAudio;
    public AudioSource scoreAudio;
    public AudioSource dieAudio;

    new Rigidbody2D rigidbody;
    Quaternion downRotation;
    Quaternion forwardRotation;
    GameMenager game;
   

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -60);
        forwardRotation = Quaternion.Euler(0, 0, 15);
       
        game = GameMenager.Instance;
        rigidbody.simulated = false;
    }
     void OnEnable()
    {
        GameMenager.OnGameStarted += OnGameStarted;
        GameMenager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
     void OnDisable()
    {
        GameMenager.OnGameStarted -= OnGameStarted;
        GameMenager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameStarted()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;
    }
    void OnGameOverConfirmed()
    {
        transform.localPosition = StartPos;
        
    }
    void Update()
    {
        if (game.GameOver1) return;

        if (Input.GetMouseButtonDown(0))
            {
            tapAudio.Play();
            transform.rotation = forwardRotation;
            rigidbody.velocity = Vector3.zero; 
            rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force );
            }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tapAudio.Play();
            transform.rotation = forwardRotation;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }


        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, TiltSmooth * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ScoreZone")
        {
            OnPlayerScore();
            scoreAudio.Play();

        }
        if  (collision.gameObject.tag == "DeathZone")
        {
            rigidbody.simulated = false;
            OnPlayerDied(); // wyrzuca nas do okna koncowego
            dieAudio.Play();
        }
    }
}
