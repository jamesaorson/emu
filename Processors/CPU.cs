using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sharp6502.Processors
{
    public static class CPU
    {
        #region Public

        #region Members
        public static bool IsPoweredOn { get; private set; }
        public static ulong NumClocks { get; private set; }
        public static ClockSpeed Speed { get; set; }
        #endregion

        #region Member Methods
        public static void PowerDown()
        {
            Console.WriteLine("CPU powering off...");
            if (!IsPoweredOn)
            {
                return;
            }
            IsPoweredOn = false;
            Console.WriteLine("CPU powered off!");
        }

        public static void PowerUp()
        {
            Console.WriteLine("CPU powering on...");
            if (IsPoweredOn)
            {
                return;
            }
            IsPoweredOn = true;
            Console.WriteLine("CPU powered on!");
        }

        public static void Run(bool interactive = false)
        {
            Console.WriteLine("Running CPU...");
            if (interactive)
            {
                Console.WriteLine("Press any key to shutdown...");
            }
            NumClocks = 0;
            _totalClock = Stopwatch.StartNew();
            _clock = Stopwatch.StartNew();
            while (IsPoweredOn)
            {
                Tick();
            }
            Console.WriteLine($"CPU uptime: {_totalClock.Elapsed}");
            Console.WriteLine($"CPU clocks: {NumClocks}");
        }

        public static async Task RunAsync(bool interactive = false)
        {
            await Task.Run(() => Run(interactive));
        }

        public static async Task RunInteractiveAsync()
        {
            PowerUp();
            var task = RunAsync(interactive: true);
            Console.ReadKey(intercept: false);
            PowerDown();
            await task;
        }

        public static void Tick()
        {
            while (_clock.ElapsedTicks < _clockSpeedToTicks)
            {
            }
            _clock.Restart();
            NumClocks++;
        }
        #endregion

        #endregion

        #region Private

        #region Members
        private static Stopwatch _clock;
        private static Stopwatch _totalClock;
        private static long _clockSpeedToTicks { get; set; }
        #endregion

        #endregion

        static CPU()
        {
            Speed = ClockSpeed.OneMegahertz;
            _clockSpeedToTicks = (long)Speed;
        }
    }

    public enum ClockSpeed : long
    {
        OneMegahertz = 1000,
        TwoMegahertz = 500,
        ThreeMegahertz = 333,
        FourMegahertz = 250,
    }
}