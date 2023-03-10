using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityAnimationEvent : UnityEvent<string>{};
[RequireComponent(typeof(Animator))]

public class Character_Move : MonoBehaviour
{
    private struct CharacterAnim
    {
        public bool bIsJump, /*bIsFloat,*/ bIsFallDown, bIsPickup, bIsRun, bIsWave, bIsWalk;

        public float fJumpSpeed_up, fJumpSpeed_float, fJumpSpeed_down;
        public float fWalkSpeed, fRunSpeed;
        public float fWaveSpeed, fPickupSpeed;

        public CharacterAnim(bool _bIsJump, bool _bIsPickup, bool _bIsRun, bool _bIsWave,
            bool _bIsWalk, float _fJumpSpeed_up, float _fJumpSpeed_float, float _fJumpSpeed_down, float _fWalkSpeed, float _fRunSpeed, float _fWaveSpeed, float _fPickupSpeed)
        {
            this.bIsJump    = _bIsJump;
            //this.bIsFloat   = false;
            this.bIsFallDown = false;
            this.bIsPickup  = _bIsPickup;
            this.bIsRun     = _bIsRun;
            this.bIsWave    = _bIsWave;
            this.bIsWalk    = _bIsWalk;
            this.fJumpSpeed_up = _fJumpSpeed_up;
            this.fJumpSpeed_float = _fJumpSpeed_float;
            this.fJumpSpeed_down = _fJumpSpeed_down;
            this.fWalkSpeed = _fWalkSpeed;
            this.fRunSpeed = _fRunSpeed;
            this.fWaveSpeed = _fWaveSpeed;
            this.fPickupSpeed = _fPickupSpeed;
        }
    }

    //move
    CharacterAnim CharacterAnimData;
    public Animator Animator;
    public UnityAnimationEvent OnAnimationStart;
    public UnityAnimationEvent OnAnimationComplete;

    private float x, y, fSpeed, fRodSpeed;
    private int dMoveForward, dRotLeft;
    private Rigidbody rg;

    private bool bInputWalk;


    //camera
    public Camera Camera;
    public GameObject DirObj;
    private float fRotX, fRotY, fRotSpeed;

    void Awake()
    {
        for(int i=0; i < Animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = Animator.runtimeAnimatorController.animationClips[i];

            AnimationEvent animationStartEvent = new AnimationEvent();
            animationStartEvent.time = 0;
            animationStartEvent.functionName = "AnimationStartHandler";
            animationStartEvent.stringParameter = clip.name;

            AnimationEvent animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = "AnimationCompleteHandler";
            animationEndEvent.stringParameter = clip.name;

            clip.AddEvent(animationStartEvent);
            clip.AddEvent(animationEndEvent);
        }
    }
    void Start()
    {
        CharacterAnimData = new CharacterAnim(false, false, false, false, false, 1f, 1f, 1f, 1f, 1f, 1f, 1f);

        dMoveForward = 0;
        dRotLeft = 0;
        fSpeed = .1f;
        fRodSpeed = 4f;

        fRotSpeed = 400f;
        rg = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
        ProcessAnim();
        UpdateCameraInput();

        //test
        test();
    }
    // anim
    // use :::::: animator.SetBool("New Bool", true);
    void OnWalk()   { CharacterAnimData.bIsWalk = true; }
    void StopWalk() { CharacterAnimData.bIsWalk = false; }
    void OnRun()    { CharacterAnimData.bIsRun = true; }
    void StopRun()  { CharacterAnimData.bIsRun = false; }
    void OnJump()   
    { 
        CharacterAnimData.bIsWalk = false;
        CharacterAnimData.bIsRun = false;
        CharacterAnimData.bIsJump = true; 
    }

    void StopJump() 
    { 
        CharacterAnimData.bIsJump = false; 
        //CharacterAnimData.bIsFloat = false;
        CharacterAnimData.bIsFallDown = false;
    }
    void OnPickUp() { CharacterAnimData.bIsPickup = true; }
    void StopPickUp() { CharacterAnimData.bIsPickup = false; }
    void OnWave()   { CharacterAnimData.bIsWave = true; }
    void StopWave()   { CharacterAnimData.bIsWave = false; }

