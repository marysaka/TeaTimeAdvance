using System;
using System.IO;
using TeaTimeAdvance.Ppu;
using static SDL2.SDL;

namespace TeaTimeAdvance.SDL2
{
    class EmulationWindow : IDisposable
    {
        private const int WindowScaling = 2;

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


                SDL_Rect textureRect = new SDL_Rect
                {
                    w = PpuContext.ScreenWidth,
                    h = PpuContext.ScreenHeight,
                    x = 0,
                    y = 0,
                };

                SDL_Rect screenRect = new SDL_Rect
                {
                    w = WindowWidth,
                    h = WindowHeight,
                    x = 0,
                    y = 0,
                };

                SDL_LockTexture(_windowSurfaceHandle, ref textureRect, out IntPtr pixels, out _);

                unsafe
                {
                    frame.CopyTo(new Span<uint>((uint*)pixels, textureRect.w * textureRect.h));
                }

                SDL_UnlockTexture(_windowSurfaceHandle);
                SDL_RenderCopy(_rendererHandle, _windowSurfaceHandle, ref textureRect, ref screenRect);

                // TODO: Render frame here
                // TODO: Handle keyboard and controls
                // TODO: Move rendering in its own thread and still pump events here?
                SDL_RenderPresent(_rendererHandle);
            }
        }
    }
}
