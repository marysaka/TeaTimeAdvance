using System.Runtime.CompilerServices;

namespace TeaTimeAdvance.Common
{
    public static class ArithmeticHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Addition(out bool outCarry, uint a, uint b, uint c = 0)
        {
            ulong res = (ulong)a + b + c;

            outCarry = res > uint.MaxValue;


            return (uint)res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Subtraction(out bool outCarry, uint a, uint b, uint c = 0)
        {
            ulong res = (ulong)a - b - c;

            outCarry = res <= uint.MaxValue;


            return (uint)res;
        }
    }
}
