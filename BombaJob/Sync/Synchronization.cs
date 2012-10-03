using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using BombaJob.Database.Context;
using BombaJob.Database.Tables;
using BombaJob.Utilities.Network;
using BombaJob.Utilities;

namespace BombaJob.Sync
{
    public class Synchronization
    {
        public delegate void EventHandler(Object sender, BombaJobEventArgs e);
        public event EventHandler SyncComplete;
        public event EventHandler SyncError;

        NetworkHelper _networkHelper;

        private AppSettings.ServiceOp currentOp;

        BackgroundWorker bgWorker;

        #region Constructor
        public Synchronization()
        {
            this._networkHelper = new NetworkHelper();
            this._networkHelper.DownloadComplete += new NetworkHelper.EventHandler(_networkHelper_DownloadComplete);
            this._networkHelper.DownloadInBackgroundComplete += new NetworkHelper.EventHandler(_networkHelper_DownloadInBackgroundComplete);
            this._networkHelper.DownloadError += new NetworkHelper.EventHandler(_networkHelper_DownloadError);
        }
        #endregion

        #region Dispatcher
        private void dispatchDownload(string xmlContent)
        {
            switch (this.currentOp)
            {
                case AppSettings.ServiceOp.ServiceOpTexts:
                    this.doTexts(xmlContent);
                    break;
                case AppSettings.ServiceOp.ServiceOpCategories:
                    this.doCategories(xmlContent);
                    break;
                case AppSettings.ServiceOp.ServiceOpNewestOffers:
                    this.doNewestOffers(xmlContent);
                    break;
                case AppSettings.ServiceOp.ServiceOpSearch:
                    this.doJobOffers(xmlContent);
                    break;
                case AppSettings.ServiceOp.ServiceOpJobs:
                    this.doJobOffers(xmlContent);
                    break;
                default:
                    SyncComplete(this, new BombaJobEventArgs(false, "", ""));
                    break;
            }
        }
        #endregion

        #region Public
        public void DoPostOffer(Dictionary<string, string> postParams)
        {
            this.currentOp = AppSettings.ServiceOp.ServiceOpPost;
            this._networkHelper.uploadURL(AppSettings.ServicesURL + "?action=postNewJob", postParams);
        }

        public void DoSearch(string keyword, int freelance)
        {
            this.currentOp = AppSettings.ServiceOp.ServiceOpSearch;
            Dictionary<string, string> postArray = new Dictionary<string, string>();
            postArray.Add("keyword", keyword);
            postArray.Add("freelance", "" + freelance);
            this._networkHelper.uploadURL(AppSettings.ServicesURL + "?action=searchOffers", postArray);
        }

        public void DoSendEmail(Dictionary<string, string> postParams)
        {
            this.currentOp = AppSettings.ServiceOp.ServiceOpSendEmail;
            this._networkHelper.uploadURL(AppSettings.ServicesURL + "?action=sendEmailMessage", postParams);
        }

        public void DoSendPM(Dictionary<string, string> postParams)
        {
            this.currentOp = AppSettings.ServiceOp.ServiceOpSendEmail;
            this._networkHelper.uploadURL(AppSettings.ServicesURL + "?action=postMessage", postParams);
        }

        public void LoadOffersInBackground()
        {
            this.currentOp = AppSettings.ServiceOp.ServiceOpJobs;
            this.bgWorker = new BackgroundWorker();
            RunProcess();
        }

        private void RunProcess()
        {
            this.bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            this.bgWorker.RunWorkerAsync();
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (this.currentOp)
            {
                case AppSettings.ServiceOp.ServiceOpTexts:
                    this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=getTextContent", true, this.currentOp);
                    break;
                case AppSettings.ServiceOp.ServiceOpCategories:
                    this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=getCategories", true, this.currentOp);
                    break;
                case AppSettings.ServiceOp.ServiceOpNewestOffers:
                    this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=getNewJobs", true, this.currentOp);
                    break;
                case AppSettings.ServiceOp.ServiceOpJobs:
                    this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=getJobs", true, this.currentOp);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Handle URLs
        private void syncTexts()
        {
            this.currentOp = AppSettings.ServiceOp.ServiceOpTexts;
            this.bgWorker = new BackgroundWorker();
            RunProcess();
        }

        private void syncCategories()
        {
            this.currentOp = AppSettings.ServiceOp.ServiceOpCategories;
            this.bgWorker = new BackgroundWorker();
            RunProcess();
        }

        private void syncNewestOffers()
        {
            this.currentOp = AppSettings.ServiceOp.ServiceOpNewestOffers;
            this.bgWorker = new BackgroundWorker();
            RunProcess();
        }
        #endregion

        #region Events
        private void SynchronizationComplete()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SyncComplete(this, new BombaJobEventArgs(false, "", ""));
            });
        }

