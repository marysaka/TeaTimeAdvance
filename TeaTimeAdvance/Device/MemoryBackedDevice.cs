using System;
using System.Buffers.Binary;
using System.Diagnostics;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Common;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance.Device
{
    public class MemoryBackedDevice : IBusDevice
    {
        private const int MaxBusAccessType = 2;
        private const int MaxBusAccessInfoMemoryType = 3;

        protected byte[] _data;

        public virtual uint MappedSize => (uint)_data.Length;

        private readonly byte[] _waitTimes;

        public MemoryBackedDevice(int size, byte[] waitTimes = null)
        {
            _data = new byte[size];

            if (waitTimes == null)
            {
                _waitTimes = new byte[MaxBusAccessType * MaxBusAccessInfoMemoryType];
                _waitTimes.AsSpan().Fill(1);
            }
            else
            {
                Debug.Assert(waitTimes.Length == MaxBusAccessType * MaxBusAccessInfoMemoryType);

                _waitTimes = waitTimes;
            }
        }

        public MemoryBackedDevice(ReadOnlySpan<byte> data, byte[] waitTimes = null)
        {
            // Enforce a full copy here.
            _data = data.ToArray();

            if (waitTimes == null)
            {
                _waitTimes = new byte[MaxBusAccessType * MaxBusAccessInfoMemoryType];
                _waitTimes.AsSpan().Fill(1);
            }
            else
            {
                Debug.Assert(waitTimes.Length == MaxBusAccessType * MaxBusAccessInfoMemoryType);

                _waitTimes = waitTimes;
            }
        }

        public MemoryBackedDevice(ReadOnlySpan<byte> data, int size, byte[] waitTimes = null) : this(size, waitTimes)
        {
            data.CopyTo(_data);
        }

        protected Span<byte> GetSpan(uint baseAddress, uint address, int size)
        {
            return _data.AsSpan().Slice((int)((address - baseAddress) % MappedSize), size);
        }

        public virtual byte Read8(uint baseAddress, uint address)
        {
            return GetSpan(baseAddress, address, sizeof(byte))[0];
        }

        public virtual ushort Read16(uint baseAddress, uint address)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(GetSpan(baseAddress, address, sizeof(ushort)));
        }

        public virtual uint Read32(uint baseAddress, uint address)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(GetSpan(baseAddress, address, sizeof(uint)));
        }

        public virtual void Write8(uint baseAddress, uint address, byte value)
        {
            GetSpan(baseAddress, address, sizeof(byte))[0] = value;
        }

        public virtual void Write16(uint baseAddress, uint address, ushort value)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(GetSpan(baseAddress, address, sizeof(ushort)), value);
        }

        public virtual void Write32(uint baseAddress, uint address, uint value)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(GetSpan(baseAddress, address, sizeof(uint)), value);
        }

        public virtual void UpdateScheduler(BusContext context, uint address, BusAccessType accessType, BusAccessInfo accessInfo)
        {
            context.UpdateCycles(_waitTimes[(int)(accessInfo & BusAccessInfo.MemoryMask) * (int)accessType]);
        }
    }
}
