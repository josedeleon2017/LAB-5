using System.Web;
using System.Web.Mvc;

namespace LAB_5___Tablas_Hash_y_Colas_de_prioridad
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
