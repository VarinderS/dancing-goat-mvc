using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

using CMS.Helpers;

namespace MvcDemo.Web.Infrastructure
{
    /// <summary>
    /// Provides an adapter for localization of the <see cref="RequiredAttribute"/> attribute.
    /// </summary>
    public class LocalizedRequiredAttributeAdapter : RequiredAttributeAdapter
    {
        /// <summary>
        /// Initializes a new instance of the LocalizedRequiredAttributeAdapter class.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The controller context.</param>
        /// <param name="attribute">The MaxLength attribute.</param>
        public LocalizedRequiredAttributeAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        /// <summary>
        /// Gets a list of localized required-value client validation rules.
        /// </summary>
        /// <returns>A list of localized required-value client validation rules.</returns>
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rules = base.GetClientValidationRules() ?? Enumerable.Empty<ModelClientValidationRule>();

            foreach (var rule in rules)
            {
                var localizedErrorMessage = ResHelper.GetString(rule.ErrorMessage);
                rule.ErrorMessage = string.Format(localizedErrorMessage, Metadata.GetDisplayName());

                yield return rule;
            }
        }


        /// <summary>
        /// Returns a list of localized validation error messages for the model.
        /// </summary>
        /// <param name="container">The container for the model.</param>
        /// <returns>A list of localized validation error messages for the model, or an empty list if no errors have occurred.</returns>
        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var validationResults = base.Validate(container);

            foreach (var result in validationResults)
            {
                var localizedMessage = ResHelper.GetString(result.Message);
                result.Message = string.Format(localizedMessage, Metadata.GetDisplayName());

                yield return result;
            }
        }
    }
}