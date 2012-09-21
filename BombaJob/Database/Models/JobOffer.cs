using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace BombaJob.Database.Models
{
    public class JobOffer
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _offerId;
        public int OfferId
        {
            get { return _offerId; }
            set { _offerId = value; }
        }

        private int _refCategoryId;
        public int RefCategoryId
        {
            get { return _refCategoryId; }
            set { _refCategoryId = value; }
        }

        private int _categoryId;
        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        private string _categoryTitle;
        public string CategoryTitle
        {
            get { return _categoryTitle; }
            set { _categoryTitle = value; }
        }

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _negativism;
        public string Negativism
        {
            get { return _negativism; }
            set { _negativism = value; }
        }

        private string _positivism;
        public string Positivism
        {
            get { return _positivism; }
            set { _positivism = value; }
        }

        private bool _freelanceYn;
        public bool FreelanceYn
        {
            get { return _freelanceYn; }
            set { _freelanceYn = value; }
        }

        private bool _humanYn;
        public bool HumanYn
        {
            get { return _humanYn; }
            set { _humanYn = value; }
        }

        private bool _readYn;
        public bool ReadYn
        {
            get { return _readYn; }
            set { _readYn = value; }
        }

        private bool _sentMessageYn;
        public bool SentMessageYn
        {
            get { return _sentMessageYn; }
            set { _sentMessageYn = value; }
        }

        private DateTime _publishDate;
        public DateTime PublishDate
        {
            get { return _publishDate; }
            set { _publishDate = value; }
        }
    }
}
