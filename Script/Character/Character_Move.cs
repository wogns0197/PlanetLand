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
        public bool bIsJump, bIsFloat, bIsFallDown, bIsPickup, bIsRun, bIsWave, bIsWalk;

        public float fJumpSpeed_up, fJumpSpeed_float, fJumpSpeed_down;
        public float fWalkSpeed, fRunSpeed;
        public float fWaveSpeed, fPickupSpeed;

        public CharacterAnim(bool _bIsJump, bool _bIsPickup, bool _bIsRun, bool _bIsWave,
            bool _bIsWalk, float _fJumpSpeed_up, float _fJumpSpeed_float, float _fJumpSpeed_down, float _fWalkSpeed, float _fRunSpeed, float _fWaveSpeed, float _fPickupSpeed)
        {
            this.bIsJump    = _bIsJump;
            this.bIsFloat   = false;
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

    CharacterAnim CharacterAnimData;
    public Animator Animator;
    public UnityAnimationEvent OnAnimationStart;
    public UnityAnimationEvent OnAnimationComplete;

    private float x, y, fSpeed, fRodSpeed;
    private int dMoveForward, dRotLeft;
    private Rigidbody rg;

    private bool bInputWalk;

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
        rg = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
        ProcessAnim();

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
        CharacterAnimData.bIsFloat = false;
        CharacterAnimData.bIsFallDown = false;
    }
    void OnPickUp() { CharacterAnimData.bIsPickup = true; }
    void StopPickUp() { CharacterAnimData.bIsPickup = false; }
    void OnWave()   { CharacterAnimData.bIsWave = true; }
    void StopWave()   { CharacterAnimData.bIsWave = false; }

    void ProcessAnim()
    {
        Animator.SetBool("bIsJump", CharacterAnimData.bIsJump );
        Animator.SetBool("bIsFloat", CharacterAnimData.bIsFloat );
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
            this.transform.Translate(new Vector3(0,0,0.1f) * ( dMoveForward == 1 ? 1 : -1 ) * fSpeed );
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
            if ( this.rg.velocity.y > 0 == false)
            {
                this.rg.AddForce(new Vector3(0, 50f, 0));
                OnJump();
            }
        }

        // CharacterAnimData.bIsFloat = true;
        if ( -4f < this.rg.velocity.y && this.rg.velocity.y < -3f && CharacterAnimData.bIsFloat == false )
        {
            CharacterAnimData.bIsFloat = true;
            //Debug.Log("222222222222222");
        }

        if ( -0.1f < this.rg.velocity.y && this.rg.velocity.y < 0 && CharacterAnimData.bIsFloat == true)
        {
            // StopJump();
            CharacterAnimData.bIsFallDown = true;
        }
    }

    void test()
    {
        if ( CharacterAnimData.bIsJump )
        {
            Debug.DrawRay(transform.position, -transform.up*8,Color.red);

        RaycastHit hit;
        if(Physics.Raycast(transform.position , -transform.forward, out hit , 8))
		{
            Debug.Log(hit.distance);
            //if (  )
        }
        }
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
    //     // 캐릭터와 땅 닿는 처리는 땅에 무조건 StepGround 태그를 달아줘야함!!
    //     if ( other.gameObject.tag == "StopGround" )
    //     {
    //         StopJump();
    //     }
    // }
}
