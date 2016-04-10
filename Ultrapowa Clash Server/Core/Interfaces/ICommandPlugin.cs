using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Sys;

namespace UCS.Core.Interfaces
{
    public interface ICommandPlugin
    {
        string Title { get; }
        string AuthorName { get; }
        string Version { get; }
        string ImageURL { get; }
        string Information { get; }
        string URL { get; }

        List<CommandActionList> CommandList();
    }
}
