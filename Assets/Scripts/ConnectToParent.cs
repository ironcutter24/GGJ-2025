using UnityEngine;

public class ConnectToParent : MonoBehaviour
{
    [SerializeField] private FixedJoint2D joint;
    
    private void Awake()
    {
        joint.connectedBody = transform.parent.gameObject.GetComponent<Rigidbody2D>();
    }
}
