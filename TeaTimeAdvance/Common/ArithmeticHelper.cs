using System.Runtime.CompilerServices;

namespace TeaTimeAdvance.Common
{
    public static class ArithmeticHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CarryFrom(ulong value) => value > uint.MaxValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BorrowFrom(ulong value) => value <= uint.MaxValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OverflowFrom(long value) => value < int.MinValue || value > int.MaxValue;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Addition(out bool outCarry, out bool overflowFlag, uint a, uint b, uint c = 0)
        {
            ulong res = (ulong)a + b + c;
            long temp = (long)a + b + c;

            outCarry = CarryFrom(res);
            overflowFlag = OverflowFrom(temp);

            return (uint)res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Subtraction(out bool carryFlag, out bool overflowFlag, uint a, uint b, uint c = 0)
        {
            ulong res = (ulong)a - b - c;
            long temp = (long)a - b - c;

            carryFlag = BorrowFrom(res);
            overflowFlag = OverflowFrom(temp);

            return (uint)res;
        }
    }
}
