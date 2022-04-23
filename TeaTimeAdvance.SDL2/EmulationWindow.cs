using System;
using System.Diagnostics;
using System.IO;
using TeaTimeAdvance.Ppu;
using static SDL2.SDL;

namespace TeaTimeAdvance.SDL2
{
    class EmulationWindow : IDisposable
    {
        private const int WindowScaling = 1;

        private const int WindowWidth = PpuContext.ScreenWidth * WindowScaling;
        private const int WindowHeight = PpuContext.ScreenHeight * WindowScaling;

        private EmulationContext _context;
        private IntPtr _windowHandle;
        private IntPtr _windowSurfaceHandle;
        private uint _windowId;
        private IntPtr _rendererHandle;
        private bool _isRunning;

        public EmulationWindow(Options options)
        {
            Initialize(options);
        }

        private void Initialize(Options options)
        {
            _context = new EmulationContext();

            byte[] bios = File.ReadAllBytes(options.BiosPath);
            byte[] rom = File.ReadAllBytes(options.GamePath);


            _context.Initialize(bios, rom);


            SDL2Driver.Instance.Initialize();

            string romName = Path.GetFileName(options.GamePath);


            _windowHandle = SDL_CreateWindow($"TeaTimeAdvance - {romName}", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, WindowWidth, WindowHeight, SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI);
            _rendererHandle = SDL_CreateRenderer(_windowHandle, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
            _windowSurfaceHandle = SDL_CreateTexture(_rendererHandle,
                                                     SDL_PIXELFORMAT_RGBA8888,
                                                     (int)(SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING),
                                                     PpuContext.ScreenWidth,
                                                     PpuContext.ScreenHeight);

            _windowId = SDL_GetWindowID(_windowHandle);
            SDL2Driver.Instance.RegisterWindow(_windowId, HandleWindowEvent);
        }

        private void HandleWindowEvent(SDL_Event evnt)
        {
            if (evnt.type == SDL_EventType.SDL_WINDOWEVENT)
            {
                switch (evnt.window.windowEvent)
                {
                    case SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
                        _isRunning = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public void Dispose()
        {
            SDL_DestroyTexture(_windowSurfaceHandle);
            SDL_DestroyRenderer(_rendererHandle);
            SDL_DestroyWindow(_windowHandle);

            SDL2Driver.Instance.Dispose();
        }

        internal void Execute()
        {
            _isRunning = true;

            while (_isRunning && _context.IsRunning())
            {
                SDL_PumpEvents();

                ReadOnlySpan<byte> rawKeyboardState;

                unsafe
                {
                    IntPtr statePtr = SDL_GetKeyboardState(out int numKeys);

                    rawKeyboardState = new ReadOnlySpan<byte>((byte*)statePtr, numKeys);
                }

                if (rawKeyboardState[(int)SDL_Scancode.SDL_SCANCODE_ESCAPE] != 0)
                {
                    _isRunning = false;

                    break;
                }

                ReadOnlySpan<uint> frame = _context.ExecuteFrame();


                SDL_Rect rect = new SDL_Rect
                {
                    w = PpuContext.ScreenWidth,
                    h = PpuContext.ScreenHeight,
                    x = 0,
                    y = 0,
                };

                int res = SDL_LockTexture(_windowSurfaceHandle, ref rect, out IntPtr pixels, out _);

                if (res < 0)
                {
                    string error = SDL_GetError();


                    Console.WriteLine(error);
                }


                Debug.Assert(pixels != IntPtr.Zero);

                unsafe
                {
                    frame.CopyTo(new Span<uint>((uint*)pixels, rect.w * rect.h));
                }

                SDL_UnlockTexture(_windowSurfaceHandle);
                SDL_RenderCopy(_rendererHandle, _windowSurfaceHandle, ref rect, ref rect);

                // TODO: Render frame here
                // TODO: Handle keyboard and controls
                // TODO: Move rendering in its own thread and still pump here?
                SDL_RenderPresent(_rendererHandle);
            }
        }
    }
}
