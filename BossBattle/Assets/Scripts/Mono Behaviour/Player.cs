using UnityEngine;

public class Player : MonoBehaviour {
        
    public enum FacingDir {
        Right,
        Left
    }
    
    [SerializeField] internal int health;
    [SerializeField] private float speed;
    [SerializeField] private float dashMultiplier; 
    [SerializeField] private float dashDuration; 
    [SerializeField] private float jumpForce;
    private Vector2 _moveVelocity;
    private Vector2 _moveDir;
    private Vector3 _faceRightScale;
    private Vector3 _faceLeftScale;
    private static bool _hasStartedDashingAtThisFrame = false;

    private Animator _anim;
    private Rigidbody2D _rb;

    public static bool IsRunning { get; private set; } = false;
    public static bool IsForcingStop { get; private set; } = false;
    public static bool IsDashing { get; private set; } = false;
    public static bool IsShooting { get; private set; } = false;
    public static FacingDir Facing { get; private set; } = FacingDir.Right;
    
    private void Start() {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        Vector3 curScale = transform.lossyScale;
        _faceRightScale = new Vector3(curScale.x, curScale.y, curScale.z);
        _faceLeftScale = new Vector3(curScale.x * -1, curScale.y, curScale.z);
    }

    private void Update() {
        _moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;
        UpdateStates();
        _anim.SetBool("isRunning", IsRunning);
        if (!IsDashing) SyncSpriteFlip();
    }
    
    private void FixedUpdate() {
        if (!IsDashing && !IsForcingStop) Move();
        if (IsDashing && _hasStartedDashingAtThisFrame) {
            Vector2 dashDir = new Vector2(0, _rb.velocity.y);
            if (Facing == FacingDir.Right) dashDir.x = 1;
            else if (Facing == FacingDir.Left) dashDir.x = -1;
            Dash(dashDir);
            _hasStartedDashingAtThisFrame = false;
        }
    }
    
    private void UpdateStates() {
        if (!IsDashing) {
            IsDashing = Input.GetKeyDown(KeyCode.Z);
            _hasStartedDashingAtThisFrame = true;
        }
        IsShooting = Input.GetKey(KeyCode.C);
        IsForcingStop = Input.GetKey(KeyCode.X);
        IsRunning = _moveDir.x != 0 && ! IsForcingStop && ! IsDashing;
        if (_moveDir.x > 0) Facing = FacingDir.Right;
        else if (_moveDir.x < 0) Facing = FacingDir.Left;
    }
    
    private void Move() => _rb.velocity = _moveDir * (speed * Time.fixedDeltaTime);

    private void Dash(Vector2 dashDir) {
        _rb.velocity = dashDir * (speed * dashMultiplier * Time.fixedDeltaTime);
        Invoke("StopDashing", dashDuration);
    }

    private void StopDashing() {
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        IsDashing = false;
    }

    private void SyncSpriteFlip() {
        if (Facing == FacingDir.Right) transform.localScale = _faceRightScale;
        else if (Facing == FacingDir.Left) transform.localScale = _faceLeftScale;
    }
    
}
