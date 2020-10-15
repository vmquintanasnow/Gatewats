using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Musala.Business.Filters
{    
    public class QueryModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {

            if (bindingContext == null)
            {
                bindingContext.ModelState.TryAddModelError(
                  "Error", "Sorry an error happens");

                return Task.CompletedTask;
            }

            var actionContext = bindingContext.ActionContext.HttpContext;

            NameValueCollection result = new NameValueCollection();

            try
            {
                result = HttpUtility.ParseQueryString(actionContext.Request.QueryString.Value);
            }
            catch (ArgumentNullException ex)
            {
                bindingContext.ModelState.TryAddModelError(
                  "Message", "sorry, an error occurred");
                bindingContext.ModelState.TryAddModelError(
                  "Error", ex.Message);

                return Task.CompletedTask;
            }


            var query = new QueryObject();

            if (result.Count > 0)
            {
                try
                {
                    foreach (string key in result.AllKeys.Select(s => s.Trim().ToLower(culture: new CultureInfo("es-ES"))))
                    {
                        switch (key)
                        {
                            case "$skip":

                                AsignValue(
                                    setValue: value => query.Skip = value,
                                    key: key,
                                    keyValue: result.Get(key),
                                    bindingContext);

                                break;

                            case "$limit":

                                AsignValue(
                                    setValue: value => query.Take = value,
                                    key: key,
                                    keyValue: result.Get(key),
                                    bindingContext);

                                break;

                            case "$orderby":
                                query.Sort = result.Get(key);
                                break;
                            case "$filter":
                                query.Filter = result.Get(key);
                                break;
                        }
                    }
                }
                catch(Exception ex)
                {
                    bindingContext.ModelState.TryAddModelError(
                  "Message", "There is an error in the query");
                    bindingContext.ModelState.TryAddModelError(
                  "Error", ex.Message);
                }


                if (bindingContext.ModelState.ErrorCount == 0)
                {
                    bindingContext.Result = ModelBindingResult.Success(query);
                }
            }

            return Task.CompletedTask;
        }

        private void AsignValue(Action<int?> setValue, string key, string keyValue, ModelBindingContext bindingContext)
        {
            if (!int.TryParse(keyValue, out int resultValue) || resultValue < 0)
            {
                bindingContext.ModelState.TryAddModelError(
                    key, $"Field:{key},the value must be a positive integer.");
            }
            else
            {
                setValue(resultValue);
            }
        }

    }

}

