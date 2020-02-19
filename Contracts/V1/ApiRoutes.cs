using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commuinity.Api.V1.Contracts
{

    public class ApiRoutes
    {
        public const string Root = "api/";

        public const string Version = "v1/Community/";
        public const string Base = Root+ Version;


        public static class Community
        {


            #region Developer
            public const string InsertCommunityDeveloper = Base + "InsertCommunityDeveloper/{CID}";
            public const string GetCommunityDevelopers = Base + "GetCommunityDevelopers/{CID}";
            public const string GetDeveloperCommunities = Base + "GetDeveloperCommunities/{DID}";
            public const string DeleteCommunity = Base + "DeleteCommunity/{CID}";

            #endregion

            #region Community
            public const string GetCommunities = Base + "GetCommunities";
            public const string GetCommunity = Base + "GetCommunity/{CID}";
            public const string GetMinCommunity = Base + "GetMinCommunity/{CID}";
            public const string CreateCommunity = Base + "CreateCommunity";
            #endregion

            #region Organization
            public const string GetOrgainizations = Base + "GetOrgainizations";
            public const string GetCommunityOrganizations = Base + "GetCommunityOrganizations/{CID}";
            public const string GetCompanyOrganizations = Base + "GetCompanyOrganizations/{CoId}";
            public const string GetOrganizationById = Base + "GetOrganizationById/{OID}";
            public const string SearchOrganizations = Base + "SearchOrganizations/{Name}";
            public const string InsertOrganizationToCommunity = Base + "InsertOrganizationToCommunity/{CID}";
            public const string InsertDeveloperOrganiztion = Base + "InsertDeveloperOrganiztion/{CID}/{OID}";

            public const string ModifyOrganization = Base + "ModifyOrganization/{CID}/{OID}";
            public const string ModifyOrganizationPic = Base + "ModifyOrganizationPic/{CID}/{OID}";
            public const string DeleteOrganization = Base + "DeleteOrganization/{CID}/{OID}";
            #endregion

            #region Company  
            public const string GetCompanies = Base + "GetCompanies";//__
            public const string GetCommunityCompanies = Base + "GetCommunityCompanies/{CID}";//__
            public const string GetCompany = Base + "GetCompany/{CoID}";//__
            public const string SearchCompany = Base + "SearchCompany/{Name}";//__
            public const string InsertCompanyToCommunity = Base + "InsertCompany/{CID}";
            public const string ModifyCompany = Base + "ModifyCompany/{CID}/{CoID}";//__
            public const string ModifyCompanyPic = Base + "ModifyCompanyPic/{CID}/{CoID}";//__
            public const string DeleteCompany = Base + "DeleteCompany/{CID}/{CoID}";//__
            #endregion

            #region Posts
            public const string GetAllPosts = Base + "GetAllPosts"; 
            public const string GetIssuerPosts = Base + "GetIssuerPosts/{IID}";
            public const string GetPost = Base + "GetPost/{PID}";
            public const string ModifyPost = Base + "ModifyPost/{CID}/{PID}";        
            public const string ModifyPostPic = Base + "ModifyPostPic/{CID}/{PID}";       
            public const string CreatePost = Base + "CreatePost/{CID}";
            public const string Emo = Base + "Emo/{CID}/{PID}";
            public const string DeEmo = Base + "DeEmo/{CID}/{PID}/{EID}";
            public const string DeletePost = Base + "DeletePost/{CID}/{PID}";
            #endregion

        }


    }
}
