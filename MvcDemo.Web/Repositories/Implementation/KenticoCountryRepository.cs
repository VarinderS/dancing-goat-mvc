using CMS.Globalization;

namespace MvcDemo.Web.Repositories.Implementation
{
    /// <summary>
    /// Represents a collection of countries and states.
    /// </summary>
    public class KenticoCountryRepository : CountryRepository
    {
        /// <summary>
        /// Returns the country with the specified code name.
        /// </summary>
        /// <param name="countryName">The code name of the country.</param>
        /// <returns>The country with the specified code name, if found; otherwise, null.</returns>
        public override CountryInfo GetCountry(string countryName)
        {
            return CountryInfoProvider.GetCountryInfo(countryName);
        }


        /// <summary>
        /// Returns the state with the specified code name.
        /// </summary>
        /// <param name="stateName">The code name of the state.</param>
        /// <returns>The state with the specified code name, if found; otherwise, null.</returns>
        public override StateInfo GetState(string stateName)
        {
            return StateInfoProvider.GetStateInfo(stateName);
        }
    }
}