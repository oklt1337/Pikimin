using System.Collections;
using UnityEngine;

namespace Players
{
    public abstract class MovingObject : MonoBehaviour
    {
        public float moveTime = 0.1f;
        public LayerMask blockLayer;

        private BoxCollider2D boxCollider2D;
        protected Rigidbody2D rb2D;
        private float inverseMoveTime;

        [SerializeField] protected bool IsWalking;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            rb2D = GetComponent<Rigidbody2D>();
            inverseMoveTime = 5f / moveTime;
        }

        protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
        {
            Vector2 start = transform.position;
            Vector2 end = start + new Vector2(xDir, yDir);

            boxCollider2D.enabled = false;
            hit = Physics2D.Linecast(start, end, blockLayer);
            boxCollider2D.enabled = true;

            if (hit.transform == null)
            {
                StartCoroutine(SmoothMovement(end));
                return true;
            }

            return false;
        }

        protected IEnumerator SmoothMovement(Vector3 end)
        {
            IsWalking = true;
            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            while (sqrRemainingDistance > float.Epsilon)
            {
                Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
                rb2D.MovePosition(newPosition);
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;
                yield return null;
            }

            IsWalking = false;
        }

        protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
        {
            RaycastHit2D hit;
            bool canMove = Move(xDir, yDir, out hit);

            if (hit.transform == null)
                return;

            T hitComponent = hit.transform.GetComponent<T>();
            
            if(!canMove && hitComponent != null)
                OnCantMove(hitComponent);
        }

        protected abstract void OnCantMove<T>(T component)
            where T : Component;
    }
}
