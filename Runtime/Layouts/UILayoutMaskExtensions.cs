namespace UIFlow.Runtime.Layouts
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public static class UILayoutMaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool[] ToArray(this UILayoutMask layoutMask)
        {
            var array = new bool[LayoutTable.MaxLayoutCount];
            ToArray(layoutMask, array);
            return array;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToArray(this UILayoutMask layoutMask, [NotNull]bool[] array)
        {
#if DEBUG
            if(array.Length != LayoutTable.MaxLayoutCount)
                throw new InvalidOperationException($"Array must be equal to {LayoutTable.MaxLayoutCount}.");
#endif
            
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = (layoutMask & (1 << i)) != 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetValue(this UILayoutMask layoutMask, UILayoutId layoutId)
        {
            return (layoutMask & (1 << layoutId)) != 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetValue(this UILayoutMask layoutMask, int index)
        {
            return (layoutMask & (1 << index)) != 0;
        }
    }
}