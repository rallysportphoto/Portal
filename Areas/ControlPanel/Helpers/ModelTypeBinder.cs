/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Portal.Areas.ControlPanel.Helpers
{
    public class ModelTypeBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            // get the parameter OrderTypeId
            ValueProviderResult result;
            result = bindingContext.ValueProvider.GetValue("modelType");
            if (result == null)
                return base.CreateModel(controllerContext, bindingContext, modelType);

            var instantiationType = Assembly.GetExecutingAssembly().GetType(result.AttemptedValue);
            var obj = Activator.CreateInstance(instantiationType);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, instantiationType);
            bindingContext.ModelMetadata.Model = obj;
            return obj;            
        }        
    }
}