using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Interactivity;

using TemsParser.Common;


namespace TemsParser.Behaviors
{
    public class ButtonGetFocusBehavior : Behavior<Button>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_GetFocus;
        }

        private void AssociatedObject_GetFocus(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Focus();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObject_GetFocus;
        }
    }
}