        void _networkHelper_DownloadComplete(object sender, BombaJobEventArgs e)
        {
            if (!e.IsError)
                this.dispatchDownload(e.XmlContent);
        }

        void _networkHelper_DownloadInBackgroundComplete(object sender, BombaJobEventArgs e)
        {
            if (!e.IsError)
            {
                if (e.ServiceOp > 0)
                    this.currentOp = e.ServiceOp;
                this.dispatchDownload(e.XmlContent);
            }
        }

        void _networkHelper_DownloadError(object sender, BombaJobEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SyncError(this, new BombaJobEventArgs(true, e.ErrorMessage, ""));
            });
        }
        #endregion

        #region Initial Synchronization
        public void StartSync()
        {
            this.syncTexts();
        }

        private void doTexts(string xmlContent)
        {
            XDocument doc = XDocument.Parse(xmlContent);
            var texts = from txt in doc.Descendants("tctxt")
                        select new Texts
                        {
                            TextId = int.Parse(txt.Attribute("id").Value),
                            Title = txt.Element("tctitle").Value,
                            Content = txt.Element("tccontent").Value,
                        };
            using (BombaJobDataContext db = new BombaJobDataContext(AppSettings.DBConnectionString))
            {
                foreach (Texts t in texts)
                    App.DbViewModel.AddText(t);
            }
            this.syncCategories();
        }

        private void doCategories(string xmlContent)
        {
            XDocument doc = XDocument.Parse(xmlContent);
            var cats = from cat in doc.Descendants("cat")
                        select new Categories
                        {
                            CategoryId = int.Parse(cat.Attribute("id").Value),
                            OffersCount = int.Parse(cat.Attribute("cnt").Value),
                            Title = cat.Element("cttl").Value,
                        };
            using (BombaJobDataContext db = new BombaJobDataContext(AppSettings.DBConnectionString))
            {
                foreach (Categories t in cats)
                    App.DbViewModel.AddCategory(t);
            }
            this.syncNewestOffers();
        }

        private void doNewestOffers(string xmlContent)
        {
            XDocument doc = XDocument.Parse(xmlContent);
            var jobs = from job in doc.Descendants("job")
                       select new JobOffers
                       {
                           OfferId = int.Parse(job.Attribute("id").Value),
                           CategoryId = int.Parse(job.Attribute("cid").Value),
                           HumanYn = int.Parse(job.Attribute("hm").Value) == 1,
                           FreelanceYn = int.Parse(job.Attribute("fyn").Value) == 1,
                           Title = job.Element("jottl").Value,
                           Email = job.Element("joem").Value,
                           CategoryTitle = job.Element("jocat").Value,
                           Positivism = job.Element("jopos").Value,
                           Negativism = job.Element("joneg").Value,
                           PublishDate = DateTime.ParseExact(job.Element("jodt").Value, AppSettings.DateTimeFormat, null),
                           Icon = ((int.Parse(job.Attribute("hm").Value) == 1) ? "iconperson" : "iconcompany"),
                       };
            using (BombaJobDataContext db = new BombaJobDataContext(AppSettings.DBConnectionString))
            {
                foreach (JobOffers t in jobs)
                    App.DbViewModel.AddJobOffer(t);
            }
            this.SynchronizationComplete();
        }
        #endregion

        #region Parsers
        private void doJobOffers(string xmlContent)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlContent);
                var jobs = from job in doc.Descendants("job")
                           select new JobOffers
                           {
                               OfferId = int.Parse(job.Attribute("id").Value),
                               CategoryId = int.Parse(job.Attribute("cid").Value),
                               HumanYn = int.Parse(job.Attribute("hm").Value) == 1,
                               FreelanceYn = int.Parse(job.Attribute("fyn").Value) == 1,
                               Title = job.Element("jottl").Value,
                               Email = job.Element("joem").Value,
                               CategoryTitle = job.Element("jocat").Value,
                               Positivism = job.Element("jopos").Value,
                               Negativism = job.Element("joneg").Value,
                               PublishDate = DateTime.ParseExact(job.Element("jodt").Value, AppSettings.DateTimeFormat, null),
                               Icon = ((int.Parse(job.Attribute("hm").Value) == 1) ? "iconperson" : "iconcompany"),
                           };
                using (BombaJobDataContext db = new BombaJobDataContext(AppSettings.DBConnectionString))
                {
                    foreach (JobOffers t in jobs)
                        App.DbViewModel.AddJobOffer(t);
                }
            }
            catch (Exception e)
            {
                AppSettings.LogThis("Synchronization - doJobOffers - " + e.ToString());
            }
            if (this.currentOp != AppSettings.ServiceOp.ServiceOpJobs)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    SyncComplete(this, new BombaJobEventArgs(false, "", ""));
                });
            }
        }
        #endregion
    }
}
