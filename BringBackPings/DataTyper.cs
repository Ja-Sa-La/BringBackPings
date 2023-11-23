using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using static BringBackPings.DataTyper.Keyboard;


namespace BringBackPings;

internal class DataTyper
{
    ASCIIEncoding ascii = new ASCIIEncoding();
    internal void SendEnter(WindowFinder windowFinder)
    {
        windowFinder.FocusWindow("League of Legends");
        Thread.Sleep(25);
        SendKey(DirectXKeyStrokes.DIK_RETURN, false, InputType.Keyboard);
        SendKey(DirectXKeyStrokes.DIK_RETURN, true, InputType.Keyboard);
        Thread.Sleep(50);
    }

    public void MessageSender(string text, WindowFinder windowFinder, bool chatmode = false)
    {
        string sendline;
        text = text.ReplaceLineEndings();
        text = Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8, Encoding.ASCII, Encoding.UTF8.GetBytes(text)));
        var enters = text.Split(Environment.NewLine);
        foreach (var enter in enters)
        {
            var lines = GetNextChars(enter, 120);

            foreach (string line in lines)
            {
                if (chatmode)
                    sendline = "/all " + line;
                else
                    sendline = line;
                SendEnter(windowFinder);
                WriteText(sendline, windowFinder);
                SendEnter(windowFinder);
            }
        }
    }

    private List<string> GetNextChars(string str, int maxlenght)
    {
        var words = new List<string>();

        for (var i = 0; i < str.Length; i += maxlenght)
        {
            if (str.Length - i >= maxlenght)
            {
                // Find the nearest space before maxlenght
                int endIndex = i + maxlenght - 1;
                while (endIndex > i && str[endIndex] != ' ')
                {
                    endIndex--;
                }

                // If no space is found, cut at maxlenght
                if (endIndex == i)
                {
                    words.Add(str.Substring(i, maxlenght));
                    continue;
                }

                words.Add(str.Substring(i, endIndex - i + 1));
            }
            else
            {
                words.Add(str.Substring(i, str.Length - i));
            }
        }

        return words;
    }

    internal void WriteText(string text, WindowFinder windowFinder)
    {
        foreach (var c in text)
        {
            var scanCode = GetScanCode(c);
            if (!scanCode.shift)
            {
                windowFinder.FocusWindow("League of Legends");
                SendKey((DirectXKeyStrokes)scanCode.Scancode, false, InputType.Keyboard);
                SendKey((DirectXKeyStrokes)scanCode.Scancode, true, InputType.Keyboard);
            }
            else
            {
                SendKey(DirectXKeyStrokes.DIK_LSHIFT, false, InputType.Keyboard);
                windowFinder.FocusWindow("League of Legends");
                SendKey((DirectXKeyStrokes)scanCode.Scancode, false, InputType.Keyboard);
                SendKey((DirectXKeyStrokes)scanCode.Scancode, true, InputType.Keyboard);
                SendKey(DirectXKeyStrokes.DIK_LSHIFT, true, InputType.Keyboard);
            }
        }
    }

    public class Keyboard
    {
        public enum DirectXKeyStrokes
        {
            DIK_ESCAPE = 0x01,
            DIK_1 = 0x02,
            DIK_2 = 0x03,
            DIK_3 = 0x04,
            DIK_4 = 0x05,
            DIK_5 = 0x06,
            DIK_6 = 0x07,
            DIK_7 = 0x08,
            DIK_8 = 0x09,
            DIK_9 = 0x0A,
            DIK_0 = 0x0B,
            DIK_MINUS = 0x0C,
            DIK_EQUALS = 0x0D,
            DIK_BACK = 0x0E,
            DIK_TAB = 0x0F,
            DIK_Q = 0x10,
            DIK_W = 0x11,
            DIK_E = 0x12,
            DIK_R = 0x13,
            DIK_T = 0x14,
            DIK_Y = 0x15,
            DIK_U = 0x16,
            DIK_I = 0x17,
            DIK_O = 0x18,
            DIK_P = 0x19,
            DIK_LBRACKET = 0x1A,
            DIK_RBRACKET = 0x1B,
            DIK_RETURN = 0x1C,
            DIK_LCONTROL = 0x1D,
            DIK_A = 0x1E,
            DIK_S = 0x1F,
            DIK_D = 0x20,
            DIK_F = 0x21,
            DIK_G = 0x22,
            DIK_H = 0x23,
            DIK_J = 0x24,
            DIK_K = 0x25,
            DIK_L = 0x26,
            DIK_SEMICOLON = 0x27,
            DIK_APOSTROPHE = 0x28,
            DIK_GRAVE = 0x29,
            DIK_LSHIFT = 0x2A,
            DIK_BACKSLASH = 0x2B,
            DIK_Z = 0x2C,
            DIK_X = 0x2D,
            DIK_C = 0x2E,
            DIK_V = 0x2F,
            DIK_B = 0x30,
            DIK_N = 0x31,
            DIK_M = 0x32,
            DIK_COMMA = 0x33,
            DIK_PERIOD = 0x34,
            DIK_SLASH = 0x35,
            DIK_RSHIFT = 0x36,
            DIK_MULTIPLY = 0x37,
            DIK_LMENU = 0x38,
            DIK_SPACE = 0x39,
            DIK_CAPITAL = 0x3A,
            DIK_F1 = 0x3B,
            DIK_F2 = 0x3C,
            DIK_F3 = 0x3D,
            DIK_F4 = 0x3E,
            DIK_F5 = 0x3F,
            DIK_F6 = 0x40,
            DIK_F7 = 0x41,
            DIK_F8 = 0x42,
            DIK_F9 = 0x43,
            DIK_F10 = 0x44,
            DIK_NUMLOCK = 0x45,
            DIK_SCROLL = 0x46,
            DIK_NUMPAD7 = 0x47,
            DIK_NUMPAD8 = 0x48,
            DIK_NUMPAD9 = 0x49,
            DIK_SUBTRACT = 0x4A,
            DIK_NUMPAD4 = 0x4B,
            DIK_NUMPAD5 = 0x4C,
            DIK_NUMPAD6 = 0x4D,
            DIK_ADD = 0x4E,
            DIK_NUMPAD1 = 0x4F,
            DIK_NUMPAD2 = 0x50,
            DIK_NUMPAD3 = 0x51,
            DIK_NUMPAD0 = 0x52,
            DIK_DECIMAL = 0x53,
            DIK_F11 = 0x57,
            DIK_F12 = 0x58,
            DIK_F13 = 0x64,
            DIK_F14 = 0x65,
            DIK_F15 = 0x66,
            DIK_KANA = 0x70,
            DIK_CONVERT = 0x79,
            DIK_NOCONVERT = 0x7B,
            DIK_YEN = 0x7D,
            DIK_NUMPADEQUALS = 0x8D,
            DIK_CIRCUMFLEX = 0x90,
            DIK_AT = 0x91,
            DIK_COLON = 0x92,
            DIK_UNDERLINE = 0x93,
            DIK_KANJI = 0x94,
            DIK_STOP = 0x95,
            DIK_AX = 0x96,
            DIK_UNLABELED = 0x97,
            DIK_NUMPADENTER = 0x9C,
            DIK_RCONTROL = 0x9D,
            DIK_NUMPADCOMMA = 0xB3,
            DIK_DIVIDE = 0xB5,
            DIK_SYSRQ = 0xB7,
            DIK_RMENU = 0xB8,
            DIK_HOME = 0xC7,
            DIK_UP = 0xC8,
            DIK_PRIOR = 0xC9,
            DIK_LEFT = 0xCB,
            DIK_RIGHT = 0xCD,
            DIK_END = 0xCF,
            DIK_DOWN = 0xD0,
            DIK_NEXT = 0xD1,
            DIK_INSERT = 0xD2,
            DIK_DELETE = 0xD3,
            DIK_LWIN = 0xDB,
            DIK_RWIN = 0xDC,
            DIK_APPS = 0xDD,
            DIK_BACKSPACE = DIK_BACK,
            DIK_NUMPADSTAR = DIK_MULTIPLY,
            DIK_LALT = DIK_LMENU,
            DIK_CAPSLOCK = DIK_CAPITAL,
            DIK_NUMPADMINUS = DIK_SUBTRACT,
            DIK_NUMPADPLUS = DIK_ADD,
            DIK_NUMPADPERIOD = DIK_DECIMAL,
            DIK_NUMPADSLASH = DIK_DIVIDE,
            DIK_RALT = DIK_RMENU,
            DIK_UPARROW = DIK_UP,
            DIK_PGUP = DIK_PRIOR,
            DIK_LEFTARROW = DIK_LEFT,
            DIK_RIGHTARROW = DIK_RIGHT,
            DIK_DOWNARROW = DIK_DOWN,
            DIK_PGDN = DIK_NEXT
        }

        [Flags]
        public enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        [Flags]
        public enum KeyEventF
        {
            KeyDown = 0x0000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            Scancode = 0x0008
        }


        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        public const int MOUSEEVENTF_MOVE = 0x0001;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;

        /// <summary>
        ///     Sends a directx key.
        ///     http://www.gamespp.com/directx/directInputKeyboardScanCodes.html
        /// </summary>
        /// <param name="key"></param>
        /// <param name="KeyUp"></param>
        /// <param name="inputType"></param>
        private Input INPUT;


        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern long mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        public static void leftpress()
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void rightpress()
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public static void SendKey(DirectXKeyStrokes key, bool KeyUp, InputType inputType)
        {
            uint flagtosend;
            if (KeyUp)
                flagtosend = (uint)(KeyEventF.KeyUp | KeyEventF.Scancode);
            else
                flagtosend = (uint)(KeyEventF.KeyDown | KeyEventF.Scancode);

            Input[] inputs =
            {
                new()
                {
                    type = (int)inputType,
                    u = new InputUnion
                    {
                        ki = new KeyboardInput
                        {
                            wVk = 0,
                            wScan = (ushort)key,
                            dwFlags = flagtosend,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
        }

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        public static extern short VkKeyScan(char ch);

        public static Chardataclass GetScanCode(char character)
        {
            var virtualKeyCode = VkKeyScan(character);
            var virtualKeyCodeLow = (byte)(virtualKeyCode & 0xFF); // Extract low-order byte
            var shiftState = (byte)((virtualKeyCode >> 8) & 0xFF);
            var scanCode = MapVirtualKey(virtualKeyCodeLow, 0);

            return new Chardataclass { Scancode = scanCode, shift = (virtualKeyCode & 0x0100) != 0 };
        }

        public class Chardataclass
        {
            internal uint Scancode { get; set; }
            internal bool shift { get; set; }
        }

        public struct Input
        {
            public int type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)] public readonly MouseInput mi;
            [FieldOffset(0)] public KeyboardInput ki;
            [FieldOffset(0)] public readonly HardwareInput hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public readonly int dx;
            public readonly int dy;
            public readonly uint mouseData;
            public readonly uint dwFlags;
            public readonly uint time;
            public readonly IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardInput
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public readonly uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HardwareInput
        {
            public readonly uint uMsg;
            public readonly ushort wParamL;
            public readonly ushort wParamH;
        }
    }
}