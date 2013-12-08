using HQServer.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HQServer.WebUI.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
PagingInfo pagingInfo,
Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            //for (int i = 1; i <= pagingInfo.TotalPages; i++)
            int remainder = pagingInfo.CurrentPage % 10;
            int pageStart;
            switch (remainder)
            {
                case 0:
                    pageStart = pagingInfo.CurrentPage - 10;
                    break;
                case 1:
                    pageStart = pagingInfo.CurrentPage - 1;
                    break;
                default:
                    pageStart = pagingInfo.CurrentPage - remainder;
                    break;
            }
            //int pageStart = remainder == 1 ? pagingInfo.CurrentPage - 1 : pagingInfo.CurrentPage - remainder;
            int pageEnd = (pageStart + 11 > pagingInfo.TotalPages) ? pagingInfo.TotalPages : pageStart + 11;
            for (int i = pageStart; i <= pageEnd; i++)
            {
                if (i != 0)
                {
                    TagBuilder tag = new TagBuilder("li");
                    TagBuilder subTag = new TagBuilder("a"); // Construct an <a> tag 
                    subTag.MergeAttribute("href", pageUrl(i));

                    if (i == pageStart)
                        subTag.InnerHtml = "<<";
                    else if (i == pageEnd && i != pagingInfo.TotalPages)
                        subTag.InnerHtml = ">>";
                    else
                        subTag.InnerHtml = i.ToString();

                    tag.InnerHtml = subTag.ToString();
                    if (i == pagingInfo.CurrentPage)
                        tag.AddCssClass("active ");
                    result.Append(tag.ToString());
                }
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }

}