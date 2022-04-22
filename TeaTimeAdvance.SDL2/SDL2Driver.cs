using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using static SDL2.SDL;

namespace TeaTimeAdvance.SDL2
{
    class SDL2Driver : IDisposable
    {
        private static SDL2Driver _instance;

        public static bool IsInitialized => _instance != null;

        public static SDL2Driver Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SDL2Driver();
                }

                return _instance;
            }
        }

        private const uint InitFlags = SDL_INIT_EVENTS | SDL_INIT_VIDEO;

        private bool _isRunning;
        private uint _refereceCount;
        private Thread _worker;
        private ConcurrentDictionary<uint, Action<SDL_Event>> _registeredWindowHandlers;

        private object _lock = new object();

        private SDL2Driver() { }

        public void Initialize()
        {
            lock (_lock)
            {
                _refereceCount++;

                if (_isRunning)
                {
                    return;
                }

                if (SDL_Init(InitFlags) != 0)
                {
                    throw new Exception($"SDL2 initlaization failed with error \"{SDL_GetError()}\"");
                }

                _registeredWindowHandlers = new ConcurrentDictionary<uint, Action<SDL_Event>>();
                _worker = new Thread(EventWorker);
                _isRunning = true;
                _worker.Start();
            }
        }

        private void EventWorker()
        {
            const int WaitTimeMs = 10;

            using ManualResetEventSlim waitHandle = new ManualResetEventSlim(false);

            while (_isRunning)
            {
                while (SDL_PollEvent(out SDL_Event evnt) != 0)
                {
                    HandleSDLEvent(ref evnt);
                }

                waitHandle.Wait(WaitTimeMs);
            }
        }

        private void HandleSDLEvent(ref SDL_Event evnt)
        {
            if (evnt.type == SDL_EventType.SDL_WINDOWEVENT || evnt.type == SDL_EventType.SDL_MOUSEBUTTONDOWN || evnt.type == SDL_EventType.SDL_MOUSEBUTTONUP)
            {
                if (_registeredWindowHandlers.TryGetValue(evnt.window.windowID, out Action<SDL_Event> handler))
                {
                    handler(evnt);
                }
            }
        }

        public bool RegisterWindow(uint windowId, Action<SDL_Event> windowEventHandler)
        {
            return _registeredWindowHandlers.TryAdd(windowId, windowEventHandler);
        }

        public void UnregisterWindow(uint windowId)
        {
            _registeredWindowHandlers.Remove(windowId, out _);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            lock (_lock)
            {
                if (_isRunning)
                {
                    _refereceCount--;

                    if (_refereceCount == 0)
                    {
                        _isRunning = false;

                        _worker?.Join();

                        SDL_Quit();
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
