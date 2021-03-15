using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Utility
{
    public interface IDialogViewModel
    {
        bool Save(); 
        void Close();
        string Title { get; set; }
    }
}
