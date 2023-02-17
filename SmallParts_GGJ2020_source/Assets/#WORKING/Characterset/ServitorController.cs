// q // cD // d // 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public enum Element
{
    None,
    Water,
    Fire,
    Air
}
public class ServitorController : MonoBehaviour
{
    /// <summary>
    /// a filter that describes raycasting for the ground layer.
    /// </summary>
    private ContactFilter2D groundFilter;
    private Vector3 normal;
    private Vector3 bodyPosition;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float targetHeight;

    [SerializeField]
    private GameObject _arrow;

    [SerializeField]
    private List<Transform> _lights = new List<Transform>();
    [SerializeField]
    private float _lightUpDistance = 10f;
    public Element Element { get; private set; }
    /// <summary>
    /// how quickly we respond to changes in terrain height
    /// this may change based on our lower body parts (thruster / legs)
    /// </summary>
    [Tooltip("How quickly we change height toward our target height")]
    [SerializeField]
    private float heightSensitivity = 1f;

    /// <summary>
    /// How quickly we rotate to match the ground normal
    /// </summary>
    [Tooltip("How quickly we rotate to match the ground normal")]
    [SerializeField]
    private float rotateSensitivity = 1f;
    private float fallSpeed = 1f;

    //private ServitorLimb legTest;
    /// <summary>
    /// how far from center on either side to determine the average normal
    /// and least distance to the ground.
    /// </summary>
    [Tooltip("How far from center to determine ground normal and distance")]
    private float groundCheckRadius;

    /// <summary>
    /// the transform we move mechanically
    /// </summary>
    private Transform rootTransform;

    /// <summary>
    /// The transform we move cosmetically,
    /// having no bearing on our actual position.
    /// </summary>
    private Transform cosmeticTransform;
    private Vector3 cosmeticGoal;
    private Vector3 cosmeticRestPosition;

    private float upForce;
    private bool isGrounded;
    private float hoverHeight;

    private bool debugMode = false;
    [SerializeField]
    private GameObject debugIndicator;

    /// <summary>
    /// the bumper reference is maintained so we don't move
    /// while undergoing a transition
    /// </summary>
    private Bumper bumper;

    private ServitorFace face;
    private bool isLocked;
    public  EventHandler ElementEvent;
    private List<IInteractable> interactables = new List<IInteractable>();
    private List<IMoveModifier> modifiers = new List<IMoveModifier>();

    private bool canJump;
    private void Awake()
    {
        this.groundFilter = new ContactFilter2D();
        this.groundFilter.SetLayerMask(LayerMask.GetMask("ground", "barrier"));
        this.bumper = FindObjectOfType<Bumper>();
        
    }
    private void Start()
    {
        //this.legTest = GetComponent<ServitorRigger>().CreateLeg();
        //this.legTest.SetActive(true);
        this.rootTransform = this.transform;
        this.face = GetComponentInChildren<ServitorFace>();
    }

    public void SetBodyTransform(Transform target)
    {
        this.cosmeticTransform = target;
        this.cosmeticRestPosition = this.cosmeticTransform.localPosition;
    }

    public void SetLocked(bool locked)
    {
        this.isLocked = locked;
    }

    public void SetElement(Element element)
    {
        this.Element = element;
        this.ElementEvent?.Invoke(this, EventArgs.Empty);
        if (element != Element.None) 
        {
            this.face.SetExpression(ServitorFace.Expression.Happy, 3f, true);
            _arrow.GetComponent<Arrow>().SetElement(Element);
        };

    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pickup = collision.gameObject.GetComponent<IPickup>();
        var interactable = collision.gameObject.GetComponent<IInteractable>();
        var modifier = collision.gameObject.GetComponent<IMoveModifier>();
        if (pickup != null) pickup.Pickup(this);
        if (interactable != null)
        {
            this.interactables.Add(interactable);
        }
        if (modifier != null)
        {

            this.modifiers.Add(modifier);
            this.fallSpeed *= modifier.NegativeModifier.y;
            this.upForce *= modifier.PositiveModifier.y;
        }
        
    }
    public void UseElement(IInteractable target)
    {
        var element = this.Element;
        if (target.VerifyElement(element))
        {
            this.face.SetExpression(ServitorFace.Expression.Happy, 3f, true);
        }
        else this.face.SetExpression(ServitorFace.Expression.Sad, 1f, true);
        this.SetElement(Element.None);
        // effect for element
        target.OnUseElement(element);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var interactable = collision.gameObject.GetComponent<IInteractable>();
        this.interactables.Remove(interactable);
        var modifier = collision.gameObject.GetComponent<IMoveModifier>();
        this.modifiers.Remove(modifier);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            this.debugMode = !this.debugMode;
            this.debugIndicator.SetActive(this.debugMode);
        }
        //if (Input.GetKeyDown(KeyCode.F12))
        //{
        //    this.SetElement(Element.None);
        //    this.SetPosition(Vector3.zero);
        //}

        
        bool lockMovement = this.isLocked || this.bumper.IsFadingIn;