    void ProcessAnim()
    {
        Animator.SetBool("bIsJump", CharacterAnimData.bIsJump );
        //Animator.SetBool("bIsFloat", CharacterAnimData.bIsFloat );
        Animator.SetBool("bIsFallDown", CharacterAnimData.bIsFallDown );

        
        Animator.SetBool("bIsWalk", CharacterAnimData.bIsWalk );

        Animator.SetBool("bIsRun", CharacterAnimData.bIsRun );

        // Allow Only NOT On Jumping
        if ( !CharacterAnimData.bIsJump )
        {
            Animator.SetBool("bIsWave", CharacterAnimData.bIsWave );
            Animator.SetBool("bIsPickup", CharacterAnimData.bIsPickup );
        }
    }
    void Move()
    {
        // w : 119, s : 115, d : 100, a : 97
        dMoveForward = 0;
        dRotLeft = 0;

        Vector3 CamPos = Camera.transform.position;
        Vector3 CharacterPos = this.gameObject.transform.position;
        Vector3 dir = ( CharacterPos - new Vector3(CamPos.x, CharacterPos.y, CamPos.z )).normalized; 

        if ( Input.GetKey(KeyCode.W) )
        {
            dMoveForward = 1;
        }

        else if ( Input.GetKey(KeyCode.S) )
        {
            dMoveForward = -1;
        }
        
        if(Input.GetKey(KeyCode.D))
        {
            dRotLeft = 1;
     	}   
     	else if(Input.GetKey(KeyCode.A)){
            dRotLeft = -1;
        }

        if ( dRotLeft != 0 )
        {
            this.transform.Rotate(new Vector3(0,0.1f,0) * ( dRotLeft == 1 ? 1 : -1 ) * fRodSpeed );
            OnWalk();
        }
        else
        {
            StopWalk();
        }

        if ( dMoveForward != 0 )
        {
            this.transform.Translate(dir * ( dMoveForward == 1 ? 1 : -1 ) * fSpeed );
            OnWalk();
        }
        else
        {
            StopWalk();
        }
    }

    void Jump()
    {
        if ( Input.GetKeyDown(KeyCode.Space) )
        {
            if ( this.rg.velocity.y > 0 == false && CharacterAnimData.bIsJump == false )
            {
                this.rg.AddForce(new Vector3(0, 40f, 0));
                OnJump();
            }
        }

        if ( CharacterAnimData.bIsJump && this.rg.velocity.y < -1 )
        {
            //Debug.DrawRay(transform.position, -transform.up*8,Color.red);
            RaycastHit hit;
            if( Physics.Raycast(transform.position , -transform.up* 8, out hit , 8) )
            {
                // .5??? ??????????????? ????????? ?????? ?????? ????????? ?????? ??? ?????? ???????????? ???????????????
                // Float ?????? ?????? ??????
                if ( hit.distance <= .5f && CharacterAnimData.bIsFallDown == false)
                {
                    CharacterAnimData.bIsFallDown = true;
                }
            }
        }
    }

    void test()
    {
        ;
    }

    public void AnimationStartHandler(string name)
    {
        // Debug.Log($"{name} animation start.");
        OnAnimationStart?.Invoke(name);
    }

    public void AnimationCompleteHandler(string name)
    {
        // Debug.Log($"{name} animation complete.");
        OnAnimationComplete?.Invoke(name);

        if (name == "jump-down")
        {
            StopJump();
        }
    }

    // private void OnCollisionEnter(Collision other) 
    // {
    //     // ???????????? ??? ?????? ????????? ?????? ????????? StepGround ????????? ???????????????!!
    //     if ( other.gameObject.tag == "StopGround" )
    //     {
    //         StopJump();
    //     }
    // }

    // =============================== Camera =================================

    void UpdateCameraInput()
    {
        if ( Input.GetMouseButton(1) )
        {
            fRotX = Input.GetAxis("Mouse X") * Time.deltaTime * fRotSpeed;
            fRotY = Input.GetAxis("Mouse Y") * Time.deltaTime * fRotSpeed;
        }

        Vector3 pos = this.transform.position;

        Camera.transform.RotateAround(pos, Vector3.right, - fRotY);
        Camera.transform.RotateAround(pos, Vector3.up, - fRotX);
        Camera.transform.LookAt(pos);
    }
}
