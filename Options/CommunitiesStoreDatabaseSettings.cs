using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commuinity.Api.Options
{

    public class CommunitiesStoreDatabaseSettings : ICommunitiestoreDatabaseSettings
    {
        public string CommunityCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface ICommunitiestoreDatabaseSettings {
        string CommunityCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

}
