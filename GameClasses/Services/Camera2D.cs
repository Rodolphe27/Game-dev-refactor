
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Camera2D(Viewport viewport)
{
    private Vector2 Position = Vector2.Zero;
    private float Zoom = 0.5f;
    private float Rotation = 0f;
    private Viewport Viewport = viewport;
    private double LevelWidth = 0;
    private double LevelHeight = 0;
    public Camera2D Update(Viewport viewport, double levelHeight, double levelWidth)
    {
        LevelHeight = levelHeight;
        LevelWidth  = levelWidth;
        Viewport    = viewport;
        return this;
    }
    public void Follow(Vector2 targetPosition)
    {
        float posX = targetPosition.X - Viewport.Width / 2f * 1/Zoom;
        float posY = targetPosition.Y - Viewport.Height / 2f * 1/Zoom;

            posX 
            = 0 < (LevelWidth - Viewport.Width * 1/Zoom)
            ? (float)Math.Clamp(posX, 0, LevelWidth - Viewport.Width * 1/Zoom) 
            : (float)(0 - (Viewport.Width * 1/Zoom - LevelWidth) / 2);
        
            posY 
            = 0 < (LevelHeight - Viewport.Height * 1/Zoom)
            ? (float)Math.Clamp(posY, 0, LevelHeight - Viewport.Height * 1/Zoom)
            : (float)(0 - (Viewport.Height * 1/Zoom - LevelHeight) / 2);

        Position = Vector2.Lerp(Position, new Vector2(posX, posY), 0.05f);
    }
    public Matrix GetTransformation()
    {
        return
            Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1f);
    }
}
