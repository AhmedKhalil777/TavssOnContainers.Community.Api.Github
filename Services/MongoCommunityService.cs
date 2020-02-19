using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Commuinity.Api.Domain;
using Commuinity.Api.Options;
using Community.Api.Contracts.V1.Requests;
using Community.Api.Contracts.V1.Responses;
using Community.Api.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;

using Newtonsoft.Json;

namespace Commuinity.Api.Services
{

    public class MongoCommunitiesService : IMongoCommunitiesService
    {
        private readonly IMongoCollection<MongoCommunitiesDto> _Community;
        private readonly IHostingEnvironment _hostingEnvironment; 

        public MongoCommunitiesService(ICommunitiestoreDatabaseSettings settings , IHostingEnvironment hostingEnvironment)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _Community = database.GetCollection<MongoCommunitiesDto>(settings.CommunityCollectionName);
            _hostingEnvironment = hostingEnvironment;
        }



        public  async Task<IEnumerable<MinCommunityListViewModel>> GetCommunities()
        {
            var communities = await _Community.FindAsync(a => true);

            return  communities.ToList().Select(a=> new MinCommunityListViewModel{ Id = a.Id ,Description= a.Description, LogoPath = a.LogoPath, Name = a.Name});

        }

        public async Task<MongoCommunitiesDto> GetCommunity(string CID)
        {
            var community = await _Community.FindAsync(a => a.Id == CID);
            return await community.FirstOrDefaultAsync();
        }


        public async Task<string> CreateCommunity(CreateCommunityViewModel Community)
        {
            MongoCommunitiesDto dto = new MongoCommunitiesDto
            {
                Name = Community.Name,
                Description = Community.Description,
                Developers = new List<Developer>(),
                LogoPath = "",
                Organizations = new List<Organization>(),
                Companies = new List<Company>(),
                Posts = new List<Post>()


            };
            await _Community.InsertOneAsync(dto);
            return "Successful";

        }
        public async Task<IEnumerable<MinCommunityListViewModel>> GetDeveloperCommunities(string DID)
        {

            var Dcoms = new List<MinCommunityListViewModel>();
            var coms = await _Community.FindAsync(a => true);
            foreach (var com in coms.ToList())
            {
                if (com.Developers.Find( a=> a.Id ==DID)!=null)
                {
                    Dcoms.Add(new MinCommunityListViewModel { Id = com.Id, Description = com.Description, LogoPath = com.LogoPath, Name = com.Name });
                }
            }
            return Dcoms;
        }

        public async Task<bool> InsertCommunityDeveloper(string CID,Developer developer)
        {
            var com = await GetCommunity(CID);
            com.Developers.Add(developer);
            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;
        }

        public async Task<bool> InsertCompanyToCommunity(string CID ,InsertCompanyViewModel company)
        {
            var com = await GetCommunity(CID);
            com.Companies.Add(new Company { 
                Id =Guid.NewGuid().ToString() , Name= company.Name ,
                StartDate = DateTime.Now ,Address = company.Address , Contacts = company.Contacts,
            Description =company.Description});
            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;
        }

        public async Task<bool> InsertOrganizationToCommunity(string CID, OrganizationInsertViewModel organization)
        {
            var org = new Organization
            {
                OType = organization.OType,
                CId = organization.CId,
                Id = Guid.NewGuid().ToString(),
                Name = organization.Name,
                Description = organization.Description,
                StartDate = organization.StartDate,
                Developers = new List<Developer>(),
                LogoPath = ""

            };
            var com = await GetCommunity(CID);
            com.Organizations.Add(org);
            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;

        }
        #region Company
        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var coms = await _Community.FindAsync(x => true);
            var  companies = new List<Company>();

