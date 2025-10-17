using Godot;
using System;

public partial class fpsLabel : Label
{
    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {       
        var FPS = 0.0d;
        FPS = Engine.GetFramesPerSecond();
        Text = "FPS: " + FPS.ToString("F2");

    }
}
