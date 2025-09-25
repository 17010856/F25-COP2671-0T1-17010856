using UnityEngine;

public class PlayerController : MonoBehaviour


{
    public bool gameOver;
    private Animator playerAnim;
    private AudioSource playerAudio;
    public bool isOnGround = true;
    private Rigidbody playerRb;
    public float jumpForce = 10.0f;
    public float gravityModifier = 2f;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
   
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerAnim.SetTrigger("Jump_trig");
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, .5f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
            if(collision.gameObject.CompareTag("Ground"))
            {
                dirtParticle.Play();
                isOnGround =true;
            } else if (collision.gameObject.CompareTag("Obstacle"))
            {   
                explosionParticle.Play();
                playerAnim.SetBool("Death_b", true);
                playerAnim.SetInteger("DeathType_int", 1);
                gameOver = true;
                Debug.Log("Game Over!");
                dirtParticle.Stop();
                playerAudio.PlayOneShot(crashSound, .5f);
            }
    }
}
