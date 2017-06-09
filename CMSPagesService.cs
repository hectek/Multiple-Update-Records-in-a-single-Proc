using Sabio.Web.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Sabio.Web.Models.Requests.Tests;
using Sabio.Web.Models.Requests;
using Sabio.Data;
using Sabio.Web.Models.Requests.CMS;
using Sabio.Web.Domain.CMS;
using Sabio.Web.Services.Interfaces;
using Sabio.Web.Classes.Data;
using Sabio.Web.Domain.CMSTemplates;

namespace Sabio.Web.Services.CMS
{
    public class CMSPagesService : BaseService, ICMSPagesService
    {

        //INSERT PAGE
        public int Insert(CMSPagesAddRequest model, string userId)
        {
            int Id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.CMSPages_InsertV2"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                 {

                     paramCollection.AddWithValue("@TypeId", model.TypeId);
                     paramCollection.AddWithValue("@TemplateId", model.TemplateId);
                     paramCollection.AddWithValue("@UserId", userId);
                     paramCollection.AddWithValue("@Name", model.Name);
                     paramCollection.AddWithValue("@URL", model.URL);
                     paramCollection.AddWithValue("@DateToPublish", model.DateToPublish);
                     paramCollection.AddWithValue("@DateToExpire", model.DateToExpire);
                     paramCollection.AddWithValue("@CoverPhoto", model.CoverPhoto);
                     paramCollection.AddWithValue("@ParentId", model.ParentId);
                     paramCollection.AddWithValue("@isNavigation", model.isNavigation);
                     paramCollection.AddWithValue("@Icon", model.Icon);
                     paramCollection.AddWithValue("PageOrder", model.PageOrder);

                     SqlParameter p = new SqlParameter("@Id", SqlDbType.Int);
                     p.Direction = ParameterDirection.Output;

                     paramCollection.Add(p);
                 }, returnParameters: delegate (SqlParameterCollection param)
                 {
                     int.TryParse(param["@Id"].Value.ToString(), out Id);
                 });
            return Id;
        }

        //UPDATE PAGE
        public void Update(CMSPagesUpdateRequest model, int id, string userId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.CMSPages_UpdateV2"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@TypeId", model.TypeId);
                paramCollection.AddWithValue("@TemplateId", model.TemplateId);
                paramCollection.AddWithValue("@UserId", userId);
                paramCollection.AddWithValue("@Name", model.Name);
                paramCollection.AddWithValue("@URL", model.URL);
                paramCollection.AddWithValue("@DateToPublish", model.DateToPublish);
                paramCollection.AddWithValue("@DateToExpire", model.DateToExpire);
                paramCollection.AddWithValue("@CoverPhoto", model.CoverPhoto);
                paramCollection.AddWithValue("@ParentId", model.ParentId);
                paramCollection.AddWithValue("@isNavigation", model.isNavigation);
                paramCollection.AddWithValue("@Icon", model.Icon);
                paramCollection.AddWithValue("@Id", id);
                paramCollection.AddWithValue("@PageOrder", model.PageOrder);
            });
        }

        //SELECT PAGE BY ID
        public CMSPage Get(int Id)
        {
            CMSPage page = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSPages_SelectById_v2", inputParamMapper: delegate (SqlParameterCollection ParamCollection)
            {
                ParamCollection.AddWithValue("@Id", Id);
            }
            , map: delegate (IDataReader reader, short set)
            {
                page = MapCMSPage(reader);

            });

            return page;
        }

        //SELECT ALL PAGES
        public List<CMSPage> Get()
        {
            List<CMSPage> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSPages_SelectAll_v2"
                , inputParamMapper: null
                , map: delegate (IDataReader reader, short set)
                {
                    CMSPage page = MapCMSPage(reader);

                    if (list == null)
                    {
                        list = new List<CMSPage>();

                    }
                    list.Add(page);
                });
            return list;
        }

        //SELECT PAGE BY URL
        public CMSPage SelectPageUrl(string url)
        {
            CMSPage page = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSPages_SelectPagebyUrl", inputParamMapper: delegate (SqlParameterCollection ParamCollection)
            {
                ParamCollection.AddWithValue("@URL", url);
            }
            , map: delegate (IDataReader reader, short set)
            {
                page = MapCMSPage(reader);

            });

            return page;
        }

        //DELETE PAGE
        public void Delete(int Id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.CMSPages_DeleteById"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                 {
                     paramCollection.AddWithValue("@Id", Id);
                 });
        }

        //SELECT PAGE ID BY URL
        public int SelectIdByUrl(string url)
        {
            int id = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSPages_SelectByUrl"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@URL", url);
                }
                , map: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    int p = reader.GetSafeInt32(startingIndex++);

                    id = p;
                }
                );
            return id;
        }

        //SELECT TEMPLATE ID BY URL
        public int SelectTemplateIdByUrl(string url)
        {
            int templateId = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSPages_SelectPageTemplateByUrl"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@URL", url);
                }
                , map: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    int p = reader.GetSafeInt32(startingIndex++);

                    templateId = p;
                }
                );
            return templateId;
        }

        //SELECT TEMPLATE NAME BY URL
        public string SelectTemplateNameByUrl(string url)
        {
            string templateName = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSPages_SelectTemplateNamebyUrl"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@URL", url);
                }
                , map: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    string t = reader.GetSafeString(startingIndex++);

                    templateName = t;
                }
                );
            return templateName;
        }

        //SELECT PAGE BY BELONG TO NAVIGATION
        public List<CMSPageNav> GetNavBar()
        {
            List<CMSPageNav> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSPages_SelectForNavBar_v2"
                , inputParamMapper: null
                , map: delegate (IDataReader reader, short set)
                {
                    CMSPageNav page = MapCMSPageNav(reader);

                    if (list == null)
                    {
                        list = new List<CMSPageNav>();
                    }
                    list.Add(page);
                });

            return OrderNavBar(list);
        }

        //SELECT SUBMENU BRANCH WITH THIS PARENTID
        public List<CMSPageNav> GetBranch(int parentId)
        {
            List<CMSPageNav> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSPages_SelectBranch"
                , inputParamMapper: delegate (SqlParameterCollection ParamCollection)
           {
               ParamCollection.AddWithValue("@ParentId", parentId);
           }
                , map: delegate (IDataReader reader, short set)
                {
                    CMSPageNav page = MapCMSPageNav(reader);

                    if (list == null)
                    {
                        list = new List<CMSPageNav>();
                    }
                    list.Add(page);
                });

            list = list.OrderBy(o => o.PageOrder).ToList();

            return list;
        }

        //MULTIPLE UPDATE OF PAGE ORDER
        public void PairTables(PairRequests pairs)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.CMSPages_UpdateNavOrder"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   SqlParameter o = new SqlParameter("@Ordering", System.Data.SqlDbType.Structured);

                   if (pairs != null  && pairs.Pairs.Any())
                   {
                       o.Value = new PairTable(pairs.Pairs);
                   }

                   paramCollection.Add(o);

               }
               );


        }

        //UPDATE PAGE ORDER IN NAVIGATION BAR INDIVIDUALLY
        public void UpdatePageOrder(int id, int pageOrder)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.CMSPages_UpdatePageOrderMainNavMenu"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
           {
               paramCollection.AddWithValue("@Id", id);
               paramCollection.AddWithValue("@PageOrder", pageOrder);
           });
        }

        //SELECT NAVIGATION TABS WITH PARENT-CHILD RELATIONSHIP
        public List<CMSPageNav> GetNavBarTabsWChildren()
        {
            List<CMSPageNav> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSPages_SelectNavBarTabsWithChildren"
                , inputParamMapper: null
                , map: delegate (IDataReader reader, short set)
                {
                    CMSPageNav page = MapCMSPageNav(reader);

                    if (list == null)
                    {
                        list = new List<CMSPageNav>();
                    }
                    list.Add(page);
                });

            return OrderNavBar(list);
        }

        // ORDER NAVIGATION TABS BY PARENT-CHILD RELATIONSHIP
        private static List<CMSPageNav> OrderNavBar(List<CMSPageNav> pages)
        {

            for (int i = 0; i < pages.Count; i++)
            {
                if (pages[i].ParentId > 0)
                {
                    for (int j = 0; j < pages.Count; j++)
                    {
                        if (pages[j].Id == pages[i].ParentId)
                        {
                            pages[j].Children.Add(pages[i]);
                        }
                    }

                }
               
            }

            for (int k = 0; k < pages.Count; k++)
            {
                if (pages[k].Children.Count > 0)
                {
                    pages[k].Children = pages[k].Children.OrderBy(o => o.PageOrder).ToList();

                }

            }


            if (pages == null)
            {
                pages = new List<CMSPageNav>();
            }


            pages = pages.OrderBy(o => o.PageOrder).ToList();

            return pages;
        }

        //SELECT ICONS WITH PAGINATION
        public PagedList<CMSPageIcon> GetCMSIconsPaging(int pageIndex, int pageSize)
        {
            PagedList<CMSPageIcon> pagedList = null;
            int totalCount = 0;

            List<CMSPageIcon> list = null;

            DataProvider.ExecuteCmd(GetConnection
                , "dbo.Icons_SelectPaginate"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@PageIndex", pageIndex);
                    paramCollection.AddWithValue("@PageSize", pageSize);
                }
                , map: delegate (IDataReader reader, short set)
                {
                    CMSPageIcon p;
                    MapIconsTotalCount(reader, out p);

                    if (list == null)
                    {
                        list = new List<CMSPageIcon>();
                    }
                    list.Add(p);
                    if (totalCount == 0)
                        totalCount = p.TotalCount;
                });

            if (list != null)
                pagedList = new PagedList<CMSPageIcon>(list, 0, pageSize, totalCount);

            return pagedList;
        }

        // GET ALL ICONS FOR NAVIGATION BAR OPTIONS
        public List<CMSPageIcon> GetIcons()
        {
            List<CMSPageIcon> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Icons_SelectAll"
                , inputParamMapper: null
                , map: delegate (IDataReader reader, short set)
                {
                    CMSPageIcon icon = MapIcons(reader);

                    if (list == null)
                    {
                        list = new List<CMSPageIcon>();

                    }
                    list.Add(icon);
                });
            return list;
        }

        //SELECT ALL PAGE TYPES
        public List<CMSType> SelectPageTypes()
        {
            List<CMSType> types = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSPagesTypes_SelectAll"
                , inputParamMapper: null
                , map: delegate (IDataReader reader, short set)
                {

                    CMSType type = new CMSType();
                    int startingIndex = 0;
                    type.Id = reader.GetSafeInt32(startingIndex++);
                    type.Name = reader.GetSafeString(startingIndex++);


                    if (types == null)
                    {
                        types = new List<CMSType>();

                    }
                    types.Add(type);
                });
            return types;
        }



        // =========== CMS CONTENT  ========================//

        // INSERT CONTENT
        public int InsertContent(CMSContentAddRequest model)
        {
            int Id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.CMSContent_Insert"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {

                    paramCollection.AddWithValue("@TemplateKeyId", model.TemplateKeyId);
                    paramCollection.AddWithValue("@Value", model.Value);
                    paramCollection.AddWithValue("@CMSPageId", model.CMSPageId);

                    SqlParameter p = new SqlParameter("@Id", SqlDbType.Int);
                    p.Direction = ParameterDirection.Output;

                    paramCollection.Add(p);
                }, returnParameters: delegate (SqlParameterCollection param)
                {
                    int.TryParse(param["@Id"].Value.ToString(), out Id);
                });
            return Id;
        }

        //UPDATE CONTENT
        public void UpdateContent(CMSContentUpdateRequest model, int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.CMSContent_Update"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@TemplateKeyId", model.TemplateKeyId);
                    paramCollection.AddWithValue("@Value", model.Value);
                    paramCollection.AddWithValue("@CMSPageId", model.CMSPageId);
                    paramCollection.AddWithValue("@Id", id);

                });
        }

        //SELECT (KEY,VALUES) BY URL
        public Dictionary<string, string> GetKeyValues(string url)
        {
            Dictionary<string, string> content = null;


            DataProvider.ExecuteCmd(GetConnection, "CMSContent_SelectKeyValuesbyUrl"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@URL", url);
                }
                , map: delegate (IDataReader reader, short set)
                {
                    if (content == null)
                    {
                        content = new Dictionary<string, string>();

                    }
                    int startingIndex = 0;
                    var key = reader.GetSafeString(startingIndex++);
                    var val = reader.GetSafeString(startingIndex++);
                    content.Add(key, val);
                });
            return content;
        }

        //SELECT ALL TEMPLATE KEYS BY TEMPLATEID
        public List<CMSTemplateKey> GetTemplateKeys(int templateId)
        {
            List<CMSTemplateKey> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSTemplateKeys_SelectbyTemplate"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@TemplateId", templateId);
                }
                , map: delegate (IDataReader reader, short set)
                {
                    CMSTemplateKey page = MapCMSTemplateKey(reader);

                    if (list == null)
                    {
                        list = new List<CMSTemplateKey>();

                    }
                    list.Add(page);
                });
            return list;
        }

        //SELECT (KEY,VALUES, KEYTYPE) BY URL
        public List<CMSContentKeyValueType> GetKeyValuesType(string url)
        {
            List<CMSContentKeyValueType> list = null;

            DataProvider.ExecuteCmd(GetConnection, "CMSContent_KeyValuesTypebyURL"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@URL", url);
                }
                , map: delegate (IDataReader reader, short set)
                {
                    CMSContentKeyValueType content = MapCMSContentKVT(reader);

                    if (list == null)
                    {
                        list = new List<CMSContentKeyValueType>();

                    }
                    list.Add(content);
                });
            return list;
        }

        //DELETE CONTENT
        public void DeleteContent(int Id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.CMSContent_Delete"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", Id);
                });
        }

        //CHECK URL EXISTENCE IN TABLE
        public int CheckUrlExists(string url)
        {
            int urlCount = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSPages_CheckUrlExists"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@URL", url);
                }
                , map: delegate (IDataReader reader, short set)
                {
                    urlCount = reader.GetSafeInt32(0);
                });
            return urlCount;
        }

        //SELECT ALL CONTENT SECTION TYPES
        public List<CMSType> SelectSections()
        {
            List<CMSType> types = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.CMSSectionTypes_SelectAll"
                , inputParamMapper: null
                , map: delegate (IDataReader reader, short set)
                {

                    CMSType type = new CMSType();
                    int startingIndex = 0;
                    type.Id = reader.GetSafeInt32(startingIndex++);
                    type.Name = reader.GetSafeString(startingIndex++);


                    if (types == null)
                    {
                        types = new List<CMSType>();

                    }
                    types.Add(type);
                });
            return types;
        }

        // =======  PAGE MAP READERS    ===========================  //
        private static CMSPage MapCMSPage(IDataReader reader)
        {
            CMSPage C = new CMSPage();
            C.Template = new CMSTemplate();
            int startingIndex = 0;

            C.Id = reader.GetSafeInt32(startingIndex++);
            C.TypeId = reader.GetSafeInt32(startingIndex++);
            C.TemplateId = reader.GetSafeInt32(startingIndex++);
            C.Template.Id = reader.GetSafeInt32(startingIndex++);
            C.Template.Name = reader.GetSafeString(startingIndex++);
            C.Template.Description = reader.GetSafeString(startingIndex++);
            C.Template.Path = reader.GetSafeString(startingIndex++);
            C.Template.TypeId = reader.GetSafeInt32(startingIndex++);
            C.Template.CreatedBy = reader.GetSafeString(startingIndex++);
            C.Template.CreatedDate = reader.GetSafeDateTime(startingIndex++);
            C.Template.ModifiedBy = reader.GetSafeString(startingIndex++);
            C.Template.ModifiedDate = reader.GetSafeDateTime(startingIndex++);
            C.UserId = reader.GetSafeString(startingIndex++);
            C.Name = reader.GetSafeString(startingIndex++);
            C.URL = reader.GetSafeString(startingIndex++);
            C.DateToPublish = reader.GetSafeDateTimeNullable(startingIndex++);
            C.DateToExpire = reader.GetSafeDateTimeNullable(startingIndex++);
            C.DateAdded = reader.GetSafeDateTime(startingIndex++);
            C.DateModified = reader.GetSafeDateTime(startingIndex++);
            C.CoverPhoto = reader.GetSafeString(startingIndex++);
            C.ParentId = reader.GetSafeInt32Nullable(startingIndex++);
            C.isNavigation = reader.GetSafeBool(startingIndex++);
            C.Icon = reader.GetSafeString(startingIndex++);
            C.PageOrder = reader.GetSafeInt32Nullable(startingIndex++);
            return C;
        }
        private static CMSPageNav MapCMSPageNav(IDataReader reader)
        {
            CMSPageNav C = new CMSPageNav();
            List<CMSPageNav> children = new List<CMSPageNav>();
            int startingIndex = 0;

            C.Id = reader.GetSafeInt32(startingIndex++);
            C.Name = reader.GetSafeString(startingIndex++);
            C.URL = reader.GetSafeString(startingIndex++);
            C.ParentId = reader.GetSafeInt32Nullable(startingIndex++);
            C.Icon = reader.GetSafeString(startingIndex++);
            C.Children = children;
            C.PageOrder = reader.GetSafeInt32Nullable(startingIndex++);
            return C;
        }
        private static CMSPageIcon MapIcons(IDataReader reader)
        {
            CMSPageIcon C = new CMSPageIcon();
            int startingIndex = 0;

            C.Id = reader.GetSafeInt32(startingIndex++);
            C.Symbol = reader.GetSafeString(startingIndex++);
            C.Name = reader.GetSafeString(startingIndex++);

            return C;
        }
        private static CMSPageIcon MapIconsTotalCount(IDataReader reader, out CMSPageIcon C)
        {
            C = new CMSPageIcon();
            int startingIndex = 0;

            C.Id = reader.GetSafeInt32(startingIndex++);
            C.Symbol = reader.GetSafeString(startingIndex++);
            C.Name = reader.GetSafeString(startingIndex++);
            C.TotalCount = reader.GetSafeInt32(startingIndex++);

            return C;
        }


        // =======  CONTENT MAP READERS   =========================== //
        private static CMSTemplateKey MapCMSTemplateKey(IDataReader reader)
        {
            CMSTemplateKey C = new CMSTemplateKey();
            int startingIndex = 0;

            C.Id = reader.GetSafeInt32(startingIndex++);
            C.Type = reader.GetSafeInt32(startingIndex++);
            C.KeyName = reader.GetSafeString(startingIndex++);
            C.Section = reader.GetSafeInt32(startingIndex++);

            return C;
        }
        private static CMSContentKeyValueType MapCMSContentKVT(IDataReader reader)
        {
            CMSContentKeyValueType C = new CMSContentKeyValueType();
            int startingIndex = 0;

            C.KeyName = reader.GetSafeString(startingIndex++);
            C.Value = reader.GetSafeString(startingIndex++);
            C.Type = reader.GetSafeInt32(startingIndex++);
            C.ContentId = reader.GetSafeInt32(startingIndex++);
            C.Section = reader.GetSafeInt32(startingIndex++);
            C.TemplateKeyId = reader.GetSafeInt32(startingIndex++);

            return C;
        }

    }
}