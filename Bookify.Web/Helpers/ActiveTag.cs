using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Bookify.Web.Helpers
{
    [HtmlTargetElement("a", Attributes = "active-when")]
    public class ActiveTag : TagHelper
    {
        //receive tag value like => active-when ="Categories" => ActiveWhen = Categories
        public string? ActiveWhen { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContextData { get; set; } //receive all viewcontext

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //output receive the element that we put new helper tag(active-when) in it like anchor tag (a)
            if(string.IsNullOrEmpty(ActiveWhen))
                return;

            var currentController = ViewContextData?.RouteData.Values["controller"]?.ToString() ?? string.Empty; // ?? string.Empty => this for identity

            if (currentController!.Equals(ActiveWhen))
            {   //in case the element(like anchor tag) contains class attribute
                if (output.Attributes.ContainsName("class"))
                    output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value} active");
                else
					//in case the element(like anchor tag) does not contain class attribute
					output.Attributes.SetAttribute("class", "active");
            }
        }
    }
}