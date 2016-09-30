using System;
using System.Web.Mvc;
using Abp.Json;

/* This class is inspired from http://www.matskarlsson.se/blog/serialize-net-objects-as-camelcase-json */

namespace Abp.Web.Mvc.Controllers.Results
{
    /// <summary>
    ///     This class is used to override returning Json results from MVC controllers.
    /// </summary>
    public class AbpJsonResult : JsonResult
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public AbpJsonResult()
        {
            JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        /// <summary>
        ///     Constructor with JSON data.
        /// </summary>
        /// <param name="data">JSON data</param>
        public AbpJsonResult(object data)
            : this()
        {
            Data = data;
        }

        /// <inheritdoc />
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var ignoreJsonRequestBehaviorDenyGet = false;
            if (context.HttpContext.Items.Contains("IgnoreJsonRequestBehaviorDenyGet"))
            {
                ignoreJsonRequestBehaviorDenyGet = string.Equals(context.HttpContext.Items["IgnoreJsonRequestBehaviorDenyGet"].ToString(), "true", StringComparison.OrdinalIgnoreCase);
            }

            if (!ignoreJsonRequestBehaviorDenyGet && JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.");
            }

            var response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                response.Write(Data.ToJsonString(true, true));
            }
        }
    }
}