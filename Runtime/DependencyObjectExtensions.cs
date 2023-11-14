namespace UIFlow.Runtime
{
    using Noesis;
    
    public static class DependencyObjectExtensions
    {
        public static T FindVisualChild<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) 
                return null;

            T foundChild = null;
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                foundChild = child as T;
                if (foundChild == null)
                {
                    foundChild = FindVisualChild<T>(child);
                    if (foundChild != null) 
                        break;
                }
                else
                {
                    break;
                }
            }

            return foundChild;
        }
        
        public static T FindVisualChild<T>(this DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) 
                return null;

            T foundChild = null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var childType = child as T;
                if (childType == null)
                {
                    foundChild = FindVisualChild<T>(child, childName);
                    if (foundChild != null) 
                        break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static object GetParentDataContext(this DependencyObject target)
        {
            var parent = VisualTreeHelper.GetParent(target);
            if (parent == null || !(parent is FrameworkElement parentElement))
                return null;

            return parentElement.DataContext;
        }

        public static T GetParentDataContext<T>(this DependencyObject target) where T : class
        {
            return target.GetParentDataContext() as T;
        }
    }
}