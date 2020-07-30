using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Sharp6502.Buses;

namespace Sharp6502
{
    public static class CPU
    {
        #region Public

        #region Static Fields
        public static OpCode CurrentInstruction { get; private set; }
        public static IList<byte> CurrentInstructionBytes { get; private set; }
        public static bool IsPoweredOn { get; private set; }
        public static UInt64 NumClocks { get; private set; }
        public static UInt16 ProgramCounter { get; private set; }
        public static byte ProgramCounterHigh => (byte)((ProgramCounter >> (UInt16)0x0004) & (UInt16)0x00FF);
        public static byte ProgramCounterLow => (byte)(ProgramCounter & (UInt16)0x00FF);
        public static byte RemainingInstructionCycles { get; private set; }
        public static ClockSpeed Speed { get; set; }
        public static Register Accumulator { get; private set; }
        public static Register IndexRegisterX { get; private set; }
        public static Register IndexRegisterY { get; private set; }
        public static Register InstructionRegister { get; private set; }
        public static Register ProcessorStatusRegister { get; private set; }
        public static Register StackPointer { get; private set; }
        #endregion

        #region Static Methods
        public static void PowerOff()
        {
            Console.WriteLine("CPU powering off...");
            if (!IsPoweredOn)
            {
                return;
            }
            IsPoweredOn = false;
            Console.WriteLine("CPU powered off!");
        }

        public static void PowerOn()
        {
            Console.WriteLine("CPU powering on...");
            if (IsPoweredOn)
            {
                return;
            }
            ProgramCounter = 0x0000;
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
            PowerOn();
            var task = RunAsync(interactive: true);
            Console.ReadKey(intercept: false);
            PowerOff();
            await task;
        }

        public static void Tick()
        {
            while (_clock.ElapsedTicks < _clockSpeedToTicks)
            {
            }
            _clock.Restart();
            NumClocks++;
            if (--RemainingInstructionCycles == 0)
            {
                Execute();
                FetchInstruction();
            }
        }
        #endregion

        #endregion

        #region Private

        #region Static Fields
        private static Stopwatch _clock;
        private static Stopwatch _totalClock;
        private static long _clockSpeedToTicks { get; set; }
        #endregion

        #region Static Methods
        private static void Execute()
        {
            CurrentInstruction.Execute(CurrentInstructionBytes);
        }

        private static void FetchInstruction()
        {
            ProgramCounter++;
            AddressBus.Bus = ProgramCounter;
            CurrentInstruction = InstructionSet.ConvertToOpCode(Memory.Data[AddressBus.Bus]);
            if (CurrentInstruction == null)
            {
                throw new Exception("Fetched instruction was null");
            }
            CurrentInstructionBytes.Clear();
            for (var i = 0; i < CurrentInstruction.InstructionBytes; i++)
            {
                CurrentInstructionBytes.Add(Memory.Data[AddressBus.Bus + i]);
            }
            ProgramCounter += (UInt16)(CurrentInstruction.InstructionBytes - 0x01);
        }
        #endregion

        #endregion

        static CPU()
        {
            CurrentInstructionBytes = new List<byte>();
            NumClocks = 0;
            ProgramCounter = 0x0000;
            RemainingInstructionCycles = 1;
            Speed = ClockSpeed.OneMegahertz;
            _clockSpeedToTicks = (long)Speed;
            
            Accumulator = new Register();
            IndexRegisterX = new Register();
            IndexRegisterY = new Register();
            InstructionRegister = new Register();
            ProcessorStatusRegister = new Register();
            StackPointer = new Register();
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