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
using Microsoft.Phone.Controls;
using BombaJob.Utilities;
using BombaJob.Utilities.Extensions;

namespace BombaJob.Views
{
    public partial class About : BombaJobBasePage
    {
        public About()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.menu_About;
            this.Loaded += new RoutedEventHandler(About_Loaded);
        }

        void About_Loaded(object sender, RoutedEventArgs e)
        {
            base.BuildApplicationBar();
            string content = AppSettings.Hyperlinkify(App.DbViewModel.GetTextContent(35));
            content = content.Replace("<br />", "");
            RichTextBoxExtensions.SetLinkedText(this.rtbAbout, content);
        }
    }
}