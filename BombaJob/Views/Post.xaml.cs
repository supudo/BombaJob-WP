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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using BombaJob.Utilities;

namespace BombaJob.Utilities.Views
{
    public partial class Post : BombaJobBasePage
    {
        public Post()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.menu_Post;
            this.Loaded += new RoutedEventHandler(Post_Loaded);
        }

        void Post_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> fl = new List<string>();
            this.ddHuman.ItemsSource = new List<string>() { AppResources.offer_iam_human, AppResources.offer_iam_company };
            this.ddHuman.FullModeHeader = AppResources.offer_iam_title;

            fl = new List<string>();
            this.ddFreelance.ItemsSource = new List<string>() { AppResources.freelance_only, AppResources.freelance_no };
            this.ddFreelance.FullModeHeader = AppResources.offer_FreelanceYn;

            this.ddCategory.ItemsSource = App.DbViewModel.GetAllCategories();
            this.ddCategory.FullModeHeader = AppResources.offer_Category;

            this.lblHuman.Text = AppResources.offer_iam_title;
            this.lblCategory.Text = AppResources.offer_Category;
            this.lblFreelance.Text = AppResources.offer_FreelanceYn;
            this.SetLabels(true);
        }

        private void SetLabels(bool humanYn)
        {
            if (humanYn)
            {
                this.lblTitle.Text = AppResources.offer_Human_Title;
                this.lblEmail.Text = AppResources.offer_Human_Email;
                this.lblNegativ.Text = AppResources.offer_Human_Negativ;
                this.lblPositiv.Text = AppResources.offer_Human_Positiv;
            }
            else
            {
                this.lblTitle.Text = AppResources.offer_Company_Title;
                this.lblEmail.Text = AppResources.offer_Company_Email;
                this.lblNegativ.Text = AppResources.offer_Company_Negativ;
                this.lblPositiv.Text = AppResources.offer_Company_Positiv;
            }
        }

        private void post_Click(object sender, EventArgs e)
        {
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }

        private void ddHuman_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetLabels(this.ddHuman.SelectedIndex == 0);
        }
    }
}