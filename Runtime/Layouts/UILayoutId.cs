namespace UIFlow.Runtime.Layouts
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    [DebuggerDisplay("Id = {m_Id}")]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct UILayoutId : IEquatable<UILayoutId>
    {
        public static readonly UILayoutId Invalid = new UILayoutId(-1);
        
        internal readonly int m_Id;

        public readonly bool IsValid;

        public UILayoutId(int id)
        {
            m_Id = id;
            IsValid = id >= 0;
        }

        public static implicit operator int(UILayoutId layoutId)
        {
            return layoutId.m_Id;
        }

        public static explicit operator UILayoutId(int id)
        {
            return new UILayoutId(id);
        }

        public static implicit operator UILayoutId(UILayout layout)
        {
            return new UILayoutId((int)layout);
        }
        
        public static string LayoutToName(in UILayoutId layoutId)
        {
            return layoutId.LayoutToName();
        }

        public bool Equals(UILayoutId other)
        {
            return m_Id == other.m_Id;
        }

        public override bool Equals(object obj)
        {
            return obj is UILayoutId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return m_Id;
        }

        public override string ToString()
        {
            return m_Id.ToString();
        }
    }
}