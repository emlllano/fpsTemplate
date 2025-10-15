using Godot;
using System;

public partial class fpsLabel : Label
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var FPS = 0.0d;
        FPS = Engine.GetFramesPerSecond();
        Text = "FPS: " + FPS.ToString("F2");

    }
}