            foreach (var com in coms.ToList())
            {
                foreach (var comp in com.Companies)
                {
                    companies.Add(comp);
                }
            }
            return companies;
           
        }

        public async Task<IEnumerable<Company>> GetCommunityCompanies(string CID)
        {
            var com = await GetCommunity(CID);
            var comps =  com.Companies;
            return comps;
        }

        public async Task<Company> GetCompany(string CoID)
        {
            var comps = await GetCompanies();
            return comps.ToList().SingleOrDefault(x => x.Id == CoID);

        }


        public async Task<Company> SearchCompany(string Name)
        {
            var comps = await GetCompanies();
            return comps.ToList().SingleOrDefault(x => x.Name == Name);
        }

        public async Task<bool> ModifyCompany(string CID, string CoID , InsertCompanyViewModel model)
        {
            var com = await GetCommunity(CID);
            var comp = com.Companies.SingleOrDefault(x => x.Id == CoID);
            com.Companies.Remove(comp);
            comp.Name = model.Name;
            comp.Address = model.Address;
            comp.Contacts = model.Contacts;
            comp.Description = model.Description;
            com.Companies.Add(comp);

            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;
        }

        public async Task<bool> ModifyCompanyPic(string CID, string CoID , IFormFile file)
        {
            var com = await GetCommunity(CID);
            var company = com.Companies.SingleOrDefault(x => x.Id == CoID);
            com.Companies.Remove(company);
            string m = com.Id + com.Name+"\\"+company.Id+company.Name;
            if (file.Length > 0)
            {
                try
                {

                    if (!Directory.Exists(_hostingEnvironment.WebRootPath + "\\" + m + "\\"))
                    {
                        Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "\\" + m + "\\");
                    }
                    string guid = Guid.NewGuid().ToString();
                    using (FileStream fileStream = System.IO.File.Create(_hostingEnvironment.WebRootPath + "\\" + m + "\\" + guid + file.FileName.Replace("\\", "s").Replace(":", "s")))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        company.LogoPath = _hostingEnvironment.WebRootPath + "\\" + m + "\\" + guid + file.FileName;
                        com.Companies.Add(company);
                        var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
                        return result.IsAcknowledged;
                    }
                }
                catch
                {

                    return false;
                }

            }

            return false;
        }

        public async Task<bool> DeleteCompany(string CID, string CoID)
        {
            var com = await GetCommunity(CID);
            var comp = com.Companies.SingleOrDefault(x => x.Id == CoID);
            com.Companies.Remove(comp);
            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;
        }

        #endregion

        #region Posts

        #region CRUD
        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            var coms =await _Community.FindAsync(x => true);
            var posts = new List<Post>();
            foreach (var com in await coms.ToListAsync())
            {
                foreach (var post in com.Posts)
                {
                    posts.Add(post);
                   
                }
            }
            return posts;
        }


        public async Task<IEnumerable<Post>> GetIssuerPosts(string IID)
        {
            var posts = await GetAllPosts();
            return posts.Where(x => x.IssuerId == IID);

        }

        public async Task<Post> GetPost(string PID )
        {
            var posts = await GetAllPosts();
            try
            {
                return posts.Single(x => x.Id == PID);
            }
            catch (Exception)
            { }
            return null;
        }

        public async Task<bool> CreatePost(string CID, InsertPostViewModel model)
        {
            var com = await GetCommunity(CID);
            Post post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                Description = model.Description,
                IssuerId = model.IssuerId,
                IssuerType = model.IssuerType,
                Time = DateTime.Now,
                Title = model.Title,
                Comments = new List<Comment>(),
                Emotions = new List<Emotion>(),
                Image = ""
            };
            com.Posts.Add(post);
            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;
        }
        public async Task<bool> ModifyPost( string CID,string PID, UpdatePostViewModel model)
        {
            var com = await GetCommunity(CID);
            var post = com.Posts.SingleOrDefault(x => x.Id == PID);
            com.Posts.Remove(post);
            post.Title = model.Title;
            post.Description = model.Description;
            com.Posts.Add(post);
            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;
        }
        public async Task<bool> ModifyPostImage(string CID, string PID, IFormFile file)
        {
            var com = await GetCommunity(CID);
            var post = com.Posts.SingleOrDefault(x => x.Id == PID);
            com.Posts.Remove(post);
            string m = com.Id + com.Name + "\\" + post.Id + post.IssuerId;
            if (file.Length > 0)
            {
                try
                {

                    if (!Directory.Exists(_hostingEnvironment.WebRootPath + "\\" + m + "\\"))
                    {
                        Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "\\" + m + "\\");
                    }
                    string guid = Guid.NewGuid().ToString();
                    using (FileStream fileStream = System.IO.File.Create(_hostingEnvironment.WebRootPath + "\\" + m + "\\" + guid + file.FileName.Replace("\\", "s").Replace(":", "s")))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        post.Image = _hostingEnvironment.WebRootPath + "\\" + m + "\\" + guid + file.FileName;
                        com.Posts.Add(post);
                        var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
                        return result.IsAcknowledged;
                    }
                }
                catch
                {

                    return false;
                }

            }

            return false;
        }

        public async Task<bool> DeletePost(string CID ,string PID)
        {
            var com = await GetCommunity(CID);
            var post = com.Posts.SingleOrDefault(x => x.Id == PID);
            com.Posts.Remove(post);
            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;
        }

        #endregion

        #region Emotion
        public async Task<bool> Emo(string CID, string PID, Emotion emotion)
        {
            var com = await GetCommunity(CID);
            var post = com.Posts.SingleOrDefault(x => x.Id == PID);
            com.Posts.Remove(post);
            try
            {
                post.Emotions.Remove(post.Emotions.Single(x => x.Id == emotion.Id));
            }
            catch (Exception) { }

            post.Emotions.Add(emotion);

            com.Posts.Add(post);

            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;


        }

        public async Task<bool> DeEmo(string CID, string PID, string EID)
        {
            var com = await GetCommunity(CID);
            var post = com.Posts.SingleOrDefault(x => x.Id == PID);
            com.Posts.Remove(post);
            try
            {
                post.Emotions.Remove(post.Emotions.Single(x => x.Id == EID));
            }
            catch (Exception) { }

            com.Posts.Add(post);

            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;
        }










        #endregion

        #endregion

        #region Organiztion
        public async Task<IEnumerable<OrganizationMinViewModel>> GetOrgainizations()
        {
            var coms =await _Community.FindAsync(x => true);
            var orgs = new List<OrganizationMinViewModel>();
            foreach (var com in await coms.ToListAsync())
            {
                foreach (var org in com.Organizations)
                {
                    orgs.Add(new OrganizationMinViewModel 
                    { Id =org.Id ,CId =org.CId,Description =org.Description ,
                        Name = org.Name, OType= org.OType,StartDate=org.StartDate});
                }
            }
            return orgs;
        }

        public async Task<IEnumerable<OrganizationMinViewModel>> GetCommunityOrganizations(string CID)
        {
            var com = await _Community.FindAsync(x => x.Id == CID);
            return  com.FirstOrDefault().Organizations.Select(x=>new OrganizationMinViewModel {
                Id = x.Id,
                CId = x.CId,
                Description = x.Description,
                Name = x.Name,
                OType = x.OType,
                StartDate = x.StartDate
            });
        }

        public async Task<IEnumerable<OrganizationMinViewModel>> GetCompanyOrganizations(string CoID)
        {
            var orgs = await GetOrgainizations();
            return orgs.ToList().Where(x => x.CId == CoID && x.OType == OrganizationType.Company);
        }

           

        public async Task<Organization> GetOrganizationById(string OID)
        {
            var coms = await _Community.FindAsync(x => true);
            var orgs = new List<Organization>();
            foreach (var com in await coms.ToListAsync())
            {
                foreach (var org in com.Organizations)
                {
                    orgs.Add(org);
                }
            }
            return orgs.SingleOrDefault(x=>x.Id == OID);
        }

        public async Task<IEnumerable<OrganizationMinViewModel>> SearchOrganizations(string Name)
        {
            var orgs = await GetOrgainizations();
            return orgs.Where(x => x.Name.ToUpper().Trim().Contains(Name.Trim().ToUpper()));
        }

        public async Task<bool> ModifyOrganization(string CID, string OID , OrganizationModifyViewModel model)
        {
            var com = await GetCommunity(CID);
            var org =com.Organizations.SingleOrDefault(x => x.Id == OID);
            com.Organizations.Remove(org);
            org.Name = model.Name;
            org.Description = model.Description;
            com.Organizations.Add(org);

            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;
        }

        public async Task<bool> ModifyOrganizationPic(string CID, string OID , IFormFile file)
        {
            var com = await GetCommunity(CID);
            var org = com.Organizations.SingleOrDefault(x => x.Id == OID);
            com.Organizations.Remove(org);
            string m = com.Id + com.Name + "\\" + org.Id + org.CId;
            if (file.Length > 0)
            {
                try
                {

                    if (!Directory.Exists(_hostingEnvironment.WebRootPath + "\\" + m + "\\"))
                    {
                        Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "\\" + m + "\\");
                    }
                    string guid = Guid.NewGuid().ToString();
                    using (FileStream fileStream = System.IO.File.Create(_hostingEnvironment.WebRootPath + "\\" + m + "\\" + guid + file.FileName.Replace("\\", "s").Replace(":", "s")))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        org.LogoPath= _hostingEnvironment.WebRootPath + "\\" + m + "\\" + guid + file.FileName;
                        com.Organizations.Add(org);
                        var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
                        return result.IsAcknowledged;
                    }
                }
                catch
                {

                    return false;
                }

            }

            return false;
        }

        public async Task<bool> DeleteOrganization(string CID, string OID)
        {
            var com = await GetCommunity(CID);
            com.Organizations.Remove(com.Organizations.SingleOrDefault(x => x.Id == OID));
            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;
        }


        public async Task<bool> InsertDeveloperOrganization(string CID, string OID, Developer developer)
        {
            var com = await GetCommunity(CID);
            var org =com.Organizations.SingleOrDefault(x => x.Id == OID);
            com.Organizations.Remove(org);
            org.Developers.Add(developer);
            com.Organizations.Add(org);
            var result = await _Community.ReplaceOneAsync(x => x.Id == CID, com);
            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteCommunity(string CID)
        {
            var result =await _Community.DeleteOneAsync(x => x.Id == CID);
            return result.IsAcknowledged;
        }

        #endregion

    }
}
