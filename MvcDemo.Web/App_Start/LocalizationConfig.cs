using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

using MvcDemo.Web.Infrastructure;

namespace MvcDemo.Web
{
    public class LocalizationConfig
    {
        public static void ConfigureLocalization()
        {
            ModelMetadataProviders.Current = new LocalizedModelMetadataProvider();

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(LocalizedRequiredAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MaxLengthAttribute), typeof(LocalizedMaxLengthAttributeAdapter));
        }
    }
}