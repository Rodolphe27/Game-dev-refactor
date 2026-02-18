using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Animation(List<Texture2D> frames, double frameTime = 1.0, bool repeat = false)
{
    public List<Texture2D> Frames { get; } = frames;
    public double FrameTime { get; } = frameTime;
    private double timer = 0.0;
    private int frameIndex = 0;
    private bool Repeat = repeat;

    public void Reset()
    {
        frameIndex = 0;
        timer = 0;
    }
    /// <summary>
    /// Returns true if animation restarts, else false 
    /// </summary>
    /// <param name="gameTime"></param>
    /// <returns></returns>
    public bool Update(GameTime gameTime)
    {
        timer += gameTime.ElapsedGameTime.TotalSeconds;

        if (timer >= FrameTime)
        {
            timer -= FrameTime;
            frameIndex++;

            if (frameIndex >= Frames.Count)
            {
                frameIndex = Repeat ? Frames.Count - 1 : 0;
                return true;
            }
        }
        return false;
    }
    public Texture2D GetCurrentFrame()
    {
        return Frames[frameIndex];
    }
}