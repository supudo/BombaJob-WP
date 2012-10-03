using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BombaJob.Utilities
{
    public class BombaJobEventArgs : EventArgs
    {
        private AppSettings.ServiceOp serviceOp;
        private bool isError;
        private string errorMessage;
        private string xmlContent;

        public BombaJobEventArgs(bool ise, string eMsg, string xml)
        {
            this.isError = ise;
            this.errorMessage = eMsg;
            this.xmlContent = xml;
            this.serviceOp = 0;
        }

        public BombaJobEventArgs(bool ise, string eMsg, string xml, AppSettings.ServiceOp sOp)
        {
            this.isError = ise;
            this.errorMessage = eMsg;
            this.xmlContent = xml;
            this.serviceOp = sOp;
        }

        public bool IsError
        {
            get { return this.isError; }
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
        }

        public string XmlContent
        {
            get { return this.xmlContent; }
        }

        public AppSettings.ServiceOp ServiceOp
        {
            get { return this.serviceOp; }
        }
    }
}
