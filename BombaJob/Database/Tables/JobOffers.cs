using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace BombaJob.Database.Tables
{
    [Table]
    public class JobOffers : INotifyPropertyChanged, INotifyPropertyChanging
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

        private int _offerId;
        [Column]
        public int OfferId
        {
            get { return _offerId; }
            set
            {
                if (_offerId != value)
                {
                    NotifyPropertyChanging("OfferId");
                    _offerId = value;
                    NotifyPropertyChanged("OfferId");
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

        private string _category;
        [Column]
        public string Category
        {
            get { return _category; }
            set
            {
                if (_category != value)
                {
                    NotifyPropertyChanging("Category");
                    _category = value;
                    NotifyPropertyChanged("Category");
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

        private string _email;
        [Column]
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    NotifyPropertyChanging("Email");
                    _email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }

        private string _negativism;
        [Column]
        public string Negativism
        {
            get { return _negativism; }
            set
            {
                if (_negativism != value)
                {
                    NotifyPropertyChanging("Negativism");
                    _negativism = value;
                    NotifyPropertyChanged("Negativism");
                }
            }
        }

        private string _positivism;
        [Column]
        public string Positivism
        {
            get { return _positivism; }
            set
            {
                if (_positivism != value)
                {
                    NotifyPropertyChanging("Positivism");
                    _positivism = value;
                    NotifyPropertyChanged("Positivism");
                }
            }
        }

        private bool _freelanceYn;
        [Column]
        public bool FreelanceYn
        {
            get { return _freelanceYn; }
            set
            {
                if (_freelanceYn != value)
                {
                    NotifyPropertyChanging("FreelanceYn");
                    _freelanceYn = value;
                    NotifyPropertyChanged("FreelanceYn");
                }
            }
        }

        private bool _humanYn;
        [Column]
        public bool HumanYn
        {
            get { return _humanYn; }
            set
            {
                if (_humanYn != value)
                {
                    NotifyPropertyChanging("HumanYn");
                    _humanYn = value;
                    NotifyPropertyChanged("HumanYn");
                }
            }
        }

        private bool _readYn;
        [Column]
        public bool ReadYn
        {
            get { return _readYn; }
            set
            {
                if (_readYn != value)
                {
                    NotifyPropertyChanging("ReadYn");
                    _readYn = value;
                    NotifyPropertyChanged("ReadYn");
                }
            }
        }

        private bool _sentMessageYn;
        [Column]
        public bool SentMessageYn
        {
            get { return _sentMessageYn; }
            set
            {
                if (_sentMessageYn != value)
                {
                    NotifyPropertyChanging("SentMessageYn");
                    _sentMessageYn = value;
                    NotifyPropertyChanged("SentMessageYn");
                }
            }
        }

        private DateTime _publishDate;
        [Column]
        public DateTime PublishDate
        {
            get { return _publishDate; }
            set
            {
                if (_publishDate != value)
                {
                    NotifyPropertyChanging("PublishDate");
                    _publishDate = value;
                    NotifyPropertyChanged("PublishDate");
                }
            }
        }

        private long _publishDateStamp;
        [Column]
        public long PublishDateStamp
        {
            get { return _publishDateStamp; }
            set
            {
                if (_publishDateStamp != value)
                {
                    NotifyPropertyChanging("PublishDateStamp");
                    _publishDateStamp = value;
                    NotifyPropertyChanged("PublishDateStamp");
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
