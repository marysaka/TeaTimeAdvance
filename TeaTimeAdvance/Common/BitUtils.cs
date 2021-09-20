namespace TeaTimeAdvance.Common
{
    public class BitUtils
    {
        public static uint AlignUp(uint value, int size)
        {
            return (uint)AlignUp((int)value, size);
        }

        public static int AlignUp(int value, int size)
        {
            return (value + (size - 1)) & -size;
        }

        public static ulong AlignUp(ulong value, int size)
        {
            return (ulong)AlignUp((long)value, size);
        }

        public static long AlignUp(long value, int size)
        {
            return (value + (size - 1)) & -(long)size;
        }

        public static uint AlignDown(uint value, int size)
        {
            return (uint)AlignDown((int)value, size);
        }

        public static int AlignDown(int value, int size)
        {
            return value & -size;
        }

        public static ulong AlignDown(ulong value, int size)
        {
            return (ulong)AlignDown((long)value, size);
        }

        public static long AlignDown(long value, int size)
        {
            return value & -size;
        }
    }
}
