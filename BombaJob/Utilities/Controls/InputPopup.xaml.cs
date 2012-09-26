using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BombaJob.Utilities.Controls
{
    public partial class InputPopup : UserControl
    {
        public InputPopup()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(InputPopup_Loaded);
        }

        void InputPopup_Loaded(object sender, RoutedEventArgs e)
        {
            this.lblValue.Text = AppResources.share_Email_Desc;
            this.btnOK.Content = AppResources.popup_OK;
            this.btnCancel.Content = AppResources.popup_Cancel;
        }
    }
}
