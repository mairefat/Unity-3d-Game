using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterController Controller;
    CharacterStats stats;
    Animator anim;
    public float speed = 5;
    Transform cam;
    float gravity = 10;
    float verticalVelocity = 0;
    public float jumpValue = 10;
    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
        stats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isSprint = Input.GetKey(KeyCode.LeftShift);
        float sprint = isSprint ? 2.5f : 1;

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }

        Vector3 moveDirection = new Vector3(horizontal,0,vertical);
       anim.SetFloat("Speed", Mathf.Clamp(moveDirection.magnitude, 0, 0.5f) + (isSprint? 0.5f : 0));
        if (Controller.isGrounded)
        {
            if (Input.GetAxis("Jump") > 0)
                verticalVelocity = jumpValue;
        }        
        else 
             verticalVelocity -= gravity * Time.deltaTime;

        if (moveDirection.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        moveDirection = cam.TransformDirection(moveDirection);
        moveDirection = new Vector3(moveDirection.x * speed *sprint,
         verticalVelocity, moveDirection.z * speed *sprint);

        Controller.Move(moveDirection* Time.deltaTime );
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Health"))
        {
            //Debug.Log("Health Increased!");
            GetComponent<CharacterStats>().ChangeHealth(20);
            LevelManager.instance.PlaySound(LevelManager.instance.levelSounds[2], LevelManager.instance.Player.position);
            Instantiate(LevelManager.instance.Particles[1], other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Item"))
        {
            LevelManager.instance.levelItems++;
            Debug.Log("Items : " + LevelManager.instance.levelItems);
            LevelManager.instance.PlaySound(LevelManager.instance.levelSounds[1], LevelManager.instance.Player.position);
            Instantiate(LevelManager.instance.Particles[0], other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
        }
            
    }

    public void DoAttack()
    {
        
        transform.Find("Collider").GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(HideCollider()); 
    }

    IEnumerator HideCollider()
    { 
        yield return new WaitForSeconds(0.5f);
        transform.Find("Collider").GetComponent<BoxCollider>().enabled = false;
    }
}
