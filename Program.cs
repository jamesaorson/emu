using System.Threading.Tasks;
using Sharp6502;

class Program
{
    static async Task Main(string[] args)
    {
        await CPU.RunInteractiveAsync("Examples/nestest.nes");
    }
}
