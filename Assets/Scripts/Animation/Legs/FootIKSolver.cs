using EventBus;
using UnityEngine;

namespace Animation
{
    public class FootIKSolver : ImprovedMonoBehaviour
    {
        [SerializeField] protected float overshoot = .25f;
        public float raycastDistance = 2;
        public float speed = 1;
        public float stepDistance = .5f;
        public LayerMask groundMask = 1 << 0;
        public Transform raycastOrigin;
        public float lerp;
        public float stepHeight = .5f;
        Vector3 currentPosition;
        Vector3 newPosition;
        public Vector3 NewPosition
        {
            get => newPosition;
            protected set
            {
                if (newPosition != value)
                {
                    lastMoveTime = Time.time;
                    newPosition = value;
                }
            }
        }
        Vector3 oldPosition;
        float lastMoveTime = 0;
        [SerializeField] protected bool grounded = true;
        public bool Grounded
        {
            get => grounded;
            protected set
            {
                if (grounded != value)
                {
                    grounded = value;
                    if (grounded) oldPosition = newPosition;
                }
            }
        }
        [SerializeField] FootIKSolver opposite;
        [SerializeField] protected Transform @default;
        [SerializeField] protected float resetTime = 1;

        private void Awake()
        {
            EventBus<ObjectDestroyed>.AddActions(Transform.root.GetEntityId(), null, Disable);
        }
        private void OnDestroy()
        {
            EventBus<ObjectDestroyed>.RemoveActions(Transform.root.GetEntityId(), null, Disable);
        }

        private void OnEnable()
        {
            ResetNewPosition();
            transform.position = oldPosition = NewPosition = currentPosition = @default.position;
            lerp = 1;
            Grounded = true;
        }

        private void Update()
        {
            Readjust();
            if (grounded && Time.time - lastMoveTime > resetTime && opposite.grounded && Time.time - opposite.lastMoveTime > resetTime)
            {
                ResetNewPosition();
            }
            if (lerp < 1 && opposite.Grounded)
            {
                Grounded = false;
                Vector3 footPosition = Vector3.Lerp(oldPosition, NewPosition, lerp);
                footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
                currentPosition = footPosition;
                lerp += Time.deltaTime * speed;
            }
            else
            {
                Grounded = true;
            }
        }

        public void Disable()
        {
            enabled = false;
        }

        void ResetNewPosition()
        {
            var ray = new Ray(raycastOrigin.position, Vector3.down);
            if (Physics.Raycast(ray, out var info, raycastDistance, groundMask))
            {
                //we have a new position
                if (NewPosition != info.point)
                {
                    NewPosition = info.point;
                    lerp = 0;
                }
            }
        }

        void Readjust()
        {
            transform.position = currentPosition;
            var ray = new Ray(raycastOrigin.position, Vector3.down);
            if (Physics.Raycast(ray, out var info, raycastDistance, groundMask))
            {
                if (Vector3.Distance(NewPosition, info.point) > stepDistance)
                { //we have a new position
                    var dir = (info.point - NewPosition).normalized * overshoot;
                    ray = new(raycastOrigin.position + dir, Vector3.down);
                    if (Physics.Raycast(ray, out info, raycastDistance, groundMask))
                    {
                        NewPosition = info.point;
                        lerp = 0;
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (@default != null)
            {
                Gizmos.color = Color.lightGreen;
                Gizmos.DrawWireCube(@default.position, Vector3.one * .08f);
            }
            Gizmos.color = Color.green;
            Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + Vector3.down * raycastDistance);
            Gizmos.DrawWireSphere(NewPosition, .1f);
            if (Grounded)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(raycastOrigin.position, .1f);
            }
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(oldPosition, .09f);
        }
    }
}