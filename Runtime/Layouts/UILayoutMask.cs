namespace UIFlow.Runtime.Layouts
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    [DebuggerDisplay("Value = {m_Value}")]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct UILayoutMask : IEquatable<UILayoutMask>
    {
        public static readonly UILayoutMask Zero = new UILayoutMask(0);
        public static readonly UILayoutMask One = new UILayoutMask(int.MaxValue);
        
        internal readonly int m_Value;

        public UILayoutMask(int value)
        {
            m_Value = value;
        }

        public static implicit operator int(UILayoutMask layoutMask)
        {
            return layoutMask.m_Value;
        }

        public static explicit operator UILayoutMask(int mask)
        {
            return new UILayoutMask(mask);
        }

        public static UILayoutMask operator ~(UILayoutMask layoutMask)
        {
            return new UILayoutMask(~layoutMask.m_Value);
        }

        public static UILayoutMask operator |(UILayoutMask a, UILayoutMask b)
        {
            return new UILayoutMask(a.m_Value | b.m_Value);
        }
        
        public static UILayoutMask operator |(UILayoutMask a, int b)
        {
            return new UILayoutMask(a.m_Value | b);
        }
        
        public static UILayoutMask operator &(UILayoutMask a, UILayoutMask b)
        {
            return new UILayoutMask(a.m_Value & b.m_Value);
        }
        
        public static UILayoutMask operator &(UILayoutMask a, int b)
        {
            return new UILayoutMask(a.m_Value & b);
        }

        public static UILayoutMask operator <<(UILayoutMask a, int b)
        {
            return new UILayoutMask(a.m_Value << b);
        }

        public static UILayoutMask operator >>(UILayoutMask a, int b)
        {
            return new UILayoutMask(a.m_Value >> b);
        }
        
        public static UILayoutMask operator ^(UILayoutMask a, UILayoutMask b)
        {
            return new UILayoutMask(a.m_Value ^ b.m_Value);
        }
        
        public static UILayoutMask operator ^(UILayoutMask a, int b)
        {
            return new UILayoutMask(a.m_Value ^ b);
        }

        public bool Equals(UILayoutMask other)
        {
            return m_Value == other.m_Value;
        }

        public override bool Equals(object obj)
        {
            return obj is UILayoutMask other && Equals(other);
        }

        public override int GetHashCode()
        {
            return m_Value;
        }

        public override string ToString()
        {
            return m_Value.ToString();
        }
    }
}