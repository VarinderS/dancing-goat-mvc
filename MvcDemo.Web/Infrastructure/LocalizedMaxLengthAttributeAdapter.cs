using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using CMS.Helpers;

namespace MvcDemo.Web.Infrastructure
{
    /// <summary>
    /// Provides an adapter for localization of the <see cref="MaxLengthAttribute"/> attribute.
    /// </summary>
    public class LocalizedMaxLengthAttributeAdapter : MaxLengthAttributeAdapter
    {
        /// <summary>
        /// Initializes a new instance of the LocalizedMaxLengthAttributeAdapter class.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The controller context.</param>
        /// <param name="attribute">The MaxLength attribute.</param>
        public LocalizedMaxLengthAttributeAdapter(ModelMetadata metadata, ControllerContext context, MaxLengthAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        /// <summary>
        /// Gets a list of localized client validation rules for a max length check.
        /// </summary>
        /// <returns>A list of client validation rules for the check.</returns>
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rules = base.GetClientValidationRules() ?? Enumerable.Empty<ModelClientValidationRule>();

            foreach (var rule in rules)
            {
                var localizedErrorMessage = ResHelper.GetString(rule.ErrorMessage);
                rule.ErrorMessage = string.Format(localizedErrorMessage, Metadata.GetDisplayName(), Attribute.Length);

                yield return rule;
            }
        }


        /// <summary>
        /// Returns a list of localized validation error messages for the model.
        /// </summary>
        /// <param name="container">The container for the model.</param>
        /// <returns>A list of validation error messages for the model, or an empty list if no errors have occurred.</returns>
        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var validationResults = base.Validate(container);

            foreach (var result in validationResults)
            {
                var localizedMessage = ResHelper.GetString(result.Message);
                result.Message = string.Format(localizedMessage, Metadata.GetDisplayName(), Attribute.Length);

                yield return result;
            }
        }
    }
}