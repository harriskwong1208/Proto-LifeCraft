using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeCraft.Models.ViewModels
{
	public class EventVM
	{
		public Event Event { get; set; }
		[ValidateNever] //Makes sure that it does not validate this property
		public IEnumerable<SelectListItem> CategoryList { get; set; }
	}
}
