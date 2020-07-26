using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{  
    // Integers
    public int curScene = 0;
    public int maxHealth = 1;
    public int curHealth;
    public int additionalJumps;
    public int defaultAdditionalJumps = 1;
    // Floats
    public float rememberSlideFor; 
    public float rememberGroundedFor;
    public float fallMultiplier = 2.5f;
    public float wallSlideSpeed = 2f;
    public float lowJumpMultiplier = 2f;
    public float checkGroundRadius;
    public float jumpForce;    
    public float speed;
    public float dashTimer;
    public float maxDash;
    public float savedVelocity;
    public float dashSpeed = 3f;
    public float wallJumpDelay = 0.5f;
    float wallJumpDelayer;
    float lastTimeGrounded;
    //Booleans
    public bool isGrounded = false;
    public bool facingRight;
    public bool wallCheck1;
    public bool wallCheck2;
    public bool wallCheck3;
    public bool wallSliding = false;
    public bool wallJumping = false;
    //Others
    private GameMaster gm;
    public Transform playerPos;
    public Transform isGroundedChecker;
    public Transform wallChecker;
    public Transform wallChecker2;
    public Transform wallChecker3;
    public LayerMask groundLayer;
    Rigidbody2D rb;   

    public DashState dashState;   
    public enum DashState{
        Ready,
        Dashing,
        Cooldown
    }

    // Start is called before the first frame update
    void Start(){ 
        rb = GetComponent<Rigidbody2D>();

        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        playerPos.position = gm.lastCheckPointPos;

        curHealth = maxHealth;
        
    }

                                        // Void Update
    void Update()
    {           

    // Dashing
                switch (dashState)
        {
            case DashState.Ready:
                var isDashkeyDown = Input.GetKeyDown(KeyCode.LeftShift);
                if (isDashkeyDown){
                    float moveBy = rb.velocity.x * dashSpeed;
                    savedVelocity = rb.velocity.x;
                    rb.velocity = new Vector2(moveBy, rb.velocity.y);
                    dashState = DashState.Dashing;
                }
                break;
            case DashState.Dashing:
                dashTimer += Time.deltaTime * 1;
                if (dashTimer >= maxDash) {
                    dashTimer = maxDash;
                    rb.velocity = new Vector2(savedVelocity, rb.velocity.y);
                    dashState = DashState.Cooldown;
                }
                break;
            case DashState.Cooldown:
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0) {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }
                break; 
        }  

// Double jump bug fix
        if (Time.time - lastTimeGrounded <= rememberGroundedFor){
            additionalJumps = defaultAdditionalJumps;
        }
// Movement override bugfix
        if (dashState != DashState.Dashing && wallJumping == false) {
            float x = Input.GetAxisRaw("Horizontal");
            float moveBy = x * speed;
            rb.velocity = new Vector2(moveBy, rb.velocity.y);

            
    }
        // Functions
        checkDirection();
        CheckIfGrounded();
        CheckIfWallTouching();
        WallJump();
        WallSliding();
        BetterJump();
        Jump();
        Die();
    }

// Methods

    void checkDirection()
    {
        if (Input.GetAxisRaw("Horizontal") > 0.1f)
        {
            facingRight = true;
            transform.rotation = new Quaternion(transform.rotation.x, 0f, transform.rotation.z, transform.rotation.w);
        }
        if (Input.GetAxisRaw("Horizontal") < -0.1f)
        {
            facingRight = false;
            transform.rotation = new Quaternion(transform.rotation.x, 180f, transform.rotation.z, transform.rotation.w);
        }
    }

    void CheckIfWallTouching()
    {
       wallCheck1 = Physics2D.OverlapCircle(wallChecker.position, checkGroundRadius, groundLayer);
       wallCheck2 = Physics2D.OverlapCircle(wallChecker2.position, checkGroundRadius, groundLayer);
       wallCheck3 = Physics2D.OverlapCircle(wallChecker3.position, checkGroundRadius, groundLayer);

        if (facingRight || !facingRight)
        {
            if (wallCheck1 == true || wallCheck2 == true || wallCheck3 == true)
            {
                wallSliding = true;
            } else
            {   
                if (wallSliding)
                {
                    wallJumpDelayer = Time.time;
                }
                wallSliding = false;
            }
        }
    }

    void WallSliding()
    {
        if (wallSliding == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, wallSlideSpeed);            
        }  
    }

    void WallJump()
    {
        if (wallSliding == true && facingRight == true && !isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Time.time - wallJumpDelayer <= wallJumpDelay)
            {
                rb.velocity = new Vector2(-4f, jumpForce);
                wallJumping = true;
                additionalJumps = defaultAdditionalJumps + 1;
            } else { wallJumping = false; }
        }
        if (wallSliding == true && facingRight != true && !isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Time.time - wallJumpDelayer <= wallJumpDelay)
            {
                rb.velocity = new Vector2(4f, jumpForce);
                wallJumping = true;
                additionalJumps = defaultAdditionalJumps + 1;
            } else { wallJumping = false; }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            wallJumping = false;
        }
    }
    void CheckIfGrounded () {
        Collider2D collider = 
         Physics2D.OverlapCircle(isGroundedChecker.position,
         checkGroundRadius, groundLayer);

        if (collider != null) {
            isGrounded = true;  
        } else {
            if (isGrounded) {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }
    }
    void BetterJump() {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity *
            (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            rb.velocity += Vector2.up * Physics2D.gravity *
            (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    void Jump() {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time
        - lastTimeGrounded <= rememberGroundedFor && !wallSliding || additionalJumps > 0)) {
            rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
            additionalJumps --;
        }
    }


    void OnTriggerEnter2D(Collider2D col) {

    if(col.CompareTag("Spike")){

        curScene = SceneManager.GetActiveScene().buildIndex; // Get the current scene-index
        Damage(1);

    } else if (col.CompareTag("CheckPoint")){

        gm.lastCheckPointPos = transform.position;
        }
}


    public void Damage(int dmg) {

        curHealth -= dmg;

    }

    void Die() {

        if (curHealth <= 0){

            SceneManager.LoadScene(curScene); // Load Scene dependant on scene-index

        }

    }


}


