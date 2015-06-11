using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

using MvcDemo.Web.Infrastructure;

namespace MvcDemo.Web
{
    public class LocalizationConfig
    {
        public static void RegisterLocalizationServices()
        {
            ModelMetadataProviders.Current = new LocalizedModelMetadataProvider();

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RangeAttribute), typeof(LocalizedRangeAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RegularExpressionAttribute), typeof(LocalizedRegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(LocalizedRequiredAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(StringLengthAttribute), typeof(LocalizedStringLengthAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MaxLengthAttribute), typeof(LocalizedMaxLengthAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MinLengthAttribute), typeof(LocalizedMinLengthAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(CreditCardAttribute), (metadata, context, attribute) => new LocalizedDataTypeAttributeAdapter(metadata, context, (DataTypeAttribute)attribute, "creditcard"));
            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(EmailAddressAttribute), (metadata, context, attribute) => new LocalizedDataTypeAttributeAdapter(metadata, context, (DataTypeAttribute)attribute, "email"));
            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(PhoneAttribute), (metadata, context, attribute) => new LocalizedDataTypeAttributeAdapter(metadata, context, (DataTypeAttribute)attribute, "phone"));
            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(UrlAttribute), (metadata, context, attribute) => new LocalizedDataTypeAttributeAdapter(metadata, context, (DataTypeAttribute)attribute, "url"));
        }
    }
}