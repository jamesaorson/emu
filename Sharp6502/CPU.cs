using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Sharp6502.Buses;

namespace Sharp6502
{
    public static class CPU
    {
        #region Public

        #region Static Fields
        public static OpCode CurrentInstruction { get; private set; }
        public static UInt16 CurrentInstructionAddress => AddressBus.Bus;
        public static IList<byte> CurrentInstructionBytes { get; private set; }
        public static bool IsInteractive { get; private set; }
        public static bool IsPoweredOn { get; private set; }
        public static UInt64 NumClocks { get; private set; }
        public static UInt16 ProgramCounter { get; private set; }
        public static byte ProgramCounterHigh => (byte)((ProgramCounter >> (UInt16)0x0004) & (UInt16)0x00FF);
        public static byte ProgramCounterLow => (byte)(ProgramCounter & (UInt16)0x00FF);
        public static byte RemainingInstructionCycles { get; private set; }
        public static ClockSpeed Speed { get; set; }
        #endregion

        #region Static Methods
        public static void LoadProgram(string filepath)
        {
            LoadProgram(File.ReadAllBytes(filepath));
        }
        
        public static void LoadProgram(byte[] program)
        {
            Memory.LoadProgram(program);
        }

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
            Initialize();
            IsPoweredOn = true;
            Console.WriteLine("CPU powered on!");
        }

        public static void Run(string filepath, bool isInteractive = false)
        {
            IsInteractive = isInteractive;
            if (!IsPoweredOn)
            {
                PowerOn();
            }
            LoadProgram(filepath);
            Console.WriteLine("Running CPU...");
            if (IsInteractive)
            {
                Console.WriteLine("Press any key to shutdown...");
            }
            NumClocks = 0;
            _totalClock = Stopwatch.StartNew();
            _clock = Stopwatch.StartNew();
            
            FetchInstruction(false); // Fetch first instruction
            while (IsPoweredOn)
            {
                Tick();
            }
            Console.WriteLine($"CPU uptime: {_totalClock.Elapsed}");
            Console.WriteLine($"CPU clocks: {NumClocks}");
        }

        public static async Task RunAsync(string filepath, bool interactive = false)
        {
            await Task.Run(() => Run(filepath, interactive));
        }

        public static async Task RunInteractiveAsync(string filepath)
        {
            var task = RunAsync(filepath, interactive: true);
            while (IsPoweredOn) {}
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

        #region Internal

        #region Static Methods
        internal static void AdvanceProgramCounter(byte advancement)
        {
            ProgramCounter += advancement;
        }

        internal static UInt16 CombineLowAndHighAddress(byte lowOrderAddress, byte highOrderOrder)
        {
            var combinedAddress = (UInt16)((highOrderOrder << 4) & 0xFF00);
            combinedAddress = (UInt16)(combinedAddress | lowOrderAddress);
            return combinedAddress;
        }

        internal static byte FetchMemoryAddress(UInt16 address)
        {
            return Memory.Data[address];
        }

        internal static void SetMemoryAddress(UInt16 address, byte value)
        {
            Memory.Data[address] = value;
        }

        #region Addressing Modes
        internal static byte AbsoluteAddress(byte lowOrderAddress, byte highOrderAddress) => FetchMemoryAddress(
            CombineLowAndHighAddress(lowOrderAddress, highOrderAddress)
        );

        internal static byte ImmediateAddress(byte value) => value;

        internal static byte IndexedIndirectAddress(byte xOffset)
        {
            var memoryLocation = ALU.AddToIndexRegisterX(xOffset);
            return FetchMemoryAddress(
                CombineLowAndHighAddress(
                    FetchMemoryAddress((UInt16)memoryLocation),
                    FetchMemoryAddress((UInt16)(memoryLocation + (byte)0x01))
                )
            );
        }

        internal static byte ZeroPageAddress(byte address) => FetchMemoryAddress(
            CombineLowAndHighAddress(address, (byte)0x00)
        );
        #endregion

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
            if (IsInteractive)
            {
                _clock.Stop();
                _totalClock.Stop();
                Console.Write("0x{0:X4} ", CurrentInstructionAddress);
                Console.Write("0x{0:X2} ", CurrentInstructionBytes[0]); Console.Write(CurrentInstruction.Name);
                for (var i = 1; i < CurrentInstructionBytes.Count; ++i)
                {
                    Console.Write(" 0x{0:X2}", CurrentInstructionBytes[i]);
                }
                Console.Write(" Continue? (y/n) ");
                
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.KeyChar == 'n')
                {
                    PowerOff();
                }
                _clock.Start();
                _totalClock.Start();
            }
            CurrentInstruction.Execute(CurrentInstructionBytes);
        }

        private static void FetchInstruction(bool increment = true)
        {
            if (increment)
            {
                ProgramCounter++;
            }
            AddressBus.Bus = ProgramCounter;
            CurrentInstruction = InstructionSet.ConvertToOpCode(Memory.Data[AddressBus.Bus]);
            if (CurrentInstruction == null)
            {
                Console.Write("0x{0:X4} ", AddressBus.Bus); Console.WriteLine("0x{0:X2}", Memory.Data[AddressBus.Bus]);
                throw new Exception("Fetched instruction was null");
            }
            RemainingInstructionCycles = CurrentInstruction.Cycles;
            CurrentInstructionBytes.Clear();
            for (var i = 0; i < CurrentInstruction.InstructionBytes; i++)
            {
                CurrentInstructionBytes.Add(Memory.Data[AddressBus.Bus + i]);
            }
            ProgramCounter += (UInt16)(CurrentInstruction.InstructionBytes - 0x01);
        }

        private static void Initialize()
        {
            ALU.Initialize();
            AddressBus.Initialize();
            DataBus.Initialize();
            Memory.Initialize();

            CurrentInstructionBytes = new List<byte>();
            NumClocks = 0;
            ProgramCounter = 0x0000;
            RemainingInstructionCycles = 1;
            Speed = ClockSpeed.OneMegahertz;
            _clockSpeedToTicks = (long)Speed;
        }
        #endregion

        #endregion

        static CPU()
        {
            Initialize();
        }
    }
}