using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OnSale.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboCategories();

        IEnumerable<SelectListItem> GetComboCountries();

        IEnumerable<SelectListItem> GetComboDepartments();

        IEnumerable<SelectListItem> GetComboCities();

        IEnumerable<SelectListItem> GetComboDepartments(int contryId);

        IEnumerable<SelectListItem> GetComboCities(int departmentId);
    }
}
