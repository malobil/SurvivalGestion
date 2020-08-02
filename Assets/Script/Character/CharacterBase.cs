using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{

    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private Transform m_camera = null ;

    private PlayerInputAction m_inputs;
    private float m_xCameraRotation;

    private Rigidbody m_rb { get => GetComponent<Rigidbody>(); }

    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }

    protected virtual void OnStart()
    {
        m_inputs = new PlayerInputAction();
        m_inputs.Character.Jump.performed += ctx => Jump();
        EnableInput();
    }

    protected virtual void OnUpdate()
    {
        Move();
        CameraControl();
    }

    protected virtual void OnFixedUpdate()
    {
      
    }

    protected void Move()
    {
        Vector3 moveDirection = new Vector3(m_inputs.Character.Move.ReadValue<Vector2>().x, m_rb.velocity.y, m_inputs.Character.Move.ReadValue<Vector2>().y);
        moveDirection =  (moveDirection.z * transform.forward) + (moveDirection.x * transform.right);
        m_rb.velocity = moveDirection * m_moveSpeed * Time.deltaTime;
    }

    protected virtual void CameraControl()
    {
        float mouseX = m_inputs.Character.CameraControl.ReadValue<Vector2>().x *5f * Time.deltaTime;
        float mouseY = m_inputs.Character.CameraControl.ReadValue<Vector2>().y * 5f * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        m_xCameraRotation -= mouseY;
        m_xCameraRotation = Mathf.Clamp(m_xCameraRotation, -90f, 90f);

        m_camera.localRotation = Quaternion.Euler(m_xCameraRotation, m_camera.localRotation.y, m_camera.localRotation.z);
    }

    protected virtual void Jump()
    {

        m_rb.AddForce(transform.up * 100f, ForceMode.Impulse);
    }

    protected virtual void EnableInput()
    {
        m_inputs.Enable();
    }

    protected virtual void DisableInput()
    {
        m_inputs.Disable();
    }
}
