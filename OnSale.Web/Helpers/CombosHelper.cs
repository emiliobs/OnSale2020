using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnSale.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace OnSale.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboCategories()
        {
            List<SelectListItem> list = _context.Categories.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            }).OrderBy(t => t.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a Category.....]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCountries()
        {
            var listCountries = _context.Countries.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}",
            }).OrderBy(t => t.Text).ToList();

            listCountries.Insert(0, new SelectListItem 
            { 
                Text = "[Select a Country.....]", 
                Value = "0" 
            });

            return listCountries;
        }

        public IEnumerable<SelectListItem> GetComboDepartments(int contryId)
        {
            var listDepartments = new List<SelectListItem>();
            var countries = _context.Countries.Include(c => c.Departments).FirstOrDefault(c => c.Id == contryId);

            if (countries != null)
            {
                listDepartments = countries.Departments.Select(t => new SelectListItem 
                {
                   Text = t.Name,
                   Value = $"{t.Id}",
                }).OrderBy(t => t.Text).ToList();
            }

            listDepartments.Insert(0, new SelectListItem 
            {
                 Text = "[Select a Department.....]",
                 Value = "0",
            });

            return listDepartments;
          
        }
        
        public IEnumerable<SelectListItem> GetComboCities(int departmentId)
        {
            var listCities = new List<SelectListItem>();
            var department = _context.Departments.Include(d => d.Cities).FirstOrDefault(d => d.Id == departmentId);

            if (department != null)
            {
                listCities = department.Cities.Select(t => new SelectListItem 
                {
                    Text = t.Name,
                    Value = $"{t.Id}",
                }).OrderBy(t => t.Text).ToList();
            }

            listCities.Insert(0, new SelectListItem
            {
               Text = "[Select a City.....]",
               Value = "0",
            });

            return listCities;

        }

      

    }
}
