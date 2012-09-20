using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BombaJob.Database.Context;
using BombaJob.Database.Tables;
using BombaJob.Database.Models;

namespace BombaJob.Database.ViewModel
{
    public class BombaJobViewModel : INotifyPropertyChanged
    {
        private BombaJobDataContext bjDB;

        public BombaJobViewModel(string bjDBConnectionString)
        {
            bjDB = new BombaJobDataContext(bjDBConnectionString);
        }

        #region Observables
        private ObservableCollection<Texts> _allTexts;
        public ObservableCollection<Texts> AllTexts
        {
            get { return _allTexts; }
            set
            {
                _allTexts = value;
                NotifyPropertyChanged("AllTexts");
            }
        }

        private ObservableCollection<Categories> _allCategories;
        public ObservableCollection<Categories> AllCategories
        {
            get { return _allCategories; }
            set
            {
                _allCategories = value;
                NotifyPropertyChanged("AllCategories");
            }
        }

        private ObservableCollection<JobOffers> _allJobOffers;
        public ObservableCollection<JobOffers> AllJobOffers
        {
            get { return _allJobOffers; }
            set
            {
                _allJobOffers = value;
                NotifyPropertyChanged("AllJobOffers");
            }
        }

        public void InitObservables()
        {
            AllTexts = new ObservableCollection<Texts>();
            AllCategories = new ObservableCollection<Categories>();
            AllJobOffers = new ObservableCollection<JobOffers>();
        }
        #endregion

        #region Texts
        public void AddText(Texts ent)
        {
            var exists = from t in bjDB.Texts
                         where t.TextId == ent.TextId
                         select t;
            if (exists.Count() == 0)
            {
                bjDB.Texts.InsertOnSubmit(ent);
                AllTexts.Add(ent);
            }
            else
            {
                Texts t = exists.FirstOrDefault();
                t.Title = ent.Title;
                t.Content = ent.Content;
            }
            bjDB.SubmitChanges();
        }

        public void DeleteText(Texts ent)
        {
            AllTexts.Remove(ent);
            bjDB.Texts.DeleteOnSubmit(ent);
            bjDB.SubmitChanges();
        }

        public string GetTextContent(int tid)
        {
            return (from t in bjDB.Texts where t.TextId == tid select new { t.Content }).FirstOrDefault().Content;
        }
        #endregion

        #region Categories
        public void AddCategory(Categories ent)
        {
            var exists = from t in bjDB.Categories
                         where t.CategoryId == ent.CategoryId
                         select t;
            if (exists.Count() == 0)
            {
                bjDB.Categories.InsertOnSubmit(ent);
                AllCategories.Add(ent);
            }
            else
            {
                Categories t = exists.FirstOrDefault();
                t.Title = ent.Title;
                t.OffersCount = ent.OffersCount;
            }
            bjDB.SubmitChanges();
        }

        public void DeleteCategory(Categories ent)
        {
            AllCategories.Remove(ent);
            bjDB.Categories.DeleteOnSubmit(ent);
            bjDB.SubmitChanges();
        }

        public void DeleteAllCategories()
        {
            AllCategories.Clear();
            var ents = from t in bjDB.Categories select t;
            bjDB.Categories.DeleteAllOnSubmit(ents);
            bjDB.SubmitChanges();
        }

        public List<Category> GetCategories(bool humanYn)
        {
            return bjDB.Categories.Where(t => t.JobOffers.Any(j => j.HumanYn == humanYn)).OrderBy(t => t.Title).Select(t => new Category
                        {
                            CategoryID = t.CategoryId,
                            Title = t.Title,
                            OffersCount = t.JobOffers.Where(j => j.HumanYn == humanYn).Count()
                        }).ToList();
        }
        #endregion

        #region Offers
        public void AddJobOffer(JobOffers ent)
        {
            var exists = from t in bjDB.JobOffers
                         where t.OfferId == ent.OfferId
                         select t;
            if (exists.Count() == 0)
            {
                ent.Category = bjDB.Categories.Where(c => c.CategoryId == ent.CategoryId).FirstOrDefault();
                bjDB.JobOffers.InsertOnSubmit(ent);
                AllJobOffers.Add(ent);
            }
            else
            {
                JobOffers t = exists.FirstOrDefault();
                t.HumanYn = ent.HumanYn;
                t.Category = ent.Category;
                t.CategoryId = ent.CategoryId;
                t.Email = ent.Email;
                t.FreelanceYn = ent.FreelanceYn;
                t.Negativism = ent.Negativism;
                t.Positivism = ent.Positivism;
                t.Title = ent.Title;
                t.PublishDate = ent.PublishDate;
                t.Icon = ent.Icon;
                t.Category = bjDB.Categories.Where(c => c.CategoryId == t.CategoryId).FirstOrDefault();
            }
            bjDB.SubmitChanges();
        }

        public List<JobOffers> GetNewestOffers()
        {
            return bjDB.JobOffers.OrderBy(t => t.ReadYn).ThenByDescending(t => t.PublishDate).Take(AppSettings.OffersPerPage).ToList();
        }

        public List<JobOffers> GetOffers(bool humanYn)
        {
            return bjDB.JobOffers.Where(t => t.HumanYn == humanYn).OrderBy(t => t.ReadYn).ThenByDescending(t => t.PublishDate).Take(AppSettings.OffersPerPage).ToList();
        }

        public List<JobOffers> GetOffers(bool humanYn, int categoryId)
        {
            return bjDB.JobOffers.Where(t => t.HumanYn == humanYn).Where(t => t.CategoryId == categoryId).OrderBy(t => t.ReadYn).ThenByDescending(t => t.PublishDate).Take(AppSettings.OffersPerPage).ToList();
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
