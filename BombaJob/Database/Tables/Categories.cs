using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace BombaJob.Database.Tables
{
    [Table]
    public class Categories : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _id;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    NotifyPropertyChanging("Id");
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        private int _categoryId;
        [Column]
        public int CategoryId
        {
            get { return _categoryId; }
            set
            {
                if (_categoryId != value)
                {
                    NotifyPropertyChanging("CategoryId");
                    _categoryId = value;
                    NotifyPropertyChanged("CategoryId");
                }
            }
        }

        private string _title;
        [Column]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    NotifyPropertyChanging("Title");
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private int _offersCount;
        [Column]
        public int OffersCount
        {
            get { return _offersCount; }
            set
            {
                if (_offersCount != value)
                {
                    NotifyPropertyChanging("OffersCount");
                    _offersCount = value;
                    NotifyPropertyChanged("OffersCount");
                }
            }
        }

        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }
        #endregion
    }
}
