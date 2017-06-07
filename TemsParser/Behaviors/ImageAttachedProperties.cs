using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace TemsParser.Behaviors
{
    public static class ImageAttachedProperties
    {
        public static readonly DependencyProperty MessageTypeProperty =
            DependencyProperty.RegisterAttached("MessageType",
                                                typeof(MessageTypes),
                                                typeof(ImageAttachedProperties));

        public static MessageTypes GetMessageType(DependencyObject obj)
        {
            return (MessageTypes)obj.GetValue(MessageTypeProperty);
        }

        public static void SetMessageType(DependencyObject obj, MessageTypes value)
        {
            obj.SetValue(MessageTypeProperty, value);
        }
    }
}
