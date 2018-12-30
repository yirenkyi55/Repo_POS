using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tulpep.NotificationWindow;

namespace POSUI
{
    public class DialogControl
    {
        public static void ShowNotification(string titleText, string messageText)
        {
            PopupNotifier notifier = new PopupNotifier();
            notifier.TitleColor = Color.White;
            notifier.HeaderColor = Color.FromArgb(0, 174, 219);
            notifier.BodyColor = Color.White;
            notifier.ContentColor = Color.FromArgb(0, 174, 219);
            notifier.TitleText = titleText;         
            notifier.ContentText = messageText;
            notifier.IsRightToLeft = true;
            notifier.Image = Properties.Resources.info;
            notifier.Popup();
        }
    }
}
