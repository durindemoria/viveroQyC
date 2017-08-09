using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace ViveroQyC.Scripts
{
    /*
     * CLASE QUE SIRVE PARA CONTROLAR Y MANEJAR ERRORES, SANEAR Y VALIDAR, FORMULARIOS (WinForms)
     * 
     * 1) No se validan los ComboBox, se da por sentado que cuando se carga los combobox se cargan con
     * lo que el usuario desea cargar. 
     */
    class HandleForm
    {
        // Sanea y valida los datos de entrada.
        public static ushort SanearValidar(string tabla, params object[] controles)
        {
            /*
             * Funciona para controles TextBox y ComboBox
             * a) Comprueba que los datos de los controles no son null o no estan vacios. 
             * b) Comprueba que los datos se pueden parsear al tipo de dato de la tabla.
             * @return un numero especificando el error Si NO cumple con 'a' o 'b', devuelve 1 si cumple las condiciones.
             */
            int index = 0;
            HandleBD.GetColData(tabla);
            // Comprueba que no los campos no esten vacios y que el campo de la tabla no acepte nulls            
            foreach (HandleBD.cColInfo item in HandleBD.LDatosColumnas.Skip(1))
            {
                if (controles[index] is TextBox)
                {
                    TextBox txt = controles[index] as TextBox;
                    if (item.IsNull)
                        continue;
                    else
                    {
                        if (string.IsNullOrEmpty(txt.Text) || string.IsNullOrWhiteSpace(txt.Text))
                            return 1000;
                    }
                }
                if (controles[index] is ComboBox)
                {
                    ComboBox cmb = controles[index] as ComboBox;
                    if (item.IsNull)
                        continue;
                    else
                    {
                        if (cmb.SelectedIndex == -1)
                            return 1000;
                    }
                }
                index++;
            }

            // Comprueba si  pueden parsearse
            ushort comprobado = ComprobarTiposDatos(tabla, controles);
            if (comprobado == 1)
            {
               // object[] x = parsearDatos()
                return 1;
            }
            else
                return comprobado;
        }

        // Comprueba que los tipos de datos esten bien.
        private static ushort ComprobarTiposDatos(string tabla, object[] controles)
        {
            /*
             * SOLO USAR SI SE COMPROBO LA EXISTENCIA DE LA TABLA.
             * 
             * Comprueba los dataType de lDatosColumna contra los tipos de dato de inputs
             * @return 1 si los tipos de datos concuerdan, 1001 si algun tipo de dato no concuerda.
             */
            try
            {
                HandleBD.GetColData(tabla);
                int index = 0;
                foreach (HandleBD.cColInfo item in HandleBD.LDatosColumnas.Skip(1))
                {
                    //ARREGLAR
                    if (controles[index] is TextBox)
                    {
                        // Si es nulo y no tiene ningun contenido continuar sin checkear, si no da error.
                        TextBox txt = controles[index] as TextBox;
                        if (item.IsNull && string.IsNullOrEmpty(txt.Text) || string.IsNullOrWhiteSpace(txt.Text))
                            continue;
                        ushort result = CheckParseTxt(item.DataType, controles[index] as TextBox);
                        if (result != 1)
                            return result;
                    }

                    if (controles[index] is ComboBox)
                    {

                    }
                    index++;
                }
            }
            catch(Exception ex)
            {
                return 1001;
            }
            return 1;
        }
        
        public static object[] parsearDatos(string tabla, params object[] controles)
        {
            /*
             * @params  tipoDato: es el tipo de dato al cual se quiere parsear
             *          controles: es un arreglo que contiene los controles de los cuales se va a obtener
             *          el dato para parsear
             *          index: es el indice para ir recorriendo el arreglo que se va a devolver.
             * @return  resultado: arreglo con los objetos parseados. 
             *          Si alguno no se puede parsear, retorna null.
             */

            if (!HandleBD.IfTablaExiste(tabla))
                return null;

            HandleBD.GetColData(tabla);
            int indiceBD = 1;
            int index = 0;

            object[] resultado = new object[controles.Length];
            try
            {
                foreach (var obj in controles)
                {
                    Type tipoDato = HandleBD.LDatosColumnas[indiceBD].DataType;

                    // String
                    if (HandleBD.LDatosColumnas[indiceBD].DataType.Equals(typeof(string)))                    
                        resultado[index] = ParseCtrl<string>(tipoDato,obj);

                    //Decimal
                    if (HandleBD.LDatosColumnas[indiceBD].DataType.Equals(typeof(decimal)))
                        resultado[index] = ParseCtrl<decimal>(tipoDato, obj);

                    // Int16
                    if (HandleBD.LDatosColumnas[indiceBD].DataType.Equals(typeof(Int16)))
                        resultado[index] = ParseCtrl<Int16>(tipoDato, obj);

                    // Int32
                    if (HandleBD.LDatosColumnas[indiceBD].DataType.Equals(typeof(Int32)))
                        resultado[index] = ParseCtrl<Int32>(tipoDato, obj);

                    // Int64
                    if (HandleBD.LDatosColumnas[indiceBD].DataType.Equals(typeof(Int64)))
                        resultado[index] = ParseCtrl<Int64>(tipoDato, obj);

                    // DateTime
                    if (HandleBD.LDatosColumnas[indiceBD].DataType.Equals(typeof(DateTime)))
                        resultado[index] = ParseCtrl<DateTime>(tipoDato, obj );

                    //Index++
                    index++;    // Proximo control.
                    indiceBD++; // Proximo atributo en la tabla.
                }
            }
            catch(Exception ex)
            {
                return null;
            }
            return resultado;
        }

        //Parsea los datos de los TextBox
        public static T ParseCtrl<T>(Type tipoDato, object ctrl)
        {
            /*
             * @params T tipo de dato de la variable que se va retornar
             * @params ctrl contro del cual se va a obtener el dato a parsear
             * @return retorna el objeto parseado si se puede, en otro caso retorna NULL.
             */
            try
            {
                //object value = null;
                if (ctrl is TextBox)
                {
                    TextBox txt = ctrl as TextBox;
                    //value = System.Configuration.ConfigurationManager.AppSettings[txt.Text];
                    return (T)Convert.ChangeType(txt.Text, tipoDato);
                }
                // Se parsea el value del combobox.
                if (ctrl is ComboBox)
                {
                    ComboBox cmb = ctrl as ComboBox;
                    if (string.IsNullOrEmpty(cmb.SelectedValue.ToString()) || string.IsNullOrWhiteSpace(cmb.SelectedValue.ToString()) || cmb.SelectedValue == null || cmb.SelectedIndex < 0)
                        return (T)Convert.ChangeType(null, tipoDato);
                    //value = System.Configuration.ConfigurationManager.AppSettings[cmb.SelectedValue.ToString()];
                    return (T)Convert.ChangeType(cmb.SelectedValue.ToString(), tipoDato);
                }
                return default(T);
            }
            catch(Exception ex)
            {
                return default(T);
            }
        }

        // Comprueba si los datos del TextBox se puede parsear.
        public static ushort CheckParseTxt(Type tipoDato, TextBox txt)
        {
            /*
             * Usar con los datos de las columnas actualizados previamente.
             * @input   tipoDato : Tipo de dato contra el que se quiere comprobar.
             *          txt : TextBox que se quiere comprobar
             * @return 1 si se puede parsear
             * @return 1001 si el tipo de dato no tiene el formato correcto
             *   
             */
            try
            {
                // Si es string no importa lo que contenga.
                if (tipoDato.Equals(typeof(string)))
                {
                    if (string.IsNullOrEmpty(txt.Text) || string.IsNullOrWhiteSpace(txt.Text))
                        return 1001;
                }

                // Si es decimal
                if (tipoDato.Equals(typeof(decimal)))
                {
                    decimal auxd;
                    if (Decimal.TryParse(txt.Text, out auxd) != true)
                        return 1001;
                }

                // Si es Int32
                if (tipoDato.Equals(typeof(Int16)))
                {
                    Int16 auxi16;
                    if (Int16.TryParse(txt.Text, out auxi16) != true)
                        return 1001;
                }

                // Si es Int32
                if (tipoDato.Equals(typeof(Int32)))
                {
                    Int32 auxi32;
                    if (Int32.TryParse(txt.Text, out auxi32) != true)
                        return 1001;
                }

                // Si es Int64
                if (tipoDato.Equals(typeof(Int64)))
                {
                    Int64 auxi64;
                    if (Int64.TryParse(txt.Text, out auxi64) != true)
                        return 1001;
                }

                // Si es date
                if (tipoDato.Equals(typeof(DateTime)))
                {
                    DateTime auxDate;
                    if (DateTime.TryParse(txt.Text, out auxDate) != true)
                        return 1001;
                }                
            }
            catch
            {
                return 1001;
            }
            return 1;
        }

        public static T Castear<T>(object entrada, Type salida)
        {
            return (T)entrada;
        }
            
            
    }
}
