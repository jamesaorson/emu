using System;
using System.Threading.Tasks;
using Sharp6502.Processors;

namespace Sharp6502
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cpu = new CPU(speed: ClockSpeed.OneMegahertz);
            await cpu.RunInteractiveAsync();
        }
    }
}
