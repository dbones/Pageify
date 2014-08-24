using System;
using System.Threading;

namespace PageObjects
{
    public static class ControlExtensions
    {
        public static void WaitTillLoaded(this Control control, int wait = 200)
        {
            for (int i = 0; i < wait; i++)
            {
                if (control.IsLoaded)
                {
                    return;
                }
                Thread.Sleep(20);
            }
            throw new Exception(control.Name);
        }
    }
}