namespace UIFlow.Runtime.Layouts
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class LayoutRelationshipTable
    {
        private readonly UILayoutMask[] m_Matrix;

        public LayoutRelationshipTable()
        {
            m_Matrix = new UILayoutMask[LayoutTable.MaxLayoutCount];
        }

        public void SetMask(in UILayoutId layoutId, in UILayoutMask layoutMask)
        {
            m_Matrix[layoutId] = layoutMask;
        }

        public void ClearMask(in UILayoutId layoutId)
        {
            m_Matrix[layoutId] = UILayoutMask.Zero;
        }

        public UILayoutMask GetMask(in UILayoutId layoutId)
        {
            return m_Matrix[layoutId];
        }

        public UILayoutMask GetInversedMask(in UILayoutId layoutId)
        {
            return ~GetMask(layoutId);
        }
    }
}