        var h = Input.GetAxis("Horizontal");


        GroundCheckResult centerResult =this.GroundCheck(this.rootTransform.position);
        //GroundCheckResult leftResult = this.WallCheck(Vector3.left, centerResult);
        //GroundCheckResult rightResult = this.WallCheck(Vector3.right, centerResult);

        RaycastHit2D[] ceilings = new RaycastHit2D[1];
        int ceilingCount = Physics2D.CircleCast(this.transform.position + Vector3.up, .5f, Vector2.up, this.groundFilter, ceilings, 4f);
        this.canJump = ceilingCount < 1 && !lockMovement;

        //GroundCheckResult result = new GroundCheckResult();
        //result.Normal = (centerResult.Normal + leftResult.Normal + rightResult.Normal) / 3f;
        //result.Position = (centerResult.Position + leftResult.Position + rightResult.Position) / 3f;
        GroundCheckResult result = centerResult;
        var goalRotation = Quaternion.FromToRotation(Vector3.up, result.Normal);
        this.rootTransform.rotation = Quaternion.Slerp(this.rootTransform.rotation, goalRotation, Time.deltaTime * this.rotateSensitivity);

        var groundHeight = result.Position.y + this.targetHeight;
        // note that goalheight is going to be -1000 if there's nothing below us.
        
        this.isGrounded = groundHeight >= (this.rootTransform.position.y - (this.targetHeight + 1f));
        this.hoverHeight = Mathf.PingPong(Time.time/3.5f, 0.4f);

        float nDot = Vector3.Dot(result.Normal, Vector3.up);
        float verticality = (1f - (nDot)) * 3f;
        Vector3 movement = lockMovement ? Vector3.zero : Vector3.right * h * Time.deltaTime * this.moveSpeed;
        movement = GetModifiedMovement(movement);
        Vector3 goalPosition = this.rootTransform.position + movement;
        var goalHeight = goalPosition.y;


