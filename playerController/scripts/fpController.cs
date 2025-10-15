using Godot;
using System;

public partial class fpController : CharacterBody3D
{
    [Export] public float MoveSpeed = 9.0f;
    [Export] public float AirAcceleration = 7.0f; //better feeling ?
    [Export] public float GroundAcceleration = 14.0f;
    [Export] public float Friction = 7.0f;
    [Export] public float JumpForce = 12.0f;
    [Export] public float Gravity = -20.0f; //velocity y is negative like what did i think
    [Export] public float MouseSensitivity = 0.001f;

    private Vector3 _velocity = Vector3.Zero;
    private Node3D _cameraPivot;
    private Camera3D _camera;
    private Transform3D _restartTransform;
    private Vector3 _restartVelocity;

    private float _yaw = 0f;
    private float _pitch = 0f;

    public override void _Ready()
    {
        _cameraPivot = GetNode<Node3D>("CameraPivot");
        _camera = _cameraPivot.GetNode<Camera3D>("Camera3D");
        _restartTransform = GlobalTransform;
        _restartVelocity = _velocity;

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion)
        {
            _yaw -= motion.Relative.X * MouseSensitivity;
            _pitch -= motion.Relative.Y * MouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, Mathf.DegToRad(-85.0f), Mathf.DegToRad(85.0f));

            Rotation = new Vector3(0, _yaw, 0);
            _cameraPivot.Rotation = new Vector3(_pitch, 0, 0);
        }

        if (@event.IsActionPressed("exit"))
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured
                ? Input.MouseModeEnum.Visible
                : Input.MouseModeEnum.Captured;
        }
        if (Input.IsActionJustPressed("restart"))
        {
            GlobalTransform = _restartTransform;
            _velocity = _restartVelocity;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 dir = GetInputDirection();

        bool grounded = IsOnFloor();

        if (grounded)
            GroundMovement(dir, delta);
        else
            AirMovement(dir, delta);

        _velocity.Y += Gravity * (float)delta;
        Velocity = _velocity;
        MoveAndSlide();


    }

    private Vector3 GetInputDirection()
    {
        Vector2 input = Input.GetVector("left", "right", "forward", "backward");

        Vector3 forward = -_cameraPivot.GlobalTransform.Basis.Z;
        Vector3 right = _cameraPivot.GlobalTransform.Basis.X;

        Vector3 dir = (forward * -input.Y) + (right * input.X);
        dir.Y = 0;
        return dir.Length() > 0.001f ? dir.Normalized() : Vector3.Zero;
    }

    private void GroundMovement(Vector3 wishDir, double delta)
    {
        Vector3 horiz = _velocity; horiz.Y = 0;
        float speed = horiz.Length();
        if (speed > 0.001f)
        {
            float speedDrop = speed * Friction * (float)delta;
            float newSpeed = Math.Max(speed - speedDrop, 0f);
            horiz = speed > 0 ? horiz * (newSpeed / speed) : Vector3.Zero;
            _velocity.X = horiz.X;
            _velocity.Z = horiz.Z;
        }

        if (wishDir != Vector3.Zero)
            Accelerate(wishDir, MoveSpeed, GroundAcceleration, delta);

        if (Input.IsActionJustPressed("jump"))
        {
            _velocity.Y = JumpForce;
        }
        else
        {
            _velocity.Y = -2f;
        }
    }

    private void AirMovement(Vector3 dir, double delta)
    {
        if (dir != Vector3.Zero)
        {
            Accelerate(dir, MoveSpeed, AirAcceleration, delta);
        }
        _velocity.Y += Gravity * (float)delta;

    }

    private void Accelerate(Vector3 dir, float speed, float accel, double delta)
    {
        if (dir == Vector3.Zero) return;
        float currentSpeed = _velocity.Dot(dir);
        float addSpeed = speed - currentSpeed;
        if (addSpeed <= 0) return;

        float accelSpeed = accel * (float)delta * speed;
        if (accelSpeed > addSpeed) accelSpeed = addSpeed;
        _velocity += dir * accelSpeed;
    }
}
