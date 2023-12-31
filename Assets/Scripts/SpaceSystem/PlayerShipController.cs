using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace SpaceSystem
{
    public class PlayerShipController : MonoBehaviour
    {
        //Input
        public InputActionAsset ActionAsset;

        private InputAction Acceleration;
        private InputAction Rotation;
        private InputAction Dock;
        private InputAction Fire;

        //Class
        [Header("Movement")]
        public float AccelSpeed = 5f;
        public float RotSpeed = 2f;

        [Header("Weapons")]
        public float Damage = 5f;
        public float BulletForce = 10f;
        public GameObject Bullet;
        public Transform Barrel;
        
        [Header("Docking")]
        public LayerMask DockerLayer;
        public float DockerDist = 1f;
        [Min(2)]
        public int DockerRays = 5;
        public float DockingWidth = 2f;
        public GameObject DockingInstruction;
    
        private Rigidbody2D _rb;
    
        void Start()
        {
            var ActionMap = ActionAsset.FindActionMap("FlyShip");
            ActionMap.Enable();
            Acceleration = ActionMap.FindAction("Acceleration");
            Rotation = ActionMap.FindAction("Rotation");
            Dock = ActionMap.FindAction("Dock");
            Fire = ActionMap.FindAction("Fire");

            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Dock.triggered)
            {
                Debug.Log("Try Dock", DetectedDock);
                var docker = DetectedDock.GetComponent<DockingPoint>();
                docker.Dock();
            }

            if (Fire.triggered)
            {
                var bulletObj = Instantiate(Bullet, Barrel.position, Barrel.rotation);
                var bullet = bulletObj.GetComponent<Bullet>();
                bullet.Damage = Damage;
                var bulletRb = bulletObj.GetComponent<Rigidbody2D>();
                bulletRb.velocity = _rb.velocity;
                bulletRb.AddForce(Barrel.up * BulletForce, ForceMode2D.Impulse);
            }
            
            if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("MainMenu");
        }

        private GameObject DetectedDock;
    
        private void FixedUpdate()
        {
            var accel = -Acceleration.ReadValue<float>();
            var rot = -Rotation.ReadValue<float>();
        
            _rb.AddForce(transform.up * accel * AccelSpeed);
            _rb.AddTorque(rot * RotSpeed);

            var hw = DockingWidth / 2;
            var stepDist = DockingWidth / (DockerRays-1);
            var left = transform.position - (transform.right * hw);

            var toDock = new GameObject[DockerRays];
            bool allMatch = true;
            for (int i = 0; i < DockerRays; i++)
            {
                var amt = stepDist * i;
                var orign = left + transform.right * amt;
                // orign = new Vector3(orign.x + i, orign.y);
                var hit = Physics2D.Raycast(orign, -transform.up, DockerDist, DockerLayer);
                var hasHit = hit.collider != null;
                if (hasHit) toDock[i] = hit.transform.gameObject;
                else allMatch = false;
                var col = hasHit ? Color.green : Color.red;
                Debug.DrawRay(orign, -transform.up*DockerDist, col);
            
                if (allMatch && i > 0)
                {
                    allMatch = toDock[i - 1] == hit.transform.gameObject;
                } 
                // if (!allMatch) break; //This would be good but debug rays are nice
            }

            DockingInstruction.gameObject.SetActive(allMatch);
        
            if (allMatch)
            {
                DetectedDock = toDock[0];
                Debug.Log(DetectedDock.name);
            }
            else
            {
                DetectedDock = null;
            }
        }
    }
}
