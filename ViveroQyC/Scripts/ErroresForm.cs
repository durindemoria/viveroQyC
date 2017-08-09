using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViveroQyC.Scripts
{
    class ErroresForm
    {
        public List<Dictionary<int, string>> lErrores = new List<Dictionary<int, string>>();
        public static Dictionary<int, string> dErrores = new Dictionary<int, string>
        {
            { 1, "Error: Hecho." },
            { 1000, "Error: Campo vacio"},
            { 1001, "Error: El formato no se corresponde con el tipo de dato."}
        };
    }
}
