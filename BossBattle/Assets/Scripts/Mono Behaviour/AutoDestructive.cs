using UnityEngine;

public class AutoDestructive: MonoBehaviour {
    
    private void Start() {
        Invoke("SetToGarbageCollection", 2);
    }
    
    protected void SetToGarbageCollection() {
        Destroy(gameObject);
    }
    
}
