
using Commuinity.Api.Domain;
using Community.Api.Contracts.V1.Requests;
using Community.Api.Contracts.V1.Responses;
using Community.Api.Domain;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commuinity.Api.Services
{
    public interface IMongoCommunitiesService
    {
        Task<IEnumerable<MinCommunityListViewModel>> GetCommunities();
        Task<MongoCommunitiesDto> GetCommunity(string CID);
        Task<IEnumerable< MinCommunityListViewModel>> GetDeveloperCommunities(string DID);
        Task<string> CreateCommunity(CreateCommunityViewModel Community);
        Task<bool> InsertCommunityDeveloper(string CID,Developer developer);

        #region Organization
        Task<IEnumerable<OrganizationMinViewModel>> GetOrgainizations();
        Task<IEnumerable<OrganizationMinViewModel>> GetCommunityOrganizations(string CID);
        Task<IEnumerable<OrganizationMinViewModel>> GetCompanyOrganizations(string CoID);
        Task<Organization> GetOrganizationById(string OID);
        Task<IEnumerable<OrganizationMinViewModel>> SearchOrganizations(string Name);
        Task<bool> ModifyOrganization(string CID,string OID, OrganizationModifyViewModel model);
        Task<bool> ModifyOrganizationPic(string CID, string OID, IFormFile file);
        Task<bool> DeleteOrganization(string CID, string OID);
        Task<bool> InsertOrganizationToCommunity(string CID, OrganizationInsertViewModel organization);
        Task<bool> InsertDeveloperOrganization(string CID, string OID , Developer developer);

        #endregion
        #region Posts
        Task<IEnumerable<Post>> GetAllPosts();
        Task<IEnumerable<Post>> GetIssuerPosts(string IID);
        Task<Post> GetPost(string PID );
        Task<bool> CreatePost(string CID, InsertPostViewModel model);
        Task<bool> ModifyPost(string CID,string PID, UpdatePostViewModel model);
        Task<bool> ModifyPostImage(string CID,string PID, IFormFile file);
        Task<bool> DeletePost(string CID, string PID);
        Task<bool> Emo( string CID,string PID, Emotion emotion);
        Task<bool> DeEmo(string CID, string PID, string EID);
        #endregion

        #region Comapny
        Task<IEnumerable<Company>> GetCompanies();
        Task<IEnumerable<Company>> GetCommunityCompanies (string CID);
        Task<Company> GetCompany (string CoID);
        Task<bool> InsertCompanyToCommunity(string CID, InsertCompanyViewModel company);
        Task<Company> SearchCompany(string Name);
        Task<bool> ModifyCompany(string CID,string CoID , InsertCompanyViewModel model);//__
        Task<bool> ModifyCompanyPic(string CID, string CoID, IFormFile file);//__
        Task<bool> DeleteCompany(string CID, string CoID);//__
        #endregion

        Task<bool> DeleteCommunity(string CID);

    }
}
