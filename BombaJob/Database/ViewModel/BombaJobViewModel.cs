using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BombaJob.Database;
using BombaJob.Database.Tables;

namespace BombaJob.Database.ViewModel
{
    public class BombaJobViewModel : INotifyPropertyChanged
    {
        private BombaJobDataContext bjDB;

        public BombaJobViewModel(string bjDBConnectionString)
        {
            bjDB = new BombaJobDataContext(bjDBConnectionString);
        }

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

        public void SaveChangesToDB()
        {
            bjDB.SubmitChanges();
        }

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
