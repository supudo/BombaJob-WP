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
using BombaJob.Sync;
using BombaJob.Utilities;

namespace BombaJob.Utilities.Views
{
    public partial class Post : BombaJobBasePage
    {
        private Synchronization syncManager;

        public Post()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.menu_Post;
            this.Loaded += new RoutedEventHandler(Post_Loaded);
        }

        void Post_Loaded(object sender, RoutedEventArgs e)
        {
            List<BombaJobListPickerItem> fl = new List<BombaJobListPickerItem>();
            fl.Add(new BombaJobListPickerItem(0, AppResources.offer_iam_human));
            fl.Add(new BombaJobListPickerItem(1, AppResources.offer_iam_company));
            this.ddHuman.ItemsSource = fl;
            this.ddHuman.FullModeHeader = AppResources.offer_iam_title;

            fl = new List<BombaJobListPickerItem>();
            fl.Add(new BombaJobListPickerItem(0, AppResources.freelance_only));
            fl.Add(new BombaJobListPickerItem(0, AppResources.freelance_no));
            this.ddFreelance.ItemsSource = fl;
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

        private void ddHuman_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetLabels(this.ddHuman.SelectedIndex == 0);
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }

        private void post_Click(object sender, EventArgs e)
        {
            string title = this.txtTitle.Text;
            string email = this.txtEmail.Text;
            string positivism = this.txtPositiv.Text;
            string negativism = this.txtNegativ.Text;

            bool humanYn = this.ddHuman.SelectedIndex == 0;
            bool freelanceYn = this.ddFreelance.SelectedIndex == 0;

            int categoryID = ((BombaJob.Database.Models.Category)this.ddCategory.SelectedItem).CategoryID;

            bool hasError = true;
            if (title.Trim().Equals(""))
                this.txtTitle.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            else if (email.Trim().Equals(""))
                this.txtEmail.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            else if (positivism.Trim().Equals(""))
                this.txtPositiv.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            else if (negativism.Trim().Equals(""))
                this.txtNegativ.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            else if (categoryID < 0)
                this.ddCategory.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            else
                hasError = false;

            if (hasError)
                MessageBox.Show(AppResources.offer_MissingReqFields);
            else
            {
                if (this.syncManager == null)
                    this.syncManager = new Synchronization();
                this.syncManager.SyncError += new Synchronization.EventHandler(syncManager_SyncError);
                this.syncManager.SyncComplete += new Synchronization.EventHandler(syncManager_SyncComplete);

                Dictionary<string, string> postParams = new Dictionary<string, string>();
                postParams.Add("oid", "0");
                postParams.Add("cid", "" + categoryID);
                postParams.Add("h", (humanYn ? "1" : "0"));
                postParams.Add("fr", (freelanceYn ? "1" : "0"));
                postParams.Add("tt", title);
                postParams.Add("em", email);
                postParams.Add("pos", positivism);
                postParams.Add("neg", negativism);
                postParams.Add("mob_app", "wp");
                this.syncManager.DoPostOffer(postParams);
            }
        }

        void syncManager_SyncComplete(object sender, BombaJobEventArgs e)
        {
            MessageBox.Show(AppResources.offer_ThankYou);
            this.ddHuman.SelectedIndex = 0;
            this.ddFreelance.SelectedIndex = 0;
            this.ddCategory.SelectedIndex = 0;
            this.txtTitle.Text = "";
            this.txtEmail.Text = "";
            this.txtPositiv.Text = "";
            this.txtNegativ.Text = "";
        }

        void syncManager_SyncError(object sender, BombaJobEventArgs e)
        {
            if (e.IsError)
                MessageBox.Show(e.ErrorMessage);
        }
    }
}