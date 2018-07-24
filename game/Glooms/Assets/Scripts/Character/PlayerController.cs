using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : PhysicsObject {
    public MovementTimerScript movementTimer;
    public Button movementButton;
    public Button weaponButton;

    public GameObject playerArrow;
    public GameObject Parachute;

    //Parachute Simulation Stuff
    private float rotationDegree = 1.1f;
    private float tempRotation;
    private float airMove = 0.1f;
    private float x = 1;

    public float maxSpeed = 2f;
    public float jumpTakeOffSpeed = 5;
    public bool aimingMode = false;
    public bool movingMode = false;
    public bool passiveMode;
    public bool spawning;
    public bool jumped;
    public bool flipped = false;
    public bool canMove = true;


    private ShootingWeapon shootingWeaponScript;
    private PlayerSpawning playSpawn;
    private Animator animator;
    private Transform canvasTransform;
    private CameraManager cam;
    private PlayerStats stats;

    // Use this for initialization
    void Awake()
    {
        spawning = true;
        tempRotation = rotationDegree;
        shootingWeaponScript = gameObject.GetComponentInChildren<ShootingWeapon>();
        animator = GetComponent<Animator>();
        canvasTransform = transform.Find("Canvas");
        movementButton = GameObject.Find("Movement Button").GetComponent<Button>();
        movementTimer = GameObject.Find("Movement Timer").GetComponent<MovementTimerScript>();
        movementButton.onClick.AddListener((UnityEngine.Events.UnityAction)this.MovingModeActive);
        weaponButton = GameObject.Find("Weapon Button").GetComponent<Button>();
        weaponButton.onClick.AddListener((UnityEngine.Events.UnityAction)this.AimingModeActive);
        cam = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        playSpawn = GameManager.instance.GetComponent<PlayerSpawning>();
        stats = gameObject.GetComponent<PlayerStats>();
    }

    //Update Method
    protected override void HandlePlayer()
    {
        if (!passiveMode)
        {
            if (movingMode)
            {
                Moving();
            }
            if (aimingMode)
            {
                animator.SetTrigger("Aim");
            } else
            {
                if (!canMove && !inAir)
                {
                    animator.SetTrigger("Idle");
                }
            }
        }
        else
        {   
            //SpawnPhase
            if (spawning)
            {
                spawning = !grounded;
                animator.SetTrigger("Offground");
                Vector3 rotationPoint = transform.position;
                rotationPoint.y += 5;
                transform.Rotate(new Vector3(0, 0, -tempRotation));
                if (transform.rotation.z < -0.5f)
                {
                    if (tempRotation > 0)
                    {
                        tempRotation = 0;
                    }
                    airMove = -0.1f;
                }
                if (transform.rotation.z < 0)
                {
                    x = -1;
                } else
                {
                    x = 1;
                }
                if (transform.rotation.z > 0.5f)
                {
                    if (tempRotation < 0)
                    {
                        tempRotation = 0;
                    }
                    airMove = 0.1f;
                }
                tempRotation += x * Random.Range(0, Time.deltaTime*2);

                Vector3 newPos = transform.position;
                newPos.x += Random.Range(0.5f, 1) * Random.Range(0, transform.rotation.z/1.5f);
                newPos.y -= Time.deltaTime*3f;
                newPos.z = 1;
                transform.position = newPos;
                //End SpawnPhase for Player
                if (grounded)
                {
                    gravityModifier = 2f;
                    Parachute.SetActive(false);
                    transform.rotation = Quaternion.identity;
                    Invoke("FullscreenOn", 0.2f);
                    playSpawn.playerInAir = false;
                    animator.SetTrigger("Idle");
                    if (transform.position.x > 0)
                    {
                        Flip();
                    }
                }               
            } else
            {
                animator.SetTrigger("Idle");
            }
        }
    }

    private void FullscreenOn(){
        cam.fullscreen = true;
        playSpawn.AnnounceCurrentPlayer();
    }

    private void Moving()
    {
        //Animation
        if (!inAir)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                animator.SetTrigger("Walk");
            }
            else
            {
                animator.SetTrigger("Idle");
            }
        }
        else
        {
            if (jumped)
            {
                animator.SetTrigger("Offground");
            }
        }

        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");


        //Jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jumped = true;
            stats.PlayJumpSound();
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }


        //Flip player when moving
        bool flipSprite = (flipped ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            Flip();
        }

        if (move.x != 0)
        {
            movementTimer.playerMoving = true;
        }
        else
        {
            movementTimer.playerMoving = false;
        }

        //MovementLeftCheck
        if (movementTimer.timeLeft <= 0)
        {
            DisableMoving();
        }

        targetVelocity = move * maxSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    //Flip
    public void Flip()
    {
        flipped = !flipped;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        canvasTransform.localScale = newScale;
        newScale = shootingWeaponScript.transform.localScale;
        newScale.x *= -1;
        newScale.y *= -1;
        shootingWeaponScript.transform.localScale = newScale;
    }

    //Set To Aiming Mode
    public void AimingModeActive()
    {
        if (!passiveMode)
        {
            movingMode = false;
            movementTimer.playerMoving = false;
            aimingMode = true;
            shootingWeaponScript.SetActive(true);
        }
    }

    //Set To Moving Mode
    public void MovingModeActive()
    {
        if (!passiveMode && canMove)
        {
            aimingMode = false;
            movingMode = true;
            shootingWeaponScript.SetActive(false);
            movementTimer.SetActive();
        }
    }

    //When moved this turn, disable moving
    public void DisableMoving()
    {
        movementTimer.SetPassive();
        canMove = false;
        movingMode = false;
    }

    //Player is Passive
    public void SetPassive()
    {
        //entering passiveMode
        movingMode = false;
        aimingMode = false;
        passiveMode = true;

        //movementReset
        movementTimer.SetPassive();
        canMove = true;

        //disablingShootingWeapon
        shootingWeaponScript.SetActive(false);
        shootingWeaponScript.canShoot = true;
        playerArrow.GetComponent<SpriteRenderer>().enabled = false;
    }

    //Player is Activ
    public IEnumerator SetActive()
    {
        StartCoroutine(stats.Utilities());
        yield return new WaitUntil(() => stats.poisonDamageTaken);
        stats.poisonDamageTaken = false;
        passiveMode = false;
        playerArrow.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void SetJumped(bool bo)
    {
        jumped = bo;
    }
}
