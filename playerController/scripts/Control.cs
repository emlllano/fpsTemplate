using Godot;

public partial class DotCrosshair : Control
{
    [ExportGroup("CHcustom")]
    [Export] public float DotSize = 2.0f;
    [Export] public Color DotColor = Colors.White;

    public override void _Ready()
    {

        Vector2 screenSize = GetViewportRect().Size;
        Position = screenSize / 2;
    }

    public override void _Draw()
    {

        DrawCircle(Vector2.Zero, DotSize, DotColor);


        DrawArc(Vector2.Zero, DotSize, 0, Mathf.Tau, 32, Colors.Black, 1.0f);
    }
}
