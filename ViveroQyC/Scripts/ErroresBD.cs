using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViveroQyC.Scripts
{
    class ErroresBD
    {
        public List<Dictionary<int, string>> lErrores = new List<Dictionary<int, string>>();
        public static Dictionary<int, string> dErrores = new Dictionary<int, string>
        {
            { 1, "Hecho." },
            { 100, "Numero de elementos de entrada no coincide con la cantidad de atributos en la tabla."},
            { 101, "Algun dato de entrada no es del tipo correcto."},
            { 102, "La tabla no existe." },
            { 103, "No existe el registro" }
        };
    }
}
