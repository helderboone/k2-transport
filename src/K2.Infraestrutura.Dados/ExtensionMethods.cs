using System.Linq;
using System.Reflection;

namespace K2.Infraestrutura.Dados
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Ordena uma coleção a partir do nome da propriedade do seu tipo
        /// </summary>
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> entities, string propertyName, string sortDirection)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
                return entities;

            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.);

            return sortDirection == "ASC"
                ? entities.OrderBy(e => propertyInfo.GetValue(e, null))
                : entities.OrderByDescending(e => propertyInfo.GetValue(e, null));
        }
    }
}
