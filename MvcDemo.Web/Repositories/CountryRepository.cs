using CMS.Globalization;

namespace MvcDemo.Web.Repositories
{
    /// <summary>
    /// Represents a contract for a collection of countries and states.
    /// </summary>
    public abstract class CountryRepository
    {
        /// <summary>
        /// Returns the country with the specified code name.
        /// </summary>
        /// <param name="countryName">The code name of the country.</param>
        /// <returns>The country with the specified code name, if found; otherwise, null.</returns>
        public abstract CountryInfo GetCountry(string countryName);


        /// <summary>
        /// Returns the state with the specified code name.
        /// </summary>
        /// <param name="stateName">The code name of the state.</param>
        /// <returns>The state with the specified code name, if found; otherwise, null.</returns>
        public abstract StateInfo GetState(string stateName);
    }
}