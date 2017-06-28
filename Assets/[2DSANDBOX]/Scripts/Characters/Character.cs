using UnityEngine;

public class Character : EntityPlayer {

    public Heart heart;
    public float speed = 3.0f;
    public bool grounded = false;
    public float jumpHeight = 3.0f;
    public AudioSource walk_audio;
    private Animator characterAnimator;
    private bool is_char_animate = false;
    private Vector3 face_left = new Vector3(-1, 1, 1);

    private KeyCode left_keycode = KeyCode.A; 
    private KeyCode right_keycode = KeyCode.D;

    public override void Awake()
    {
        base.Awake();
        characterAnimator = (Animator)GetComponent<Animator>();
    }

    public override void Start()
    {
        base.Start();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        grounded = true;
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        grounded = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag=="enemy")
		{
            heart.Hit();
        }
    }

    public override void Update()
    {
        base.Update();

        Vector3 pos = transform.position;
        if(Input.GetKey(left_keycode))
        {
            if(!is_char_animate){
                is_char_animate = true;
                characterAnimator.SetBool("walking", true);
                walk_audio.Play();
            }
        
            // walk_audio.PlayOneShot(walk_audio.clip, 1);

            pos.x -= speed *Time.deltaTime;
            transform.position = pos;
            transform.localScale = face_left;
        }
        else if(Input.GetKey(right_keycode))
        {
            if(!is_char_animate){
                is_char_animate = true;
                characterAnimator.SetBool("walking", true);
                walk_audio.Play();
            }
            // walk_audio.PlayOneShot(walk_audio.clip, 1);

            pos.x += speed * Time.deltaTime;
            transform.position = pos;
            transform.localScale = Vector3.one;
        }
        else if(is_char_animate)
        {
            is_char_animate =false;
            characterAnimator.SetBool("walking", false);
            
            walk_audio.Stop();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            if (grounded)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpHeight);
                grounded = false;
            }
        
        }
    }
}