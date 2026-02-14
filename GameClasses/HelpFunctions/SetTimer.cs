
using System;
using System.Collections.Generic;
public class SetTimer
{
    public event Action Elapsed;
    private double TimerCurrTime;
    private readonly double TimerEndTime;
    private static readonly List<SetTimer> Timers = [];

    public SetTimer(int miliseconds)
    {
        TimerCurrTime = Globals.TotalTime;
        TimerEndTime = TimerCurrTime + miliseconds;
        Timers.Add(this);
    }
    public static void Update()
    {
        for(int i = Timers.Count - 1; i >= 0; i--)
        {
            Timers[i].TimerCurrTime = Globals.TotalTime;
            if(Timers[i].TimerCurrTime >= Timers[i].TimerEndTime)
            {
                Timers[i].Elapsed?.Invoke();
                Timers.RemoveAt(i);
            }
        }
    }
}