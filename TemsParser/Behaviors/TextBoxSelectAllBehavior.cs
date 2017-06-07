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
    public class TextBoxSelectAllBehavior : Behavior<TextBox>
    {
        private readonly MouseBinding MouseBindingTextBox;


        public TextBoxSelectAllBehavior()
        {
            var mg = new MouseGesture()
            {
                MouseAction = MouseAction.LeftDoubleClick
            };

            MouseBindingTextBox = new MouseBinding()
            {
                MouseAction = MouseAction.LeftDoubleClick,
                Command = new Command(ex =>
                    {
                        AssociatedObject.SelectAll();
                    }),
                Gesture = mg
            };
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.InputBindings.Add(MouseBindingTextBox);
            AssociatedObject.LostFocus += AssociatedObject_LostFocus;
        }

        private void AssociatedObject_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            AssociatedObject.Select(0, 0);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.InputBindings.Remove(MouseBindingTextBox);
            AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
        }
    }
}
