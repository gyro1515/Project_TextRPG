using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    internal class ScreenManager
    {
        private static ScreenManager? instance;
        private ScreenManager() { }

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null) instance = new ScreenManager();

                return instance;
            }
        }

        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint CONSOLE_TEXTMODE_BUFFER = 1;

        private IntPtr[] buffers = new IntPtr[2];
        private int currentIndex = 0;
        private int width = Console.WindowWidth;
        private int height = Console.WindowHeight;

        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            public short X;
            public short Y;
            public COORD(short x, short y) { X = x; Y = y; }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateConsoleScreenBuffer(
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr securityAttributes,
            uint flags,
            IntPtr screenBufferData);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleActiveScreenBuffer(IntPtr hConsoleOutput);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteConsoleOutputCharacter(
            IntPtr hConsoleOutput,
            string lpCharacter,
            uint nLength,
            COORD dwWriteCoord,
            out uint lpNumberOfCharsWritten);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCursorPosition(IntPtr hConsoleOutput, COORD dwCursorPosition);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCursorInfo(IntPtr hConsoleOutput, ref CONSOLE_CURSOR_INFO lpConsoleCursorInfo);

        [DllImport("kernel32.dll")]
        private static extern bool FillConsoleOutputCharacter(
            IntPtr hConsoleOutput, char cCharacter, int nLength, COORD dwWriteCoord, out uint lpNumberOfCharsWritten);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        [StructLayout(LayoutKind.Sequential)]
        private struct CONSOLE_CURSOR_INFO
        {
            public uint dwSize;
            public bool bVisible;
        }

        public void Initialize()
        {
            for (int i = 0; i < 2; i++)
            {
                buffers[i] = CreateConsoleScreenBuffer(
                    GENERIC_READ | GENERIC_WRITE,
                    0, IntPtr.Zero, CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero);

                var cursorInfo = new CONSOLE_CURSOR_INFO { dwSize = 1, bVisible = false };
                SetConsoleCursorInfo(buffers[i], ref cursorInfo);
            }
        }

        public void Clear()
        {
            COORD origin = new COORD(0, 0);
            FillConsoleOutputCharacter(buffers[currentIndex], ' ', width * height, origin, out _);
        }

        public void Flip()
        {
            SetConsoleActiveScreenBuffer(buffers[currentIndex]);
            currentIndex = 1 - currentIndex;
        }

        public void Print(int x, int y, string text)
        {
            COORD pos = new COORD((short)x, (short)y);
            SetConsoleCursorPosition(buffers[currentIndex], pos);
            WriteConsoleOutputCharacter(buffers[currentIndex], text, (uint)text.Length, pos, out _);
        }

        public void Release()
        {
            CloseHandle(buffers[0]);
            CloseHandle(buffers[1]);
        }
    }
}
