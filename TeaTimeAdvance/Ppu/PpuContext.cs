namespace TeaTimeAdvance.Ppu
{
    public class PpuContext
    {
        private const int PaletteMemorySize = 0x1000;
        private const int VideoMemorySize = 0x18000;
        private const int ObjectAttributesMemorySize = 0x1000;

        public byte[] PaletteMemory { get; }
        public byte[] VideoMemory { get; }
        public byte[] ObjectAttributesMemory { get; }

        public PpuContext()
        {
            PaletteMemory = new byte[PaletteMemorySize];
            VideoMemory = new byte[VideoMemorySize];
            ObjectAttributesMemory = new byte[ObjectAttributesMemorySize];
        }
    }
}
