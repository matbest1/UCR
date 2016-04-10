using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCS.Core.Interfaces
{
    public interface IGeneralPlugin
    {
        string Title { get; }
        string AuthorName { get; }
        string Version { get; }
        string ImageURL { get; }
        string Information { get; }
        string URL { get; }

        void RunUI();
    }
}
