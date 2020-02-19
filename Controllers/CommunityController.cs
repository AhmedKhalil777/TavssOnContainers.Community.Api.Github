using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commuinity.Api.Domain;
using Commuinity.Api.Services;
using Commuinity.Api.V1.Contracts;
using Community.Api.Contracts.V1.Requests;
using Community.Api.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Commuinity.Api.Controllers
{
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly IMongoCommunitiesService _MongocourseService;
        public CommunityController(IMongoCommunitiesService mongoCourseService)
        {
            _MongocourseService = mongoCourseService;
        }

        #region Get
        [HttpGet(ApiRoutes.Community.GetCommunities)]
        public async Task<IActionResult> GetCommunities() => Ok(await _MongocourseService.GetCommunities());
        [HttpGet(ApiRoutes.Community.GetCommunity)]
        public async Task<IActionResult> GetCommunity([FromRoute] string CID) => Ok(await _MongocourseService.GetCommunity(CID));
        [HttpGet(ApiRoutes.Community.GetDeveloperCommunities)]
        public async Task<IActionResult> GetDeveloperCommunities([FromRoute] string DID) =>
            Ok(await _MongocourseService.GetDeveloperCommunities(DID));
        #endregion

        #region Create
        [HttpPost(ApiRoutes.Community.CreateCommunity)]
        public async Task<IActionResult> CreateCommunity([FromBody] CreateCommunityViewModel community)
        {
            var result = await _MongocourseService.CreateCommunity(community);
            if (result == "Successful")
            {
                return Ok(new
                {
                    Status = 1,
                    Message = result
                });
            }
            return BadRequest(new
            {
                Status = 0,
                Message = "Error occured"
            });
        }

        [HttpPut(ApiRoutes.Community.InsertCommunityDeveloper)]
        public async Task<IActionResult> InsertCommunityDeveloper([FromRoute]string CID, [FromBody] Developer developer)
        {

            var result = await _MongocourseService.InsertCommunityDeveloper(CID, developer);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Inserted" });
            }
            return BadRequest();
        }

        [HttpPut(ApiRoutes.Community.InsertCompanyToCommunity)]
        public async Task<IActionResult> InsertCompanyToCommunity([FromRoute] string CID, [FromBody] InsertCompanyViewModel model)
        {
            var result = await _MongocourseService.InsertCompanyToCommunity(CID, model);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Inserted" });
            }
            return BadRequest();
        }



        #region Company

        [HttpGet(ApiRoutes.Community.GetCompanies)]
        public async Task<IActionResult> GetCompanies() =>Ok( await _MongocourseService.GetCompanies());

        [HttpGet(ApiRoutes.Community.GetCommunityCompanies)]
        public async Task<IActionResult> GetCommunityCompanies([FromRoute]string CID)=>Ok(await _MongocourseService.GetCommunityCompanies(CID));//__
        
        [HttpGet(ApiRoutes.Community.GetCompany)]
        public async Task<IActionResult> GetCompany([FromRoute]string CoID)=> Ok(await _MongocourseService.GetCompany(CoID)) ;//__
        
        [HttpGet(ApiRoutes.Community.SearchCompany)]
        public async Task<IActionResult> SearchCompany([FromRoute]string Name)=>Ok(await _MongocourseService.SearchCompany(Name));//__
        
        [HttpPut(ApiRoutes.Community.ModifyCompany)]
        public async Task<IActionResult> ModifyCompany([FromRoute]string CID, [FromRoute] string CoID, [FromBody] InsertCompanyViewModel model) 
        {
            var result = await _MongocourseService.ModifyCompany(CID, CoID ,model);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Modified" });
            }
            return BadRequest();
        }

        [HttpPut(ApiRoutes.Community.ModifyCompanyPic)]
        public async Task<IActionResult> ModifyCompanyPic([FromRoute]string CID, [FromRoute] string CoID,  IFormFile file)
        {
            var result = await _MongocourseService.ModifyCompanyPic(CID, CoID, file);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Modified" });
            }
            return BadRequest();
        }

        [HttpPut(ApiRoutes.Community.DeleteCompany)]
        public async Task<IActionResult> DeleteCompany(string CID, string CoID) {
            var result = await _MongocourseService.DeleteCompany(CID, CoID);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Deleted" });
            }
            return BadRequest();

        }
        #endregion

        #endregion

        #region Posts
        [HttpGet(ApiRoutes.Community.GetAllPosts)]
        public async Task<IActionResult> GetAllPosts() => Ok(await _MongocourseService.GetAllPosts());

        [HttpGet(ApiRoutes.Community.GetIssuerPosts)]
        public async Task<IEnumerable<Post>> GetIssuerPosts([FromRoute]string IID) => await _MongocourseService.GetIssuerPosts(IID);
       
        [HttpGet(ApiRoutes.Community.GetPost)]
        public async Task<Post> GetPost(string PID) => await _MongocourseService.GetPost(PID);

        [HttpPut(ApiRoutes.Community.ModifyPost)]
        public async Task<IActionResult> ModifyPost([FromRoute] string CID, [FromRoute] string PID,[FromBody] UpdatePostViewModel post)
        {
            var result = await _MongocourseService.ModifyPost(CID, PID, post);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Modified" });
            }
            return BadRequest();

        }

        [HttpPut(ApiRoutes.Community.ModifyPostPic)]
        public async Task<IActionResult> ModifyPostPic([FromRoute] string CID, [FromRoute] string PID, IFormFile form)
        {
            var result = await _MongocourseService.ModifyPostImage(CID, PID, form);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Modified" });
            }
            return BadRequest();

        }

        [HttpPut(ApiRoutes.Community.CreatePost)]
        public async Task<IActionResult> CreatePost([FromRoute] string CID, [FromBody] InsertPostViewModel post)
        {
            var result = await _MongocourseService.CreatePost(CID, post);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Created" });
            }
            return BadRequest();
        }
        [HttpPut(ApiRoutes.Community.Emo)]
        public async Task<IActionResult> Emo([FromRoute] string CID , [FromRoute] string PID ,Emotion emotion)
        {
           var result = await _MongocourseService.Emo(CID, PID, emotion);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Emo" });
            }
            return BadRequest();

        }

        [HttpDelete(ApiRoutes.Community.DeEmo)]
        public async Task<IActionResult> DeEmo([FromRoute] string CID, [FromRoute] string PID, [FromRoute] string EID)
        {
            var result = await _MongocourseService.DeEmo(CID, PID, EID);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Emo" });
            }
            return BadRequest();

        }
        [HttpDelete(ApiRoutes.Community.DeletePost)]
        public async Task<IActionResult> DeletePost([FromRoute] string CID, [FromRoute] string PID)
        {
            var result = await _MongocourseService.DeletePost(CID, PID);
            if (result)
            {
                return Ok(new { status = 1, Message = "Deleted Successfully" });
            }
            return BadRequest();

        }
        #endregion

        #region Organizations
        [HttpPut(ApiRoutes.Community.InsertOrganizationToCommunity)]
        public async Task<IActionResult> InsertOrganizationToCommunity([FromRoute] string CID, [FromBody] OrganizationInsertViewModel model)
        {
            var result = await _MongocourseService.InsertOrganizationToCommunity(CID, model);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Inserted" });
            }
            return BadRequest();
        }
        [HttpGet(ApiRoutes.Community.GetOrgainizations)]
        public async Task<IActionResult> GetOrgainizations()=> Ok(await _MongocourseService.GetOrgainizations());
        [HttpGet(ApiRoutes.Community.GetCommunityOrganizations)]
        public async Task<IActionResult> GetCommunityOrganizations([FromRoute]string CID)=> 
            Ok(await _MongocourseService.GetCommunityOrganizations(CID));
        [HttpGet(ApiRoutes.Community.GetCompanyOrganizations)]
        public async Task<IActionResult> GetCompanyOrganizations([FromRoute]string CoID) =>
            Ok(await _MongocourseService.GetCompanyOrganizations(CoID));
        [HttpGet(ApiRoutes.Community.GetOrganizationById)]
        public async Task<IActionResult> GetOrganizationById([FromRoute]string OID) =>
            Ok(await _MongocourseService.GetOrganizationById(OID));
        [HttpGet(ApiRoutes.Community.SearchOrganizations)]
        public async Task<IActionResult> SearchOrganizations([FromRoute]string Name) =>
            Ok(await _MongocourseService.SearchOrganizations(Name));
        [HttpGet(ApiRoutes.Community.ModifyOrganization)]
        public async Task<IActionResult> ModifyOrganization
            ([FromRoute]string CID, [FromRoute]string OID,[FromBody] OrganizationModifyViewModel model)
        {
            var result = await _MongocourseService.ModifyOrganization(CID,OID, model);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Modified" });
            }
            return BadRequest();
        }
        [HttpPut(ApiRoutes.Community.ModifyOrganizationPic)]
        public async Task<IActionResult> ModifyOrganizationPic([FromRoute]string CID, [FromRoute] string OID, IFormFile file)
        {
            var result = await _MongocourseService.ModifyOrganizationPic(CID, OID, file);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Inserted" });
            }
            return BadRequest();
        }
        [HttpDelete(ApiRoutes.Community.DeleteOrganization)]
        public async Task<IActionResult> DeleteOrganization([FromRoute]string CID, [FromRoute] string OID)
        {
            var result = await _MongocourseService.DeleteOrganization(CID, OID);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Deleted" });
            }
            return BadRequest();
        }
        [HttpPut(ApiRoutes.Community.InsertDeveloperOrganiztion)]
        public async Task<IActionResult> InsertDeveloperOrganization([FromRoute]string CID, [FromRoute]string OID,[FromBody] Developer developer)
        {
            var result = await _MongocourseService.InsertDeveloperOrganization(CID, OID, developer);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Modified" });
            }
            return BadRequest();
        }
        #endregion
        [HttpDelete(ApiRoutes.Community.DeleteCommunity)]
        public async Task<IActionResult> DeleteCommunity([FromRoute]string CID)
        {
            var result = await _MongocourseService.DeleteCommunity(CID);
            if (result)
            {
                return Ok(new { status = 1, Message = "Successfully Deleted" });
            }
            return BadRequest();
        }
    }
}