using Microsoft.Practices.Unity;
//using Sabio.Web.Controllers.Attributes;
using Sabio.Web.Domain;
using Sabio.Web.Domain.CMS;
using Sabio.Web.Domain.Tests;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Requests.Tests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.CMS;
using Sabio.Web.Services.Tests;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sabio.Web.Models.Requests.CMS;
using Sabio.Web.Services.Interfaces;

namespace Sabio.Web.Controllers.CMSPages
{

    [RoutePrefix("api/cms/pages")]
    public class CMSPagesApiController : ApiController
    {

        public ICMSPagesService _cmsPageService = null;
        public IUserService _userService = null;

        public CMSPagesApiController(ICMSPagesService cmsPageInject, IUserService userServiceInject)
        {
            _cmsPageService = cmsPageInject;
            _userService = userServiceInject;
        }
        //INSERT
        [Route, HttpPost]
        public HttpResponseMessage AddCMSPages(CMSPagesAddRequest model)
        {
            if (!ModelState.IsValid || model == null || model.DateToPublish > model.DateToExpire)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();

            string userId = _userService.GetCurrentUserId();

            response.Item = _cmsPageService.Insert(model, userId);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //UPDATE 
        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage UpdateCMSPages(CMSPagesUpdateRequest model, int id)
        {
            if (!ModelState.IsValid || model == null || model.DateToPublish > model.DateToExpire)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            SuccessResponse response = new SuccessResponse();

            string userId = _userService.GetCurrentUserId();

            _cmsPageService.Update(model, id, userId);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //SELECT BY ID 
        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage Get(int id)
        {
            ItemResponse<CMSPage> response = new ItemResponse<CMSPage>();

            response.Item = _cmsPageService.Get(id);

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        //SELECT FOR PAGINATION
        [Route("icons/{pageIndex:int}/{pageSize:int}"), HttpGet]
        public HttpResponseMessage GetIcons(int pageIndex, int pageSize)
        {
            ItemResponse<PagedList<CMSPageIcon>> response = new ItemResponse<PagedList<CMSPageIcon>>();
            response.Item =  _cmsPageService.GetCMSIconsPaging(pageIndex, pageSize);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //SELECT ALL 
        [Route, HttpGet]
        public HttpResponseMessage Get()
        {
            ItemsResponse<CMSPage> response = new ItemsResponse<CMSPage>();

            response.Items = _cmsPageService.Get();

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        //DELETE 
        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            SuccessResponse response = new SuccessResponse();

            _cmsPageService.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        // SELECT FOR NAV BAR
        [Route("navpages"), HttpGet]
        public HttpResponseMessage GetNav()
        {
            ItemsResponse<CMSPageNav> response = new ItemsResponse<CMSPageNav>();

            response.Items = _cmsPageService.GetNavBar();

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        //  UPDATE ARRAY OF PAIR ID AND PAGEORDER
        [Route("pageorder"), HttpPut]
        public HttpResponseMessage MultiUpdatePageOrder(PairRequests pairs)
        {

            SuccessResponse response = new SuccessResponse();

            _cmsPageService.PairTables(pairs);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        // UPDATE PAGEORDER FOR NAVTABS INDIVIDUALLY
        [Route("pageorder/{id:int}/{pageOrder:int}"), HttpPut]
        public HttpResponseMessage UpdateNavPageOrder(int id, int pageOrder)
        {

            SuccessResponse response = new SuccessResponse();

            _cmsPageService.UpdatePageOrder(id, pageOrder);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        // SELECT ICONS FOR NAV BAR OPTIONS
        [Route("icons"), HttpGet]
        public HttpResponseMessage GetNavIcons()
        {
            ItemsResponse<CMSPageIcon> response = new ItemsResponse<CMSPageIcon>();

            response.Items = _cmsPageService.GetIcons();

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        //  SELECT SUBMENU PAGES
        [Route("tab/{id:int}"), HttpGet]
        public HttpResponseMessage GetBranch(int id)
        {
            ItemsResponse<CMSPageNav> response = new ItemsResponse<CMSPageNav>();

            response.Items = _cmsPageService.GetBranch(id);

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("navtabparents"), HttpGet]
        public HttpResponseMessage GetNavTabsWithChildren()
        {
            ItemsResponse<CMSPageNav> response = new ItemsResponse<CMSPageNav>();

            response.Items = _cmsPageService.GetNavBarTabsWChildren();

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [Route("checkurl"), HttpPost]
        public HttpResponseMessage CheckUrlExists(CMSUrlRequest model)
        {
            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = _cmsPageService.SelectIdByUrl(model.URL);

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        //SELECT PAGE TYPES 
        [Route("types"), HttpGet]
        public HttpResponseMessage GetSections()
        {
            ItemsResponse<CMSType> response = new ItemsResponse<CMSType>();

            response.Items = _cmsPageService.SelectPageTypes();

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


    }

}