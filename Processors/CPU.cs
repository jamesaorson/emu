using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sharp6502.Processors
{
    public class CPU
    {
        #region Public

        #region Constructors
        public CPU(ClockSpeed speed = ClockSpeed.OneMegahertz)
        {
            Speed = speed;
            _clockSpeedToTicks = (long)Speed;
        }
        #endregion

        #region Members
        public bool IsPoweredOn { get; private set; }
        public ulong NumClocks { get; private set; }
        public ClockSpeed Speed { get; private set; }
        #endregion

        #region Member Methods
        public void PowerUp()
        {
            Console.WriteLine("CPU powering on...");
            if (IsPoweredOn)
            {
                return;
            }
            IsPoweredOn = true;
            Console.WriteLine("CPU powered on!");
        }

        public void PowerDown()
        {
            Console.WriteLine("CPU powering off...");
            if (!IsPoweredOn)
            {
                return;
            }
            IsPoweredOn = false;
            Console.WriteLine("CPU powered off!");
        }

        public void Run(bool interactive = false)
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

        public async Task RunAsync(bool interactive = false)
        {
            await Task.Run(() => Run(interactive));
        }

        public async Task RunInteractiveAsync()
        {
            PowerUp();
            var task = RunAsync(interactive: true);
            Console.ReadKey(intercept: false);
            PowerDown();
            await task;
        }

        public void Tick()
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
        private Stopwatch _clock { get; set; }
        private Stopwatch _totalClock { get; set; }
        private long _clockSpeedToTicks { get; set; }
        #endregion

        #endregion
    }

    public enum ClockSpeed : long
    {
        OneMegahertz = 1000,
        TwoMegahertz = 500,
        ThreeMegahertz = 333,
        FourMegahertz = 250,
    }
}