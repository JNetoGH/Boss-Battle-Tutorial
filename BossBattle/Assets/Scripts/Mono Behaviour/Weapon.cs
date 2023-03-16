using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] private  float interval = 0.5f;
    private float _timer = 0 ;
    
    public GameObject projectile;
    public Transform shotPoint;
    public Animator camAnim;
    
    private void Update() {
        bool aimingUp = (Input.GetAxisRaw("Vertical") > 0);
        bool aimingRight = (Input.GetAxisRaw("Horizontal") > 0);
        bool aimingLeft = (Input.GetAxisRaw("Horizontal") < 0);
        
        // cant shoot or aim while dashing
        if (Player.IsDashing) return;
        
        Aim(aimingUp, aimingRight, aimingLeft);
        if (Player.IsShooting) Shoot();
    }

    private void Aim(bool aimingUp, bool aimingRight, bool aimingLeft) {
        if (aimingUp) transform.rotation = Quaternion.Euler(0f, 0f, 0);
        if (aimingRight) {
            transform.rotation = Quaternion.Euler(0f, 0f, -90);
            if (aimingUp) transform.rotation = Quaternion.Euler(0f, 0f, -45);
        }
        else if (aimingLeft) {
            transform.rotation = Quaternion.Euler(0f, 0f, 90);
            if (aimingUp) transform.rotation = Quaternion.Euler(0f, 0f, 45);
        }
    }
    
    private void Shoot() {
        if (_timer <= 0) {
            camAnim.SetTrigger("shake");
            Instantiate(projectile, shotPoint.position, transform.rotation);
            _timer = interval;
            return;
        }
        _timer -= Time.deltaTime;
    }
    
}
