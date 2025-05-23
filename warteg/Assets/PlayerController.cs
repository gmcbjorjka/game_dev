using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    private void FixedUpdate(){
       if(movementInput != Vector2.zero){
        bool success = TryMove(movementInput);
        if(!success){
            success = TryMove(new Vector2(movementInput.x, 0));
            if(!success){
                success = TryMove(new Vector2(0 ,movementInput.y));
            }
        }
         animator.SetBool("isMoving",success);
       }else{
        animator.SetBool("isMoving",false);
       }

      
    }

    private bool TryMove(Vector2 direction){
         if(movementInput != Vector2.zero){
            int count = rb.Cast(
                movementInput,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
            );
            if(count == 0 ){
                rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
                return true;  // ← Tambahkan ini
            }else{
                return false;
            }
        }
            return false; 
    }

    void OnMove(InputValue movementValue){
        movementInput = movementValue.Get<Vector2>();
    }
}
