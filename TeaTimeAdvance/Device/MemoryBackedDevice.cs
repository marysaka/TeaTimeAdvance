using System;
using System.Buffers.Binary;
using TeaTimeAdvance.Bus;

namespace TeaTimeAdvance.Device
{
    public class MemoryBackedDevice : IBusDevice
    {
        protected byte[] _data;

        // 16 KiB
        public virtual uint MappedSize => (uint)_data.Length;

        public MemoryBackedDevice(int size)
        {
            _data = new byte[size];
        }

        public MemoryBackedDevice(ReadOnlySpan<byte> data)
        {
            // Enforce a full copy here.
            _data = data.ToArray();
        }

        public MemoryBackedDevice(ReadOnlySpan<byte> data, int size) : this(size)
        {
            data.CopyTo(_data);
        }

        protected Span<byte> GetSpan(uint address, int size)
        {
            return _data.AsSpan().Slice((int)(address & MappedSize), size);
        }

        public virtual byte Read8(uint address)
        {
            return GetSpan(address, sizeof(byte))[0];
        }

        public virtual ushort Read16(uint address)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(GetSpan(address, sizeof(ushort)));
        }

        public virtual uint Read32(uint address)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(GetSpan(address, sizeof(uint)));
        }

        public virtual void Write8(uint address, byte value)
        {
            GetSpan(address, sizeof(byte))[0] = value;
        }

        public virtual void Write16(uint address, ushort value)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(GetSpan(address, sizeof(ushort)), value);
        }

        public virtual void Write32(uint address, uint value)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(GetSpan(address, sizeof(uint)), value);
        }
    }
}