        goalHeight += verticality;
        if (!this.isGrounded && upForce < 0.1f)
        {
            // if we're definitely not 'grounded' we will begin to fall.
            if (this.fallSpeed < 10f) this.fallSpeed += Time.deltaTime * 4f;
            goalHeight -= this.fallSpeed;
        }
        else
        {
            if (this.isGrounded && upForce < 0.1f) goalHeight = groundHeight;
            this.fallSpeed = 0f;
        }

       
        if (!lockMovement && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.S)))
        {
            this.cosmeticGoal = Vector3.down * 0.5f;
            this.face.SetExpression(ServitorFace.Expression.Small, 0.1f);
        }
        else if (canJump && Input.GetKeyUp(KeyCode.Space) && (this.isGrounded || (this.Element == Element.Air || this.debugMode)))
        {
            this.upForce = 2f;
            if (!this.isGrounded)
            {
                this.upForce = 2.7f;
                this.face.SetExpression(ServitorFace.Expression.Happy, 2f, true);
                this.SetElement(Element.None);
            }
            
            this.cosmeticGoal = Vector3.up;
            
            this.face.SetExpression(ServitorFace.Expression.Surprise, 0.3f);
        }
        else
        {
            this.cosmeticGoal.y = (this.hoverHeight - 0.2f) * 0.5f;
        }

        if (this.upForce > 0f)
        {
            this.upForce -= Time.deltaTime * 3f;
            goalHeight += this.upForce;
        }
        goalPosition.y = Mathf.Lerp(this.rootTransform.position.y, goalHeight, Time.deltaTime * this.heightSensitivity);
        if (goalPosition.y < groundHeight) goalPosition.y = groundHeight;
        
        this.rootTransform.position = goalPosition;

        if (this.cosmeticTransform != null)
        {
            this.cosmeticTransform.localPosition = Vector3.Lerp(this.cosmeticTransform.localPosition, this.cosmeticGoal, Time.deltaTime * 8f);
        }

        if (this.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1))
            {
                if (this.interactables.Count > 0)
                {
                    for (int i = 0; i < this.interactables.Count; i++)
                    {
                        this.interactables[i].Interact(this);
                    }
                }
            }
        }
    }

    public void FixedUpdate()
    {
        // check through the distances to elements, if player is within a certain range then the area lights up
        if (Vector3.Distance(transform.position, _lights[0].position) < _lightUpDistance) _lights[0].gameObject.SetActive(true);
        else _lights[0].gameObject.SetActive(false);

        if (Vector3.Distance(transform.position, _lights[1].position) < _lightUpDistance) _lights[1].gameObject.SetActive(true);
        else _lights[1].gameObject.SetActive(false);

        if (Vector3.Distance(transform.position, _lights[2].position) < _lightUpDistance) _lights[2].gameObject.SetActive(true);
        else _lights[2].gameObject.SetActive(false);
    }

    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public Vector3 GetModifiedMovement(Vector3 original)
    {
        Vector3 v = original;
        for (int i = 0; i < this.modifiers.Count; i++)
        {
            for (int n = 0; n < 3; n++)
            {
                if (v[n] > 0) v[n] *= this.modifiers[i].PositiveModifier[n];
                else v[n] *= this.modifiers[i].NegativeModifier[n];
            }
        }
        return v;
    }

    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
        this.fallSpeed = 0f;
        this.upForce = 0f;
    }

    private GroundCheckResult GroundCheck(Vector3 origin)
    {
        var result = new GroundCheckResult();
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int count = Physics2D.Raycast(origin + Vector3.up, Vector3.down, groundFilter, hits, 4f);
        if (count > 0)
        {
            int closestIndex = -1;
            float closest = 10000f;
            for (int i = 0; i < count; i ++)
            {
                if (hits[i].distance < closest)
                {
                    closest = hits[i].distance;
                    closestIndex = i;
                }
            }

            RaycastHit2D hit = hits[closestIndex];
            // if we are 2 units or less above the ground, we will match its normal.
            // otherwise, from 2 to 4 units above we will revert to a vertical normal.
            // we subtract 1 to account for the check starting at origin + 1 vertical unit.
            if (hit.distance >= 2f)
            {
                // dist / 2 turns [2,4] into [1,2].  -1 turns it into [0,1]
                result.Normal =Vector3.Slerp(hit.normal, Vector3.up, (hit.distance /2f) -1 );
            }

            result.Normal = hit.normal;
            result.Position = hit.point;
            // to avoid gritty vertical bumps we want to lerp our height to the goal,
            // instead of just snapping to it.
            // goalPosition.y = Mathf.Lerp(this.BodyTransform.position.y, hits[0].point.y + this.targetHeight, Time.deltaTime * this.heightSensitivity);

        }
        else
        {
     
            result.Normal = Vector3.up;
            result.Position = this.transform.position + (Vector3.down * 1000f);
        }
        return result;
    }

    private GroundCheckResult WallCheck(Vector3 offset, GroundCheckResult defaultResult = default(GroundCheckResult))
    {
        Vector3 newPosition = this.rootTransform.position + Vector3.up + offset;
        var result = defaultResult;
        RaycastHit2D hit = Physics2D.Linecast(this.rootTransform.position + Vector3.up, newPosition, LayerMask.GetMask("ground", "barrier"));
        if (hit.collider == null)
        {
            result = GroundCheck(newPosition);
        }
        else
        {
            result.Normal = hit.normal;
            result.Position = hit.point;
        }
        return result;
    }
    
    private struct GroundCheckResult
    {
        public Vector3 Normal;
        public Vector3 Position;

    }
}

// q // cD // d // 
// valkymaera Q1 2020
// Unity 2019.3.0f5